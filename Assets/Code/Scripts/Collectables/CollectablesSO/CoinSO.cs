using Assets.Code.Scripts.Collectables.CollectablesMono;
using Code.Scripts.Collectables.Abstraction;
using UnityEngine;

namespace Code.Scripts.Collectables.CollectablesSO
{
    [CreateAssetMenu(fileName = "New Coin", menuName = "Collectables/Coin")]

    public class CoinSO : CollectableBase
    {
        public float coinValue;
        public override GameObject Activate()
        {
            GameObject itemGo = Instantiate(itemPrefab);
            Coin coin = itemGo.GetComponent<Coin>();
            coin.Initialize(coinValue);
            return itemGo;
        }

    }
}
