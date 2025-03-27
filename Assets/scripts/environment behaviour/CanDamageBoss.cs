using UnityEngine;

public class CanDamageBoss : MonoBehaviour
{
    public void TryDamageBoss(RaycastHit hit, float  speed)
    {
        BossTakingDamages bossTakingDamages = hit.transform.GetComponent<BossTakingDamages>();
        if (bossTakingDamages != null)
        {
            bossTakingDamages.TakeDamages(speed);
        }
    }
}