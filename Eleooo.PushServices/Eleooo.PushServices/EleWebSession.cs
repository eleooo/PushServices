using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket;
using Eleooo.DAL;
using Eleooo.Common;
using System.Web.Security;

namespace Eleooo.PushServices
{
    public class EleWebSession : WebSocketSession<EleWebSession>
    {
        internal FormsAuthenticationTicket Ticket { get; set; }

        private SysMember _user;
        public SysMember User
        {
            get
            {
                if (_user == null)
                {
                    _user = Ticket != null ? SysMember.FetchByID(Utilities.ToInt(Ticket.Name)) : _user;
                }
                return _user;
            }
        }
        private SysCompany _company;
        public SysCompany Company
        {
            get
            {
                if (_company == null)
                {
                    _company = User != null ? SysCompany.FetchByID(User.CompanyId) : _company;
                }
                return _company;
            }
        }

        public new string CurrentToken
        {
            get;
            internal set;
        }

        protected override void OnSessionStarted()
        {
            base.OnSessionStarted();
        }
    }
}
