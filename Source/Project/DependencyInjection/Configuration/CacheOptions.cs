using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PostConfigureCookieAuthenticationOptions = RegionOrebroLan.Web.Authentication.Cookies.Configuration.PostConfigureCookieAuthenticationOptions;

namespace RegionOrebroLan.Web.Authentication.Cookies.DependencyInjection.Configuration
{
	public abstract class CacheOptions : TicketStoreOptions
	{
		#region Methods

		protected internal override void AddInternal(ITicketStoreBuilder builder)
		{
			if(builder == null)
				throw new ArgumentNullException(nameof(builder));

			builder.Services.Configure<Cookies.Configuration.TicketStoreOptions>(options =>
			{
				this.Options?.Bind(options);
			});

			builder.Services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>();
		}

		#endregion
	}
}