using System;
using System.Collections.Generic;

namespace RegionOrebroLan.Web.Authentication.Cookies.Configuration
{
	public class TicketStoreOptions
	{
		#region Properties

		public virtual ISet<string> CookieAuthenticationSchemes { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		public virtual string Type { get; set; }

		#endregion
	}
}