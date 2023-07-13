using Code.Scripts.Abilities.AbilitiesMono;
using Code.Scripts.Abilities.Abstraction;
using UnityEngine;

[CreateAssetMenu(fileName = "New ForceAreaAbility", menuName = "Abilities/ForceAreaAbility")]
public class ForceAreaSO : ActiveAbilityBase
{
    public float damage;
    public float radius;
    
    public override void Activate(GameObject caster)
    {
        GameObject forceArea = Instantiate(effectPrefab, 
            caster.transform.position, 
            Quaternion.identity);

        ForceArea forceAreaScript = forceArea.GetComponent<ForceArea>();
        int level = currentLevel > 3 ? 3 : currentLevel;
        forceAreaScript.Initialize(damage,caster.transform,radius*level);


    }
    
    public void DestroyActiveForceArea(Transform caster)
    {
        ForceArea activeForceArea = FindObjectOfType<ForceArea>();
        if (activeForceArea != null)
        {
            Destroy(activeForceArea.gameObject);
        }
        Activate(caster.gameObject);
    }

    public override void UpdateCooldown()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }
}
