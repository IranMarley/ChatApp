using System.Web.Mvc;
using ChatApp.Domain.Interface.Repository;

namespace ChatApp.MVC.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: Users
        public ActionResult Index()
        {
            return View(_userRepository.GetAll());
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            return View(_userRepository.GetById(id));
        }

        public ActionResult DisableLock(string id)
        {
            _userRepository.DisableLock(id);
            return RedirectToAction("Index");
        }
    }
}
