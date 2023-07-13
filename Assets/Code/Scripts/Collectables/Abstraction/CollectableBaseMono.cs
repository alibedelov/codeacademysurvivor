using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Player;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Collectables.Abstraction
{
    public class CollectableBaseMono : MonoBehaviour
    {
        private bool isMoving;
        private Vector3 _direction;
        private Rigidbody2D _playerPhysics;

        private void Awake()
        {
            _playerPhysics = FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody2D>();
        }

        public void AnimateCollectable(TweenCallback onComplete, float duration = 0.2f)
        {
            _direction = _playerPhysics.velocity;
            var position = transform.position;
            GetComponent<BoxCollider2D>().enabled = false;

            transform.DOMove(position + _direction, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
            {

                StartCoroutine(MoveToPlayer(onComplete, duration));
            });
        }

        public IEnumerator MoveToPlayer(TweenCallback onComplete, float duration)
        {
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / duration);
                t = 1f - Mathf.Pow(1f - t, 2f);
                transform.position = Vector3.Lerp(transform.position, _playerPhysics.transform.position, t * 0.15f);
                yield return null;
            }

            onComplete?.Invoke();
        }
    }
}