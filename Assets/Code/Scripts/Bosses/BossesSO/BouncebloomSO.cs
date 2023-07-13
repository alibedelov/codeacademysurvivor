using Code.Scripts.Bosses.Abstraction;
using Code.Scripts.Bosses.BossesMono;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.Bosses.BossesSO
{
    [CreateAssetMenu(fileName = "New Bouncebloom", menuName = "Bosses/Bouncebloom")]
    public class BouncebloomSO : BossBase
    {
        public int ballCount;
        public int attackDelay;
        public float bulletSpeed;
        public int bulletDamage;

        private Sequence sequence;

        public override void Activate(GameObject caster)
        {
            GameObject boss = Instantiate(effectPrefab, caster.transform.position + Vector3.up * 5,
                Quaternion.identity);

            boss.GetComponent<Bouncebloom>()
                .Initialize(bossName, attackDelay, ballCount, health, areaPrefab, fencePrefab, bulletSpeed,
                    bulletDamage);
        }
    }
}