using RallyRacing.Core;
using RallyRacing.Racing;
using UnityEngine;
using UnityEngine.UI;

namespace RallyRacing.UI
{
    /// <summary>Small runtime-generated prototype UI. Replace presentation incrementally without changing race logic.</summary>
    public sealed class RuntimeUI
    {
        private readonly Color dark = new Color(0.035f, 0.055f, 0.075f, 0.96f);
        private readonly Color accent = new Color(0.22f, 0.95f, 0.56f);
        private GameObject canvasObject;
        private Canvas canvas;
        private GameBootstrap bootstrap;
        private RaceSession session;
        private Text title;
        private Text details;
        private Text countdown;
        private Text speed;

        public void Initialize(GameBootstrap owner) { bootstrap = owner; }
        public void Clear()
        {
            if (canvasObject != null) Object.Destroy(canvasObject);
            canvasObject = null; session = null;
        }

        public void ShowMainMenu()
        {
            CreateCanvas("Main Menu");
            AddText("RALLY", new Vector2(0.5f, 0.78f), 72, accent, TextAnchor.MiddleCenter);
            AddText("RACING", new Vector2(0.5f, 0.69f), 34, Color.white, TextAnchor.MiddleCenter);
            AddButton("PLAY", new Vector2(0.5f, 0.51f), () => bootstrap.OpenLobby());
            AddButton("GARAGE", new Vector2(0.5f, 0.40f), () => bootstrap.OpenGarage());
            AddButton("QUIT", new Vector2(0.5f, 0.29f), () => bootstrap.ExitGame());
            AddText("ARCADE CIRCUIT  /  OFFLINE PROTOTYPE", new Vector2(0.5f, 0.12f), 16, Color.gray, TextAnchor.MiddleCenter);
        }

        public void ShowGarage()
        {
            CreateCanvas("Garage");
            AddText("SELECT YOUR CAR", new Vector2(0.5f, 0.82f), 38, Color.white, TextAnchor.MiddleCenter);
            for (int i = 0; i < bootstrap.Cars.Count; i++)
            {
                int selected = i;
                var car = bootstrap.Cars[i];
                AddButton(car.displayName.ToUpper() + "\nTOP SPEED  " + Mathf.RoundToInt(car.topSpeed * 3.6f) + "\nGRIP  " + car.grip.ToString("0.0"), new Vector2(0.5f, 0.65f - i * 0.17f), () =>
                {
                    bootstrap.SelectCar(selected);
                    ShowGarage();
                });
            }
            AddButton("CONFIRM / LOBBY", new Vector2(0.5f, 0.12f), () => bootstrap.OpenLobby());
            AddButton("BACK", new Vector2(0.12f, 0.1f), () => bootstrap.BackToMenu());
        }

        public void ShowLobby()
        {
            CreateCanvas("Lobby");
            AddText("COASTAL CIRCUIT", new Vector2(0.5f, 0.72f), 42, Color.white, TextAnchor.MiddleCenter);
            AddText("LOCAL TEST LOBBY  •  1 / 8 PLAYERS", new Vector2(0.5f, 0.61f), 20, accent, TextAnchor.MiddleCenter);
            AddText("Three laps  /  ordered checkpoints", new Vector2(0.5f, 0.52f), 18, Color.gray, TextAnchor.MiddleCenter);
            AddButton("START SOLO RACE", new Vector2(0.5f, 0.37f), () => bootstrap.StartRace());
            AddButton("BACK", new Vector2(0.5f, 0.23f), () => bootstrap.BackToMenu());
        }

        public void ShowRace(RaceSession race)
        {
            session = race;
            CreateCanvas("Race HUD");
            title = AddText("POSITION 1 / 1", new Vector2(0.13f, 0.92f), 20, accent, TextAnchor.MiddleLeft);
            details = AddText("LAP 1 / 3\n00:00.00\nCHECKPOINT 1 / 5", new Vector2(0.13f, 0.83f), 22, Color.white, TextAnchor.UpperLeft);
            speed = AddText("000", new Vector2(0.87f, 0.12f), 48, Color.white, TextAnchor.MiddleRight);
            AddText("KM/H", new Vector2(0.88f, 0.07f), 14, accent, TextAnchor.MiddleRight);
            countdown = AddText("3", new Vector2(0.5f, 0.58f), 96, accent, TextAnchor.MiddleCenter);
            AddButton("MENU", new Vector2(0.91f, 0.93f), () => bootstrap.BackToMenu());
        }

