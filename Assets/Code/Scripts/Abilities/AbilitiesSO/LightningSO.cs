using System.Collections;
using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.Managers;
using UnityEngine;

[CreateAssetMenu(fileName = "New LightningAbility", menuName = "Abilities/LightningAbility")]
public class LightningSO : ActiveAbilityBase
{
    public float damage;
    public int fireCount;
    public int fireDelay;


    public override void Activate(GameObject caster)
    {

        CoroutineStarter.Instance.StartCoroutine(ActivateLightning(caster));
    }

    private IEnumerator ActivateLightning(GameObject caster)
    {
        for (int i = 0; i < currentLevel; i++)
        {
            GameObject lightning = Instantiate(effectPrefab,
                caster.transform.position,
                Quaternion.identity);

            Lightning lightningScript = lightning.GetComponent<Lightning>();
            lightningScript.Initialize(damage);
            yield return new WaitForSeconds(0.05f);
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