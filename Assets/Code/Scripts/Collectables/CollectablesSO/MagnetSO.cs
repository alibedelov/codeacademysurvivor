using Code.Scripts.Collectables.Abstraction;
using Code.Scripts.Collectables.CollectablesMono;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Collectables.CollectablesSO
{
    [CreateAssetMenu(fileName = "New Magnet", menuName = "Collectables/Magnet")]
    public class MagnetSO : CollectableBase
    {
        public float _xpGettingTime;
        public override GameObject Activate()
        {
            GameObject itemGO = Instantiate(itemPrefab,Vector3.zero, Quaternion.identity);
            Magnet magnet = itemGO.GetComponent<Magnet>();
            magnet.Initialize(_xpGettingTime);
            return itemGO;
        }
       
    }
}