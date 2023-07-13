using Code.Scripts.Bosses.Abstraction;
using Code.Scripts.Bosses.BossesMono;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "New DevourerBoss", menuName = "Bosses/DevourerBoss")]
public class DevourerSO : BossBase
{
    public float damage;
    public float speed;
    public int attackDelay;
    public int ballCount;
    public int bulletSpeed;



    public override void Activate(GameObject caster)
    {
        GameObject devourerBoss = Instantiate(effectPrefab, caster.transform.position + Vector3.up * 5,
                Quaternion.identity);
        DevourerMono devourerBossGO = devourerBoss.GetComponent<DevourerMono>();
        devourerBossGO.Initialize(name,attackDelay,ballCount,health,areaPrefab,fencePrefab,bulletSpeed,speed);
    }
}