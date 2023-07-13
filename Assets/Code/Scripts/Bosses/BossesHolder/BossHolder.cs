using System.Collections.Generic;
using Code.Scripts.Bosses.Abstraction;
using Code.Scripts.Managers;
using Code.Scripts.Player;
using UnityEngine;

namespace Code.Scripts.Bosses.BossesHolder
{
    public class BossHolder : SingletoneBase<BossHolder>
    {
        
        
        [SerializeField] private List<BossBase> bosses;

       
        private PlayerMovement _player;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerMovement>();
        }

        private void OnEnable()
        {
            TimeManager.Instance.OnFiveMinuteEvent += OnFiveMinuteEvent;
            TimeManager.Instance.OnThreeMinuteEvent += OnThreeMinuteEvent;
        }

        private void OnThreeMinuteEvent()
        {
            SpawnBoss(bosses[1], _player.transform);
        }

        private void OnFiveMinuteEvent()
        {
            SpawnBoss(bosses[0], _player.transform);
        }

        public void SpawnBoss(BossBase boss, Transform caster)
        {
            boss.Activate(caster.gameObject);
        }

      
    }
}