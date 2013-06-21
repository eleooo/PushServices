using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket.SubProtocol;
using System.Web.Security;
using Eleooo.Common;

namespace Eleooo.PushServices.SubCommand
{
    public class Login : SubCommandBase<EleWebSession>
    {
        public override void ExecuteCommand(EleWebSession session, SubRequestInfo requestInfo)
        {
            if (string.IsNullOrEmpty(requestInfo.Token))
            {
                if (!string.IsNullOrEmpty(requestInfo.Body))
                {
                    var arr = requestInfo.Body.Split('@');
                    var pwd = arr.Length > 0 ? arr[0] : string.Empty;
                    var name = arr.Length > 1 ? arr[1] : string.Empty;
                    if (string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(name))
                        goto lbl_false;
                    //FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(name,true,
                    var ticket = Utilities.GenFormsAuthenticationTicket(0, SubSystem.Admin, LoginSystem.Web);
                    session.Ticket = ticket;
                    session.CurrentToken = FormsAuthentication.Encrypt(ticket);
                    session.Send(session.CurrentToken);
                    return;
                }
            lbl_false:
                session.Send(CommandResult.GetInstance(-1, requestInfo.Key, "登录失败.").ToString());
                session.Close();
                return;
            }
            try
            {
                var ticket = FormsAuthentication.Decrypt(requestInfo.Token);
                session.Ticket = ticket;
                session.CurrentToken = requestInfo.Token;
            }
            catch (Exception ex)
            {
                session.Send(CommandResult.GetInstance(-1, requestInfo.Key, ex.Message).ToString());
                session.Close();
            }
        }
    }
}
