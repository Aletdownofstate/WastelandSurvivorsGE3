using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] Camera miniMapCamera;

    public float panSpeed = 15f;
    public float panBorderThickness = 20f;
    public Vector2 panLimit;
    public float scrollSpeed = 28f;
    public float minY = 20f;
    public float maxY = 120f;

    public bool canPlayerContol = false;

    [SerializeField] private GameObject workerPrefab; // For Testing
    [SerializeField] private GameObject spawnPointD; // For Testing

    private void Update()
    {
        // For Testing

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ResourceManager.Instance.AddResource("Food", 1000);
            ResourceManager.Instance.AddResource("Wood", 1000);
            ResourceManager.Instance.AddResource("Metal", 1000);
            ResourceManager.Instance.AddResource("Water", 1000);
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {            
            GameObject worker = Instantiate(workerPrefab, spawnPointD.transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            TimeManager.Instance.daysRemaining = 0;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameManager.Instance.currentGameState = GameManager.GameState.End;
        }

        // End Testing

        if (Input.GetKeyDown(KeyCode.H))
        {
            playerCamera.transform.position = new Vector3(10, 10, -2.99f);            
            miniMapCamera.transform.position = new Vector3(8.5f, 34.5f, 9.5f);

            Debug.Log("Returning camera to homepoint");
        }
    }

    private void LateUpdate()
    {
        if (canPlayerContol)
        {
            Vector3 pos = playerCamera.transform.position;
            Vector3 mapPos = miniMapCamera.transform.position;

            if (Input.GetKey("w") || Input.GetKey("up") || Input.mousePosition.y >= Screen.height - panBorderThickness)
            {
                pos.z += panSpeed * Time.deltaTime;
                mapPos.z += panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("s") || Input.GetKey("down") || Input.mousePosition.y <= panBorderThickness)
            {
                pos.z -= panSpeed * Time.deltaTime;
                mapPos.z -= panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("a") || Input.GetKey("left") || Input.mousePosition.x <= panBorderThickness)
            {
                pos.x -= panSpeed * Time.deltaTime;
                mapPos.x -= panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("d") || Input.GetKey("right") || Input.mousePosition.x >= Screen.width - panBorderThickness)
            {
                pos.x += panSpeed * Time.deltaTime;
                mapPos.x += panSpeed * Time.deltaTime;
            }

            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * scrollSpeed * 100 * Time.deltaTime;

            pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);
            mapPos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
            mapPos.y = Mathf.Clamp(pos.y, minY, maxY);
            mapPos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

            playerCamera.transform.position = pos;
            miniMapCamera.transform.position = mapPos;
        }
    }    
}