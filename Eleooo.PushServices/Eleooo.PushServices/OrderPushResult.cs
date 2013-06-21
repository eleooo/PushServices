using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Eleooo.PushServices
{
    public class OrderPushResult
    {
        private static readonly string _OrderPushCommand = "Order";
        public string cmd { get { return _OrderPushCommand; } }
        public bool hasNew { get; set; }
        public object data { get; set; }
        public int count { get; set; }
        public override string ToString( )
        {
            return JsonConvert.SerializeObject(this);
        }
        public static OrderPushResult GetInstance(int count, object data, bool hasNew = true)
        {
            return new OrderPushResult
            {
                count = count,
                data = data,
                hasNew = hasNew
            };
        }
    }
}
