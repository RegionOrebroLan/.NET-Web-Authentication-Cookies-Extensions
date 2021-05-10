using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace RegionOrebroLan.Web.Authentication.Cookies.DependencyInjection.Configuration
{
	public class MemoryCacheOptions : CacheOptions
	{
		#region Methods

		protected internal override void AddInternal(ITicketStoreBuilder builder)
		{
			if(builder == null)
				throw new ArgumentNullException(nameof(builder));

			builder.Services.AddMemoryCache();
			builder.Services.AddSingleton<ITicketStore, MemoryCacheTicketStore>();

			base.AddInternal(builder);
		}

		#endregion
	}
}