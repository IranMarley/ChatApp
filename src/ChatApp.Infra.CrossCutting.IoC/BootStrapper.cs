using ChatApp.Application.Interface;
using ChatApp.Application.Service;
using ChatApp.Domain.Interface.Repository;
using ChatApp.Infra.CrossCutting.Identity.Configuration;
using ChatApp.Infra.CrossCutting.Identity.Context;
using ChatApp.Infra.CrossCutting.Identity.Model;
using ChatApp.Infra.Data.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SimpleInjector;

namespace ChatApp.Infra.CrossCutting.IoC
{
    public class BootStrapper
    {
        public static void RegisterServices(Container container)
        {
            container.RegisterPerWebRequest<ApplicationDbContext>();
            container.RegisterPerWebRequest<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(new ApplicationDbContext()));
            container.RegisterPerWebRequest<IRoleStore<IdentityRole, string>>(() => new RoleStore<IdentityRole>());
            container.RegisterPerWebRequest<ApplicationRoleManager>();
            container.RegisterPerWebRequest<ApplicationUserManager>();
            container.RegisterPerWebRequest<ApplicationSignInManager>();

            //container.RegisterPerWebRequest<IChatService, ChatService>();
            //container.RegisterPerWebRequest<IRabbitMQService, RabbitMQService>();
            container.Register<IStockService, StockService>();
            container.Register<IRequestService, RequestService>();

            container.Register<IUserService, UserService>();
            container.Register<IUserRepository, UserRepository>();
        } 
    }
}