using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RegionOrebroLan.DependencyInjection;
using RegionOrebroLan.Web.Authentication.Cookies.Configuration;

namespace RegionOrebroLan.Web.Authentication.Cookies.DependencyInjection.Extensions
{
	public static class ServiceCollectionExtension
	{
		#region Methods

		public static ITicketStoreBuilder AddTicketStore(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment, IInstanceFactory instanceFactory)
		{
			return services.AddTicketStore(configuration, ConfigurationKeys.TicketStorePath, hostEnvironment, instanceFactory);
		}

		public static ITicketStoreBuilder AddTicketStore(this IServiceCollection services, IConfiguration configuration, string configurationKey, IHostEnvironment hostEnvironment, IInstanceFactory instanceFactory)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			var ticketStoreBuilder = new TicketStoreBuilder(configuration, hostEnvironment, instanceFactory, services)
			{
				ConfigurationKey = configurationKey
			};

			ticketStoreBuilder.Configure();

			return ticketStoreBuilder;
		}

		#endregion
	}
}