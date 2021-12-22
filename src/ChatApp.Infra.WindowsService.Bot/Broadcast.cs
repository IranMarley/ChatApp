using System.Diagnostics;
using System.ServiceProcess;
using Microsoft.Owin.Hosting;
using System.Timers;
using Microsoft.AspNet.SignalR.Client;
using ChatApp.Application.Service;
using RabbitMQ.Client.Events;
using System.Text;
using RabbitMQ.Client;
using Newtonsoft.Json;
using ChatApp.Application.ViewModels;


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

            #region Receive messages

            var connection = new HubConnection(url);
            var hub = connection.CreateHubProxy("BroadcastHub");
            connection.Start().Wait();

            var con = _rabbitMQService.GetConnection();

            var channel = con.CreateModel();

            channel.QueueDeclare(queue: "all",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var msg = JsonConvert.DeserializeObject<MessageDetailViewModel>(Encoding.UTF8.GetString(body));

                //hub.Invoke("SendMessageToAll", msg.UserName, msg.Message, msg.Date).Wait();

                if (!string.IsNullOrEmpty(msg.Message) && msg.Message.Contains("/"))
                {
                    var quote = _stockService.GetQuote(msg.Message);
                    //_rabbitMQService.send(con, quote, "all");
                    hub.Invoke("SendMessageToAll", quote.UserName, quote.Message, quote.Date).Wait();
                }
            };

            channel.BasicConsume(queue: "all",
                                 autoAck: true,
                                 consumer: consumer);


            #endregion

            _timer = new Timer(1000 * 360) { AutoReset = true };
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        #endregion
    }
}
