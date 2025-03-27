using UnityEngine;
using System;

public class BossMovementBehaviour : MonoBehaviour
{
    public Transform[] bossPositions;
    private int currentPosition = 0;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //if(bossPositions[0] == null || bossPositions.Length == 0)this.enabled = false;
        if(bossPositions.Length == 0)this.enabled = false;
        else if(bossPositions[0] == null) this.enabled = false;
    }
    
    void Start()
    {
        BossTakingDamages bossTakingDamages = GetComponent<BossTakingDamages>();
        bossTakingDamages.onBossTakingDamages += GoToNextPosition;
        
        transform.position = bossPositions[0].position;
    }

    // Update is called once per frame
    void GoToNextPosition(object sender, EventArgs e)
    {
        if (currentPosition < bossPositions.Length - 1)currentPosition++;
        transform.position = bossPositions[currentPosition].position;
    }
}
