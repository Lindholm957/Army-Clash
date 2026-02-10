using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArmyClash.Battle
{
    public class BattleGameController : MonoBehaviour
    {
        [Header("UI References")]
        public Canvas uiCanvas;
        public TMP_Text titleText;
        public TMP_Text leftPreviewText;
        public TMP_Text rightPreviewText;
        public TMP_Text battleHudText;
        public TMP_Text resultText;

        [Header("Button References")]
        public Button randomizeButton;
        public Button startButton;
        public Button backButton;
        public Button speedButton;

        [Header("Scene References")]
        public GameObject ground;

        [Header("Data")]
        public StatConfiguration statConfiguration;

        private readonly List<GameObject> _spawned = new();
        private readonly List<GameObject> _battleUnits = new();

        private List<UnitBlueprint> _leftArmyDraft;
        private List<UnitBlueprint> _rightArmyDraft;

        private BattleArmy _leftArmy;
        private BattleArmy _rightArmy;

        private enum GameState
        {
            Menu,
            Simulation,
            Result
        }

        private GameState _state;
        private int _speedModeIndex;
        private readonly float[] _speedModes = { 1f, 2f, 4f };

        private void Start()
        {
            ValidateReferences();
            EnterMenu();
        }

        private void ValidateReferences()
        {
            if (uiCanvas == null) Debug.LogError("UI Canvas is not assigned!");
            if (titleText == null) Debug.LogError("Title Text is not assigned!");
            if (leftPreviewText == null) Debug.LogError("Left Preview Text is not assigned!");
            if (rightPreviewText == null) Debug.LogError("Right Preview Text is not assigned!");
            if (battleHudText == null) Debug.LogError("Battle HUD Text is not assigned!");
            if (resultText == null) Debug.LogError("Result Text is not assigned!");
            if (randomizeButton == null) Debug.LogError("Randomize Button is not assigned!");
            if (startButton == null) Debug.LogError("Start Button is not assigned!");
            if (backButton == null) Debug.LogError("Back Button is not assigned!");
            if (speedButton == null) Debug.LogError("Speed Button is not assigned!");
            if (ground == null) Debug.LogError("Ground is not assigned!");
            if (statConfiguration == null) Debug.LogError("Stat Configuration is not assigned!");

            UnitStatCalculator.Initialize(statConfiguration);

            randomizeButton.onClick.AddListener(OnRandomizeClicked);
            startButton.onClick.AddListener(OnStartClicked);
            backButton.onClick.AddListener(EnterMenu);
            speedButton.onClick.AddListener(ToggleSpeedMode);
        }

        private void Update()
        {
            if (_state != GameState.Simulation)
            {
                return;
            }

            battleHudText.text = $"Left: {_leftArmy.AliveCount} | Right: {_rightArmy.AliveCount}";

            if (_leftArmy.AliveCount == 0 || _rightArmy.AliveCount == 0)
            {
                EnterResult(_leftArmy.AliveCount > 0 ? "LEFT ARMY WINS" : "RIGHT ARMY WINS");
            }
        }

        private void EnterMenu()
        {
            Time.timeScale = 1f;
            _speedModeIndex = 0;
            CleanupBattleUnits();

            _state = GameState.Menu;
            titleText.text = "Army Clash Prototype";
            resultText.text = string.Empty;
            battleHudText.text = string.Empty;

            titleText.gameObject.SetActive(true);
            leftPreviewText.gameObject.SetActive(true);
            rightPreviewText.gameObject.SetActive(true);
            randomizeButton.gameObject.SetActive(true);
            startButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(false);
            speedButton.gameObject.SetActive(false);

            OnRandomizeClicked();
        }

        private void OnRandomizeClicked()
        {
            var unitDatabase = new UnitDatabase();
            _leftArmyDraft = unitDatabase.GetRandomArmyBlueprints();
            _rightArmyDraft = unitDatabase.GetRandomArmyBlueprints();

            leftPreviewText.text = $"LEFT\n{ArmyBuilder.BuildSummary(_leftArmyDraft)}";
            rightPreviewText.text = $"RIGHT\n{ArmyBuilder.BuildSummary(_rightArmyDraft)}";
        }

        private void OnStartClicked()
        {
            _state = GameState.Simulation;
            titleText.gameObject.SetActive(false);
            leftPreviewText.gameObject.SetActive(false);
            rightPreviewText.gameObject.SetActive(false);
            randomizeButton.gameObject.SetActive(false);
            startButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(false);
            speedButton.gameObject.SetActive(true);

            SpawnBattle();
        }

        private void SpawnBattle()
        {
            CleanupBattleUnits();

            _leftArmy = new BattleArmy("Left");
            _rightArmy = new BattleArmy("Right");

            SpawnArmy(_leftArmyDraft, _leftArmy, _rightArmy, new Vector3(-9f, 0.6f, 0f), 1);
            SpawnArmy(_rightArmyDraft, _rightArmy, _leftArmy, new Vector3(9f, 0.6f, 0f), -1);
        }

        private void SpawnArmy(
            IReadOnlyList<UnitBlueprint> blueprints,
            BattleArmy myArmy,
            BattleArmy enemyArmy,
            Vector3 center,
            int lookDirection)
        {
            const int columns = 5;
            const float spacing = 2.2f;

            for (var i = 0; i < blueprints.Count; i++)
            {
                var row = i / columns;
                var col = i % columns;
                var x = center.x + (col - 2) * spacing * 0.8f;
                var z = center.z + (row - 2) * spacing;

                var unitObject = ArmyBuilder.CreateUnitObject(blueprints[i]);
                unitObject.transform.position = new Vector3(x, 0.5f, z);
                unitObject.transform.rotation = Quaternion.Euler(0f, lookDirection == 1 ? 90f : -90f, 0f);

                var agent = unitObject.AddComponent<UnitAgent>();
                var stats = UnitStatCalculator.Calculate(blueprints[i]);
                agent.Initialize(blueprints[i], stats, myArmy, enemyArmy);

                myArmy.Add(agent);
                _battleUnits.Add(unitObject);
            }
        }

        private void EnterResult(string winnerText)
        {
            _state = GameState.Result;
            resultText.text = winnerText;
            backButton.gameObject.SetActive(true);
            speedButton.gameObject.SetActive(false);
        }

        private void ToggleSpeedMode()
        {
            _speedModeIndex = (_speedModeIndex + 1) % _speedModes.Length;
            Time.timeScale = _speedModes[_speedModeIndex];
            speedButton.GetComponentInChildren<TMP_Text>().text = $"x{_speedModes[_speedModeIndex]}";
        }

        private void CleanupBattleUnits()
        {
            for (var i = _battleUnits.Count - 1; i >= 0; i--)
            {
                if (_battleUnits[i] != null)
                {
                    Destroy(_battleUnits[i]);
                }
                _battleUnits.RemoveAt(i);
            }
        }
    }
}
