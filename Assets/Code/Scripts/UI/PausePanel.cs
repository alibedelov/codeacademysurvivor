using System;
using System.Collections.Generic;
using System.Globalization;
using Code.Scripts.Managers;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Code.Scripts.UI
{
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private List<AbilityItem> activeAbilityItems;
        [SerializeField] private List<AbilityItem> passiveAbilityItems;

        [SerializeField] private TMP_Text killCountText;
        [SerializeField] private TMP_Text coinText;

        private void Awake()
        {
            LevelManager.Instance.OnSceneLoad += ResetItemData;
        }

        private void OnEnable()
        {
            killCountText.text = TimeManager.Instance.TotalEnemyKilled.ToString();
            coinText.text = StatsManipulator.Instance.Coin.ToString(CultureInfo.InvariantCulture);

            int activeAbilityCount = 0;
            foreach (var activeAbility in StatsManipulator.Instance.ActiveAbilities)
            {
                activeAbilityItems[activeAbilityCount].Initialize(activeAbility);
                activeAbilityCount++;
            }

            int passiveAbilityCount = 0;
            foreach (var passiveAbility in StatsManipulator.Instance.PassiveAbilities)
            {
                passiveAbilityItems[passiveAbilityCount].Initialize(passiveAbility);
                passiveAbilityCount++;
            }
        }

        private void ResetItemData()
        {
            foreach (var item in activeAbilityItems)
            {
                item.gameObject.SetActive(false);
            }

            foreach (var item in passiveAbilityItems)
            {
                item.gameObject.SetActive(false);
            }
        }

        public void OnClickBackToHome()
        {
            UIManager.Instance.QuittingPopup.Initialize("This game will yield no earnings. Back To Home?",
                () => UIManager.Instance.BackToHome());
        }
    }
}