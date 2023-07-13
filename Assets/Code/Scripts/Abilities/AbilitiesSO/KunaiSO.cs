using System.Collections;
using Code.Scripts.Abilities.AbilitiesMono;
using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.Managers;
using Unity.Mathematics;
using UnityEngine;

namespace Code.Scripts.Abilities.AbilitiesSO
{
    [CreateAssetMenu(fileName = "New KunaiAbility", menuName = "Abilities/KunaiAbility")]
    public class KunaiSO :ActiveAbilityBase
    {
        public float speed;
        public float damage;
        public float activeTime;

        public override void Activate(GameObject caster)
        {
            CoroutineStarter.Instance.StartCoroutine(ThrowKunai(caster));

        }
        
        private IEnumerator ThrowKunai(GameObject caster)
        {
            for (int i = 0; i < currentLevel; i++)
            {
                GameObject kunai = Instantiate(effectPrefab, 
                    caster.transform.position, 
                    Quaternion.identity);
                Kunai kunaiScript = kunai.GetComponent<Kunai>();
                kunaiScript.Initialize(speed,damage,activeTime,caster);
                yield return new WaitForSeconds(0.1f);
            }
        }

        public override void UpdateCooldown()
        {
            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }
        }

        public override void ResetToDefault()
        {
            base.ResetToDefault();
            currentLevel = 0;
        }
    }
}