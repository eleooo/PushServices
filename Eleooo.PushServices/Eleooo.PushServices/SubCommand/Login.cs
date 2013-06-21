using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket.SubProtocol;
using System.Web.Security;
using Eleooo.Common;
using Newtonsoft.Json;

namespace Eleooo.PushServices.SubCommand
{

    public class Login : SubCommandBase<EleWebSession>
    {
        private class LoginData
        {
            public DateTime Date { get; set; }
            public int UserId { get; set; }
            public int CompanyId { get; set; }
            public SubSystem SubSys { get; set; }
            public LoginSystem LoginSys { get; set; }
        }
        public override void ExecuteCommand(EleWebSession session, SubRequestInfo requestInfo)
        {
            if (string.IsNullOrEmpty(requestInfo.Token))
            {
                session.Send(CommandResult.GetInstance(-1, requestInfo.Key, "登录失败.").ToString());
                session.Close();
                return;
            }
            try
            {
                session.CurrentToken = requestInfo.Token;
                var data = JsonConvert.DeserializeObject<LoginData>(requestInfo.Body);
                session.LastPushDate = data.Date;
                session.UserID = data.UserId;
                session.CompanyId = data.CompanyId;
                session.SubSys = data.SubSys;
                session.LoginSys = data.LoginSys;
            }
            catch (Exception ex)
            {
                session.Send(CommandResult.GetInstance(-1, requestInfo.Key, ex.Message).ToString());
                session.Close();
            }
        }
    }
}
