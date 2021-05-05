using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RegionOrebroLan.Logging.Extensions;

namespace RegionOrebroLan.Web.Authentication.Cookies.Configuration
{
	public class PostConfigureCookieAuthenticationOptions : IPostConfigureOptions<CookieAuthenticationOptions>
	{
		#region Constructors

		public PostConfigureCookieAuthenticationOptions(ILoggerFactory loggerFactory, ITicketStore ticketStore, IOptionsMonitor<TicketStoreOptions> ticketStoreOptionsMonitor)
		{
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
			this.TicketStore = ticketStore ?? throw new ArgumentNullException(nameof(ticketStore));
			this.TicketStoreOptionsMonitor = ticketStoreOptionsMonitor ?? throw new ArgumentNullException(nameof(ticketStoreOptionsMonitor));
		}

		#endregion

		#region Properties

		protected internal virtual ILogger Logger { get; }
		protected internal virtual ITicketStore TicketStore { get; }
		protected internal virtual IOptionsMonitor<TicketStoreOptions> TicketStoreOptionsMonitor { get; }

		#endregion

		#region Methods

		public virtual void PostConfigure(string name, CookieAuthenticationOptions options)
		{
			this.Logger.LogDebugIfEnabled($"Starting post-configuration of cookie-authentication-options for setting the session-store, \"{typeof(ITicketStore)}\".");

			if(options == null)
				throw new ArgumentNullException(nameof(options));

			var ticketStoreOptions = this.TicketStoreOptionsMonitor.CurrentValue;

			if(!ticketStoreOptions.CookieAuthenticationSchemes.Contains(name))
			{
				this.Logger.LogDebugIfEnabled($"The cookie-authentication-options with name \"{name}\" is not configured.");
				return;
			}

			if(options.SessionStore != null)
			{
				this.Logger.LogDebugIfEnabled($"The session-store is already set for cookie-authentication-options \"{name}\". Skipping use of \"{this.TicketStore.GetType()}\".");
				return;
			}

			this.Logger.LogDebugIfEnabled($"Setting the session-store to \"{this.TicketStore.GetType()}\" for cookie-authentication-options named \"{name}\".");

			options.SessionStore = this.TicketStore;
		}

		#endregion
	}
}