using System.Collections;
using Assets.Code.Scripts.Enemies.Abstraction;
using System.Collections.Generic;
using Code.Scripts.Collectables.CollectablesHolder;
using Code.Scripts.Enemies;
using Code.Scripts.Managers;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using Code.Scripts.Player;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using CASP.SoundManager;

public class Dog : EnemyMonoBase, IDestructable
{
    private bool _isAttackable = false;
    private float _attackRate;
    private float _damage;
    private float _health;
    bool dead;
    private SpriteRenderer render;
    [SerializeField] EnemyBase _enemyData;

    private float _lastAttackTime;

    private List<GameObject> _expPrefab;

    private bool _isDestructable;
    public bool IsDestructable => _isDestructable;
    private Animator _animator;
    private Coroutine _attackCoroutine;

    private void OnEnable()
    {
        _isDestructable = true;
        _animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        render.enabled = true;
        //_animator.SetFloat("Run", 1f);
        if (dead == true)
        {
           // _animator.SetTrigger("Die");
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

    public override void TakeDamage(float damage)
    {
        _health -= damage;
        DamageUIManager.Instance.DamageCreateUI(transform.position, damage);
        if (_health < 0 && _isDestructable)
        {
            SoundManager.Instance.Play("HitNear", false);
            UIManager.Instance.GamePanel.UpdateEnemyKill();
            TimeManager.Instance.IncreaseEnemyKillCount();
            _animator.SetTrigger("Die");
            dead = true;
            CallEnemyDie(gameObject, 1);
            _isDestructable = false;
            float randomValue = UnityEngine.Random.value;
            if (randomValue <= TimeManager.Instance.ChanceToSpawnCollectable)
            {
                CollectableHolder.Instance.SpawnEnemyCollectable(this);
            }
        }
    }
}