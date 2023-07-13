using Code.Scripts.Abilities.Abstraction;
using UnityEngine;

namespace Code.Scripts.Abilities.AbilitiesSO
{
    [CreateAssetMenu(fileName = "New MolotovAbility", menuName = "Abilities/MolotovAbility")]
    public class MolotovSO : ActiveAbilityBase
    {
        public float speed;
        public float damage;
        public float activeTime;
        public float circleRadius;
        public override void Activate(GameObject caster)
        {
            GameObject molotov = Instantiate(effectPrefab, 
                caster.transform.position, 
                Quaternion.identity);

            MolotovHolder molotovHolderScript = molotov.GetComponent<MolotovHolder>();
            molotovHolderScript.Initialize(speed,damage,activeTime,circleRadius,currentLevel,caster.transform);


        }

        public override void UpdateCooldown()
        {
            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }
        }
    }
}
