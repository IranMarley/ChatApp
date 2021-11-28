using ChatApp.Application.ViewModels;
using System.Collections.Generic;

namespace ChatApp.Application.Interface
{
    public interface IStockService
    {
        MessageDetailViewModel GetQuote(string id);
    }
}
