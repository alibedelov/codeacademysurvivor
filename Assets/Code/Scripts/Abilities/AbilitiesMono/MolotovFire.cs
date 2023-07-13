using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.Abilities.Abstraction;
using DG.Tweening;
using UnityEngine;

public class MolotovFire : AbilityMonoBase
{
    private float _damage;
    private float _radiusFire;
    private float _destroyAfterSeconds;
    private Collider2D[] _colliders;
    private LayerMask _layerMask;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    public void Initialize(float damage, float destroyAfterSeconds, float radius, LayerMask layerMask,int level)
    {
        transform.DOScale(new Vector3(0.2f*level, 0.2f*level, 0.2f*level), 0.2f);
        _damage = damage;
        _destroyAfterSeconds = destroyAfterSeconds;
        _radiusFire = (radius /8)*level;
        _layerMask = layerMask;
        StartCoroutine(Attack());
        Destroy(gameObject, _destroyAfterSeconds);
    }
    
    private IEnumerator Attack()
    {
        while (true)
        {
            _colliders = Physics2D.OverlapCircleAll(transform.position, _radiusFire, _layerMask);

            foreach (var collider in _colliders)
            {
                if (collider.TryGetComponent<IDestructable>(out var enemy))
                {
                    enemy.TakeDamage(_damage);
                }
            }

            yield return new WaitForSeconds(.3f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radiusFire);
    }
}
