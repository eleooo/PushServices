using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket.SubProtocol;
using Newtonsoft.Json;
using SuperWebSocket;

namespace Eleooo.PushServices.SubCommand
{
    public class Notify : SubCommandBase<EleWebSession>
    {
        private static readonly string _Command = "Notify";
        class NotifyDTO
        {
            public string toUser { get; set; }
            public string message { get; set; }
            public string title { get; set; }

        }
        public override void ExecuteCommand(EleWebSession session, SubRequestInfo requestInfo)
        {
            //session.AppServer.GetAppSessionByID(
            if (!string.IsNullOrEmpty(requestInfo.Body))
            {
                var dto = JsonConvert.DeserializeObject<NotifyDTO>(requestInfo.Body);
                if (!string.IsNullOrEmpty(dto.toUser) && !string.IsNullOrEmpty(dto.message))
                {
                    foreach (var s in session.AppServer.GetAllSessions())
                        SendToUser(s, dto);
                }
                else if (!string.IsNullOrEmpty(dto.message))
                    SendToUser(session.AppServer.GetAppSessionByID(dto.toUser), dto);

            }
        }
        private static void SendToUser(EleWebSession session, NotifyDTO dto)
        {
            dto.toUser = null;
            session.Send(CommandResult.GetInstance(0, _Command, null, dto).ToString());
        }
        public static void SendToUser(EleWebSession session, string title, string message)
        {
            SendToUser(session, new NotifyDTO { message = message, title = title });
        }
    }
}
