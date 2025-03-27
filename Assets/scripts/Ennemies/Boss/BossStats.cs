using UnityEngine;
using System;

public class BossStats : MonoBehaviour
{
    public int HealthPoint = 3;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BossTakingDamages bossTakingDamages = GetComponent<BossTakingDamages>();
        bossTakingDamages.onBossTakingDamages += TakeDamages;
    }

    private void TakeDamages(object sender, EventArgs e)
    {
        HealthPoint--;
        if(HealthPoint <= 0)Destroy(gameObject);
    }
}
