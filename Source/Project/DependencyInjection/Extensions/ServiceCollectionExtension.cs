using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RegionOrebroLan.Web.Authentication.Cookies.Configuration;
using PostConfigureCookieAuthenticationOptions = RegionOrebroLan.Web.Authentication.Cookies.Configuration.PostConfigureCookieAuthenticationOptions;

namespace RegionOrebroLan.Web.Authentication.Cookies.DependencyInjection.Extensions
{
	public static class ServiceCollectionExtension
	{
		#region Methods

		public static IServiceCollection AddTicketStore(this IServiceCollection services, IConfiguration configuration, string configurationKey = ConfigurationKeys.TicketStorePath)
		{
			if(services == null)
				throw new ArgumentNullException(nameof(services));

			if(configuration == null)
				throw new ArgumentNullException(nameof(configuration));

			var ticketStoreConfigurationSection = configuration.GetSection(configurationKey);
			var ticketStoreOptions = new TicketStoreOptions();
			ticketStoreConfigurationSection.Bind(ticketStoreOptions);

			// ReSharper disable InvertIf
			if(ticketStoreOptions.Type != null)
			{
				var ticketStoreType = Type.GetType(ticketStoreOptions.Type, true, true);

				if(!typeof(ITicketStore).IsAssignableFrom(ticketStoreType))
					throw new InvalidOperationException($"The type \"{ticketStoreOptions.Type}\" does not implement \"{typeof(ITicketStore)}\".");

				services.Configure<TicketStoreOptions>(ticketStoreConfigurationSection);
				services.Add(new ServiceDescriptor(typeof(ITicketStore), ticketStoreType, ServiceLifetime.Singleton));
				services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>();
			}
			// ReSharper restore InvertIf

			return services;
		}

		#endregion
	}
}