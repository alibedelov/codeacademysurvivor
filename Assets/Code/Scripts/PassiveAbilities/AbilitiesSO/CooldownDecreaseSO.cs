using System.Collections.Generic;
using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.PassiveAbilities.Abstraction;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using UnityEngine;

namespace Code.Scripts.PassiveAbilities.AbilitiesSO
{
    [CreateAssetMenu(fileName = "New CooldownDecrease", menuName = "PassiveAbilities/CooldownDecreaseAbility")]
    public class CooldownDecreaseSO : PassiveAbilityBase
    {
        [SerializeField] private List<float> cooldownReductionPercentages;

        public override void ApplyEffect(StatsManipulator statsManipulator)
        {
            if (currentLevel >= 5)
                return;
            float cooldownReduction = cooldownReductionPercentages[currentLevel - 1] / 100f;
            foreach (var abilityBase in statsManipulator.ActiveAbilities)
            {
                var activeAbility = (ActiveAbilityBase)abilityBase;
                activeAbility.cooldown = activeAbility.initialCooldown * (1 - cooldownReduction);
            }
        }

        public override void RemoveEffect(StatsManipulator statsManipulator)
        {
            if (currentLevel > 0)
            {
                foreach (var abilityBase in statsManipulator.ActiveAbilities)
                {
                    var activeAbility = (ActiveAbilityBase)abilityBase;
                    activeAbility.cooldown = activeAbility.initialCooldown;
                }
                currentLevel = 0;
            }
        }
    }
}
