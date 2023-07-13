using UnityEngine;

namespace Code.Scripts.Abilities.Abstraction
{
    public abstract class AbilityBase : ScriptableObject
    {
        public string abilityName;
        public string abilityDescription;
        public int maxLevel;
        public int currentLevel;
        public Sprite icon;
        public bool IsActive => this is ActiveAbilityBase;
        
        public virtual void ResetToDefault()
        {
            currentLevel = 0;
        }
    }
}