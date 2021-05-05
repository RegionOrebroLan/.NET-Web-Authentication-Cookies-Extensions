using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using RegionOrebroLan.Logging.Extensions;

namespace RegionOrebroLan.Web.Authentication.Cookies
{
	public abstract class CacheTicketStore : ITicketStore
	{
		#region Constructors

		protected CacheTicketStore(ILoggerFactory loggerFactory)
		{
			this.KeyPrefix = $"{this.GetType().Name}:";
			this.Logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
		}

		#endregion

		#region Properties

		protected internal virtual string KeyPrefix { get; }
		protected internal virtual ILogger Logger { get; }

		#endregion

		#region Methods

		protected internal virtual async Task<string> CreateKeyAsync()
		{
			return await Task.FromResult(this.KeyPrefix + Guid.NewGuid());
		}

		public abstract Task RemoveAsync(string key);
		public abstract Task RenewAsync(string key, AuthenticationTicket ticket);
		public abstract Task<AuthenticationTicket> RetrieveAsync(string key);

		public virtual async Task<string> StoreAsync(AuthenticationTicket ticket)
		{
			this.Logger.LogDebugIfEnabled($"StoreAsync: authentication-scheme = \"{ticket?.AuthenticationScheme}\", identity-name = \"{ticket?.Principal?.Identity?.Name}\"");

			var key = await this.CreateKeyAsync();
			this.Logger.LogDebugIfEnabled($"StoreAsync: created key = \"{key}\"");

			await this.RenewAsync(key, ticket);

			return key;
		}

		#endregion
	}
}