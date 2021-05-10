using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace RegionOrebroLan.Web.Authentication.Cookies.DependencyInjection.Configuration
{
	public abstract class TicketStoreOptions
	{
		#region Properties

		public virtual IConfigurationSection Options { get; set; }

		#endregion

		#region Methods

		public virtual void Add(ITicketStoreBuilder builder)
		{
			try
			{
				if(builder == null)
					throw new ArgumentNullException(nameof(builder));

				this.AddInternal(builder);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not add ticket-store with options of type \"{this.GetType()}\".", exception);
			}
		}

		protected internal abstract void AddInternal(ITicketStoreBuilder builder);

		public virtual void Use(IApplicationBuilder builder)
		{
			try
			{
				this.UseInternal(builder);
			}
			catch(Exception exception)
			{
				throw new InvalidOperationException($"Could not use ticket-store with options of type \"{this.GetType()}\".", exception);
			}
		}

		protected internal virtual void UseInternal(IApplicationBuilder builder) { }

		#endregion
	}
}