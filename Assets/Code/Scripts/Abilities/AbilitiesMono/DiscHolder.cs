using System;
using System.Collections;
using System.Collections.Generic;
using CASP.SoundManager;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Abilities.AbilitiesMono
{
    public class DiscHolder : MonoBehaviour
    {
        public LayerMask layerMask;

        private Transform _casterTransform;
        private float _speed;
        private float _destroyAfterSeconds;
        private int _discCount;
        private float _radius;
        private List<Disc> _discs;

        private Transform _transform;
        private float _angle;
        public void Initialize(float speed, float damage, float activeTime,float radius,int currentLevel,Transform caster)
        {
            _discs = new List<Disc>();
            _casterTransform = caster;
            _radius = radius;
            _speed = speed;
            _destroyAfterSeconds = activeTime;
            _discCount = currentLevel+1;
            _transform = GetComponent<Transform>();
            //InitializeDiscs
            for (int i = 0; i < _discCount; i++)
            {
                Disc disc = _transform.GetChild(i).GetComponent<Disc>();
                _discs.Add(disc);
            }

            for (int i = 0; i < _discs.Count; i++)
            {
                _discs[i].gameObject.SetActive(true);
                _discs[i].Initialize(damage,layerMask);
            }

            _transform.parent = _casterTransform;  // Make Player our parent so we can follow him
            Destroy(gameObject,_destroyAfterSeconds);
            StartCoroutine(FalseDiscs());
        }

        private IEnumerator FalseDiscs()
        {
            yield return new WaitForSeconds(_destroyAfterSeconds);
            foreach (var disc in _discs)
            {
                disc.transform.DOScale(Vector3.zero, 1f);
            }

            yield return new WaitForSeconds(1.2f);
            
            Destroy(gameObject);
        }
        private void Update()
        {
            if (_discs.Count < 1) return;
            AttackDiscs();
        }

        private void AttackDiscs()
        {
            SoundManager.Instance.Play("Disc", false);
            _angle += _speed * Time.deltaTime;

            for (int i = 0; i < _discs.Count; i++)
            {
                float currentAngle = i * Mathf.PI * 2 / _discs.Count + _angle;
                Vector3 newPos = _transform.position +
                                 new Vector3(Mathf.Cos(-currentAngle), Mathf.Sin(-currentAngle),0 ) * _radius;
                _discs[i].transform.position = newPos;
            }
        }
    }
}
