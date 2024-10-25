using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 15f;
    public float panBorderThickness = 20f;
    public Vector2 panLimit;
    public float scrollSpeed = 28f;
    public float minY = 20f;
    public float maxY = 120f;

    public bool canPlayerContol = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ResourceManager.Instance.AddResource("Food", 1000);
            ResourceManager.Instance.AddResource("Wood", 1000);
            ResourceManager.Instance.AddResource("Metal", 1000);
            ResourceManager.Instance.AddResource("Water", 1000);
        }
    }

    private void LateUpdate()
    {
        if (canPlayerContol)
        {
            Vector3 pos = transform.position;

            if (Input.GetKey("w") || Input.GetKey("up") || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                pos.z += panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("s") || Input.GetKey("down") || Input.mousePosition.y <= panBorderThickness)
            {
                pos.z -= panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("a") || Input.GetKey("left") || Input.mousePosition.x <= panBorderThickness)
            {
                pos.x -= panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("d") || Input.GetKey("right") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                pos.x += panSpeed * Time.deltaTime;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * scrollSpeed * 100 * Time.deltaTime;

            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

            transform.position = pos;
        }
    }    
}