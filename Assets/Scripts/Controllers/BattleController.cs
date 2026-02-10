using UnityEngine;
using ArmyClash.Events;
using ArmyClash.Battle;
using ArmyClash.Models;
using ArmyClash.Services;
using ArmyClash.Views;

namespace ArmyClash.Controllers
{
    public class BattleController : MonoBehaviour, IEventHandler<UnitDiedEvent>, IEventHandler<ArmyCountChangedEvent>
    {
        [Header("Configuration")]
        public StatConfiguration statConfiguration;
        
        [Header("UI Components - View")]
        public BattleView battleView;
        
        [Header("Scene Components")]
        public GameObject ground;

        private BattleModel _model;
        private ArmySpawner _spawner;

        private void Start()
        {
            ValidateReferences();
            InitializeGame();
        }

        private void ValidateReferences()
        {
            if (statConfiguration == null) Debug.LogError("Конфигурация характеристик не назначена!");
            if (battleView == null) Debug.LogError("Battle View не назначен!");
            if (ground == null) Debug.LogError("Ground не назначен!");
        }

        private void InitializeGame()
        {
            UnitStatCalculator.Initialize(statConfiguration);
            
            _model = new BattleModel();
            _spawner = new ArmySpawner();
            
            EventBus.Subscribe<UnitDiedEvent>(this);
            EventBus.Subscribe<ArmyCountChangedEvent>(this);
            
            battleView.Initialize(this);
            
            _model.SetState(GameState.Menu);
            battleView.ShowMenu();
            
            OnRandomizeClicked();
        }

        private void Update()
        {
            if (_model.State == GameState.Simulation)
            {
                UpdateSimulation();
            }
        }

        private void UpdateSimulation()
        {
            if (_model.LeftArmy != null && _model.RightArmy != null)
            {
                battleView.UpdateBattleHUD(_model.LeftArmy.AliveCount, _model.RightArmy.AliveCount);
                
                if (_model.LeftArmy.AliveCount == 0 || _model.RightArmy.AliveCount == 0)
                {
                    var winner = _model.LeftArmy.AliveCount > 0 ? "LEFT ARMY WINS" : "RIGHT ARMY WINS";
                    _model.SetState(GameState.Result);
                    battleView.ShowResult(winner);
                }
            }
        }

        #region Public Methods
        public void OnRandomizeClicked()
        {
            var unitDatabase = new UnitDatabase();
            _model.LeftArmyDraft = unitDatabase.GetRandomArmyBlueprints();
            _model.RightArmyDraft = unitDatabase.GetRandomArmyBlueprints();

            battleView.UpdateArmyPreviews(_model.LeftArmyDraft, _model.RightArmyDraft);
        }

        public void OnStartClicked()
        {
            if (_model.LeftArmyDraft == null || _model.RightArmyDraft == null)
            {
                Debug.LogWarning("Составы армий пусты, генерирую новые...");
                OnRandomizeClicked();
            }

            _model.SetState(GameState.Simulation);
            battleView.ShowSimulation();
            StartBattle();
        }

        public void OnBackToMenuClicked()
        {
            CleanupBattle();
            _model.SetState(GameState.Menu);
            battleView.ShowMenu();
            OnRandomizeClicked();
        }

        public void OnSpeedToggled()
        {
            _model.ToggleSpeedMode();
            battleView.UpdateSpeedButton(_model.CurrentSpeedMultiplier);
        }
        #endregion

        #region Event Handlers
        public void Handle(UnitDiedEvent eventData)
        {
            eventData.army.NotifyUnitDied(eventData.unit);
            
            EventBus.Publish(new ArmyCountChangedEvent 
            { 
                leftCount = _model.LeftArmy.AliveCount, 
                rightCount = _model.RightArmy.AliveCount 
            });
        }

        public void Handle(ArmyCountChangedEvent eventData)
        {
            Debug.Log($"Обновлен счетчик армий - Левая: {eventData.leftCount}, Правая: {eventData.rightCount}");
            battleView.UpdateBattleHUD(eventData.leftCount, eventData.rightCount);
            
            if (eventData.leftCount == 0 || eventData.rightCount == 0)
            {
                var winner = eventData.leftCount > 0 ? "LEFT ARMY WINS" : "RIGHT ARMY WINS";
                EventBus.Publish(new BattleEndedEvent { winner = winner });
            }
        }
        #endregion

        private void StartBattle()
        {
            CleanupBattle();
            
            if (_model.LeftArmyDraft == null || _model.RightArmyDraft == null)
            {
                Debug.LogError("Невозможно начать битву - составы армий пусты!");
                _model.SetState(GameState.Menu);
                battleView.ShowMenu();
                return;
            }
            
            _model.LeftArmy = new BattleArmy("Left");
            _model.RightArmy = new BattleArmy("Right");

            _spawner.SpawnArmy(_model.LeftArmyDraft, _model.LeftArmy, _model.RightArmy, new Vector3(-9f, 0.6f, 0f), 1);
            _spawner.SpawnArmy(_model.RightArmyDraft, _model.RightArmy, _model.LeftArmy, new Vector3(9f, 0.6f, 0f), -1);
        }

        private void CleanupBattle()
        {
            _spawner.CleanupBattleUnits();
            _model.ResetArmies();
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<UnitDiedEvent>(this);
            EventBus.Unsubscribe<ArmyCountChangedEvent>(this);
        }
    }
}