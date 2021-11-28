using ChatApp.Application.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChatApp.Tests.Application.Services
{
    [TestClass]
    public class StockServiceTest
    {
        [TestMethod]
        public void ReturnNotValidComand()
        {
            var stockService = new StockService();
            var result = stockService.GetQuote("/stockCode").Message;
            Assert.AreEqual(result, "Command is not valid.");
        }

        [TestMethod]
        public void ReturnSymbolNotFound()
        {
            var stockService = new StockService();
            var result = stockService.GetQuote("/stock=APPLE").Message;

            Assert.AreEqual(result, "Symbol not found.");
        }

        [TestMethod]
        public void ReturnShareQuote()
        {
            var stockService = new StockService();
            var result = stockService.GetQuote("/stock=AAPL.US").Message;

            Assert.IsTrue(result.Contains("AAPL.US quote is"));
        }

    }
}
