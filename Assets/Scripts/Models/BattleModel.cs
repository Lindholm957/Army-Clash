using UnityEngine;
using System.Collections.Generic;
using ArmyClash.Events;
using ArmyClash.Battle;
using ArmyClash.Controllers;

namespace ArmyClash.Models
{
    public class BattleModel
    {
        public GameState State { get; private set; }
        public float CurrentSpeedMultiplier { get; private set; }
        
        public List<UnitBlueprint> LeftArmyDraft { get; set; }
        public List<UnitBlueprint> RightArmyDraft { get; set; }
        public BattleArmy LeftArmy { get; set; }
        public BattleArmy RightArmy { get; set; }

        private readonly float[] _speedModes = { 1f, 2f, 4f };
        private int _speedModeIndex;

        public BattleModel()
        {
            CurrentSpeedMultiplier = 1f;
            _speedModeIndex = 0;
        }

        public void SetState(GameState newState)
        {
            State = newState;
            
            if (newState == GameState.Menu)
            {
                Time.timeScale = 1f;
                _speedModeIndex = 0;
                CurrentSpeedMultiplier = 1f;
            }
        }

        public void OnUnitDied(BattleArmy army)
        {
        }

        public void ToggleSpeedMode()
        {
            _speedModeIndex = (_speedModeIndex + 1) % _speedModes.Length;
            CurrentSpeedMultiplier = _speedModes[_speedModeIndex];
            Time.timeScale = CurrentSpeedMultiplier;
        }

        public void ResetArmies()
        {
            LeftArmy = null;
            RightArmy = null;
        }
    }
}