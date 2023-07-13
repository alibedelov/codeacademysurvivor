using System.Collections.Generic;
using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.Managers;
using Code.Scripts.PassiveAbilities.Abstraction;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    public class ChooseAbilityItem : MonoBehaviour
    {
        [Header("Static Data")] [SerializeField]
        private Sprite activeAbilitySprite;

        [SerializeField] private Sprite passiveAbilitySprite;
        [SerializeField] private Image bgImage;

        [Header("Evolve Section")] [SerializeField]
        private GameObject evolveSection;

        [SerializeField] private Animator evolveAnimator;
        [SerializeField] private Image evolveImage;

        [Header("Members")] [SerializeField] private Image iconImage;

        [SerializeField] private Slider starSlider;
        [SerializeField] private List<Animator> starAnimators;
        [SerializeField] private TMP_Text isNewText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button button;

        public AbilityBase ability;

        private static readonly int IsSelecting = Animator.StringToHash("IsSelecting");
        private static readonly int IsAvailable = Animator.StringToHash("IsAvailable");

        /// <summary>
        /// Use it for setting UI ability choosing.
        /// You must call Initialize method after creating ability element.
        /// </summary>
        public void Initialize(AbilityBase abilityBase, int starCount, bool isNew = true)
        {
            gameObject.SetActive(true);

            button.interactable = true;
            ability = abilityBase;

            bgImage.sprite = abilityBase.IsActive ? activeAbilitySprite : passiveAbilitySprite;

            if (!abilityBase.IsActive && !((PassiveAbilityBase)abilityBase).evolvedAbility.IsUnityNull())
            {
                evolveImage.sprite = ((PassiveAbilityBase)abilityBase).evolvedAbility.icon;

                evolveSection.SetActive(true);

                if (StatsManipulator.Instance.ActiveAbilities.Contains(((PassiveAbilityBase)abilityBase).evolvedAbility))
                {
                    evolveAnimator.SetBool(IsAvailable, true);
                }
            }

            if (starAnimators.Count > starCount)
            {
                starAnimators[starCount].SetBool(IsSelecting, true);
            }

            iconImage.sprite = abilityBase.icon;
            descriptionText.text = abilityBase.abilityName;
            nameText.text = abilityBase.abilityName;
            starSlider.value = starCount + 1;
            isNewText.text = isNew ? "New!" : "";
        }

        private void OnDisable()
        {
            evolveSection.SetActive(false);
            evolveAnimator.SetBool(IsAvailable, false);
        }

        public void OnClick()
        {
            button.interactable = false;

            foreach (var animator in starAnimators)
            {
                animator.SetBool(IsSelecting, true);
            }

            StatsManipulator.Instance.AddAbility(ability);
            UIManager.Instance.CloseChoiceAbilityPanel();
            GameManager.Instance.ResumeGame();
        }
    }
}