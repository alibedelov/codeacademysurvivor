using Assets.Code.Scripts.Enemies.Abstraction;
using System;
using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.Enemies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Code.Scripts.Player;
using CASP.SoundManager;

namespace Code.Scripts.Abilities.AbilitiesMono
{

    public class Kunai : AbilityMonoBase
    {
        [SerializeField] private GameObject gem;

        private float _speed;
        private float _damage;
        private float _destroyAfterSeconds;
        public LayerMask layerMask;
        private Collider2D[] colliders;
        private Vector2 _direction;
        private Transform _transform;


        public void Initialize(float speed, float damage, float destroyAfterSeconds, GameObject caster)
        {
            _speed = speed;
            _damage = damage;
            _destroyAfterSeconds = destroyAfterSeconds;
            _transform = GetComponent<Transform>();



            colliders = Physics2D.OverlapCircleAll(transform.position, 10f, layerMask);

            if (colliders.Length < 1)
            {
                Destroy(gameObject);
                return;
            }

            float minDistance = Mathf.Infinity;
            Collider2D closestCollider = null;

            foreach (var collider in colliders)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestCollider = collider;

                }
            }


            closestCollider.TryGetComponent<IDestructable>(out var enemy);
            if (closestCollider != null && enemy != null && enemy.IsDestructable)
            {
                SetDirection((closestCollider.transform.position - transform.position));
                Destroy(gameObject, _destroyAfterSeconds);
                SoundManager.Instance.Play("Kunai", false);

            }

            else
            {
                Destroy(gameObject);
            }

        }

        private void Update()
        {
            _transform.up = _direction.normalized;
            _transform.position += (Vector3)_direction.normalized * (_speed * Time.deltaTime);
        }


        public void SetDirection(Vector2 newDirection)
        {
            _direction = newDirection.normalized;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {

            if (col.TryGetComponent<IDestructable>(out var enemyBase)) // может использовать Матрицу ?
            {
                if (col != null)
                {
                    col.TryGetComponent<IDestructable>(out var destructionable);
                    destructionable.TakeDamage(_damage);
                    Destroy(gameObject, 0.02f);
                }

            }

        }


    }
}