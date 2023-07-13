using System.Collections;
using Code.Scripts.Abilities.Abstraction;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    public class LuckyWonItem : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image tableIconImage;
        [SerializeField] private Slider starSlider;
        [SerializeField] private TMP_Text tittleText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private RectTransform tableItem;


        private static readonly int OpenKey = Animator.StringToHash("Open");
        private static readonly int ShowKey = Animator.StringToHash("Show");

        public void Initialize(AbilityBase abilityBase, RectTransform caster)
        {
            gameObject.SetActive(true);
            animator.SetTrigger(OpenKey);

            Vector3 comeTo = new Vector3(0, 0, 0);

            tableItem.transform.position = caster.transform.position;
            iconImage.sprite = abilityBase.icon;
            starSlider.value = abilityBase.currentLevel;
            tittleText.text = abilityBase.abilityName;
            descriptionText.text = abilityBase.abilityDescription;
            tableIconImage.sprite = abilityBase.icon;


            tableItem.DOLocalMove(comeTo, 0.5f).OnComplete(() => { animator.SetTrigger(ShowKey); }).SetUpdate(true);
        }
    }
}