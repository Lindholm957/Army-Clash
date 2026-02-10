using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using ArmyClash.Events;
using ArmyClash.Controllers;
using ArmyClash.Battle;
using ArmyClash.Models;

namespace ArmyClash.Views
{
    public class BattleView : MonoBehaviour, IEventHandler<BattleEndedEvent>
    {
        [Header("UI Components")]
        public Canvas uiCanvas;
        public TMP_Text titleText;
        public TMP_Text leftPreviewText;
        public TMP_Text rightPreviewText;
        public TMP_Text battleHudText;
        public TMP_Text resultText;

        [Header("Buttons")]
        public Button randomizeButton;
        public Button startButton;
        public Button backButton;
        public Button speedButton;

        private BattleController _controller;

        public void Initialize(BattleController controller)
        {
            _controller = controller;
            
            randomizeButton.onClick.AddListener(_controller.OnRandomizeClicked);
            startButton.onClick.AddListener(_controller.OnStartClicked);
            backButton.onClick.AddListener(_controller.OnBackToMenuClicked);
            speedButton.onClick.AddListener(_controller.OnSpeedToggled);
            
            EventBus.Subscribe<BattleEndedEvent>(this);
        }

        #region UI State Management
        public void ShowMenu()
        {
            titleText.text = "Army Clash Prototype";
            resultText.text = string.Empty;
            battleHudText.text = string.Empty;

            ShowUIElements(true, true, true, true, false, false);
        }

        public void ShowSimulation()
        {
            ShowUIElements(false, false, false, false, false, true);
        }

        public void ShowResult(string winnerText)
        {
            Debug.Log($"Битва окончена: {winnerText}");
            resultText.text = winnerText;
            ShowUIElements(false, false, false, false, true, false);
        }

        private void ShowUIElements(bool title, bool leftPreview, bool rightPreview, 
            bool randomize, bool back, bool speed)
        {
            titleText.gameObject.SetActive(title);
            leftPreviewText.gameObject.SetActive(leftPreview);
            rightPreviewText.gameObject.SetActive(rightPreview);
            randomizeButton.gameObject.SetActive(randomize);
            startButton.gameObject.SetActive(randomize);
            backButton.gameObject.SetActive(back);
            speedButton.gameObject.SetActive(speed);
        }
        #endregion

        #region UI Updates
        public void UpdateArmyPreviews(List<UnitBlueprint> leftArmy, List<UnitBlueprint> rightArmy)
        {
            leftPreviewText.text = $"LEFT\n{ArmyBuilder.BuildSummary(leftArmy)}";
            rightPreviewText.text = $"RIGHT\n{ArmyBuilder.BuildSummary(rightArmy)}";
        }

        public void UpdateBattleHUD(int leftCount, int rightCount)
        {
            battleHudText.text = $"Left: {leftCount} | Right: {rightCount}";
        }

        public void UpdateSpeedButton(float multiplier)
        {
            speedButton.GetComponentInChildren<TMP_Text>().text = $"x{multiplier}";
        }
        #endregion

        #region Event Handlers
        public void Handle(BattleEndedEvent eventData)
        {
            ShowResult(eventData.winner);
        }
        #endregion

        private void OnDestroy()
        {
            EventBus.Unsubscribe<BattleEndedEvent>(this);
        }
    }
}