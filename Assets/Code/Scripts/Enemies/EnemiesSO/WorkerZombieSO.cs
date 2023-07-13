using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkerZombie", menuName = "Enemies/WorkerZombie")]
public class WorkerZombieSO : EnemyBase
{
    public override GameObject Activate()
    {
        GameObject enemyGO = Instantiate(enemyPrefab);
        WorkerZombie workerZombie = enemyGO.GetComponent<WorkerZombie>();
        workerZombie.Initialize(health, damage, attackRate,speed, xp);
        return enemyGO;

    }

    public override void AttackNearby()
    {
        //Debug.Log("Worker ATTACKED");
    }
}
