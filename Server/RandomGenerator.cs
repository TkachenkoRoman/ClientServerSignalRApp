using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Server;


namespace WindowsFormsClient
{
    class RandomGenerator
    {
        private IHubContext _hubs;
        private readonly int _pollIntervalMillis;
        static Random _numberRand;

        public RandomGenerator(int pollIntervalMillis)
        {
            //HostingEnvironment.RegisterObject(this);
            _hubs = GlobalHost.ConnectionManager.GetHubContext<MyHub>();
            _pollIntervalMillis = pollIntervalMillis;
            _numberRand = new Random();
        }

        public async Task OnRandomMonitor()
        {
            while (true)
            {
                await Task.Delay(_pollIntervalMillis);
                int number = _numberRand.Next(0, 100);

                _hubs.Clients.All.broadcastData(number, DateTime.UtcNow.ToString());
            }
        }
    }
}
