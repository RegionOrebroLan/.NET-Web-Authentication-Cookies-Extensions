using Microsoft.AspNetCore.Authentication;

namespace RegionOrebroLan.Web.Authentication.Cookies
{
	public class AuthenticationTicketSerializer : IDataSerializer<AuthenticationTicket>
	{
		#region Properties

		protected internal virtual IDataSerializer<AuthenticationTicket> InternalSerializer => TicketSerializer.Default;

		#endregion

		#region Methods

		public virtual AuthenticationTicket Deserialize(byte[] data)
		{
			return this.InternalSerializer.Deserialize(data);
		}

		public virtual byte[] Serialize(AuthenticationTicket model)
		{
			return this.InternalSerializer.Serialize(model);
		}

		#endregion
	}
}