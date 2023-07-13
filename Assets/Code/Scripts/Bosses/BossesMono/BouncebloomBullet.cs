using System.Collections;
using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Bosses.BossesMono
{
    public class BouncebloomBullet : MonoBehaviour
    {
        private float _bulletSpeed;
        private int _damage;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Transform _transform;

        public void Initialize(Vector2 direction, float bulletSpeed, int damage)
        {
            _damage = damage;
            _bulletSpeed = bulletSpeed;

            SetDirection(direction);
        }

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody2D>();
            _transform.DOScale(0, 0);
        }

        private void Start()
        {
            _transform.DORotate(Vector3.forward * 180, 2).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
            _transform.DOScale(1, 0.3f);

            StartCoroutine(DestroyBullet());
        }

        private void Update()
        {
            float distance = _direction.magnitude;

            if (distance > 0)
            {
                _direction /= distance;

                _rigidbody.velocity = _direction * _bulletSpeed;
            }
        }

        private void SetDirection(Vector2 newDirection)
        {
            _direction = newDirection.normalized;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out FenceItem fence))
            {
                Vector3 contactPoint = other.ClosestPoint(transform.position);
                Vector2 normal = _transform.position - contactPoint;
                SetDirection(normal);
            }

            if (other.TryGetComponent(out IDestructable destructable))
            {
                destructable.TakeDamage(_damage);
            }

            if (other.TryGetComponent(out StatsManipulator statsManipulator))
            {
                statsManipulator.TakeDamage(_damage);
            }
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(8f);

            transform.DOScale(0, 0.5f).OnComplete(() => { Destroy(gameObject); });
        }
    }
}