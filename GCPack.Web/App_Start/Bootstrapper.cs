using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac.Integration.Mvc;
using Autofac;
using GCPack.Service;
using GCPack.Service.Interfaces;
using AutoMapper;
using System.Web.Mvc;
using GCPack.Repository.Mappers;
using GCPack.Repository.Interfaces;
using GCPack.Repository;
using GCPack.Service;
using GCPack.Service.Interfaces;
using GCPack.Web.Filters;

namespace GCPack.Web.App_Start
{
    public static class Bootstrapper
    {
        public static void Configure()
        {
            ConfigureAutofacContainer();
            ConfigureAutomapper();
        }

        private static void ConfigureAutomapper()
        {

            Mapper.Initialize(mapper =>
            {
                mapper.AddProfile<ViewModelToModelMappingProfile>();
            });
        }

        public static void ConfigureAutofacContainer()
        {
            var webApiContainerBuilder = new ContainerBuilder();
            ConfigureWebApiContainer(webApiContainerBuilder);
        }
        public static void ConfigureWebApiContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<UsersService>().As<IUsersService>().AsImplementedInterfaces().InstancePerRequest();
            containerBuilder.RegisterType<UsersRepository>().As<IUsersRepository>().AsImplementedInterfaces().InstancePerRequest();
            containerBuilder.RegisterType<DocumentsService>().As<IDocumentsService>().AsImplementedInterfaces().InstancePerRequest();
            containerBuilder.Register(r => new UsersService(r.Resolve<IUsersRepository>())).AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.Register(r => new GCAuthentization (r.Resolve<IUsersService>())).AsAuthenticationFilterFor<Controller>().InstancePerLifetimeScope();

            containerBuilder.RegisterControllers(typeof(MvcApplication).Assembly);
            containerBuilder.RegisterModelBinders(typeof(MvcApplication).Assembly);
            containerBuilder.RegisterModule<AutofacWebTypesModule>();
            containerBuilder.RegisterFilterProvider();


            var container = containerBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            


        }


    }
}