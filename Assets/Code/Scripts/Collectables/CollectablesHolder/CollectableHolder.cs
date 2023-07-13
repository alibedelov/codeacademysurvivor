using System.Collections.Generic;
using System.Linq;
using Assets.Code.Scripts.Collectables.CollectablesMono;
using Assets.Code.Scripts.Collectables.CollectablesSO;
using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.Collectables.Abstraction;
using Code.Scripts.Collectables.CollectablesMono;
using Code.Scripts.Collectables.CollectablesSO;
using UnityEngine;

namespace Code.Scripts.Collectables.CollectablesHolder
{
    public class CollectableHolder : SingletoneBase<CollectableHolder>
    {
        [Header("Enemy collectable settings")] [SerializeField]
        private int randomPercentage;

        [SerializeField] private List<GameObject> xps;
        [SerializeField] private List<GameObject> coins;
        [SerializeField] private List<GameObject> magnets;
        [SerializeField] private List<GameObject> meats;
        [SerializeField] private List<GameObject> bombs;
        [SerializeField] private List<GameObject> rainbowChests;

        [SerializeField] private List<CollectableBase> boxCollectables;

        [SerializeField] private List<CollectableBase> enemyCollectables;
        [SerializeField] private CollectableBase rainbowCollectable;


        private void Awake()
        {
            xps = new List<GameObject>();
            coins = new List<GameObject>();
            magnets = new List<GameObject>();
            meats = new List<GameObject>();
            bombs = new List<GameObject>();
            rainbowChests = new List<GameObject>();

            for (int i = 0; i < 10; i++)
            {
                GameObject goItem = rainbowCollectable.Activate();

                goItem.transform.parent = transform;

                goItem.SetActive(false);
            }

            foreach (CollectableBase item in boxCollectables)
            {
                for (int i = 0; i < 100; i++)
                {
                    GameObject goItem = item.Activate();

                    goItem.transform.parent = transform;

                    switch (item)
                    {
                        case CoinSO:
                            coins.Add(goItem);
                            break;

                        case MagnetSO:
                            magnets.Add(goItem);
                            break;

                        case BombSO:
                            bombs.Add(goItem);
                            break;

                        case MeatSO:
                            meats.Add(goItem);
                            break;

                        case RainbowSO:
                            rainbowChests.Add(goItem);
                            break;
                    }

                    goItem.SetActive(false);
                }
            }

            foreach (CollectableBase item in enemyCollectables)
            {
                for (int i = 0; i < 500; i++)
                {
                    GameObject goItem = item.Activate();

                    goItem.transform.parent = transform;
                    goItem.SetActive(false);
                    switch (item)
                    {
                        case XpSO:
                            xps.Add(goItem);
                            break;

                        case RainbowSO:
                            rainbowChests.Add(goItem);
                            break;
                    }
                }
            }
        }

        public void SpawnRainbowCollectable(Transform itemTransform)
        {
            rainbowChests[^1].SetActive(true);
            rainbowChests[^1].transform.position = itemTransform.position;
            rainbowChests.Remove(rainbowChests[^1]);
        }

        public void SpawnBoxCollectable(Transform itemTransform)
        {
            //CollectableBase goItem = boxCollectables[Random.Range(0, boxCollectables.Count - 1)];
            float totalProbability = boxCollectables.Sum(collectable => collectable.probability);

            float randomValue = Random.Range(0f, totalProbability);

            CollectableBase goItem = null;
            float cumulativeProbability = 0f;
            foreach (var collectable in boxCollectables)
            {
                cumulativeProbability += collectable.probability;
                if (randomValue <= cumulativeProbability)
                {
                    goItem = collectable;
                    break;
                }
            }

            switch (goItem)
            {
                case CoinSO:
                    coins[^1].SetActive(true);
                    coins[^1].transform.position = itemTransform.position;
                    coins.Remove(coins[^1]);
                    break;

                case MagnetSO:
                    magnets[^1].SetActive(true);
                    magnets[^1].transform.position = itemTransform.position;
                    magnets.Remove(magnets[^1]);
                    break;

                case BombSO:
                    bombs[^1].SetActive(true);
                    bombs[^1].transform.position = itemTransform.position;
                    bombs.Remove(bombs[^1]);
                    break;

                case MeatSO:
                    meats[^1].SetActive(true);
                    meats[^1].transform.position = itemTransform.position;
                    meats.Remove(meats[^1]);
                    break;
            }
        }

        public void SpawnEnemyCollectable(EnemyMonoBase enemyTransform)
        {
            GameObject collectable = null;

            switch (enemyTransform)
            {
                case WorkerZombie:
                    collectable = xps.Find(x => x.GetComponent<Xp>().XpName == "BlueXP");
                    break;

                // case MiniBossWorkerZombie:
                //     collectable = xps.Find(x => x.GetComponent<Xp>().XpName == "BlueXP");
                //     break;

                default:
                    int randomNumber = Random.Range(0, 101);
                    collectable = randomPercentage < randomNumber
                        ? xps.Find(x => x.GetComponent<Xp>().XpName == "GreenXP")
                        : xps.Find(x => x.GetComponent<Xp>().XpName == "BigGreenXP");
                    break;
            }

            if (collectable != null)
            {
                collectable.SetActive(true);
                collectable.transform.position = enemyTransform.transform.position;
                xps.Remove(collectable);
            }
        }

        public void DestroyCollectable(CollectableBaseMono item)
        {
            switch (item)
            {
                case Xp xp:
                    xps.Add(xp.gameObject);
                    break;

                case Coin coin:
                    coins.Add(coin.gameObject);
                    break;

                case Magnet magnet:
                    magnets.Add(magnet.gameObject);
                    break;

                case Bomb bomb:
                    bombs.Add(bomb.gameObject);
                    break;

                case Rainbow rainbow:
                    rainbowChests.Add(rainbow.gameObject);
                    break;

                case Meat meat:
                    meats.Add(meat.gameObject);
                    break;
            }

            item.GetComponent<BoxCollider2D>().enabled = true;
            item.gameObject.SetActive(false);
        }
    }
}