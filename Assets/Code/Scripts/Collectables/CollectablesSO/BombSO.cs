using Code.Scripts.Collectables.Abstraction;
using Code.Scripts.Collectables.CollectablesMono;
using UnityEngine;

namespace Code.Scripts.Collectables.CollectablesSO
{
    [CreateAssetMenu(fileName = "New Bomb", menuName = "Collectables/Bomb")]

    public class BombSO : CollectableBase
    {
        public override GameObject Activate()
        {
            GameObject itemGo = Instantiate(itemPrefab);
            Bomb bomb = itemGo.GetComponent<Bomb>();
            bomb.Initialize();
            return itemGo;
        }
    }
}