        public void Tick(float deltaTime)
        {
            if (session == null) return;
            session.Tick(deltaTime);
            RefreshRace(session);
            if (speed != null) speed.text = Mathf.RoundToInt(session.Car.SpeedKph).ToString("000");
        }

        public void RefreshRace(RaceSession race)
        {
            if (details != null) details.text = "LAP " + race.CurrentLap + " / " + race.TotalLaps + "\n" + FormatTime(race.Elapsed) + "\nCHECKPOINT " + race.CurrentCheckpoint + " / " + (race.CheckpointCount - 1);
        }
        public void SetCountdown(string value) { if (countdown != null) countdown.text = value; }
        public void ShowFinished() { SetCountdown("FINISH!"); }
        public void ShowResults(float time)
        {
            Clear(); CreateCanvas("Results");
            AddText("RACE RESULTS", new Vector2(0.5f, 0.72f), 46, accent, TextAnchor.MiddleCenter);
            AddText("1ST  /  RALLY DRIVER\n" + FormatTime(time) + "\n+150 XP     +100 RALLY COINS", new Vector2(0.5f, 0.52f), 26, Color.white, TextAnchor.MiddleCenter);
            AddButton("RACE AGAIN", new Vector2(0.5f, 0.31f), () => bootstrap.RestartRace());
            AddButton("MAIN MENU", new Vector2(0.5f, 0.19f), () => bootstrap.BackToMenu());
        }

        private void CreateCanvas(string objectName)
        {
            Clear(); canvasObject = new GameObject(objectName); Object.DontDestroyOnLoad(canvasObject);
            canvas = canvasObject.AddComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
            canvasObject.AddComponent<GraphicRaycaster>();
            if (Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
            {
                var events = new GameObject("EventSystem"); Object.DontDestroyOnLoad(events);
                events.AddComponent<UnityEngine.EventSystems.EventSystem>(); events.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
            }
        }

        private Text AddText(string value, Vector2 anchor, int fontSize, Color color, TextAnchor alignment)
        {
            var textObject = new GameObject("Text"); textObject.transform.SetParent(canvas.transform, false);
            var rect = textObject.AddComponent<RectTransform>(); rect.anchorMin = anchor; rect.anchorMax = anchor; rect.pivot = new Vector2(0.5f, 0.5f); rect.sizeDelta = new Vector2(700, 180);
            var text = textObject.AddComponent<Text>(); text.text = value; text.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); text.fontSize = fontSize; text.color = color; text.alignment = alignment; text.horizontalOverflow = HorizontalWrapMode.Overflow; text.verticalOverflow = VerticalWrapMode.Overflow;
            return text;
        }

        private void AddButton(string label, Vector2 anchor, UnityEngine.Events.UnityAction action)
        {
            var buttonObject = new GameObject("Button"); buttonObject.transform.SetParent(canvas.transform, false);
            var rect = buttonObject.AddComponent<RectTransform>(); rect.anchorMin = anchor; rect.anchorMax = anchor; rect.pivot = new Vector2(0.5f, 0.5f); rect.sizeDelta = new Vector2(340, 72);
            var image = buttonObject.AddComponent<Image>(); image.color = dark;
            var button = buttonObject.AddComponent<Button>(); button.targetGraphic = image; button.onClick.AddListener(action);
            var labelText = AddText(label, anchor, 19, Color.white, TextAnchor.MiddleCenter); labelText.transform.SetParent(buttonObject.transform, false);
            var labelRect = labelText.rectTransform; labelRect.anchorMin = Vector2.zero; labelRect.anchorMax = Vector2.one; labelRect.offsetMin = new Vector2(10, 6); labelRect.offsetMax = new Vector2(-10, -6);
        }

        private static string FormatTime(float time) => string.Format("{0:00}:{1:00}.{2:00}", Mathf.FloorToInt(time / 60f), Mathf.FloorToInt(time % 60f), Mathf.FloorToInt(time * 100f % 100f));
    }
}
