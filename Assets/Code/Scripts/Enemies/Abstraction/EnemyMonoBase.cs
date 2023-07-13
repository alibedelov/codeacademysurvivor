using System;
using UnityEngine;

namespace Assets.Code.Scripts.Enemies.Abstraction
{
    public class EnemyMonoBase : MonoBehaviour, IDestructable
    {
        public Action<GameObject, int> OnEnemyDie;
        public float movementSpeed = 0;

        public void CallEnemyDie(GameObject obj, int index)
        {
            OnEnemyDie?.Invoke(obj, index);
        }

        public virtual bool IsDestructable { get; }

        public virtual void TakeDamage(float damage)
        {
        }
    }
}