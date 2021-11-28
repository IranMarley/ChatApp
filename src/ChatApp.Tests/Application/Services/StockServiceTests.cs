using System;
using ChatApp.Application.Interface;
using ChatApp.Application.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace ChatApp.Tests.Application.Services
{
    [TestClass]
    public class StockServiceTest
    {
        private readonly Mock<IRequestService> _mockRequestService;
        private readonly StockService _stockService;

        public StockServiceTest()
        {
            _mockRequestService = new Mock<IRequestService>();
            _stockService = new StockService(_mockRequestService.Object);
        }

        [TestMethod]
        public void GetQuote_Should_Return_Not_Valid_Comand()
        {
            var result = _stockService.GetQuote("/stockCode").Message;
            Assert.AreEqual("Command is not valid.", result);
        }

        [TestMethod]
        public void GetQuote_Should_Return_Share_Symbol_Not_Found()
        {
            _mockRequestService
               .Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Method>()))
               .Returns(@"Symbol,Date,Time,Open,High,Low,Close,Volume
                          APPLE,N/D,N/D,N/D,N/D,N/D,N/D,N/D");

            var result = _stockService.GetQuote("/stock=APPLE").Message;

            Assert.AreEqual("Symbol not found.", result);
        }

        [TestMethod]
        public void GetQuote_Should_Return_Share_Quote()
        {
            _mockRequestService
                .Setup(x => x.SendRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Method>()))
                .Returns(@"Symbol,Date,Time,Open,High,Low,Close,Volume
                           AAPL.US,2021-11-26,19:00:20,159.565,160.45,156.36,156.81,76959752");

            var result = _stockService.GetQuote("/stock=AAPL.US").Message;

            Assert.AreEqual("AAPL.US quote is $156.81 per share.", result);
        }

       
    }
}
