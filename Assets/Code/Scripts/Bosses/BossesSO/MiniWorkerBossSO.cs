using Code.Scripts.Bosses.Abstraction;
using Code.Scripts.Bosses.BossesMono;
using UnityEngine;

[CreateAssetMenu(fileName = "New MiniWorkerBoss", menuName = "Bosses/MiniWorkerBoss")]
public class MiniWorkerBossSO : BossBase
{
    public float damage;
    public float speed;

    public override void Activate(GameObject caster)
    {
        GameObject enemyGO = Instantiate(effectPrefab);
        MiniWorkerBoss workerZombie = enemyGO.GetComponent<MiniWorkerBoss>();
        workerZombie.Initialize(health, speed, damage);
    }
}