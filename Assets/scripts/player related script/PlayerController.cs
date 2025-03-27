using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [Header("metrixes")]
    public float moveSpeed = 5f;
    public float tongLength = 5f;
    public float accelerationForce = 0.015f;
    public float BulletTimePositionOffset = 2f;
    public UnityEvent onEnteringBulletTime;

    [Header("a renseigner")] 
    [SerializeField] private Transform camTransorm;
    
    private PlayerDeathBehaviour playerDeathBehaviour;
    
    [HideInInspector]public float actualSpeed = 0f;
    public PlayerControls playerControls;
    [HideInInspector]public Vector3 movementInput;
    private Vector3 directionAtStart;
    [HideInInspector]public bool canMove = true;
    [HideInInspector] public bool isInMotion = false;
    [HideInInspector]public InputAction move;
    private InputAction tong;
    private bool mustExitBulletTime = false;
    [HideInInspector]public bool isInBulletTime = false;
    [HideInInspector]public GameObject actualEncrage;

    [HideInInspector]public Vector3 directionToGo;


    private void Awake()
    {
        playerControls = new PlayerControls();
        playerDeathBehaviour = GetComponent<PlayerDeathBehaviour>();
    }


    void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        tong = playerControls.Player.Fire;
        tong.Enable();
        tong.performed += TryShootTong;
    }

    void OnDisable()
    {
        move.Disable();
        tong.Disable();
    }

    void Update()
    {
        float x = move.ReadValue<Vector2>().x;
        float z = move.ReadValue<Vector2>().y;
        movementInput = RelativeMovementInput(camTransorm, x, z);
        if (isInMotion) canMove = false;
        if (movementInput != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementInput);
            Physics.Raycast(transform.position, transform.forward,  out RaycastHit hit, tongLength);
            if (hit.collider != null && hit.transform.gameObject.GetComponent<NotGrabbable>() == null)
            {
                directionToGo = (hit.point - transform.position).normalized;
            }
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if(movementInput != Vector3.zero)ShootTong(directionToGo);
        }
    }

    public Vector3 RelativeMovementInput(Transform camTransorm, float absoluteX, float absoluteY)
    {
        Vector3 camForward = camTransorm.forward;
        Vector3 camRight = camTransorm.right;
        
        camForward.y = 0;
        camRight.y = 0;
        
        Vector3 forwardRelative = absoluteY * camForward;
        Vector3 rightRelative = absoluteX * camRight;
        Vector3 relativDirection = (forwardRelative + rightRelative).normalized;
        return relativDirection;
    }

    void TryShootTong(InputAction.CallbackContext context)
    {
        if(movementInput != Vector3.zero) ShootTong(directionToGo);
    }
    
    public void ShootTong(Vector3 direction)
    {
        Physics.Raycast(transform.position, direction,  out RaycastHit hit, tongLength);
        if (canMove && direction != Vector3.zero)
        {
            if (hit.transform != null && hit.transform.GetComponent<NotGrabbable>() == null)
            {
                actualEncrage = hit.transform.gameObject;
                directionAtStart = direction;
                mustExitBulletTime = true;
                StartCoroutine(MovePlayerToTarget(hit.point, directionAtStart, hit));
            }
        }
       
    }

    public IEnumerator MovePlayerToTarget(Vector3 targetPoint,Vector3 dirOnStart, RaycastHit hit, bool accelerate = true)
    {
        float speedFactor = 1f;
        isInMotion = true;
        bool interrupted = false;
        while (Vector3.Distance(transform.position, targetPoint) > 0.5f)
        {
            Physics.Raycast(transform.position, dirOnStart, out RaycastHit hitback, tongLength);
            if (hit.collider != hitback.collider)
            {
                interrupted = true;
                break;
            }
            if(accelerate)speedFactor += accelerationForce;
            actualSpeed = moveSpeed * Time.deltaTime * speedFactor;
            transform.position = Vector3.MoveTowards(transform.position, targetPoint, actualSpeed);
            yield return null;
        }
        if(!interrupted)TryDestroyWall(actualSpeed, hit, dirOnStart);
        else playerDeathBehaviour.Death();
        actualSpeed = 0f;
        isInMotion = false;
        canMove = true;
        
    }

    void TryDestroyWall(float speed, RaycastHit hit, Vector3 direction)
    {
        WallDestroy wallDestroy = hit.transform.GetComponent<WallDestroy>();
        if (wallDestroy != null)
        {
            bool wallDestroyed = wallDestroy.TryDestroyWall(speed);
            if (hit.transform != null && wallDestroyed)
            {
                Destroy(hit.transform.gameObject);
                StartCoroutine(BulletTime(direction, BulletTimePositionOffset));
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, tongLength);
    }

    public IEnumerator BulletTime(Vector3 direction, float offset)
    {
        /*
        //check si il y a un mur
        if (Physics.Raycast(transform.position, direction, out RaycastHit wallHit, 0.2f))
        {
            if (wallHit.transform != null)
            {
                mustExitBulletTime = true;
                ShootTong(direction);
                Debug.Log("il y a un mur");
                yield break;
            }
        }
        
        // si non , lance le bullet time
        */
        
        Vector3 targetPoint = transform.position + direction.normalized * offset;
        mustExitBulletTime = false;
        isInBulletTime = true;
        onEnteringBulletTime.Invoke();
        while (!mustExitBulletTime)
        {
            
            transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * moveSpeed);
            Time.timeScale = Mathf.Lerp(Time.timeScale, 0f, Time.deltaTime * moveSpeed);
            yield return null;
        }
        isInBulletTime = false;
        Time.timeScale = 1f;
    }
}

