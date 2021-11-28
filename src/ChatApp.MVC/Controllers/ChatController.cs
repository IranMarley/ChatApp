using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System;
using ChatApp.Application.Interface;

namespace ChatApp.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {

        private readonly IUserService _userService;

        public ChatController(IUserService userService)
        {
            _userService = userService;
        }

        #region Methods

        public ActionResult Chat()
        {
            ViewBag.UserName = User.Identity.GetUserName();
            return View();
        }
        
        #endregion
    }
}