using Code.Scripts.Abilities.Abstraction;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    public class AbilityListItem : MonoBehaviour
    {
        [SerializeField] private Image iconImage;

        public void Initialize(AbilityBase abilityBase)
        {
            gameObject.SetActive(true);
            iconImage.sprite = abilityBase.icon;
        }
    }
}