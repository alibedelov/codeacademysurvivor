using System;
using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Bosses.BossesMono
{
    public class FenceItem : MonoBehaviour, IDestructable
    {
        [SerializeField] private Color damageColor;
        [SerializeField] private Color defaultColor;

        [SerializeField] private float damage;
        [SerializeField] private float attackRate;

        private float _lastAttackTime;
        private SpriteRenderer _spriteRenderer;
        private bool _isAnimating;
        private bool _isDestructable = false;
        public bool IsDestructable => _isDestructable;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void TakeDamage(float damage)
        {
            if (!_isAnimating)
            {
                _isAnimating = true;
                _spriteRenderer.material.DOColor(damageColor, "_EmissionColor", 0.1f).OnComplete(() =>
                {
                    _spriteRenderer.material.DOColor(defaultColor, "_EmissionColor", 0.1f)
                        .OnComplete(() => _isAnimating = false);
                });
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out StatsManipulator destructable))
            {
                while (Time.time > _lastAttackTime + attackRate)
                {
                    _lastAttackTime = Time.time;

                    destructable.TakeDamage(damage);
                }
            }
        }
    }
}