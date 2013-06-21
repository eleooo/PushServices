using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SuperWebSocket;
using System.Threading;
using Eleooo.DAL;
using Eleooo.Common;

namespace Eleooo.PushServices
{
    public class EleWebPushServer : WebSocketServer<EleWebSession>
    {
        private static readonly int TimerInterval = 5;
        private Timer m_pushTimer;
        public EleWebPushServer( )
        {
        }

        protected override void OnStartup( )
        {
            m_pushTimer = new Timer(PushTimerProcess, null, TimerInterval * 1000, TimerInterval * 1000);
            base.OnStartup( );
        }

        private void PushTimerProcess(object state)
        {
            m_pushTimer.Change(Timeout.Infinite, Timeout.Infinite);
            try
            {
                var sessions = this.GetSessions(session => session.CompanyId > 0);
                foreach (var session in sessions)
                {
                    //todo:complete push process
                    var query = DB.Select(Utilities.GetTableColumns(Order.Schema),
                                          SysMember.Columns.MemberPhoneNumber,
                                         SysMember.Columns.MemberFullname)
                                 .From<Order>( )
                                 .InnerJoin(SysMember.IdColumn, Order.OrderMemberIDColumn)
                                 .Where(Order.OrderSellerIDColumn).IsEqualTo(session.CompanyId)
                                 .And(Order.OrderUpdateOnColumn).IsGreaterThan(session.LastPushDate)
                                 .OrderDesc(Order.OrderUpdateOnColumn.QualifiedName);
                    var dt = query.ExecuteDataTable( );
                    if (dt.Rows.Count > 0)
                    {
                        session.LastPushDate = dt.Rows[0].Field<DateTime>(Order.Columns.OrderUpdateOn);
                        session.Send(OrderPushResult.GetInstance(dt.Rows.Count, dt, true).ToString( ));
                    }
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
