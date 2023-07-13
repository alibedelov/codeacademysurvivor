using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : ScriptableObject
{
    public string enemyName;
    public Sprite icon;
    public GameObject enemyPrefab;
    public float health;
    public float speed;
    public float damage;
    public float attackRate;
    public List<GameObject> xp;

    public abstract GameObject Activate();
    public abstract void AttackNearby();

}
