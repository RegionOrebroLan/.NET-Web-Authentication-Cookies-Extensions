using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using RegionOrebroLan.Logging.Extensions;

namespace RegionOrebroLan.Web.Authentication.Cookies
{
	public class MemoryCacheTicketStore : CacheTicketStore
	{
		#region Constructors

		public MemoryCacheTicketStore(ILoggerFactory loggerFactory, IMemoryCache memoryCache) : base(loggerFactory)
		{
			this.MemoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
		}

		#endregion

		#region Properties

		protected internal virtual IMemoryCache MemoryCache { get; }

		#endregion

		#region Methods

		public override async Task RemoveAsync(string key)
		{
			this.Logger.LogDebugIfEnabled($"RemoveAsync: key = \"{key}\"");

			this.MemoryCache.Remove(key);

			await Task.CompletedTask;
		}

		public override async Task RenewAsync(string key, AuthenticationTicket ticket)
		{
			this.Logger.LogDebugIfEnabled($"RenewAsync: authentication-scheme = \"{ticket?.AuthenticationScheme}\", identity-name = \"{ticket?.Principal?.Identity?.Name}\", key = \"{key}\"");

			if(ticket == null)
				throw new ArgumentNullException(nameof(ticket));

			var options = new MemoryCacheEntryOptions();
			var expires = ticket.Properties.ExpiresUtc;

			if(expires.HasValue)
			{
				this.Logger.LogDebugIfEnabled($"RenewAsync: setting absolute expiration to \"{expires.Value}\"");

				options.SetAbsoluteExpiration(expires.Value);
			}

			await Task.FromResult(this.MemoryCache.Set(key, ticket, options));
		}

		public override async Task<AuthenticationTicket> RetrieveAsync(string key)
		{
			this.Logger.LogDebugIfEnabled($"RetrieveAsync: key = \"{key}\"");

			if(this.MemoryCache.TryGetValue(key, out AuthenticationTicket ticket))
				return await Task.FromResult(ticket);

			return null;
		}

		#endregion
	}
}