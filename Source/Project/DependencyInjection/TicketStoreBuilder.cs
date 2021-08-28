using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using RegionOrebroLan.Configuration;
using RegionOrebroLan.DependencyInjection;
using RegionOrebroLan.Web.Authentication.Cookies.Configuration;
using RegionOrebroLan.Web.Authentication.Cookies.DependencyInjection.Configuration;
using TicketStoreOptions = RegionOrebroLan.Web.Authentication.Cookies.DependencyInjection.Configuration.TicketStoreOptions;

namespace RegionOrebroLan.Web.Authentication.Cookies.DependencyInjection
{
	public class TicketStoreBuilder : ITicketStoreBuilder
	{
		#region Constructors

		public TicketStoreBuilder(IConfiguration configuration, IHostEnvironment hostEnvironment, IInstanceFactory instanceFactory, IServiceCollection services)
		{
			this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
			this.HostEnvironment = hostEnvironment ?? throw new ArgumentNullException(nameof(hostEnvironment));
			this.InstanceFactory = instanceFactory ?? throw new ArgumentNullException(nameof(instanceFactory));
			this.Services = services ?? throw new ArgumentNullException(nameof(services));
		}

		#endregion

		#region Properties

		public virtual IConfiguration Configuration { get; }
		public virtual string ConfigurationKey { get; set; } = ConfigurationKeys.TicketStorePath;
		public virtual IHostEnvironment HostEnvironment { get; }
		public virtual IInstanceFactory InstanceFactory { get; }
		public virtual IServiceCollection Services { get; }

		#endregion

		#region Methods

		public virtual void Configure()
		{
			try
			{
				var configurationSection = this.Configuration.GetSection(this.ConfigurationKey);

				var dynamicOptions = new DynamicOptions();
				configurationSection.Bind(dynamicOptions);

				var ticketStoreOptions = dynamicOptions.Type != null ? (TicketStoreOptions)this.InstanceFactory.Create(dynamicOptions.Type) : new EmptyOptions();

				configurationSection.Bind(ticketStoreOptions);

				ticketStoreOptions.Add(this);

				this.Services.TryAddSingleton<IDataSerializer<AuthenticationTicket>, CryptographicAuthenticationTicketSerializer>();
				this.Services.AddSingleton(ticketStoreOptions);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException("Could not configure ticket-store.", exception);
			}
		}

		#endregion
	}
}