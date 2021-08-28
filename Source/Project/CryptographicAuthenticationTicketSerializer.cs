using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;

namespace RegionOrebroLan.Web.Authentication.Cookies
{
	public class CryptographicAuthenticationTicketSerializer : AuthenticationTicketSerializer
	{
		#region Constructors

		public CryptographicAuthenticationTicketSerializer(IDataProtectionProvider dataProtectionProvider)
		{
			this.DataProtector = (dataProtectionProvider ?? throw new ArgumentNullException(nameof(dataProtectionProvider))).CreateProtector(this.GetType().FullName);
		}

		#endregion

		#region Properties

		protected internal virtual IDataProtector DataProtector { get; }

		#endregion

		#region Methods

		public override AuthenticationTicket Deserialize(byte[] data)
		{
			return base.Deserialize(this.DataProtector.Unprotect(data));
		}

		public override byte[] Serialize(AuthenticationTicket model)
		{
			return this.DataProtector.Protect(base.Serialize(model));
		}

		#endregion
	}
}