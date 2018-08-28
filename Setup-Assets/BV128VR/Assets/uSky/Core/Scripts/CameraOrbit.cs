using UnityEngine; 
using System.Collections; 

public class CameraOrbit : MonoBehaviour 
{ 
    public Transform target; 
    
    public float targetHeight = 1.7f; 
    public float distance = 10.0f;
//    public float offsetFromWall = 0.1f;

    public float maxDistance = 20; 
    public float minDistance = .6f; 

    public float xSpeed = 200.0f; 
    public float ySpeed = 200.0f; 

    public int yMinLimit = -80; 
    public int yMaxLimit = 80; 

    public int zoomRate = 40; 

    public float rotationDampening = 3.0f; 
    public float zoomDampening = 5.0f; 
    
	public float LeftBorder   = 0f;
	public float TopBorder    = 0f;
//	public float RightBorder = 0f;
//	public float BottomBorder = 0f;

//    public LayerMask collisionLayers = -1;

    private float xDeg = 0.0f; 
    private float yDeg = 0.0f; 
    private float currentDistance; 
    private float desiredDistance; 
    private float correctedDistance; 
	

    void Start () 
    { 
        Vector3 angles = transform.eulerAngles; 
        xDeg = angles.y; 
        yDeg = angles.x; 

        currentDistance = distance; 
        desiredDistance = distance; 
        correctedDistance = distance; 

        // Make the rigid body not change rotation 
//        if (rigidbody) 
//            rigidbody.freezeRotation = true; 
    } 

    /** 
     * Camera logic on LateUpdate to only update after all character movement logic has been handled. 
     */ 
    void LateUpdate () 
    { 

    	Vector3 vTargetOffset;
    	
       // Don't do anything if target is not defined 
        if (!target) 
            return; 

        // If either mouse buttons are down, let the mouse govern camera position 
        if (Input.GetMouseButton(0)) 
        { 
			if (!Input.GetKey ("mouse 0") || 
			    Input.mousePosition.x < LeftBorder && Input.mousePosition.y > Screen.height - TopBorder) { // mask out upper left
				return;
			}

            xDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.02f; 
            yDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f; 
        } 
        // otherwise, ease behind the target if any of the directional keys are pressed 
        else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0) 
        { 
            float targetRotationAngle = target.eulerAngles.y; 
            float currentRotationAngle = transform.eulerAngles.y; 
            xDeg = Mathf.LerpAngle (currentRotationAngle, targetRotationAngle, rotationDampening * Time.deltaTime); 
        } 

        yDeg = ClampAngle (yDeg, yMinLimit, yMaxLimit); 

        // set camera rotation 
        Quaternion rotation = Quaternion.Euler (yDeg, xDeg, 0); 

        // calculate the desired distance 
        desiredDistance -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs (desiredDistance); 
        desiredDistance = Mathf.Clamp (desiredDistance, minDistance, maxDistance); 
        correctedDistance = desiredDistance; 

        // calculate desired camera position
        vTargetOffset = new Vector3 (0, -targetHeight , 0);
        Vector3 position = target.position - (rotation * Vector3.forward * desiredDistance + vTargetOffset); 		

        bool isCorrected = false; 

        // For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance 
        currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp (currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance; 

		// keep within legal limits
        currentDistance = Mathf.Clamp (currentDistance, minDistance, maxDistance); 

        // recalculate position based on the new currentDistance 
        position = target.position - (rotation * Vector3.forward * currentDistance + vTargetOffset); 
        
        transform.rotation = rotation; 
        transform.position = position; 
    } 

    private static float ClampAngle (float angle, float min, float max) 
    { 
        if (angle < -360) 
            angle += 360; 
        if (angle > 360) 
            angle -= 360; 
        return Mathf.Clamp (angle, min, max); 
    } 
} 