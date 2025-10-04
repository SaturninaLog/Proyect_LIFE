using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlantTool : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 5f;
    public CropType cropToPlant = CropType.Corn;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) TryPlant();
    }

    void TryPlant()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            SoilTile tile = hit.collider.GetComponent<SoilTile>();
            if (tile != null && tile.currentCrop == null)
            {
                tile.PlantSeed(cropToPlant); // ahora coincide
                Debug.Log("Plantaste " + cropToPlant + " en " + tile.name);
            }
        }
    }
}

