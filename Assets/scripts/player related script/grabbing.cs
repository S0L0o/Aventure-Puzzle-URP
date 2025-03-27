using System.Collections;
using UnityEngine;

public class Grabbing : MonoBehaviour
{
    private PlayerController playerController;
    public GameObject grabbedMovementPrevisualisation;
    [SerializeField]private float blocWallDistance = 0.2f;
    public float projectionForce = 5f;
    [SerializeField]private float blocMoveSpeed = 2f;
    private bool buttonPressed = false;
    public bool isGrabbing = false;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        grabbedMovementPrevisualisation = Instantiate(grabbedMovementPrevisualisation);
        grabbedMovementPrevisualisation.SetActive(false);
    }

    void Update()
    {
        if (( Input.GetKeyDown(KeyCode.JoystickButton1) ||Input.GetMouseButtonDown(1)) && !playerController.isInMotion)
        {
            buttonPressed = true;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, playerController.movementInput, out hit, playerController.tongLength) && !playerController.isInBulletTime)
            {
                TryGrabbingObject(hit);
            }
        }

        if (Input.GetKeyUp(KeyCode.JoystickButton1) || Input.GetMouseButtonUp(1))
        {
            buttonPressed = false;
        }
        
    }

    void TryGrabbingObject(RaycastHit hit)
    {
        if (hit.transform == null) return;

        MovableObject movableObject = hit.transform.GetComponent<MovableObject>();
        if (movableObject != null && !movableObject.isMoving)
        {
            StartCoroutine(WaitBeforeMovingObject(hit.transform, movableObject));
        }
    }

    IEnumerator WaitBeforeMovingObject(Transform transformToMove, MovableObject movableObject)
    {
        isGrabbing = true;
        grabbedMovementPrevisualisation.GetComponent<MeshFilter>().mesh =
            transformToMove.GetComponent<MeshFilter>().mesh;
        grabbedMovementPrevisualisation.transform.localScale = transformToMove.localScale;
        grabbedMovementPrevisualisation.transform.rotation = transformToMove.rotation;
        grabbedMovementPrevisualisation.SetActive(true);
        while (buttonPressed)
        {
            Physics.BoxCast(transformToMove.position, transformToMove.localScale / 2, playerController.movementInput.normalized, out RaycastHit pravisualisationHit, transformToMove.rotation, projectionForce);
            Vector3 previsualisationPosition;
            if (pravisualisationHit.collider != null)
            {
                previsualisationPosition = pravisualisationHit.point;
            }
            else
            {
                previsualisationPosition = transformToMove.position + playerController.movementInput.normalized * projectionForce;
                
            }
            previsualisationPosition.y = transformToMove.position.y;
            grabbedMovementPrevisualisation.transform.position = previsualisationPosition;
            Debug.DrawLine(transformToMove.position, previsualisationPosition, Color.red);
            yield return null;
        }
        grabbedMovementPrevisualisation.SetActive(false);
        
        if (playerController.movementInput != Vector3.zero && !movableObject.isMoving)
        {
            if (transformToMove.gameObject == playerController.actualEncrage)
            {
                StartCoroutine(playerController.BulletTime(-playerController.movementInput, 0.5f));
            }
            StartCoroutine(MoveObject(transformToMove, playerController.movementInput.normalized, movableObject));
        }
        isGrabbing = false;
    }

    IEnumerator MoveObject(Transform target, Vector3 direction, MovableObject movableObject)
    {
        Vector3 startPos = target.position;
        Vector3 endPos = startPos + direction * projectionForce;
        
        movableObject.StartCoroutine(movableObject.WaitUnitilCollision(direction));
        movableObject.blocWallDistance = blocWallDistance;

        while (!movableObject.obstacleHited)
        {
            movableObject.selfVelocity = Vector3.Distance(target.position, Vector3.Lerp(target.position, endPos, Time.deltaTime));
            target.position = Vector3.Lerp(target.position, endPos, Time.deltaTime * blocMoveSpeed);

            if (Vector3.Distance(target.position, endPos) < .1f)
            {
                target.position = endPos;
                movableObject.StopMoving();
            }
            yield return null;
        }
        movableObject.obstacleHited = false;
    }
}
