using AutoServices.Core.ServiceInterface;
using Cars.ApplicationServices.Services;
using Cars.CarTest.Mock;
using Cars.Core.ServiceInterface;
using Cars.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CarTest.Macros;
using System;
using System.Linq;

namespace Cars.CarTest
{
    public abstract class TestBase : IDisposable
    {
        protected IServiceProvider ServiceProvider { get; set; }

        protected TestBase()
        {
            var services = new ServiceCollection();
            SetupServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        public void Dispose()
        {
            
            if (ServiceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        protected T Svc<T>()
        {
            return ServiceProvider.GetService<T>();
        }

        public virtual void SetupServices(IServiceCollection services)
        {
            services.AddScoped<ICarsServices, CarsServices>();

            services.AddDbContext<CarsContext>(options =>
            {
                options.UseInMemoryDatabase("TEST");
                options.ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            RegisterMacros(services);
        }

        private void RegisterMacros(IServiceCollection services)
        {
            var macroBaseType = typeof(IMacros);
            var macros = macroBaseType.Assembly.GetTypes()
                .Where(x => macroBaseType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

            foreach (var macro in macros)
            {
                services.AddSingleton(macro);
            }
        }
    }
}
