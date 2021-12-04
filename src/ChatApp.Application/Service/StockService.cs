using ChatApp.Application.Interface;
using ChatApp.Application.ViewModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatApp.Application.Service
{
    public class StockService : IStockService
    {
        #region Fields

        private string link = $"https://stooq.com/q/l/?s=symbol&f=sd2t2ohlcv&h&e=csv";

        private readonly IRequestService _requestService;

        #endregion

        #region Constructors

        public StockService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        #endregion

        #region Methods

        public MessageDetailViewModel GetQuote(string message)
        {
            message = message.ToLower();
            var msg = "";

            if (message.StartsWith("/stock="))
            {
                var symbol = message.Replace("/stock=", "").ToUpper();

                var result = _requestService.SendRequest(link.Replace("symbol", symbol), null, Method.GET);

                foreach (var line in result.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    List<string> columns = line.Split(',').ToList<string>();

                    if (columns.First() != "Symbol")
                    {
                        var stock = new StockViewModel
                        {
                            Symbol = columns[0],
                            Open = columns[3],
                            High = columns[4],
                            Low = columns[5],
                            Close = columns[6],
                            Volume = columns[7]
                        };                          

                        if (!stock.Close.Equals("N/D"))
                            msg = $"{symbol} quote is ${stock.Close} per share.";
                    }
                }

                if (string.IsNullOrEmpty(msg))
                    msg = $"Symbol not found.";
            }

            if (string.IsNullOrEmpty(msg))
                msg = $"Command is not valid.";


            return new MessageDetailViewModel
            {
                UserName = "Bot",
                Message = msg,
                Date = DateTime.Now.ToString("dd/MM/yyyy")
            };
        }

        #endregion

    }
}
