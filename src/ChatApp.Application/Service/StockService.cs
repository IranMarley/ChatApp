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

        //private readonly IRequestService _requestService;

        #endregion

        #region Constructors

        public StockService(/*IRequestService requestService*/)
        {
            //_requestService = requestService;
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

                var client = new RestClient(link.Replace("symbol", symbol));

                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);

                foreach (var line in response.Content.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    List<string> columns = line.Split(',').ToList<string>();

                    if (columns.First() != "Symbol")
                    {
                        var stock = new StockViewModel
                        {
                            Symbol = columns[0],
                            Open = columns[2],
                            High = columns[3],
                            Low = columns[4],
                            Close = columns[5],
                            Volume = columns[6]
                        };                          

                        if (stock.Close != "N/D")
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
                Message = msg
            };
        }

        #endregion

    }
}
