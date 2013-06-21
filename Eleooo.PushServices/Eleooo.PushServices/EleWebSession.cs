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
        public int UserID { get; set; }
        public int CompanyId { get; set; }
        public SubSystem SubSys { get; set; }
        public LoginSystem LoginSys { get; set; }

        private SysMember _user;
        public SysMember User
        {
            get
            {
                if (_user == null)
                {
                    _user = SysMember.FetchByID(UserID);
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
                    _company = SysCompany.FetchByID(CompanyId);
                }
                return _company;
            }
        }
        public DateTime LastPushDate { get; set; }

        public new string CurrentToken
        {
            get;
            internal set;
        }

        protected override void OnSessionStarted( )
        {
            base.OnSessionStarted( );
        }
    }
}
