using System.Collections;
using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.Bosses.Abstraction;
using Code.Scripts.Collectables.CollectablesHolder;
using Code.Scripts.Managers;
using Code.Scripts.Player;
using DG.Tweening;
using Spine.Unity;
using UnityEngine;

namespace Code.Scripts.Bosses.BossesMono
{
    public class Bouncebloom : BossBaseMono, IDestructable
    {
        [SerializeField] private Transform bounceBallPrefab;

        private string _bossName;
        private float _attackDelay;
        private int _ballCount;
        private float _bulletSpeed;
        private int _bulletDamage;
        private GameObject _areaPrefab;
        private GameObject _fencePrefab;

        private Transform _area;
        private Transform _fence;

        private float _lastAttackTime;
        private bool _isFacingRight = true;
        private bool _isSpawned = false;
        private Vector2 _direction;

        private SkeletonAnimation _skeletonAnimation;
        private PlayerMovement _player;
        private Transform _transform;
        private MeshRenderer _meshRenderer;

        private bool _isDestructable = true;
        public bool IsDestructable => _isDestructable;


        public void Initialize(string bossName, int attackDelay, int ballCount, float health,
            GameObject areaPrefab, GameObject fencePrefab, float bulletSpeed, int bulletDamage)
        {
            _bossName = bossName;
            _health = health;
            _bulletSpeed = bulletSpeed;
            _bulletDamage = bulletDamage;
            _attackDelay = attackDelay;
            _ballCount = ballCount;
            _areaPrefab = areaPrefab;
            _fencePrefab = fencePrefab;
        }

        private void Awake()
        {
            _transform = transform;
            _player = FindObjectOfType<PlayerMovement>();
            _skeletonAnimation = GetComponent<SkeletonAnimation>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            Sequence sequence = DOTween.Sequence();

            _area = Instantiate(_areaPrefab, _transform.position, Quaternion.identity, parent: _transform).transform;
            _fence = Instantiate(_fencePrefab, _transform.position, Quaternion.identity, parent: _transform).transform;

            _fence.gameObject.SetActive(false);
            _meshRenderer.enabled = false;
            _isDestructable = false;

            UIManager.Instance.GamePanel.OpenBossUI(_bossName, _health);

            sequence.AppendInterval(2.8f);

            sequence.AppendCallback(() =>
            {
                StartCoroutine(Attack());
                _meshRenderer.enabled = true;
                _isDestructable = true;

                _isSpawned = true;

                _fence.gameObject.SetActive(true);
            });

            sequence.AppendInterval(0.5f);

            sequence.AppendCallback(() => { _area.gameObject.SetActive(false); });
        }

        private void Update()
        {
            if (_isSpawned)
            {
                _direction = _player.transform.position - transform.position;

                Flip();
            }
        }

        private IEnumerator Attack()
        {
            while (true)
            {
                yield return new WaitForSeconds(_attackDelay);

                int offset = -2;

                for (int i = 0; i < _ballCount; i++)
                {
                    Transform ball = Instantiate(bounceBallPrefab, transform.position, Quaternion.identity);
                    BouncebloomBullet bullet = ball.GetComponent<BouncebloomBullet>();

                    Vector3 bulletDirection =
                        (_player.transform.position - ball.position) + new Vector3(offset, offset, 0);

                    bullet.Initialize(bulletDirection, _bulletSpeed, _bulletDamage);

                    offset += 2;

                    yield return new WaitForSeconds(0.15f);
                }
            }
        }

        private void Flip()
        {
            if (_isFacingRight && _direction.x < 0f || !_isFacingRight && _direction.x > 0f)
            {
                _isFacingRight = !_isFacingRight;
                _skeletonAnimation.skeleton.ScaleX *= -1;
            }
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health < 0 && _isDestructable)
            {
                _isDestructable = false;

                _meshRenderer.enabled = false;
                // Crumbling animation
                transform.GetChild(0).gameObject.SetActive(true);
                Destroy(gameObject, 0.3f);
                DOTween.Sequence().SetUpdate(true)
                    .AppendInterval(0.35f).AppendCallback(() => { UIManager.Instance.OpenWinPanel(); });
            }
            else
            {
                DOTween.Sequence().SetUpdate(true)
                    .AppendCallback(() => { _skeletonAnimation.AnimationName = "damage"; })
                    .AppendInterval(0.35f)
                    .AppendCallback(() => { _skeletonAnimation.AnimationName = "boss idle"; });
            }

            UIManager.Instance.GamePanel.UpdateBossHealth(_health);
        }
    }
}