using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Web.Mvc;
using WebMatrix.WebData;
using Chess.Models;
using Chess.Domain.Concrete;
using System.Collections.Generic;
using System.Web.Security;
using Chess.Domain.Entities;

namespace Chess.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
    {
        private static SimpleMembershipInitializer _initializer;
        private static object _initializerLock = new object();
        private static bool _isInitialized;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Ensure ASP.NET Simple Membership is initialized only once per app start
            LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
        }

        private class SimpleMembershipInitializer
        {
            public SimpleMembershipInitializer()
            {
                Database.SetInitializer<ChessDatabase>(null);

                try
                {
                    using (var context = new ChessDatabase())
                    {
                        if (!context.Database.Exists())
                        {
                            // Create the SimpleMembership database without Entity Framework migration schema
                            ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
                        }
                    }
                    if (!WebSecurity.Initialized)
                        WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfiles", "ID", "UserName", autoCreateTables: true);
                    RegisterRoles();
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
                }
            }

            /// <summary>
            /// Registering roles for the application.
            /// </summary>
            protected void RegisterRoles()
            {
                ISet<String> rolesSet = new SortedSet<String>();
                rolesSet.Add(UserRoles.Administrator.ToString());
                rolesSet.Add(UserRoles.Player.ToString());

                var allRoles = Roles.GetAllRoles();
                foreach (var r in allRoles)
                    rolesSet.Remove(r);

                foreach (var r in rolesSet)
                    Roles.CreateRole(r);
            }
        }
    }
}
