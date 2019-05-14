using Autofac;
using Autofac.Core;
using System;
using System.Data.Entity;
using WBS.Data;
using WBS.Data.Infrastructure;
using WBS.Data.Repositories;
using WBS.Services;
using WBS.Services.Abstract;

namespace WBS.Wpf
{
    public static class BootStrapper
    {
        public static ILifetimeScope _rootScope;

        public static void Start()
        {
            if (_rootScope != null)
            {
                return;
            }

            var builder = new ContainerBuilder();
            builder.RegisterType<WBSContext>()
                .As<DbContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DbFactory>()
                .As<IDbFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(EntityBaseRepository<>))
                .As(typeof(IEntityBaseRepository<>));

            builder.RegisterType<EncryptionService>()
                .As<IEncryptionService>();

            builder.RegisterType<MembershipService>()
                .As<IMembershipService>();

            _rootScope = builder.Build();
        }

        public static void Stop()
        {
            _rootScope.Dispose();
        }

        public static T Resolve<T>()
        {
            if (_rootScope == null)
            {
                throw new Exception("Bootstrapper hasn't been started!");
            }

            return _rootScope.Resolve<T>(new Parameter[0]);
        }

        public static T Resolve<T>(Parameter[] parameters)
        {
            if (_rootScope == null)
            {
                throw new Exception("Bootstrapper hasn't been started!");
            }

            return _rootScope.Resolve<T>(parameters);
        }
    }
}
