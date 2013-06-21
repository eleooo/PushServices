using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket.SubProtocol;

namespace Eleooo.PushServices.SubCommand
{
    public class Log : SubCommandBase<EleWebSession>
    {
        public override void ExecuteCommand(EleWebSession session, SubRequestInfo requestInfo)
        {
            if (!string.IsNullOrEmpty(requestInfo.Body))
            {
                session.AppServer.Logger.Info(requestInfo.Body);
                Console.WriteLine(requestInfo.Body);
            }
        }
    }
}
