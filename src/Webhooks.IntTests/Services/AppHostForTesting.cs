﻿using System.Linq;
using Funq;
using ServiceStack.Auth;
using ServiceStack.Validation;

namespace ServiceStack.Webhooks.IntTests.Services
{
    public class AppHostForTesting : AppSelfHostBase
    {
        public AppHostForTesting() : base("AppHostForTesting", typeof(AppHostForTesting).Assembly)
        {
        }

        public override void Configure(Container container)
        {
            Config.DebugMode = true;
            Config.ReturnsInnerException = true;
            RegisterAuthentication(container);
            Plugins.Add(new ValidationFeature());
            Plugins.Add(new WebhookFeature());
        }

        private void RegisterAuthentication(Container container)
        {
            Plugins.Add(new AuthFeature(() => new AuthUserSession(), new IAuthProvider[]
            {
                new BasicAuthProvider(),
                new CredentialsAuthProvider()
            }));
            Plugins.Add(new RegistrationFeature());
            container.Register<IUserAuthRepository>(new InMemoryAuthRepository());
        }

        public string LoginUser(JsonServiceClient client, string username, string roles)
        {
            var password = "apassword";
            var userRepo = Resolve<IAuthRepository>();
            if (userRepo.GetUserAuthByUserName(username) == null)
            {
                string hash;
                string salt;
                new SaltedHash().GetHashAndSaltString(password, out hash, out salt);

                userRepo.CreateUserAuth(new UserAuth
                {
                    UserName = username,
                    Roles = roles.SafeSplit(WebhookFeature.RoleDelimiters).ToList(),
                    PasswordHash = hash,
                    Salt = salt
                }, password);
            }

            return client.Post(new Authenticate
            {
                UserName = username,
                Password = password,
                provider = CredentialsAuthProvider.Name
            }).UserId;
        }
    }
}