using Spine.Unity;
using UnityEngine;

namespace Code.Scripts.Bosses.Abstraction
{
    public abstract class BossBase : ScriptableObject
    {
        public string bossName;
        public GameObject effectPrefab;
        public GameObject areaPrefab;
        public GameObject fencePrefab;
        public SpineAnimation spawnAnimation;
        public float health;
        public abstract void Activate(GameObject caster);
    }
}
