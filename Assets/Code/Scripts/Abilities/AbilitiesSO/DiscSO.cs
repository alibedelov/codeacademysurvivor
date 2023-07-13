
using Code.Scripts.Abilities.AbilitiesMono;
using Code.Scripts.Abilities.Abstraction;
using UnityEngine;

[CreateAssetMenu(fileName = "New DiscAbility", menuName = "Abilities/DiscAbility")]
public class DiscSO : ActiveAbilityBase
{
    public float speed;
    public float damage;
    public float activeTime;
    public float circleRadius;

    public override void Activate(GameObject caster)
    {
        GameObject kunai = Instantiate(effectPrefab, 
            caster.transform.position, 
            Quaternion.identity);
        
        DiscHolder discHolderScript = kunai.GetComponent<DiscHolder>();
        discHolderScript.Initialize(speed,damage,activeTime,circleRadius,currentLevel,caster.transform);

        if(kunai.transform.parent == null)
        {
            Destroy(kunai);
        }
    }

    public override void UpdateCooldown()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }
}
