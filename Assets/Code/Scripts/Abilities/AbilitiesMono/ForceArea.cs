using System.Collections;
using System.Security.Cryptography;
using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.Abilities.Abstraction;
using UnityEngine;

namespace Code.Scripts.Abilities.AbilitiesMono
{
    public class ForceArea : AbilityMonoBase
    {
        public LayerMask layerMask;
        
        private float _damage;
        private float _radius;
        private Collider2D[] _colliders;
        private Transform _transform;
        private float _attackRate;
        public void Initialize(float damage, Transform casterTransform, float radius)
        {
            _damage = damage;
            _radius = radius/1.3f;
            _attackRate = 0.1f;
            _transform = transform;
            _transform.parent = casterTransform;
            var localScale = _transform.localScale;
            localScale = new Vector3(radius / 3, radius / 3, radius / 3);
            _transform.localScale = localScale;
            StartCoroutine(DamageEnemiesOverTime());
        }

        private IEnumerator DamageEnemiesOverTime()
        {
            while (true)
            {
                _colliders = Physics2D.OverlapCircleAll(transform.position, _radius, layerMask);

                bool bossInRadius = false;
        
                foreach (var collider in _colliders)
                {
                    if (collider.transform.CompareTag("Boss"))
                    {
                        bossInRadius = true;
                    }

                    if (collider.TryGetComponent<IDestructable>(out var enemy))
                    {
                        if (enemy.IsDestructable)
                        {
                            enemy.TakeDamage(_damage);
                        }
                    }
                }

                // Задержка между нанесением урона. Можно изменить на нужное значение.
                float delay = bossInRadius ? 0.8f : 0.5f; // босс получает урон реже

                yield return new WaitForSeconds(delay);
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
