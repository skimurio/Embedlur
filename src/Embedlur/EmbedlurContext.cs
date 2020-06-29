using System;
using Embedlur.Helpers;
using Autofac;

namespace Embedlur
{
    public static class EmbedlurContext
    {
        private static IContainer Container { get; set; }

        static EmbedlurContext()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<RequestService>().As<IRequestService>().SingleInstance();
            builder.RegisterType<ProviderDiscovery>().As<IProviderDiscovery>().SingleInstance();
            builder.RegisterType<ProviderResolver>().As<IProviderResolver>().SingleInstance();
            builder.RegisterType<HtmlParser>().As<IHtmlParser>().SingleInstance();

            Container = builder.Build();
        }

        public static IProviderResolver Resolver
        {
            get
            {
                return Container.Resolve<IProviderResolver>();
            }
        }
    }
}
