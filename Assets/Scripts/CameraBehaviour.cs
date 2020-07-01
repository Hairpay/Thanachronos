using UnityEngine;

/// <summary>
/// Defines complete behaviour of the camera
/// </summary>
public class CameraBehaviour : MonoBehaviour
{
    private Transform _cameraTransform;
    private Vector3 finalCameraPos;
    public float yPos = -0.5f;
    public float maxPosLeft;
    public float maxPosRight;
    public float fixedCameraPos = 0;
    public float t;

    private float smoothVelocityX = 0.1f;
    public float horizontalSmoothTime = 0.1f;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
        _cameraTransform.position = gameObject.transform.position;
    }

    private void Update()
    {
        if (fixedCameraPos == 0)
        {
            finalCameraPos = new Vector3(this.transform.position.x, yPos, -10f);
            finalCameraPos.x = Mathf.SmoothDamp(_cameraTransform.position.x, finalCameraPos.x, ref smoothVelocityX, horizontalSmoothTime);

            if (finalCameraPos.x < maxPosLeft)
            {
               finalCameraPos = new Vector3(maxPosLeft, yPos, -10f);
            }
            if (finalCameraPos.x > maxPosRight)
            {
                finalCameraPos = new Vector3(maxPosRight, yPos, -10f);
            }       
        }
        else if (t < 1.2)
        {
            finalCameraPos = new Vector3(Mathf.Lerp(_cameraTransform.position.x, fixedCameraPos,t), yPos, -10f);
            t += 0.1f * Time.deltaTime;
        }
        
     
        _cameraTransform.position = finalCameraPos;
    }
    public void fixCamera (float xpos)
    {
        fixedCameraPos = xpos;
        t = 0;
    }
}