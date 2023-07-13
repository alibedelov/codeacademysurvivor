using System.Collections.Generic;
using Code.Scripts.PassiveAbilities.Abstraction;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts.PassiveAbilities.AbilitiesSO
{
    [CreateAssetMenu(fileName = "New MovementSpeedIncrease", menuName = "PassiveAbilities/MovementSpeedIncreaseAbility")]
    public class MovementSpeedIncreaseSO : PassiveAbilityBase
    {
       [SerializeField] private List<float> speedIncreasePercentages;

        public override void ApplyEffect(StatsManipulator statsManipulator)
        {
            
            if (currentLevel >= 5)
                return;
            float speedBonus = statsManipulator.MovementSpeed * (speedIncreasePercentages[currentLevel - 1] / 100f);
            statsManipulator.movementSpeed = statsManipulator.MovementSpeed+speedBonus;
        }

        public override void RemoveEffect(StatsManipulator statsManipulator)
        {
            currentLevel = 0;
        }
    }
}
