using UnityEngine;

public class PlantTool : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 10f;
    public LayerMask soilMask;
    public CropType selectedCropType;

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

    private void HandlePlanting()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, soilMask))
        {
            SoilTile soil = hit.collider.GetComponent<SoilTile>();
            if (soil != null && Input.GetMouseButtonDown(0))
            {
                soil.PlantSeed(selectedCropType); // ahora PlantSeed maneja la grilla
            }
        }
    }
}
