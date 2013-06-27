using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperSocket.SocketBase;
using Eleooo.PushServices.SubCommand;
using SuperSocket.SocketEngine;

namespace Eleooo.PushServices
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to start the server!");

            Console.ReadKey( );
            Console.WriteLine( );

            var bootstrap = BootstrapFactory.CreateBootstrap( );

            if (!bootstrap.Initialize( ))
            {
                Console.WriteLine("Failed to initialize!");
                Console.ReadKey( );
                return;
            }

            var result = bootstrap.Start( );

            Console.WriteLine("Start result: {0}!", result);
            Console.WriteLine( );
            if (result == StartResult.Failed)
            {
                Console.WriteLine("Failed to start!");
                Console.ReadKey( );
                return;
            }
            //appServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(appServer_NewMessageReceived);

            var appServer = bootstrap.AppServers.First( ) as EleWebPushServer;

            Console.WriteLine("The server started successfully, press key 'q' to stop it!");
            char c;
            while ((c = Console.ReadKey( ).KeyChar) != 'q')
            {
                if (c == 'l')
                {
                    foreach (var s in appServer.GetAllSessions( ))
                    {
                        Console.WriteLine(s.SessionID);
                    }
                }
                else if (c == 'p')
                {
                    Console.WriteLine( );
                    var message = Console.ReadLine( );
                    foreach (var s in appServer.GetAllSessions( ))
                    {
                        Console.WriteLine("send to:{0}", s.SessionID);
                        Notify.SendToUser(s, string.Empty, message);
                        //s.Send(CommandResult.GetInstance(0, "Notify", message).ToString( ));
                    }
                }
                Console.WriteLine( );
                continue;
            }

            //Stop the appServer
            bootstrap.Stop( );

            Console.WriteLine( );
            Console.WriteLine("The server was stopped!");
            Console.ReadKey( );
        }
    }
}
