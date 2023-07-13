using System;
using System.Collections;
using System.Collections.Generic;
using CASP.SoundManager;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine;

public class MolotovBottle : MonoBehaviour
{
    [SerializeField] private GameObject _firePrefab;
    private float _forcePower;
    private LayerMask _layerMask;
    private float _damage;
    private float _destroyAfterSeconds;
    private Rigidbody2D _physics;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _physics = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(float forcePower, LayerMask layerMask, Vector2 forceDirection, float destroyAfterSeconds,
        float damage, float radius,int level)
    {
        //Initialize data
        _forcePower = forcePower;
        _layerMask = layerMask;
        _damage = damage;
        _destroyAfterSeconds = destroyAfterSeconds;
        float scale = 0.0f;
        if (level == 2)
        {
            scale = level / 4f;
        }
        else
        {
            scale = ((level-1 ) / 4f)+0.2f;

        }
        scale = scale > 1 ? 1 : scale;
        transform.localScale = new Vector3(scale, scale, 1f);
        
        //Actions
        _physics.AddTorque(25f, ForceMode2D.Impulse);
        transform.DOLocalJump(new Vector3(forceDirection.x, forceDirection.y, 0), forcePower, 1, 1.2f)
            .OnComplete(() =>
            {
                GameObject go = Instantiate(_firePrefab, transform.position, quaternion.identity);
                go.GetComponent<MolotovFire>().Initialize(_damage, _destroyAfterSeconds, radius, layerMask, level);
                gameObject.SetActive(false);
                SoundManager.Instance.Play("Molotov", true);
            });
    }
}
