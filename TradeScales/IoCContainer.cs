using Autofac;
using System.Data.Entity;
using TradeScales.Data;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Services;
using TradeScales.Services.Abstract;

namespace TradeScales
{
    //Inversion of Control(IoC)
    public static class IoCContainer
    {
        public static IContainer BaseContainer { get; set; }

        public static void Build()
        {
            if (BaseContainer == null)
            {
                var builder = new ContainerBuilder();

                // EF TradeScalesContext
                builder.RegisterType<TradeScalesContext>()
                    .As<DbContext>()
                    .InstancePerRequest();

                builder.RegisterType<UnitOfWork>()
                    .As<IUnitOfWork>()
                    .InstancePerRequest();

                builder.RegisterGeneric(typeof(EntityBaseRepository<>))
                    .As(typeof(IEntityBaseRepository<>))
                    .InstancePerRequest();

                // Services
                builder.RegisterType<EncryptionService>()
                    .As<IEncryptionService>()
                    .InstancePerRequest();

                builder.RegisterType<MembershipService>()
                    .As<IMembershipService>()
                    .InstancePerRequest();

                BaseContainer = builder.Build();
            }
        }

        public static TService Resolve<TService>()
        {
            return BaseContainer.Resolve<TService>();
        }
    }
}
