using Code.Scripts.Abilities.Abstraction;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    /// <summary>
    /// Use it for setting UI ability items.
    /// You must call Initialize method after creating ability element.
    /// </summary>
    public class AbilityItem : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private Slider starSlider;
        
        public void Initialize(AbilityBase abilityBase)
        {
            iconImage.sprite = abilityBase.icon;
            starSlider.value = abilityBase.currentLevel;
            
            gameObject.SetActive(true);
        }
    }
}