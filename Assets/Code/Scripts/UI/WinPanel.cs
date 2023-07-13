using System;
using Code.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Code.Scripts.UI
{
    public class WinPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text killedEnemiesText;

        public void Initialize()
        {
            killedEnemiesText.text = TimeManager.Instance.TotalEnemyKilled.ToString();
        }
    }
}