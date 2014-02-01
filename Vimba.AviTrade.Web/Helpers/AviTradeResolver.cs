using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Vimba.AviTrade.Repositories;
using Microsoft.Practices.Unity;

namespace Vimba.AviTrade.Web.Helpers
{
    public class AviTradeResolver : IDependencyResolver
    {
        private readonly UnityContainer _container;
 
        public AviTradeResolver()
        {
            _container = new UnityContainer();

            // Singleton Instance to register
            // It is important to have a singleton context!!
            AviTradeContext context = new AviTradeContext();
            _container.RegisterInstance(context);

            // It is important to create a the DbContext per web request. This is the recommended approach. In Unity ID, we implement the 
            // PerCallContextLifeTimeManager to force this. Please see notes below.
            // TODO: Perhaps I should consider my context to implement the IUnitOfWork. So the code below becomes:
            //_container.RegisterType<IUnitOfWork, AviTradeContext>(
            //            new PerCallContextLifeTimeManager(),
            //            new InjectionConstructor());
            // TODO: Also I am still not sure where to call Dispose on the DbContext
            _container.RegisterType<DbContext, AviTradeContext>(
                        new PerCallContextLifeTimeManager(),
                        new InjectionConstructor());

            // Repositories registration
            _container.RegisterType<IAircraftsRepository, AircraftsRepository>();
            _container.RegisterType<IAirportsRepository, AirportsRepository>();
            _container.RegisterType<IContractsRepository, ContractsRepository>();
            _container.RegisterType<ICurrenciesRepository, CurrenciesRepository>();
            _container.RegisterType<IGroupsRepository, GroupsRepository>();
            _container.RegisterType<IRolesRepository, RolesRepository>();
            _container.RegisterType<IInvoicesRepository, InvoicesRepository>();
            _container.RegisterType<IOrdersRepository, OrdersRepository>();
            _container.RegisterType<IItemsRepository, ItemsRepository>();
            _container.RegisterType<IPeriodsRepository, PeriodsRepository>();
            _container.RegisterType<ITradersRepository, TradersRepository>();
            _container.RegisterType<IUsersRepository, UsersRepository>();
            _container.RegisterType<IUserConfigurationItemsRepository, UserConfigurationItemsRepository>();

            //Register all controller type found in current assembly to the Unity container will be able to resolve them
            foreach (Type controllerType in (from t in Assembly.GetExecutingAssembly().GetTypes() where typeof(IController).IsAssignableFrom(t) select t))
            {
                _container.RegisterType(controllerType);
            }
        }

        public object GetService(Type serviceType)
        {
            //
            //  Unity "Resolve" method throws exception so we need to wrap it up with non throwing method (like MVC expects)
            //
            return TryGetService(serviceType);
        }

        public object TryGetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            //
            //  Unity "Resolve" method throws exception so we need to wrap it up with non throwing method (like MVC expects)
            //
            return TryGetServices(serviceType);
        }

        public IEnumerable<object> TryGetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    // Unity Life Time Manager to allow the DbContext to created per web request
    public class PerCallContextLifeTimeManager : LifetimeManager
    {
        // Each instance of LifetimeManager should use a unique key rather than a constant.
        // Otherwise if you have more than one object registered with PerCallContextLifeTimeManager, 
        // they're sharing the same key to access CallContext, and you won't get your expected object back.
        private string _key = string.Format("PerCallContextLifeTimeManager_{0}", Guid.NewGuid());

        // If ASP.Net does its thread-swap then the only thing taken across to the new thread through CallContext is the current HttpContext - 
        // anything else you store in CallContext will be gone. This means under heavy load the code above could have unintended results - and I 
        // imagine it would be a real pain to track down why!
        public override object GetValue()
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
                return httpContext.Items[_key];
            else
                return null;
        }

        public override void SetValue(object newValue)
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
                httpContext.Items[_key] = newValue;
        }

        public override void RemoveValue()
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext != null)
                httpContext.Items[_key] = null;
        }

        // This was my first implementation but then I saw people talking about the above so I replaced it.
        //public override object GetValue()
        //{
        //    return CallContext.GetData(_key);
        //}

        //public override void SetValue(object newValue)
        //{
        //    CallContext.SetData(_key, newValue);
        //}

        //public override void RemoveValue()
        //{
        //    CallContext.FreeNamedDataSlot(_key);
        //}
    }
}