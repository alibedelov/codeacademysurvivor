using Code.Scripts.Collectables.Abstraction;
using Code.Scripts.Collectables.CollectablesSO;
using UnityEngine;

namespace Assets.Code.Scripts.Collectables.CollectablesMono
{
    public class Coin: CollectableBaseMono
    {
        [SerializeField] private CollectableBase _itemData;
        private float _coinValue;
        public float CoinValue 
        {
            get
            {
                return _coinValue;
            }
        }
        public void Initialize(float coinValue)
        {
            _coinValue = coinValue;
        }
    }
}
