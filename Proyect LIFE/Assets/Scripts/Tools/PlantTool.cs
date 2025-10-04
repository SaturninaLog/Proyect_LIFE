using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTool : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 5f;
    public LayerMask soilMask;
    public GameObject seedPrefab;   // 👈 Arrastra aquí el prefab de la planta o semilla

    private bool isActive = false;

    void Update()
    {
        if (!isActive) return;

        HandlePlanting();
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    void HandlePlanting()
    {
        // Raycast desde el centro de la pantalla
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, soilMask))
        {
            if (Input.GetMouseButtonDown(0))
            {
                SoilTile soil = hit.collider.GetComponent<SoilTile>();

                if (soil != null)
                {
                    // 🌱 Aquí plantamos la semilla (el prefab se instancia dentro del SoilTile)
                    //soil.PlantSeed(seedPrefab);
                }
            }
        }
    }
}
