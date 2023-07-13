using System;
using Assets.Code.Scripts.Collectables.CollectablesMono;
using CASP.SoundManager;
using Code.Scripts.Collectables.Abstraction;
using Code.Scripts.Collectables.CollectablesMono;
using UnityEngine;

namespace Code.Scripts.Player
{
    public class CollectableDetector : MonoBehaviour
    {
        public event Action<Xp> OnXpDetect;
        public event Action<Magnet> OnMagnetDetect;
        public event Action<Bomb> OnBombDetect;
        public event Action<Meat> OnMeatDetect;
        public event Action<Rainbow> OnRainbowDetect;
        public event Action<Coin> OnCoinDetect;

        private void OnTriggerEnter2D(Collider2D other)
        {
            other.TryGetComponent(out CollectableBaseMono collectable);
            switch (collectable)
            {
                case Xp xp:
                    OnXpDetect?.Invoke(xp);
                    break;
                case Magnet magnet:
                    OnMagnetDetect?.Invoke(magnet);
                    break;
                case Bomb bomb:
                    OnBombDetect?.Invoke(bomb);
                    break;
                case Meat meat:
                    OnMeatDetect?.Invoke(meat);
                    break;
                case Rainbow rainbow:
                    OnRainbowDetect?.Invoke(rainbow);
                    break;
                case Coin coin:
                    OnCoinDetect?.Invoke(coin);
                    break;
            }
        }
    }
}