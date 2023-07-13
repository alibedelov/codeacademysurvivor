using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.Collectables.CollectablesMono;
using Code.Scripts.Enemies;
using UnityEngine;

namespace Code.Scripts.Abilities.AbilitiesMono
{
    public class Rocket : AbilityMonoBase
    {
        [SerializeField] private LayerMask layerMask;

        private float _speed;
        private float _damage;
        private float _activeTime;
        private float _attackRadius;
        private float _explosionRadius;
        private Vector2 _direction;
        private Collider2D[] _enemyColliders;
        private Transform _transform;
        private Animator _animator;
        private Transform _target;
        private Rigidbody2D _rigidbody;


        private static readonly int ExplosionAnimation = Animator.StringToHash("Explode");
        private bool _isAttacked = false;


        public void Initialize(float speed, float damage, float activateTime, float attackRadius, float explosionRadius)
        {
            _speed = speed;
            _damage = damage;
            _activeTime = activateTime;
            _attackRadius = attackRadius;
            _explosionRadius = explosionRadius;

            _transform = GetComponent<Transform>();
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();

            Destroy(gameObject, activateTime);

            _enemyColliders = Physics2D.OverlapCircleAll(transform.position, _attackRadius, layerMask);

            float minDistance = Mathf.Infinity;
            Collider2D closestCollider = null;


            foreach (var collider in _enemyColliders)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCollider = collider;
                }
            }

            if (closestCollider == null)
            {
                Destroy(gameObject);
                return;
            }

            closestCollider.TryGetComponent<IDestructable>(out var enemy);

            if (!enemy.IsDestructable)
            {
                Destroy(gameObject);
                return;
            }

            _target = closestCollider.transform;
            _direction = (Vector2)_target.position - _rigidbody.position;
        }

        private void Update()
        {
            _direction.Normalize();

            float angle = Mathf.Atan2(_direction.x, _direction.y) * Mathf.Rad2Deg;
            _rigidbody.rotation = angle;
            _transform.up = _direction;
            _transform.position += (Vector3)_direction.normalized * (_speed * Time.deltaTime);
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<IDestructable>(out var enemyBase))
            {
                if (col != null && enemyBase.IsDestructable)
                {
                    Explode();
                }
            }
        }

        private void Explode()
        {
            _speed = 0;

            _animator.SetTrigger(ExplosionAnimation);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, layerMask);

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent<IDestructable>(out var enemy) && enemy.IsDestructable)
                {
                    enemy.TakeDamage(_damage);
                }
            }

            Destroy(gameObject, 0.1f);
        }
    }
}