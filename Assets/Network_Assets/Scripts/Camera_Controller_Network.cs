using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Camera_Controller_Network : NetworkBehaviour {

    public Transform target;

    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(0, 1.5f, 0);
        public float lookSmooth = 100f;
        public float distanceFromTarget = -8;
        public float zoomSmooth = 100;
        public float maxZoom = -2;
        public float minZoom = -15;
        public bool smoothFollow = true;
        public float smooth = 0.05f;

        [HideInInspector]
        public float adjustmentDistance = -8;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = -20;
        public float yRotation = -180;
        public float maxXRotation = 25;
        public float minXRotation = -85;
        public float vOrbitSmooth = 150;
        public float hOrbitSmooth = 150;
    }

    [System.Serializable]
    public class InputSettings
    {
        public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap";
        public string ORBIT_HORIZONTAL = "OrbitHorizontal";
        public string ORBIT_VERTICAL = "OrbitVertical";
        public string ZOOM = "Mouse ScrollWheel";
    }

    // camera collision codes:
    [System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayer;
        public float sizeOfCollisionSpace = 3.41f;
        [HideInInspector]
        public bool colliding = false;
        [HideInInspector]
        public Vector3[] adjustedCameraClipPoints;  //current camera position clip points
        [HideInInspector]
        public Vector3[] desiredCameraClipPoints;   // the clip points surrounding camera's expected position if it isn't colliding

        Camera camera;

        public void initialise(Camera cam)
        {
            camera = cam;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        public void updateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray) // can be into adjusted/desired clip points
        {
            if (!camera)
                return;

            // clear contents of array
            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / sizeOfCollisionSpace) * z;
            float y = x / camera.aspect;

            // top left clip point
            intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition; // added and rotated point relative to camera
            // top right clip point
            intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition; // added and rotated point relative to camera
            // bottom left clip point
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition; // added and rotated point relative to camera
            // bottom right clip point
            intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition; // added and rotated point relative to camera
            // camera's position
            intoArray[4] = cameraPosition - camera.transform.forward;   // give a little more room behind camera to collide with
        }

        public bool collisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);  // create a ray from fromPosition going towards each clipPoint
                float distance = Vector3.Distance(clipPoints[i], fromPosition);
                if (Physics.Raycast(ray, distance, collisionLayer))  // cast a ray from ray to collision layer, for distance
                {
                    return true;
                }
            }
            return false;
        }

        // return distance camera needs to be from target
        // only useful if there is a collision
        public float getAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1;

            for (int i = 0; i < desiredCameraClipPoints.Length; i++)
            {
                // finding shortest distance between each of the collision
                Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (distance == -1) //if distance haven't been set, set the distance
                        distance = hit.distance;
                    else
                    {
                        if (hit.distance < distance)
                        {
                            distance = hit.distance;    //find minimum distance
                        }
                    }
                }
            }

            // if distance did not change, there is no collision, return 0
            if (distance == -1)
                return 0;
            else
                return distance;
        }

        public void checkColliding(Vector3 targetPosition)
        {
            if (collisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition))
            {
                colliding = true;
            }
            else
            {
                colliding = false;
            }
        }
    }

    [System.Serializable]
    public class DebugSettings
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedCollisionLines = true;
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();
    public DebugSettings debug = new DebugSettings();
    public CollisionHandler collision = new CollisionHandler();

    Vector3 targetPos = Vector3.zero;   // position of target, initialised to zero
    Vector3 destination = Vector3.zero;
    Vector3 adjustedDestination = Vector3.zero; // if colliding use this
    Vector3 camVel = Vector3.zero;
    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput;

	// Use this for initialization
	void Start () {

        if (GameObject.Find("Camera_Destroythis")) { GameObject.Find("Camera_Destroythis").SetActive(false); }
        SetCameraTarget(transform.parent.gameObject.GetComponent<Transform>());
        moveToTarget();

        collision.initialise(Camera.main);
        collision.updateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.updateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);
    }
	
    public void SetCameraTarget(Transform t)    // set the target for camera to follow
                                                // can call to set the camera a new target to look at
    {
        target = t;
        if(target== null)
        {
            Debug.LogError("your camera needs a target");
        }
    }

    void getInput()
    {
        vOrbitInput = Input.GetAxis(input.ORBIT_VERTICAL);
        hOrbitInput = Input.GetAxis(input.ORBIT_HORIZONTAL);
        hOrbitSnapInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL_SNAP);
        zoomInput = Input.GetAxisRaw(input.ZOOM);
    }

    void Update()
    {
        // since these functions don't require any sort of physics based calculation and don't depend on character, put in generic update
        getInput();
        orbitTarget();
        zoomInOnTarget();
    }

    void FixedUpdate()
    {
        collision.updateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.updateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        // draw debug lines here
        for(int i=0; i<5; i++)
        {
            if(debug.drawAdjustedCollisionLines == true)
            {
                Debug.DrawLine(targetPos, collision.adjustedCameraClipPoints[i], Color.green);
            }
            if(debug.drawDesiredCollisionLines == true)
            {
                Debug.DrawLine(targetPos, collision.desiredCameraClipPoints[i], Color.white);
            }
        }

        collision.checkColliding(targetPos);    //using raycasts to check if colliding
        position.adjustmentDistance = collision.getAdjustedDistanceWithRayFrom(targetPos);
        //rotating
        lookAtTarget();
        //moving
        moveToTarget();
    }

    void LateUpdate () {    //only called after Update is called, so moving and rotating is only called after the latest Update to position of target
	
	}

    void moveToTarget()
    {
        targetPos = target.position + position.targetPosOffset;
        // uncomment target.eularAngles.y to follow the rotation of the target when rotating the target
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPos; // camera follows the character
        //transform.position = destination;

        if (collision.colliding)
        {
            adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * Vector3.forward * position.adjustmentDistance;
            adjustedDestination += targetPos;

            if (position.smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination, ref camVel, position.smooth);
            }
            else
            {
                transform.position = adjustedDestination;
            }
        }
        else
        {
            if (position.smoothFollow)
            {
                //use smooth damp function
                transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVel, position.smooth);
            }
            else
            {
                transform.position = destination;
            }
        }
    }

    void lookAtTarget()
    {
        // base our rotation around looking at target's origin
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.lookSmooth * Time.deltaTime);
        
    }

    void orbitTarget()
    {
        if (hOrbitSnapInput > 0) // has it been pressed?
        {
            orbit.yRotation = -180; // place camera behind target
        }


        orbit.xRotation += vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime; // adding x rotation by the direction of input * how fast we want to move in that direction on that rotation
        orbit.yRotation += hOrbitInput * orbit.hOrbitSmooth * Time.deltaTime; // adding x rotation by the direction of input * how fast we want to move in that direction on that rotation
        
        if(orbit.xRotation > orbit.maxXRotation)
        {
            orbit.xRotation = orbit.maxXRotation;
        }
        if(orbit.xRotation < orbit.minXRotation)
        {
            orbit.xRotation = orbit.minXRotation;
        }
    }

    void zoomInOnTarget()
    {
        position.distanceFromTarget += zoomInput * position.zoomSmooth * Time.deltaTime;

        if(position.distanceFromTarget < position.minZoom)
        {
            position.distanceFromTarget = position.minZoom;
        }
        if(position.distanceFromTarget > position.maxZoom)
        {
            position.distanceFromTarget = position.maxZoom;
        }
    }


    
}
