using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Owin.Hosting;
using System.Timers;
using Microsoft.AspNet.SignalR.Client;
using ChatApp.Application.Service;

namespace ChatApp.Infra.WindowsService.Bot
{
    public partial class Broadcast : ServiceBase
    {
        #region Fields

        const string url = @"http://localhost:8080/";

        private readonly RabbitMQService _rabbitMQService;
        private readonly StockService _stockService;

        private Timer _timer;

        #endregion

        #region Constructors
        
        public Broadcast()
        {
            _rabbitMQService = new RabbitMQService();
            _stockService = new StockService(new RequestService());

            InitializeComponent();
            AutoLog = false;
        }

        #endregion

        #region Methods

        protected override void OnStart(string[] args)
        {
            if (!EventLog.SourceExists("SignalRServer"))
            {
                EventLog.CreateEventSource("SignalRServer", "Application");
            }

            EventLogService.Source = "SignalRServer";
            EventLogService.Log = "Application";
            EventLogService.WriteEntry("SignalRServer Started");

            InitializeSelfHosting();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var connection = new HubConnection(url);
            var hub = connection.CreateHubProxy("BroadcastHub");
            connection.Start().Wait();

            var con = _rabbitMQService.GetConnection();
            var message = _rabbitMQService.receive(con, "all");

            if (!string.IsNullOrEmpty(message) && message.Contains("/"))
            {
                var quote = _stockService.GetQuote(message);
                hub.Invoke("SendMessageToAll", quote.UserName, quote.Message).Wait();
            }
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Dispose();
            EventLogService.WriteEntry("SignalRServer Stoped");
        }

        public void InitializeSelfHosting()
        {
            WebApp.Start(url);

            _timer = new Timer(1000) { AutoReset = true };
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        #endregion
    }
}
