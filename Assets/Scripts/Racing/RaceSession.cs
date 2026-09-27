using System.Collections.Generic;
using RallyRacing.Backend;
using RallyRacing.UI;
using RallyRacing.Vehicles;
using UnityEngine;

namespace RallyRacing.Racing
{
    public sealed class RaceSession
    {
        private readonly ArcadeCarController car;
        private readonly IReadOnlyList<CheckpointGate> gates;
        private readonly int totalLaps;
        private readonly RuntimeUI ui;
        private readonly IRallyBackend backend;
        private float countdown = 3f;
        private float elapsed;
        private int expectedGate = 1;
        private int completedLaps;
        private bool started;
        private bool finished;
        public int CurrentLap => Mathf.Min(completedLaps + 1, totalLaps);
        public int TotalLaps => totalLaps;
        public float Elapsed => elapsed;
        public float Countdown => countdown;
        public bool Started => started;
        public bool Finished => finished;
        public ArcadeCarController Car => car;
        public int CurrentCheckpoint => expectedGate;
        public int CheckpointCount => gates.Count;

        public RaceSession(ArcadeCarController controller, IReadOnlyList<CheckpointGate> checkpoints, int laps, RuntimeUI view, IRallyBackend data)
        {
            car = controller; gates = checkpoints; totalLaps = laps; ui = view; backend = data;
            if (gates == null || gates.Count < 2)
            {
                Debug.LogError("Race track needs a finish gate and at least one ordered checkpoint.");
                finished = true;
                return;
            }
            foreach (var gate in gates) gate.Crossed += OnGateCrossed;
            car.SetControlsEnabled(false);
        }

        public void Tick(float deltaTime)
        {
            if (finished) return;
            if (!started)
            {
                countdown -= deltaTime;
                if (countdown <= 0f) { started = true; car.SetControlsEnabled(true); ui.SetCountdown("GO!"); }
                else ui.SetCountdown(Mathf.CeilToInt(countdown).ToString());
                return;
            }
            elapsed += deltaTime;
        }

        private void OnGateCrossed(int index, ArcadeCrossing crossing)
        {
            if (finished || !started || crossing.Car != car) return;
            if (index != expectedGate) return;
            if (index == 0)
            {
                completedLaps++;
                if (completedLaps >= totalLaps) { Finish(); return; }
                expectedGate = 1;
            }
            else expectedGate = index == gates.Count - 1 ? 0 : index + 1;
            ui.RefreshRace(this);
        }

        private async void Finish()
        {
            finished = true; car.SetControlsEnabled(false); ui.ShowFinished();
            await backend.SaveRaceResult(new RaceResult { Position = 1, TimeSeconds = elapsed, Laps = totalLaps });
            await backend.AddXP(150);
            await backend.AddRallyCoins(100);
            ui.ShowResults(elapsed);
        }
    }
}
