namespace RegionOrebroLan.Web.Authentication.Cookies.DependencyInjection.Configuration
{
	/// <summary>
	/// Options used when no type is configured.
	/// </summary>
	public class EmptyOptions : TicketStoreOptions
	{
		#region Methods

		protected internal override void AddInternal(ITicketStoreBuilder builder)
		{
			// Do nothing.
		}

		#endregion
	}
}