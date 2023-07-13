using Assets.Code.Scripts.Collectables.CollectablesMono;
using Code.Scripts.Collectables.Abstraction;
using Code.Scripts.Collectables.CollectablesMono;
using System;
using UnityEngine;

namespace Assets.Code.Scripts.Collectables.CollectablesSO
{
    [CreateAssetMenu(fileName = "New Meat", menuName = "Collectables/Meat")]

    public class MeatSO : CollectableBase
    {
        public override GameObject Activate()
        {
            GameObject itemGo = Instantiate(itemPrefab);
            Meat meat = itemGo.GetComponent<Meat>();
            meat.Initialize();
            return itemGo;
        }
     
    }
}
