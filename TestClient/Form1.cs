using GameEmulator.Grains.Interfaces;
using System.Net.Http.Json;

namespace TestClient
{
    public partial class Form1 : Form
    {
        private bool _inProcess;

        private readonly HttpClient _httpClient;
        private readonly List<Func<int, GameEvent>> _gameEventGenerators;
        private readonly List<Task> _tasks = new List<Task>();

        public Form1()
        {
            InitializeComponent();
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(@"http://localhost:5265")
            };


            _gameEventGenerators = new List<Func<int, GameEvent>>()
            {
                playerId => new CreateAObject(playerId, Random.Shared.Next()),
                playerId => new CreateBObject(playerId, Random.Shared.Next().ToString()),
                playerId => new RemoveObject(playerId)
            };
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            _inProcess = true;

            for (int i = 0; i < NumberOfClientsControl.Value; i++)
            {
                var task = Task.Run(async () =>
                {
                    var playerId = Random.Shared.Next();
                    while (_inProcess)
                    {
                        var index = Random.Shared.Next(3);
                        var gameEvent = _gameEventGenerators[index](playerId);

                        var str = System.Text.Json.JsonSerializer.Serialize(gameEvent, gameEvent.GetType());

                        var response = await _httpClient.PostAsJsonAsync("api/gamesession", gameEvent);

                        await Task.Delay(1000);
                    }

                    Console.WriteLine("finished");
                });

                _tasks.Add(task);
            }
            
            UpdateButtonsState();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            _inProcess = false;
            Task.WhenAll(_tasks).Wait();
            _tasks.Clear();
            UpdateButtonsState();
        }

        private void UpdateButtonsState()
        {
            StartButton.Enabled = !_inProcess;
            StopButton.Enabled = _inProcess;
        }
    }
}