using System.Collections.Generic;

namespace ChatApp.Application.Interface
{
    public interface IStockService
    {
        Dictionary<string, string> GetQuote(string id);
    }
}
