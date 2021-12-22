using System;
using ChatApp.Infra.CrossCutting.Identity.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace ChatApp.Infra.CrossCutting.Identity.Configuration
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
            // Setting validator for username
            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Validation Logic and Password Complexity
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Lockout configuration
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Providers de Two Factor Autentication
            RegisterTwoFactorProvider("SMS Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });

            RegisterTwoFactorProvider("E-mail Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security code",
                BodyFormat = "Your security code is: {0}"
            });

            // Defining the Email Service Class
            EmailService = new EmailService();

            // Defining the SMS class of service
            SmsService = new SmsService();

            var provider = new DpapiDataProtectionProvider("Iran");
            var dataProtector = provider.Create("ChatApp");

            UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtector);

        }
    }
}