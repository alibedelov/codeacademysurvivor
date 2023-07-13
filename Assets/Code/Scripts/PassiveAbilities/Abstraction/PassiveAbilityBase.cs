using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.PassiveAbilities.StatsManipulation;
namespace Code.Scripts.PassiveAbilities.Abstraction
{
    public abstract class PassiveAbilityBase : AbilityBase
    {
        public ActiveAbilityBase evolvedAbility;
        public abstract void ApplyEffect(StatsManipulator statsManipulator);
        public abstract void RemoveEffect(StatsManipulator statsManipulator);
    }
}

