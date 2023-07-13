using Code.Scripts.Abilities.Abstraction;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    public class LuckyTableItem : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Animator animator;

        public AbilityBase ability;

        private static readonly int LightUpKey = Animator.StringToHash("LightUp");
        private static readonly int OpenKey = Animator.StringToHash("Open");
        private static readonly int SelectKey = Animator.StringToHash("Select");

        public void Initialize(AbilityBase abilityBase)
        {
            iconImage.sprite = abilityBase.icon;
            ability = abilityBase;
        }

        public void LightUpItem()
        {
            animator.SetTrigger(LightUpKey);
        }

        public void OpenItem()
        {
            animator.SetTrigger(OpenKey);
        }
        
        public void SelectItem()
        {
            animator.SetTrigger(SelectKey);
        }
    }
}