using Code.Scripts.PassiveAbilities.StatsManipulation;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Player
{
    public class PlayerCanvas : SingletoneBase<PlayerCanvas>
    {
        [Header("Settings")] [SerializeField] private Color middleHealthColor;
        [SerializeField] private Color lowHealthColor;
        [SerializeField] private Color defaultHealthColor;
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Image healthSliderImage;
        [SerializeField] private Slider abilityCooldownSlider;

        public void UpdatePlayerHealth()
        {
            healthSlider.maxValue = StatsManipulator.Instance.MaxHealth;
            healthSlider.value = StatsManipulator.Instance.CurrentHealth;

            healthSliderImage.color = defaultHealthColor;

            if (healthSlider.value < healthSlider.maxValue / 2)
            {
                healthSliderImage.color = middleHealthColor;
            }

            if (healthSlider.value < healthSlider.maxValue / 3)
            {
                healthSliderImage.color = lowHealthColor;
            }
        }

        public void UpdateAbilityCooldown()
        {
            abilityCooldownSlider.minValue = StatsManipulator.Instance.DefaultAbility.cooldown * -1;
            abilityCooldownSlider.maxValue = 0;
            abilityCooldownSlider.value = StatsManipulator.Instance.DefaultAbility.currentCooldown * -1;
        }
    }
}