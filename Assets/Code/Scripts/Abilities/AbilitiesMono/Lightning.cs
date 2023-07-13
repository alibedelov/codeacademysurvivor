using System;
using Assets.Code.Scripts.Enemies.Abstraction;
using CASP.SoundManager;
using Code.Scripts.Abilities.Abstraction;
using UnityEngine;
using Random = UnityEngine.Random;

public class Lightning : AbilityMonoBase
{
    [SerializeField] private GameObject gem;
    private float _damage;
    public LayerMask layerMask;
    private Collider2D[] colliders;
   
    private Transform _transform;


    private void OnEnable()
    {
        _transform = GetComponent<Transform>();
    }

    public void Initialize(float damage)
    {
        
        _damage = damage;
        
        colliders = Physics2D.OverlapCircleAll(transform.position, 10f,layerMask);

        if (colliders.Length < 1)
        {
            Destroy(gameObject);
            return;
        }

        float minDistance = Mathf.Infinity;
        Collider2D closestCollider = colliders[Random.Range(0,colliders.Length-1)];
        
        // foreach (var collider in colliders)
        // {
        //     float distance = Vector2.Distance(transform.position, collider.transform.position);
        //     if (distance < minDistance)
        //     {
        //         minDistance = distance;
        //         closestCollider = collider;
        //     }
        // }
        _transform.position = new Vector3(closestCollider.transform.position.x,closestCollider.transform.position.y,closestCollider.transform.position.z);
       

    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent<IDestructable>(out var enemyBase)) // может использовать Матрицу ?
        {
            if (col != null)
            {
                SoundManager.Instance.Play("LightningHit",false);
                enemyBase.TakeDamage(_damage);
                Destroy(gameObject,0.5f);

            }
               
        }
    }
}
