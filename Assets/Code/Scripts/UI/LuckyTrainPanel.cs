using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CASP.SoundManager;
using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.Managers;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.UI
{
    public class LuckyTrainPanel : MonoBehaviour
    {
        [Header("Train Settings")] [SerializeField]
        private float rotationSpeed;

        [SerializeField] private int roundCount;
        [SerializeField] private int priseCount;

        [Header("Train Data")] [SerializeField]
        private List<LuckyTableItem> tableItems;

        [Header("Win Panel")] [SerializeField] private List<LuckyWonItem> luckyWonItems;

        [SerializeField] private Transform skipButton;

        private List<AbilityBase> _existingAbilities;
        private List<LuckyTableItem> _selectedItems;
        private bool _isSkipped;

        private void Awake()
        {
            transform.GetChild(0).DOScale(0, 0).SetUpdate(true);
        }

        private void OnDisable()
        {
            transform.GetChild(0).DOScale(0, 0).SetUpdate(true);
        }

        private void OnEnable()
        {
            transform.GetChild(0).DOScale(1, 0.25f).SetUpdate(true);

            FetchAbilities();
            SelectAbilities();

            StartCoroutine(AnimateFirstSequence());
        }

        public void AnimateSelect()
        {
            skipButton.gameObject.SetActive(true);
            StartCoroutine(AnimateSecondSequence());
        }

        private void FetchAbilities()
        {
            _existingAbilities = StatsManipulator.Instance.ActiveAbilities
                .Select(x => x as AbilityBase)
                .Union(StatsManipulator.Instance.PassiveAbilities.Select(x => x as AbilityBase))
                .ToList();

            List<AbilityBase> showedAbilities = new List<AbilityBase>();

            while (showedAbilities.Count < tableItems.Count)
            {
                int randomIndex = Random.Range(0, _existingAbilities.Count);
                AbilityBase randomAbility = _existingAbilities[randomIndex];
                showedAbilities.Add(randomAbility);

                tableItems[showedAbilities.Count - 1].Initialize(randomAbility);
            }
        }

        private void SelectAbilities()
        {
            _selectedItems = new List<LuckyTableItem>();

            int randomIndex = Random.Range(1, tableItems.Count);

            for (int i = 0; i < priseCount; i++)
            {
                int index = (randomIndex + i) % tableItems.Count;
                _selectedItems.Add(tableItems[index]);
                tableItems[index].ability.currentLevel++;
            }
        }

        private IEnumerator AnimateFirstSequence()
        {
            int firstPart = tableItems.Count;
            int secondPart = tableItems.Count / 2;

            for (int i = 0, j = secondPart; i < secondPart && i < firstPart; i++, j++)
            {
                tableItems[i].OpenItem();
                tableItems[j].OpenItem();

                yield return new WaitForSecondsRealtime(0.1f);
            }
        }

        private IEnumerator AnimateSecondSequence()
        {
            float speed = rotationSpeed;
            for (int i = 0; i < roundCount; i++)
            {
                foreach (var item in tableItems)
                {
                    if (i >= roundCount - 1)
                    {
                        speed += 0.03f;
                        if (_isSkipped)
                            yield break;
                        //SoundManager.Instance.Play("SearchingItem", false);

                        if (_selectedItems.Contains(item))
                        {
                            StartCoroutine(SelectItems());
                        }
                    }

                    item.LightUpItem();
                    yield return new WaitForSecondsRealtime(speed);
                }
            }
        }

        private IEnumerator SelectItems()
        {
            for (int j = 0; j < priseCount; j++)
            { 
                SoundManager.Instance.Play("SelectItem",false);
                _selectedItems[j].SelectItem();

                yield return new WaitForSecondsRealtime(0.5f);

                UIManager.Instance.OpenLuckyTrainWinPanel();

                luckyWonItems[j].Initialize(_selectedItems[j].ability,
                    _selectedItems[j].GetComponent<RectTransform>());
                yield return new WaitForSecondsRealtime(0.2f);
            }
        }

        public void Skip()
        {
            StartCoroutine(SelectItems());
            _isSkipped = true;
        }
    }
}