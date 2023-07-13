using System;
using Code.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Code.Scripts.UI
{
    public class LoosePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text lifetimeText;
        [SerializeField] private TMP_Text killedEnemiesText;

        public void Initialize()
        {
            lifetimeText.text = TimeSpan.FromSeconds(TimeManager.Instance.GameTime).ToString(@"mm\:ss");
            killedEnemiesText.text = TimeManager.Instance.TotalEnemyKilled.ToString();
        }
    }
}