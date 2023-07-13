using System;
using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.Abilities.Abstraction;
using DG.Tweening;
using UnityEngine;

public class Disc : AbilityMonoBase
{
    private float _damage;
    private LayerMask _layerMask;

    public void Initialize(float damage, LayerMask layerMask)
    {
        _damage = damage;
        _layerMask = layerMask;
    }

    private void Update()
    {
        transform.localRotation *= Quaternion.Euler(0, 0, 180 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDestructable>(out var enemy))
        {
            enemy.TakeDamage(_damage);
        }
    }
}