using UnityEngine;

public class RTSCameraController : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float rotateSpeed = 100f;
    public float zoomSpeed = 200f;

    public float minHeight = 10f;
    public float maxHeight = 40f;

    Camera cam;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        Move();
        Rotate();
        Zoom();
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = transform.forward * v + transform.right * h;
        dir.y = 0;

        transform.position += dir * moveSpeed * Time.deltaTime;
    }

    void Rotate()
    {
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.E))
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }

    void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = cam.transform.localPosition;

        pos.y -= scroll * zoomSpeed * Time.deltaTime;

        pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);

        cam.transform.localPosition = pos;
    }
}