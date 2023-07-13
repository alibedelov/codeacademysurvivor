using Code.Scripts.Collectables.Abstraction;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.Collectables.CollectablesMono
{
    public class Magnet : CollectableBaseMono
    {
        public float xpCollectTime;
        public void Initialize(float gettingTime)
        {
            xpCollectTime = gettingTime;
        }
        
        public List<Xp> CollectAllXP()
        {
            var xpList = new List<Xp>();
            foreach (Xp xp in FindObjectsOfType<Xp>())
            {
                if (xp != null && xp.gameObject.activeSelf)
                {
                    xp.GetComponent<BoxCollider2D>().enabled = false;
                    xpList.Add(xp);
                }
            }
            return xpList;
        }
    }
}