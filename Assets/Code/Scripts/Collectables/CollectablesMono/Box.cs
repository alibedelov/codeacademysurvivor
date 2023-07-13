using System;
using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.Collectables.CollectablesHolder;
using UnityEngine;

namespace Code.Scripts.Collectables.CollectablesMono
{
    public class Box : MonoBehaviour,IDestructable
    {
        private bool _isDestrucatable;

        private void OnEnable()
        {
            _isDestrucatable = true;
        }

        public bool IsDestructable => _isDestrucatable;
        public void TakeDamage(float damage)
        {
            if (_isDestrucatable)
            {
                BoxHolder.Instance.BreakBox(transform);
                _isDestrucatable = false;
            }
        }
    }
}
