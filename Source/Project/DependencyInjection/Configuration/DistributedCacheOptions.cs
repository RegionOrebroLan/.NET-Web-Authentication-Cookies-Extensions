using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace RegionOrebroLan.Web.Authentication.Cookies.DependencyInjection.Configuration
{
	public class DistributedCacheOptions : CacheOptions
	{
		#region Methods

		protected internal override void AddInternal(ITicketStoreBuilder builder)
		{
			if(builder == null)
				throw new ArgumentNullException(nameof(builder));

			builder.Services.AddSingleton<ITicketStore, DistributedCacheTicketStore>();

			base.AddInternal(builder);
		}

		#endregion
	}
}