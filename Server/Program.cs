using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsClient;
using Server;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Globals.Delay = 300;
            string url = "http://*:8080";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }

    /// <summary>
    /// Used by OWIN's startup process. 
    /// </summary>
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();

            RandomGenerator randomGenerator = new RandomGenerator(Globals.Delay);
            Task.Factory.StartNew(async () => await randomGenerator.OnRandomMonitor());
        }
    }
    /// <summary>
    /// Echoes messages sent using the Send message by calling the
    /// addMessage method on the client. Also reports to the console
    /// when clients connect and disconnect.
    /// </summary>
    public class MyHub : Hub
    {
        public void SendStatus(bool isPaused)
        {
            //Clients.All.addMessage(isPaused);
            if (isPaused)
            {
                Console.WriteLine("Client {0} paused the flow", Context.ConnectionId);
            }               
            else
            {
                Console.WriteLine("Client {0} resumed the flow", Context.ConnectionId);
            }
        }

        public override Task OnConnected()
        {
            Console.WriteLine("Client connected: " + Context.ConnectionId);
            Clients.All.setDelay(Globals.Delay);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine("Client disconnected: " + Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}

