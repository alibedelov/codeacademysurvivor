using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Plant", menuName = "Enemies/Plant")]
public class PlantSO : EnemyBase
{

    public override GameObject Activate()
    {
        GameObject enemyGO = Instantiate(enemyPrefab);
        PlantZombie zombieMono = enemyGO.GetComponent<PlantZombie>();
        zombieMono.Initialize(health, damage, attackRate, speed, xp);
        return enemyGO;
    }

    public override void AttackNearby()
    {
        //Debug.Log("Student ATTACKED");
    }
}

