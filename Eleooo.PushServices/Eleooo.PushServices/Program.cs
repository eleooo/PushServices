using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;

namespace Eleooo.PushServices
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start the eleooo push services!");

            Console.ReadKey();
            Console.WriteLine();

            var appServer = new EleWebPushServer();

            //Setup the appServer
            if (!appServer.Setup(8080)) //Setup with listening port
            {
                Console.WriteLine("Failed to setup!");
                Console.ReadKey();
                return;
            }

            //appServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(appServer_NewMessageReceived);

            Console.WriteLine();

            //Try to start the appServer
            if (!appServer.Start())
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");
            char c;
            while ((c = Console.ReadKey().KeyChar) != 'q')
            {
                if (c == 'c')
                {
                    foreach (var s in appServer.GetAllSessions())
                    {
                        Console.WriteLine(s.SessionID);
                        s.CloseWithHandshake("your no permission use this services.");
                    }
                }
                else if (c == 's')
                {
                    var message = Console.ReadLine();
                    foreach (var s in appServer.GetAllSessions())
                    {
                        Console.WriteLine(s.SessionID);
                        s.Send(message);
                    }
                }
                Console.WriteLine();
                continue;
            }

            //Stop the appServer
            appServer.Stop();

            Console.WriteLine();
            Console.WriteLine("The server was stopped!");
            Console.ReadKey();
        }
    }
}
