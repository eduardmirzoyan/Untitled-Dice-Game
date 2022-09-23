using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    
    [Header("Targets")]
    [SerializeField] private Transform defaultTarget;
    [SerializeField] private List<Transform> targets;

    [Header("Settings")]
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.5f;

    [SerializeField] private float minZoom = 40f;
    [SerializeField] private float maxZoom = 10f;
    [SerializeField] private float zoomLimiter = 50f;

    [SerializeField] private bool isEnabled;

    private Camera cam;
    private Vector3 velocity;

    private void Awake()
    {
        // Singleton logic
        if (instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;

        cam = GetComponent<Camera>();

        // Set to default
        targets.Add(defaultTarget);
    }

    private void Start() {
        // If this is not enabled then don't sub
        if (!isEnabled) return;

        // Sub to events
        CombatEvents.instance.onActionConfirm += OnActionConfirm;
        CombatEvents.instance.onTurnEnd += OnTurnEnd;
    }

    private void OnActionConfirm(Combatant source, Action action, Dice dice, Combatant target) {
        // Select the targeter and targetee as the focus
        if (target != null) {
            // Clear any targets first
            targets.Clear();
            // Add current combatant
            targets.Add(source.modelTransform);
            // Add target
            targets.Add(target.modelTransform);
        }
    }

    private void OnTurnEnd(int value) {
        // Clear any targets
        targets.Clear();
        // Set to default
        targets.Add(defaultTarget);
    }
    
    private void LateUpdate() {
        // Make sure you have more than 1 target
        if (targets.Count == 0) return;

        // Move the camera to the center of targets
        MoveCamera();

        // Zoom camera to better capture the targets
        ZoomCamera();
    }

    private void ZoomCamera() {
        // Calculate new zoom level
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        
        // If we only have 1 target, then set the zoom to min
        if (targets.Count == 1) {
            newZoom = minZoom;
        }
        
        // Change FoV based on new zoom
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    private float GetGreatestDistance() {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (var target in targets) {
            bounds.Encapsulate(target.position);
        }
        
        return bounds.size.x;
    }

    private void MoveCamera() {
        // Get center point between all the targets
        Vector3 centerPoint = GetCenterPoint();

        // Set new position to centerpoint +offset
        Vector3 newPosition = centerPoint + offset;

        // Smoothly move camera
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private Vector3 GetCenterPoint() {
        if (targets.Count == 1) {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach (var target in targets) {
            bounds.Encapsulate(target.position);
        }

        return bounds.center;
    }
}
