using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Eleooo.PushServices.SubCommand
{
    public class CommandResult
    {
        public int code { get; set; }
        public string message { get; set; }
        public string cmd { get; set; }
        public object data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static CommandResult GetInstance(int code, string cmd, string message = null, object data = null)
        {
            return new CommandResult
            {
                code = code,
                cmd = cmd,
                message = message,
                data = data
            };
        }
    }
}
