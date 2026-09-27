using System.Collections.Generic;
using RallyRacing.Backend;
using RallyRacing.Data;
using RallyRacing.Racing;
using RallyRacing.UI;
using RallyRacing.Vehicles;
using UnityEngine;

namespace RallyRacing.Core
{
    /// <summary>Builds the runtime composition root and owns the prototype screen flow.</summary>
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private int lapsToWin = 3;
        private MockRallyBackend backend;
        private RallyRacing.UI.RuntimeUI ui;
        private ArcadeCarController car;
        private RaceSession race;
        private List<CarDefinition> cars;
        private int selectedCar;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Application.targetFrameRate = 60;
            backend = new MockRallyBackend();
            cars = CarDefinition.CreateDefaults();
            ui = new RallyRacing.UI.RuntimeUI();
            ui.Initialize(this);
            ui.ShowMainMenu();
        }

        private void Update() => ui?.Tick(Time.deltaTime);

        public IReadOnlyList<CarDefinition> Cars => cars;
        public int SelectedCar => selectedCar;
        public void SelectCar(int index) => selectedCar = Mathf.Clamp(index, 0, cars.Count - 1);
        public void OpenGarage() => ui.ShowGarage();
        public void OpenLobby() => ui.ShowLobby();
        public void BackToMenu() => ui.ShowMainMenu();
        public void StartRace() => BeginRace();

        private void BeginRace()
        {
            ui.Clear();
            if (car != null) Destroy(car.gameObject);
            var track = DemoTrackBuilder.Build();
            car = track.SpawnCar(cars[selectedCar]);
            race = new RaceSession(car, track.Gates, Mathf.Max(1, lapsToWin), ui, backend);
            ui.ShowRace(race);
        }

        public void RestartRace() => BeginRace();
        public void ExitGame() => Application.Quit();
    }
}
