using UnityEngine;

namespace Code.Scripts.Collectables.Abstraction
{
    public abstract class CollectableBase : ScriptableObject
    {
        public string itemName;
        public Sprite icon;
        public GameObject itemPrefab;
        public float probability;
        public abstract GameObject Activate();
    }
}
