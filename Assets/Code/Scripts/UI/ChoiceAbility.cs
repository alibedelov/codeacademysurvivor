using System.Collections.Generic;
using System.Linq;
using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.Managers;
using Code.Scripts.PassiveAbilities.Abstraction;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Scripts.UI
{
    public class ChoiceAbility : MonoBehaviour
    {
        [SerializeField] private PassiveAbilityBase defaultPassiveAbility;

        [SerializeField] private List<AbilityListItem> activeSkillListItems;
        [SerializeField] private List<AbilityListItem> passiveSkillListItems;

        [SerializeField] private List<ChooseAbilityItem> abilityItems;
        [SerializeField] private List<AbilityBase> existingAbilities;
        [SerializeField] private List<AbilityBase> abilities;

        [SerializeField] private TMP_Text xpLevelText;

        private void Awake()
        {
            transform.GetChild(1).DOScale(0, 0).SetUpdate(true);
        }

        private void OnEnable()
        {
            transform.GetChild(1).DOScale(1, 0.15f).SetUpdate(true);

            FetchSkillList();
            ShowRandomAbilities();
        }

        private void OnDisable()
        {
            transform.GetChild(1).DOScale(0, 0).SetUpdate(true);
            foreach (var abilityItem in abilityItems)
            {
                abilityItem.gameObject.SetActive(false);
            }
        }

        public void Initialize(int xpLevel)
        {
            xpLevelText.text = xpLevel.ToString();
        }

        private void ShowRandomAbilities()
        {
            existingAbilities = StatsManipulator.Instance.ActiveAbilities
                .Select(x => x as AbilityBase)
                .Union(StatsManipulator.Instance.PassiveAbilities.Select(x => x as AbilityBase))
                .ToList();

            List<AbilityBase> selectedAbilities = new List<AbilityBase>();
            while (selectedAbilities.Count < abilityItems.Count)
            {
                int availableAbilities = CheckAvailableAbilitiesCount();

                if (availableAbilities <= 0)
                {
                    selectedAbilities.Add(defaultPassiveAbility);
                    abilityItems[selectedAbilities.Count - 1].Initialize(defaultPassiveAbility,
                        defaultPassiveAbility.currentLevel, false);
                    break;
                }

                int randomIndex = Random.Range(0, abilities.Count);
                AbilityBase randomAbility = abilities[randomIndex];

                if (randomAbility.currentLevel >= randomAbility.maxLevel)
                {
                    continue;
                }

                if (UIManager.Instance.GamePanel.XpLevel <= 2 && !randomAbility.IsActive)
                {
                    continue;
                }

                if (selectedAbilities.Contains(randomAbility))
                {
                    if (availableAbilities == selectedAbilities.Count())
                    {
                        break;
                    }

                    continue;
                }


                selectedAbilities.Add(randomAbility);

                if (existingAbilities.Contains(randomAbility))
                {
                    abilityItems[selectedAbilities.Count - 1]
                        .Initialize(randomAbility, randomAbility.currentLevel, false);
                }
                else
                {
                    abilityItems[selectedAbilities.Count - 1].Initialize(randomAbility, randomAbility.currentLevel);
                }
            }
        }

        private int CheckAvailableAbilitiesCount()
        {
            return abilities.Count(ability => ability.currentLevel < ability.maxLevel);
        }

        private void FetchSkillList()
        {
            int activeAbilityCount = 0;
            foreach (var activeAbility in StatsManipulator.Instance.ActiveAbilities)
            {
                activeSkillListItems[activeAbilityCount].Initialize(activeAbility);
                activeAbilityCount++;
            }

            int passiveAbilityCount = 0;
            foreach (var passiveAbility in StatsManipulator.Instance.PassiveAbilities)
            {
                passiveSkillListItems[passiveAbilityCount].Initialize(passiveAbility);
                passiveAbilityCount++;
            }
        }
    }
}