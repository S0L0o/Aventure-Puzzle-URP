using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;         
    public float positionSmoothSpeedWhenFollow = 0.125f;
    public float rotationSmoothSpeed = 3f;
    public Vector3 basicOffset;
    public Quaternion rotationOnPlayerFocus;
    
    
    [HideInInspector]public bool mustFollowPlayerPosition = true;
    [HideInInspector]public bool MustBeBasicRotation = true;
    private Vector3 velocity = Vector3.zero;
    [HideInInspector]public Vector3 desiredPosition;
    [HideInInspector]public Quaternion desiredRotation;
    private float actualCamSpeed;
    private Vector3 actualCamOffset;

    void Start()
    {
        actualCamOffset = basicOffset;
        desiredPosition = target.position + basicOffset;
        desiredRotation = transform.rotation;
        actualCamSpeed = positionSmoothSpeedWhenFollow;
    }
    
    void LateUpdate()
    {
        if (mustFollowPlayerPosition)
        { 
            desiredPosition = target.position + actualCamOffset;
        }
        
        if (MustBeBasicRotation)
        {
            desiredRotation = rotationOnPlayerFocus;
        }
        
        
        
        
        
        
        
        float angleDiff = Quaternion.Angle(transform.rotation, desiredRotation);
        if (angleDiff > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSmoothSpeed);
        }
        else 
        {
            transform.rotation = desiredRotation;
        }
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, actualCamSpeed);
       }

    public void ChangeCameraModeToStatic(bool isUnfixed ,Vector3 newCameraPosition , Quaternion newCameraRotation,  float newCamSpeed)
    {
        mustFollowPlayerPosition = isUnfixed;
        MustBeBasicRotation = isUnfixed;
        
        actualCamSpeed = newCamSpeed;
        
        if (!isUnfixed)
        {
            actualCamSpeed = newCamSpeed;
            desiredRotation = newCameraRotation;
            desiredPosition = newCameraPosition;
        }
    }

    public void ChangeCameraModeToFollowPlayer(bool stop, Vector3 newOffset, Quaternion newCameraRotation, float newCamSpeed)
    {
        MustBeBasicRotation = stop;

        if (!stop)
        {
            actualCamSpeed = newCamSpeed;
            actualCamOffset = newOffset;
            desiredRotation = newCameraRotation;
        }
        else
        {
            actualCamSpeed = newCamSpeed;
            actualCamOffset = basicOffset;
            desiredRotation = rotationOnPlayerFocus;
        }
    }
}