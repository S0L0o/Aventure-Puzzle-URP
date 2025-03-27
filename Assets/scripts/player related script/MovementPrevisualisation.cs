using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MovementPrevisualisation : MonoBehaviour
{
    public GameObject previsualisation;
    private GameObject currentPrevisualisation;
    private PlayerController playerController;
    private Grabbing grabbing;

    private Vector3 empty;
    
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        grabbing = GetComponent<Grabbing>();
    }

    void Start()
    {
        currentPrevisualisation = Instantiate(previsualisation, transform.position, Quaternion.identity);
        currentPrevisualisation.SetActive(false);
    }
    

    void Update()
    {
        if(playerController.movementInput != Vector3.zero && !grabbing.isGrabbing)Previsualize();
        else currentPrevisualisation.SetActive(false);
    }

    void Previsualize()
    {
        if (Physics.Raycast(transform.position, playerController.directionToGo, out RaycastHit hit, playerController.tongLength))
        {
            if (hit.transform.GetComponent<NotGrabbable>() != null)
            {
                currentPrevisualisation.SetActive(false);
            }
            else
            {
                currentPrevisualisation.SetActive(true);
                currentPrevisualisation.transform.position = hit.point;
    
                RaycastHit endLine;
                Physics.Raycast(transform.position, playerController.directionToGo, out endLine);
                Debug.DrawLine(transform.position, endLine.point, Color.blue);
            }
        }
        
        
    }
}
