using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float maxZoom = 300f,
         minZoom = 150f,
         panSpeed = 6f;
        Vector3 bottomLeft, topRight;
        float cameraMaxX, cameraMinX, cameraMaxY, cameraMinY, x, y;
    public Camera cam;

    // Update is called once per frame
    void Start()
    {
        cam = GetComponent<Camera>();
        topRight = cam.ScreenToWorldPoint(
            new Vector3(cam.pixelWidth, cam.pixelHeight, -transform.position.z));
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -transform.position.z));
        cameraMaxX = topRight.x;
        cameraMinX = bottomLeft.x;
        cameraMaxY = topRight.y;
        cameraMinY = bottomLeft.y;
    }
    private void Update()
    {
        x = Input.GetAxis("Mouse X") * panSpeed;
        y = Input.GetAxis("Mouse Y") * panSpeed;
        transform.Translate(x, y, 0);

        if((Input.GetAxis("Mouse Scrollwheel") >0) && cam.orthographicSize > minZoom)
        {
            cam.orthographicSize = cam.orthographicSize - 50f;
        }
        if ((Input.GetAxis("Mouse Scrollwheel") < 0) && cam.orthographicSize < maxZoom)
        {
            cam.orthographicSize = cam.orthographicSize + 50f;
        }
        topRight = cam.ScreenToWorldPoint(
            new Vector3(cam.pixelWidth, cam.pixelHeight, -transform.position.z));
        bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, -transform.position.z));
        if(topRight.x > cameraMaxX)
        {
        if(topRight.y > cameraMaxY)
            {
                transform.position = new Vector3(
                    transform.position.x, transform.position.y - (topRight.y - cameraMaxY), transform.position.z);
            }
        if(bottomLeft.x < cameraMinX)
            {
                transform.position = new Vector3(
                    transform.position.x + (cameraMinX - bottomLeft.x), transform.position.y, transform.position.z);
            }
            if (bottomLeft.y < cameraMinY)
            {
                transform.position = new Vector3(
                    transform.position.x, transform.position.y + (cameraMinY - bottomLeft.y), transform.position.z);
            }

        }
    }
}
