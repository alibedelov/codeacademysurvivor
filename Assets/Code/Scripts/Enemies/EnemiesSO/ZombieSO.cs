using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Zombie", menuName = "Enemies/Zombie")]
public class ZombieSO : EnemyBase
{

    public override GameObject Activate()
    {
        GameObject enemyGO = Instantiate(enemyPrefab);
        StudentZombie zombieMono = enemyGO.GetComponent<StudentZombie>();
        zombieMono.Initialize(health, damage, attackRate,speed, xp);
        return enemyGO;
    }

    public override void AttackNearby()
    {
        //Debug.Log("Student ATTACKED");
    }
}

