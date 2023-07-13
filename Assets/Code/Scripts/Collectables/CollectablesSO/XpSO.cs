using Code.Scripts.Collectables.Abstraction;
using Code.Scripts.Collectables.CollectablesMono;
using UnityEngine;

namespace Code.Scripts.Collectables.CollectablesSO
{
    [CreateAssetMenu(fileName = "New XP", menuName = "Collectables/XP")]
    public class XpSO : CollectableBase
    {
        public float xpValue;

        public override GameObject Activate()
        {
            GameObject itemGo = Instantiate(itemPrefab);
            Xp xp = itemGo.GetComponent<Xp>();
            xp.Initialize(xpValue, itemName);
            return itemGo;
        }
     
    }
}