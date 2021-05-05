using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RegionOrebroLan.Logging.Extensions;

namespace RegionOrebroLan.Web.Authentication.Cookies
{
	public class DistributedCacheTicketStore : CacheTicketStore
	{
		#region Constructors

		public DistributedCacheTicketStore(IDataProtectionProvider dataProtectionProvider, IDistributedCache distributedCache, ILoggerFactory loggerFactory) : base(loggerFactory)
		{
			this.DataProtector = (dataProtectionProvider ?? throw new ArgumentNullException(nameof(dataProtectionProvider))).CreateProtector(this.GetType().FullName);
			this.DistributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
		}

		#endregion

		#region Properties

		protected internal virtual IDataProtector DataProtector { get; }
		protected internal virtual IDistributedCache DistributedCache { get; }

		#endregion

		#region Methods

		protected internal virtual async Task<AuthenticationTicket> DeserializeAuthenticationTicketAsync(byte[] bytes)
		{
			if(bytes == null)
				return null;

			return await Task.FromResult(TicketSerializer.Default.Deserialize(this.DataProtector.Unprotect(bytes)));
		}

		public override async Task RemoveAsync(string key)
		{
			this.Logger.LogDebugIfEnabled($"RemoveAsync: key = \"{key}\"");

			await this.DistributedCache.RemoveAsync(key);
		}

		public override async Task RenewAsync(string key, AuthenticationTicket ticket)
		{
			this.Logger.LogDebugIfEnabled($"RenewAsync: authentication-scheme = \"{ticket?.AuthenticationScheme}\", identity-name = \"{ticket?.Principal?.Identity?.Name}\", key = \"{key}\"");

			if(ticket == null)
				throw new ArgumentNullException(nameof(ticket));

			var expires = ticket.Properties.ExpiresUtc;
			var options = new DistributedCacheEntryOptions();

			if(expires.HasValue)
			{
				this.Logger.LogDebugIfEnabled($"RenewAsync: setting absolute expiration to \"{expires.Value}\"");

				options.SetAbsoluteExpiration(expires.Value);
			}

			var bytes = await this.SerializeAuthenticationTicketAsync(ticket);

			await this.DistributedCache.SetAsync(key, bytes, options);
		}

		public override async Task<AuthenticationTicket> RetrieveAsync(string key)
		{
			this.Logger.LogDebugIfEnabled($"RetrieveAsync: key = \"{key}\"");

			var bytes = await this.DistributedCache.GetAsync(key);

			return await this.DeserializeAuthenticationTicketAsync(bytes);
		}

		protected internal virtual async Task<byte[]> SerializeAuthenticationTicketAsync(AuthenticationTicket ticket)
		{
			if(ticket == null)
				return null;

			return await Task.FromResult(this.DataProtector.Protect(TicketSerializer.Default.Serialize(ticket)));
		}

		#endregion
	}
}