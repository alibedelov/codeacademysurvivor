using System;
using System.Collections.Generic;
using Code.Scripts.Bosses.Abstraction;
using Code.Scripts.Managers;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    public class GamePanel : MonoBehaviour
    {
        [Header("XP Leveling")] [SerializeField]
        public List<float> xpLevels;

        [Header("Boss UI")] [SerializeField] private TMP_Text bossNameText;
        [SerializeField] private Slider bossHealthSlider;
        [SerializeField] private GameObject bossUI;

        [Header("Alert Panel")] [SerializeField]
        private GameObject alertPanel;

        [SerializeField] private TMP_Text alertText;
        [SerializeField] private float alertLifetime;

        [SerializeField] private TMP_Text timeText;
        [SerializeField] private TMP_Text xpLevelText;
        [SerializeField] private Slider xpSlider;
        [SerializeField] private TMP_Text coin;
        [SerializeField] private TMP_Text enemyKill;
        [SerializeField] private CanvasGroup fadePanel;
        [SerializeField] private float fadeDuration;

        public int XpLevel => _xpLevel;

        private int _xpLevel = 0;

        private void Start()
        {
            xpSlider.maxValue = xpLevels[_xpLevel];

            LevelManager.Instance.OnSceneLoad += () =>
            {
                _xpLevel = 0;
                
                xpLevels.Clear();
                xpLevels.Add(200);
                
                xpSlider.minValue = 0;
                xpSlider.maxValue = xpLevels[_xpLevel];
                xpSlider.value = 0;
                xpLevelText.text = (_xpLevel + 1).ToString();
            };
        }

        private void Update()
        {
            timeText.text = TimeSpan.FromSeconds(TimeManager.Instance.GameTime).ToString(@"mm\:ss");
        }

        public void OpenBossUI(string bossName, float maxHealth)
        {
            bossNameText.text = bossName;
            bossHealthSlider.maxValue = maxHealth;
            bossUI.SetActive(true);
        }

        public void CloseBossUI()
        {
            bossUI.SetActive(false);
        }

        public void UpdateBossHealth(float currentHealth)
        {
            bossHealthSlider.value = currentHealth;
        }

        public void UpdateXp()
        {
            xpSlider.value = StatsManipulator.Instance.Experience;

            if (xpSlider.value >= xpLevels[_xpLevel])
            {
                xpLevels.Add(xpLevels[_xpLevel] * 1.5f);
                UpgradeXpLevel();
                UIManager.Instance.OpenChoiceAbilityPanel();
            }
        }

        public void UpdateCoin()
        {
            coin.text = Convert.ToInt32(StatsManipulator.Instance.Coin).ToString();
        }

        public void UpdateEnemyKill()
        {
            enemyKill.text = Convert.ToInt32(TimeManager.Instance.TotalEnemyKilled).ToString();
        }

        public void FadeGameUI()
        {
            fadePanel.DOFade(1, fadeDuration).SetLoops(2, LoopType.Yoyo);
        }

        public void ShowAlert(string message)
        {
            alertText.text = message;

            DOTween.Sequence().SetUpdate(true)
                .AppendCallback(() => { alertPanel.SetActive(true); })
                .AppendInterval(alertLifetime).SetUpdate(true)
                .OnComplete(() => { alertPanel.SetActive(false); });
        }

        private void UpgradeXpLevel()
        {
            xpSlider.minValue = xpLevels[_xpLevel];
            _xpLevel++;
            xpSlider.maxValue = xpLevels[_xpLevel];
            xpLevelText.text = (_xpLevel + 1).ToString();
        }
    }
}