using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovHolder : MonoBehaviour
{
    public LayerMask layerMask;

    private Transform _casterTransform;
    private float _speed;
    private int _bottleCount;
    private float _radius;
    private float _destroyAfterSeconds;
    private List<MolotovBottle> _bottles;
    private float _damage;
    private Transform _transform;
    private float _angle;

    public void Initialize(float speed, float damage, float activeTime, float radius, int currentLevel, Transform caster)
    {
        _bottles = new List<MolotovBottle>();
        _casterTransform = caster;
        _radius = radius;
        _speed = speed;
        _damage = damage;
        _bottleCount = currentLevel+1;
        _destroyAfterSeconds = activeTime;
        _transform = GetComponent<Transform>();
        
        //InitializeDiscs
        for (int i = 0; i < _bottleCount; i++)
        {
            MolotovBottle bottle = _transform.GetChild(i).GetComponent<MolotovBottle>();
            _bottles.Add(bottle);
        }

        StartCoroutine(ActivateBottles());
        Destroy(gameObject,_destroyAfterSeconds);


    }

    private IEnumerator ActivateBottles()
    {
        float angleStep = 360f / _bottles.Count;

        for (int i = 0; i <_bottles.Count; i++)
        {
            
            float angle = Mathf.Deg2Rad * i * angleStep;
            Vector2 forceDirection = new Vector2(Mathf.Cos(angle) * _radius, Mathf.Sin(angle) * (_radius+(_bottleCount*0.4f)));
            _bottles[i].gameObject.SetActive(true);
            _bottles[i].Initialize(_speed,layerMask,forceDirection,_destroyAfterSeconds,_damage,_radius,_bottleCount);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
