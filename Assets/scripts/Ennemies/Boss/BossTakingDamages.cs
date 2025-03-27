using UnityEngine;
using UnityEngine.Events;
using System;

public class BossTakingDamages : MonoBehaviour
{
    public event EventHandler onBossTakingDamages;
    [SerializeField] private float minSpeedToDamageBoss = 0.001f;
    
    public void TakeDamages(float speed)
    {
        if (onBossTakingDamages != null)
        {
            if(speed > minSpeedToDamageBoss) onBossTakingDamages(this, EventArgs.Empty);
        }
    }
}
