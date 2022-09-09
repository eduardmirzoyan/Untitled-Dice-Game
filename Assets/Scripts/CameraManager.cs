using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] private float panSpeed = 20f;
    [SerializeField] private float panBorderThickness = 10f;
    [SerializeField] private Vector2 panLimit;
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float maxY = 20f;
    [SerializeField] private float minY = 20f;
    [SerializeField] private float panCoef = 0.25f;
    [SerializeField] private bool debugMode;

    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        // If debugging, draw a box of border thickness
        if (debugMode) {
            Vector3 bottomLeft = new Vector3(panBorderThickness, panBorderThickness, 0);
            Vector3 topRight = new Vector3(Screen.width - panBorderThickness, Screen.height - panBorderThickness, 0);
            Debug.DrawRay(bottomLeft, Vector3.up * 2000, Color.red, 60f);
            Debug.DrawRay(bottomLeft, Vector3.right * 2000, Color.red, 60f);
            Debug.DrawRay(topRight, Vector3.down * 2000, Color.red, 60f);
            Debug.DrawRay(topRight, Vector3.left * 2000, Color.red, 60f);
        }
    }
    
    private void Update() {
        // IN PROGRESS
        Vector3 cameraPosition = transform.position;

        //if (debugMode) return;

        // Debug, reset
        if(Input.GetKeyDown(KeyCode.P)) {
            cameraPosition = Vector3.zero;
        }

        // Pan up
        if (Input.mousePosition.y >= Screen.height - panBorderThickness) 
        {
            //cameraPosition.y += panSpeed * Time.deltaTime;
            cameraPosition.y = Mathf.Lerp(cameraPosition.y, panLimit.y, panCoef);
        }

        // Pan down
        if (Input.mousePosition.y <= panBorderThickness)
        {
            //cameraPosition.y -= panSpeed * Time.deltaTime;
            cameraPosition.y = Mathf.Lerp(cameraPosition.y, -panLimit.y, panCoef);
        }

        // Pan right
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            //cameraPosition.x += panSpeed * Time.deltaTime;
            cameraPosition.x = Mathf.Lerp(cameraPosition.x, panLimit.x, panCoef);
        }

        // Pan left
        if (Input.mousePosition.x <= panBorderThickness)
        {
            //cameraPosition.x -= panSpeed * Time.deltaTime;
            cameraPosition.x = Mathf.Lerp(cameraPosition.x, -panLimit.x, panCoef);
        }
        
        // Limit Panning
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, -panLimit.x, panLimit.x);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, -panLimit.y, panLimit.y);

        // Check zooming with scrolling
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cameraPosition.z -= scroll * zoomSpeed * 100 * Time.deltaTime;

        // Limit zooming
        cameraPosition.z = Mathf.Clamp(cameraPosition.z, minY, maxY);

        print("w: " + Screen.width);
        print("h: " + Screen.height);

        transform.position = cameraPosition;
    }

    private void OnDrawGizmos()
    {
        if (debugMode) {
            // Draw a red cube to indicate area where u can pan
            Gizmos.color = Color.red;
            Vector3 pos = transform.position;
            pos.z = 0;
            Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(panBorderThickness, panBorderThickness, 0));
            Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - panBorderThickness, Screen.height - panBorderThickness, 0));
            Gizmos.DrawWireCube(pos, new Vector3(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y, 0));
        }
    }
}
