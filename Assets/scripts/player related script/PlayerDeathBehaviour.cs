using UnityEngine;

public class PlayerDeathBehaviour : MonoBehaviour
{
    public Transform SpawnPosition;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.transform.position = SpawnPosition.position;
    }

    public void Death()
    {
        gameObject.transform.position = SpawnPosition.position;
        Debug.Log("Player Death");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
