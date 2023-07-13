using Assets.Code.Scripts.Enemies.Abstraction;
using System.Collections.Generic;
using Code.Scripts.Collectables.CollectablesHolder;
using Code.Scripts.Enemies;
using Code.Scripts.Managers;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class PlantZombie : EnemyMonoBase, IDestructable
{
    private Animator _animator;
    private bool _isAttackable = false;
    private float _attackRate;
    private float _damage;
    private float _health;
    private bool _isDead;
    [SerializeField] private GameObject bullet;
    [SerializeField] EnemyBase _enemyData;

    private float _lastAttackTime;

    private List<GameObject> _expPrefab;

    private bool _isDestructable;
    public bool IsDestructable => _isDestructable;

    private void OnEnable()
    {
        _isDestructable = true;
        _isDead = false;
    }

    public void Initialize(float health, float damage, float attackRate, float speed, List<GameObject> expPrefabs)
    {
        _health = health;
        _damage = damage;
        _attackRate = attackRate;
        _expPrefab = expPrefabs;
        movementSpeed = speed;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (_isDead)
            return;
        Attack();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            while (Time.time > _lastAttackTime + _attackRate)
            {
                _enemyData.AttackNearby();
                _lastAttackTime = Time.time;

                if (collision.TryGetComponent(out IDestructable destructable))
                {
                    destructable.TakeDamage(_damage);
                }
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        _health -= damage;
        DamageUIManager.Instance.DamageCreateUI(transform.position, damage);
        if (_health < 0 && _isDestructable)
        {
            UIManager.Instance.GamePanel.UpdateEnemyKill();

            _animator.SetTrigger("Die");
            _isDead = true;
            float randomValue = UnityEngine.Random.value;
            if (randomValue <= TimeManager.Instance.ChanceToSpawnCollectable)
            {
                CollectableHolder.Instance.SpawnEnemyCollectable(this);
            }
            TimeManager.Instance.IncreaseEnemyKillCount();
            Destroy(gameObject);
            CallEnemyDie(gameObject, 2);
            _isDestructable = false;

           //Destroy(gameObject);
        }
        else
        {
            _animator.SetTrigger("Damage");
        }
    }

    public void Attack()
    {
        if (Time.time > _lastAttackTime + _attackRate && GameManager.Instance.gameState != GameState.Paused)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);

            _lastAttackTime = Time.time;
        }
        
    }
}