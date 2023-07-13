using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Code.Scripts.Abilities.AbilitiesMono;
using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.Managers;
using UnityEditor;
using UnityEngine;

namespace Code.Scripts.Abilities.AbilitiesSO
{
    [CreateAssetMenu(fileName = "New RocketAbility", menuName = "Abilities/RocketAbility")]
    public class RocketSO : ActiveAbilityBase
    {
        public float speed;
        public float damage;
        public float activeTime;
        public float attackRadius;
        public float explosionRadius;
        public int fireDelay;
        public int rocketCount;

        public override void Activate(GameObject caster)
        {
            CoroutineStarter.Instance.StartCoroutine(ThrowRocket(caster));
        }

        private IEnumerator ThrowRocket(GameObject caster)
        {
            for (int i = 0; i < rocketCount; i++)
            {
                GameObject rocket = Instantiate(effectPrefab, caster.transform.position, Quaternion.identity);
                Rocket rocketScript = rocket.GetComponent<Rocket>();
                rocketScript.Initialize(speed, damage, activeTime, attackRadius, explosionRadius);
                yield return new WaitForSeconds(0.8f);
            }
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