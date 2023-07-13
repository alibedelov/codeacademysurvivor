using Code.Scripts.Collectables.Abstraction;
using Code.Scripts.Collectables.CollectablesMono;
using UnityEngine;

namespace Code.Scripts.Collectables.CollectablesSO
{
    [CreateAssetMenu(fileName = "New Rainbow", menuName = "Collectables/Rainbow")]

    public class RainbowSO : CollectableBase
    {
        public override GameObject Activate()
        {
            GameObject itemGo = Instantiate(itemPrefab);
            Rainbow rainbow = itemGo.GetComponent<Rainbow>();
            rainbow.Initialize();
            return itemGo;
        }
    }
}
