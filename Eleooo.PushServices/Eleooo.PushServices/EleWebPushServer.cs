using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperWebSocket;
using System.Threading;

namespace Eleooo.PushServices
{
    public class EleWebPushServer : WebSocketServer<EleWebSession>
    {
        private static readonly int TimerInterval = 5;
        private Timer m_pushTimer;
        public EleWebPushServer()
        {
        }

        protected override void OnStartup()
        {
            m_pushTimer = new Timer(PushTimerProcess, null, TimerInterval * 1000, TimerInterval * 1000);
            base.OnStartup();
        }

        private void PushTimerProcess(object state)
        {
            m_pushTimer.Change(Timeout.Infinite, Timeout.Infinite);
            try
            {

                var sessions = this.GetSessions(session => session.Company != null)
                                   .GroupBy(session => session.Company.Id);
                foreach (var companys in sessions)
                {
                    //todo:complete push process
                }

            }
            catch (Exception ex)
            {
                if (Logger.IsErrorEnabled)
                    Logger.Error(ex);
            }
            finally
            {
                m_pushTimer.Change(TimerInterval * 1000, TimerInterval * 1000);
            }
        }
    }
}
