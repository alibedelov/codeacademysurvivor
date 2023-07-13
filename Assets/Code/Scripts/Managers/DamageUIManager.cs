using UnityEngine;
using DamageNumbersPro;
public class DamageUIManager: SingletoneBase<DamageUIManager>
{
    [SerializeField] private Color _critDamageColor;
    [SerializeField] private Color _highDamageColor;
    
    public DamageNumber numberPrefab;
    public void DamageCreateUI(Vector2 spawnPos,float damage)
    {
        
        if(damage<250)
           numberPrefab.Spawn(new Vector2(spawnPos.x,spawnPos.y+1f), damage);
        else if(damage<350)
            numberPrefab.Spawn(new Vector2(spawnPos.x,spawnPos.y+1f), damage,_highDamageColor);
        else 
            numberPrefab.Spawn(new Vector2(spawnPos.x,spawnPos.y+1f), damage,_critDamageColor);


    }
}
