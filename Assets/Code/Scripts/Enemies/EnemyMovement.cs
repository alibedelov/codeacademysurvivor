using System;
using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.Managers;
using Code.Scripts.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Code.Scripts.Enemies
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private Transform _player;

        private EnemyMonoBase _enemyMonoBase;
        private NavMeshAgent _agent;
        private bool _isFacingRight = true;
        private Animator _animator;

        private void OnEnable()
        {
            _agent.SetDestination(_player.transform.position);
            _agent.updateRotation = false;
            _agent.updateUpAxis = false;
        }

        private void Awake()
        {
            _player = FindAnyObjectByType<PlayerMovement>().transform;
            var agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            _agent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
            _enemyMonoBase = GetComponent<EnemyMonoBase>();
        }


        private void Start()
        {
            _agent.speed = _enemyMonoBase.movementSpeed;
        }

        private void OnPauseAndResumeGame(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Paused:
                    _agent.speed = 0;
                    break;

                case GameState.Resumed:
                    _agent.speed = _enemyMonoBase.movementSpeed;
                    break;
            }
        }

        private void Update()
        {
            if (_agent.velocity.magnitude > 0)
            {
                //_animator.SetBool("Run", true);
                _animator.SetFloat("Run", _agent.velocity.magnitude);
            }
        }

        private void LateUpdate()
        {
            _agent.SetDestination(_player.transform.position);
            Flip();
        }

        private void Flip()
        {
            if (_isFacingRight && _agent.velocity.x < 0f || !_isFacingRight && _agent.velocity.x > 0f)
            {
                _isFacingRight = !_isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
    }
}
