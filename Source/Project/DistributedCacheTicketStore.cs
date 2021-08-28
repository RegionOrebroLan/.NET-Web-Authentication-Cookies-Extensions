using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using RegionOrebroLan.Logging.Extensions;

namespace RegionOrebroLan.Web.Authentication.Cookies
{
	public class DistributedCacheTicketStore : CacheTicketStore
	{
		#region Constructors

		public DistributedCacheTicketStore(IDataSerializer<AuthenticationTicket> authenticationTicketSerializer, IDistributedCache distributedCache, ILoggerFactory loggerFactory) : base(loggerFactory)
		{
			this.AuthenticationTicketSerializer = authenticationTicketSerializer ?? throw new ArgumentNullException(nameof(authenticationTicketSerializer));
			this.DistributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
		}

		#endregion

		#region Properties

		protected internal virtual IDataSerializer<AuthenticationTicket> AuthenticationTicketSerializer { get; }
		protected internal virtual IDistributedCache DistributedCache { get; }

		#endregion

		#region Methods

		protected internal virtual async Task<AuthenticationTicket> DeserializeAuthenticationTicketAsync(byte[] bytes)
		{
			if(bytes == null)
				return null;

			return await Task.FromResult(this.AuthenticationTicketSerializer.Deserialize(bytes));
		}

		public override async Task RemoveAsync(string key)
		{
			this.Logger.LogDebugIfEnabled($"{this.LogPrefix}RemoveAsync: key = \"{key}\"");

			await this.DistributedCache.RemoveAsync(key);
		}

		public override async Task RenewAsync(string key, AuthenticationTicket ticket)
		{
			this.Logger.LogDebugIfEnabled($"{this.LogPrefix}RenewAsync: authentication-scheme = \"{ticket?.AuthenticationScheme}\", identity-name = \"{ticket?.Principal?.Identity?.Name}\", key = \"{key}\"");

			if(ticket == null)
				throw new ArgumentNullException(nameof(ticket));

			var expires = ticket.Properties.ExpiresUtc;
			var options = new DistributedCacheEntryOptions();

			if(expires.HasValue)
			{
				this.Logger.LogDebugIfEnabled($"{this.LogPrefix}RenewAsync: setting absolute expiration to \"{expires.Value}\"");

				options.SetAbsoluteExpiration(expires.Value);
			}

			var bytes = await this.SerializeAuthenticationTicketAsync(ticket);

			await this.DistributedCache.SetAsync(key, bytes, options);
		}

		public override async Task<AuthenticationTicket> RetrieveAsync(string key)
		{
			this.Logger.LogDebugIfEnabled($"{this.LogPrefix}RetrieveAsync: key = \"{key}\"");

			var bytes = await this.DistributedCache.GetAsync(key);

			var ticket = await this.DeserializeAuthenticationTicketAsync(bytes);

			this.Logger.LogDebugIfEnabled($"{this.LogPrefix}RetrieveAsync: {(ticket == null ? "ticket = null" : $"ticket with identity \"{ticket.Principal?.Identity?.Name}\"")} for key \"{key}\"");

			return ticket;
		}

		protected internal virtual async Task<byte[]> SerializeAuthenticationTicketAsync(AuthenticationTicket ticket)
		{
			if(ticket == null)
				return null;

			return await Task.FromResult(this.AuthenticationTicketSerializer.Serialize(ticket));
		}

		#endregion
	}
}