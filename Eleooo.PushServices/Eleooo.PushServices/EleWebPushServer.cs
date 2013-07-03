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
        private SubSonic.StoredProcedure _getOrdersProc;
        SubSonic.StoredProcedure GetOrdersProc
        {
            get
            {
                if (_getOrdersProc == null)
                {
                    _getOrdersProc = SP_.SpGetOrders(0, null, null, null, null, 0, 0, 0);
                }
                return _getOrdersProc;
            }
        }
        public DataTable GetLatestOrderData(EleWebSession session)
        {
            var proc = GetOrdersProc;
            proc.Command.Parameters[0].ParameterValue = session.CompanyId;
            proc.Command.Parameters[4].ParameterValue = session.LastPushDate;
            proc.Command.OutputValues.Clear( );
            return proc.GetDataSet( ).Tables[0];
        }

        private void PushTimerProcess(object state)
        {
            m_pushTimer.Change(Timeout.Infinite, Timeout.Infinite);
            try
            {
                var sessions = this.GetSessions(session => session.CompanyId > 0);
                int status = (int)OrderStatus.InProgress;
                foreach (var session in sessions)
                {
                    //todo:complete push process
                    var dt = GetLatestOrderData(session);
                    if (dt.Rows.Count > 0)
                    {
                        var count = dt.Rows.OfType<DataRow>( ).Count(dr => dr.Field<int>(Order.Columns.OrderStatus) < status);
                        session.LastPushDate = dt.Rows[dt.Rows.Count - 1].Field<DateTime>(Order.Columns.OrderUpdateOn);
                        session.Send(OrderPushResult.GetInstance(dt.Rows.Count, dt, count > 0).ToString( ));
                        Console.WriteLine("成功向商家ID:{0} 推送{1}条订单数据", session.CompanyId,dt.Rows.Count);
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
