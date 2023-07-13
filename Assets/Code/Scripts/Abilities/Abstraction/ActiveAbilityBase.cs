using UnityEngine;

namespace Code.Scripts.Abilities.Abstraction
{
    public abstract class ActiveAbilityBase : AbilityBase
    {
        public bool hasCooldown;
        public float initialCooldown;
        public float cooldown;
        public GameObject effectPrefab;
        public float currentCooldown;
        public abstract void Activate(GameObject caster);
        public abstract void UpdateCooldown();

        public override void ResetToDefault()
        {
            base.ResetToDefault();
            currentCooldown = 0;
        }
    }
}