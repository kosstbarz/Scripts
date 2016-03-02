using UnityEngine;


public class CameraMovementScript : MonoBehaviour {

    public float cameraSpeed;
    public float cameraRotationSpeed;

    Vector3 movement;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        float i = Input.GetAxisRaw("Mouse ScrollWheel");
        float mX = Input.GetAxisRaw("Mouse X");
        float mY = Input.GetAxisRaw("Mouse Y");
        MoveCamera(x, y);
        if (Input.mousePosition.x == 0f)
            MoveCamera(-cameraSpeed, 0);
        
        if (Input.mousePosition.x >= Camera.main.pixelWidth - 10 && Input.mousePosition.x <= Camera.main.pixelWidth - 1)
            MoveCamera(cameraSpeed, 0);

        if (Input.mousePosition.y < 5f)
            MoveCamera(0f, -cameraSpeed);

        if (Input.mousePosition.y >= Camera.main.pixelHeight - 20 &&  Input.mousePosition.y <= Camera.main.pixelHeight - 1)
            MoveCamera(0f, cameraSpeed);

        if (i != 0)
        {
            ZoomCamera(i);
        }
        if (Input.GetMouseButton(2))
        {
            
            RotateCamera(mX, mY);
        }

    }
    
    void MoveCamera(float x, float y)
    {
        transform.Translate(x * cameraSpeed * Time.deltaTime, 0f, 0f);
        Vector3 direction = transform.forward * cameraSpeed * Time.deltaTime;
        direction.y = 0f;
        transform.Translate(direction * y, Space.World);
                
    }
    void ZoomCamera(float i)
    {
        Vector3 zoomAxis = Vector3.forward * i;
        transform.Translate(zoomAxis * cameraSpeed);
    }
    void RotateCamera(float x, float y)
    {
        Vector3 rotateAxis = Vector3.forward;
        Ray centralRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool xRot = false;
        if (Mathf.Abs(x) >= Mathf.Abs(y))
            xRot = true;

        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 axis = new Vector3((xRot ? 0f : -right.x), (xRot ? 1f : 0f), (xRot ? 0f : -right.z));
        if (Physics.Raycast(centralRay, out hit, Mathf.Infinity))
        {
            transform.RotateAround(hit.point, axis, (xRot ? x : y) * cameraRotationSpeed);
        }
        
    }
    void ResetCamera()
    {

    }
    void RotateDownCamera(float s)
    {
        transform.Rotate(-s * cameraRotationSpeed, 0f , 0f);
    }
}
