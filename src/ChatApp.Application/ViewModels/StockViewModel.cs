using System;

namespace ChatApp.Application.ViewModels
{
    public class StockViewModel
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public string Volume { get; set; }
    }
}
