using UnityEngine;
using System;

public class BossVisualsTrigger : MonoBehaviour
{
    public Animation bossTakingDamageAnimation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BossTakingDamages bossTakingDamages = GetComponent<BossTakingDamages>();
        bossTakingDamages.onBossTakingDamages += TakeDamages;
    }

    private void TakeDamages(object sender, EventArgs e)
    {
        bossTakingDamageAnimation.Play();
    }
}
