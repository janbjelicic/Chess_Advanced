using Ninject;
using System;
using System.Web.Mvc;
using System.Web.Routing;
using Chess.Domain.Abstract;
using Chess.Domain.Concrete;

namespace Chess.Infrastructure
{
    public class NinjectControllerFactory: DefaultControllerFactory
    {
        private readonly IKernel kernel;
        public NinjectControllerFactory()
        {
            kernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)kernel.Get(controllerType);
        }
        private void AddBindings()
        {
            kernel.Bind<IMatchRepository>().To<MatchRepository>();
            kernel.Bind<IPositionRepository>().To<PositionRepository>();
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IGameRepository>().To<GameRepository>();
            kernel.Bind<IUserSolutionRepository>().To<UserSolutionRepository>();
        }
    }
}