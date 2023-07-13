using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dog", menuName = "Enemies/Dog")]
public class DogSO : EnemyBase
{
    public override GameObject Activate()
    {
        GameObject enemyGO = Instantiate(enemyPrefab);
        Dog Dog = enemyGO.GetComponent<Dog>();
        Dog.Initialize(health, damage, attackRate, speed, xp);
        return enemyGO;

    }

    public override void AttackNearby()
    {
        //Debug.Log("Dog ATTACKED");
    }
}
