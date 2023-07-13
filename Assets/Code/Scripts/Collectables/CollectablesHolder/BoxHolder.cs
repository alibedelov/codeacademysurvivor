using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Player;
using UnityEngine;

namespace Code.Scripts.Collectables.CollectablesHolder
{
    public class BoxHolder : SingletoneBase<BoxHolder>
    {
        [SerializeField] private GameObject boxPrefab;
        [SerializeField] private List<GameObject> boxes;

        private float _randomX;
        private float _randomY;

        private Transform _player;
        private Camera _mainCamera;

        private void Awake()
        {
            for (int i = 0; i < 50; i++)
            {
                GameObject go = Instantiate(boxPrefab, Vector2.zero, Quaternion.identity);
                go.transform.SetParent(transform);
                boxes.Add(go);
                go.SetActive(false);
            }
        }

        private void Start()
        {
            _mainCamera = Camera.main;
            _player = FindObjectOfType<PlayerMovement>().transform;
            
            InvokeRepeating("SpawnBox", 10, 10);
        }

        private void SpawnBox()
        {
            int countItem = 1;
            if (boxes.Count >= countItem)
            {
                for (int i = 0; i < countItem; i++)
                {
                    Restart:

                    _randomX = Random.Range(_player.transform.position.x - 20, _player.transform.position.x + 20);
                    _randomY = Random.Range(_player.transform.position.y - 20, _player.transform.position.y + 20);

                    Vector2 randomPos = new Vector2(_randomX, _randomY);
                    Vector2 screenPos = _mainCamera.WorldToScreenPoint(randomPos);
                    bool onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f &&
                                    screenPos.y < Screen.height;

                    if (!onScreen)
                    {
                        boxes[^1].SetActive(true);
                        boxes[^1].transform.position = randomPos;
                        boxes.Remove(boxes[^1]);
                    }
                    else
                    {
                        goto Restart;
                    }
                }
            }
        }

        public void BreakBox(Transform boxTransform)
        {
            boxTransform.GetComponent<Animator>().SetBool("Broken", true);

            CollectableHolder.Instance.SpawnBoxCollectable(boxTransform);

            StartCoroutine(DestroyBox(boxTransform));
        }

        IEnumerator DestroyBox(Transform boxTransform)
        {
            yield return new WaitForSeconds(1.5f);
            boxTransform.gameObject.SetActive(false);
            boxes.Add(boxTransform.gameObject);
            boxTransform.GetComponent<Animator>().SetBool("Broken", false);
        }
    }
}