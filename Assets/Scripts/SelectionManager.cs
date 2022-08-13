using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager instance;

    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap selectionTilemap;
    [SerializeField] private Tile selectionTile;
    [SerializeField] private Vector3Int selectionPosition;

    [SerializeField] private Vector3Int[] targetPositions;
    
    private void Awake() {
        // Singleton logic
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;

        ResetSelection();
    }

    private void Update() {

        // Left click to make selection
        if (Input.GetMouseButtonDown(0)) {
            // Get world position from camera
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0; // Because camera is -10 from the world
            Vector3Int worldPos = groundTilemap.WorldToCell(pos);

            // For Debugging
            // print(worldPos);

            // Check to see if this is a selection or target
            if (CombatManager.instance.hasActionBeenChoosen()) {
                // Attempt to select target
                CombatManager.instance.SelectTarget(worldPos);
            }

            // Check if selected map has tile
            if (groundTilemap.HasTile(worldPos)) {
                // Reset previous selection if exists
                if (SelectionExists()) {
                    ResetSelection();
                }

                // Cache selected position
                selectionPosition = worldPos;
                selectionTilemap.SetTile(worldPos, selectionTile);
            }
        }

        // Right click to clear any selection
        if (Input.GetMouseButtonDown(1) && SelectionExists()) {
            ResetSelection();
        }
    }

    private bool SelectionExists() {
        return selectionPosition.z != -1;
    }

    private void ResetSelection() {
        if (SelectionExists()) {
            // Delete tile at selected location
            selectionTilemap.SetTile(selectionPosition, null);
        }
        // Reset position
        selectionPosition = new Vector3Int(0, 0, -1);
    }

    public Vector3Int GetSelection() {
        return selectionPosition;
    }

    public void SelectTarget() {

    }
}
