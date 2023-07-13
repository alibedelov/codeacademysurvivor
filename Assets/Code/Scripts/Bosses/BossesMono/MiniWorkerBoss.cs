using System.Collections;
using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.Collectables.CollectablesHolder;
using Code.Scripts.Managers;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using Code.Scripts.Player;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace Code.Scripts.Bosses.BossesMono
{
    public class MiniWorkerBoss : EnemyMonoBase, IDestructable
    {
        [SerializeField] private SkeletonAnimation _skeletonAnimation;
        private Animator _animator;
        private Transform _playerTransform;
        private MeshRenderer _meshRenderer;
        private float _health;
        private float _speed;
        private float _damage;
        private bool _isAttackable = false;
        private float _attackRate;
        private float _lastAttackTime;
        private bool _isDestructable = true;
        private Coroutine _attackCoroutine;


        private void Awake()
        {
            _attackRate = 0.8f;
            _playerTransform = FindObjectOfType<PlayerMovement>().transform;
            _skeletonAnimation = GetComponent<SkeletonAnimation>();

            _animator = GetComponentInChildren<Animator>();
            _meshRenderer = GetComponent<MeshRenderer>();

            transform.position = _playerTransform.position + new Vector3(Random.Range(-9, 9), Random.Range(-9, 9));
        }

        public void Initialize(float health, float speed, float damage)
        {
            _health = health;
            _speed = speed;
            _damage = damage;
        }

        private void Update()
        {
            if (_playerTransform != null)
            {
                Vector2 targetPosition = _playerTransform.position;
                Vector2 bossPosition = transform.position;

                transform.position = Vector2.MoveTowards(bossPosition, targetPosition, _speed * Time.deltaTime);
                FlipTowardsPlayer(targetPosition, bossPosition);
            }
        }

        private void FlipTowardsPlayer(Vector2 targetPosition, Vector2 bossPosition)
        {
            bool shouldFaceRight = targetPosition.x > bossPosition.x;

            if ((shouldFaceRight && transform.localScale.x < 0) || (!shouldFaceRight && transform.localScale.x > 0))
            {
                Vector3 bossScale = transform.localScale;
                bossScale.x *= -1; // flip the boss by inverting the x-scale
                transform.localScale = bossScale;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && _attackCoroutine == null)
            {
                _attackCoroutine = StartCoroutine(AttackCoroutine(collision));
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && _attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine);
                _attackCoroutine = null;
            }
        }

        private IEnumerator AttackCoroutine(Collider2D collision)
        {
            // Wait for the first attack
            yield return new WaitForSeconds(0.2f);

            // Start the attack loop
            while (true)
            {
                if (collision.TryGetComponent<PlayerMovement>(out PlayerMovement destructable))
                {
                    StatsManipulator.Instance.TakeDamage(_damage);
                }
                yield return new WaitForSeconds(_attackRate);
            }
        }

        public bool IsDestructable => _isDestructable;

        public override void TakeDamage(float damage)
        {
            _health -= damage;
            DamageUIManager.Instance.DamageCreateUI(transform.position, damage);
            if (_health < 0 && _isDestructable)
            {
                UIManager.Instance.GamePanel.UpdateEnemyKill();
                CollectableHolder.Instance.SpawnRainbowCollectable(transform);
                TimeManager.Instance.IncreaseEnemyKillCount();
                _skeletonAnimation.AnimationName = "Damage";
                _meshRenderer.enabled = false;
                _speed = 0;
                _animator.SetTrigger("Die");
                _isDestructable = false;
                Destroy(gameObject, 0.5f);
            }
            else
            {
                DOTween.Sequence().AppendCallback(() => { _skeletonAnimation.AnimationName = "Damage"; })
                    .AppendInterval(0.35f)
                    .AppendCallback(() => { _skeletonAnimation.AnimationName = "Walk"; });
            }
        }
    }
}