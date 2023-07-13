using Assets.Code.Scripts.Enemies.Abstraction;
using System.Collections.Generic;
using Code.Scripts.Collectables.CollectablesHolder;
using Code.Scripts.Enemies;
using Code.Scripts.Managers;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using Code.Scripts.Player;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class WorkerZombie : EnemyMonoBase, IDestructable
{
    private Animator _animator;
    private bool _isAttackable = false;
    private float _attackRate;
    private float _damage;
    private float _health;
    private bool dead;
    private SpriteRenderer render;
    [SerializeField] EnemyBase _enemyData;

    private float _lastAttackTime;

    private List<GameObject> _expPrefab;

    private bool _isDestructable;
    public bool IsDestructable => _isDestructable;

    private void OnEnable()
    {
        _isDestructable = true;
        _animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        render.enabled = true;
        //_animator.SetFloat("Run", 1f);
        if (dead == true)
        {
            //_animator.SetTrigger("Die");
            dead = false;
        }
        //_animator.SetTrigger("Default");
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
        //StartAttack();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _lastAttackTime = Time.time;
            while (Time.time > _lastAttackTime + _attackRate && GameManager.Instance.gameState != GameState.Paused)
            {
                _enemyData.AttackNearby();
                _lastAttackTime = Time.time;
                if (collision.TryGetComponent<PlayerMovement>(out PlayerMovement destructable))
                {
                    StatsManipulator.Instance.TakeDamage(_damage);
                }
            }
        }
    }

    public override void TakeDamage(float damage)
    {
        _health -= damage;
        DamageUIManager.Instance.DamageCreateUI(transform.position, damage);
        if (_health <= 0 && _isDestructable)
        {
            UIManager.Instance.GamePanel.UpdateEnemyKill();
            TimeManager.Instance.IncreaseEnemyKillCount();
            _animator.SetTrigger("Die");
            float randomValue = UnityEngine.Random.value;
            if (randomValue <= TimeManager.Instance.ChanceToSpawnCollectable)
            {
                CollectableHolder.Instance.SpawnEnemyCollectable(this);
            }
            CallEnemyDie(gameObject,3);
            dead = true;
            _isDestructable = false;

            //Destroy(gameObject);
        }
        else
        {
            _animator.SetTrigger("Damage");
        }
    }
}