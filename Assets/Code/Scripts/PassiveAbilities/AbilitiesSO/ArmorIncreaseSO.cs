using System.Collections.Generic;
using Code.Scripts.PassiveAbilities.Abstraction;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using UnityEngine;

namespace Code.Scripts.PassiveAbilities.AbilitiesSO
{
    [CreateAssetMenu(fileName = "New ArmorIncrease", menuName = "PassiveAbilities/ArmorIncreaseAbility")]
    public class ArmorIncreaseSO : PassiveAbilityBase
    {
        [SerializeField] private List<float> armorIncreasePercentages;

        public override void ApplyEffect(StatsManipulator statsManipulator)
        {
            
            if (currentLevel >= 5)
                return;
            float armorBonus = statsManipulator.Armor * (armorIncreasePercentages[currentLevel - 1] / 100f);
            statsManipulator.armor = statsManipulator.Armor+armorBonus;
        }

        public override void RemoveEffect(StatsManipulator statsManipulator)
        {
            currentLevel = 0;
        }
    }
}
