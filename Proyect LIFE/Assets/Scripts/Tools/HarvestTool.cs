using UnityEngine;

public class HarvestTool : MonoBehaviour
{
    public Camera playerCamera;
    public float maxDistance = 10f;
    public LayerMask soilMask;
    public Inventory inventory;  // referencia al sistema de inventario

    private bool isActive = false;

    void Update()
    {
        if (!isActive) return;

        HandleHarvest();
    }

    public void Activate()
    {
        isActive = true;
        Debug.Log("🌾 HarvestTool activado");
    }

    public void Deactivate()
    {
        isActive = false;
        Debug.Log("❌ HarvestTool desactivado");
    }

    private void HandleHarvest()
    {
        // Raycast desde el centro de la pantalla
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, soilMask))
        {
            SoilTile soil = hit.collider.GetComponent<SoilTile>();
            if (soil != null)
            {
                // Detectar clic izquierdo
                if (Input.GetMouseButtonDown(0))
                {
                    TryHarvest(soil);
                }
            }
        }
    }

    private void TryHarvest(SoilTile soil)
    {
        // Buscar cultivos maduros dentro del soil tile (grid 3x3)
        int harvestedCount = 0;

        for (int x = 0; x < soil.gridSize; x++)
        {
            for (int z = 0; z < soil.gridSize; z++)
            {
                var cropVisual = soil.GetCropAt(x, z);
                if (cropVisual != null && cropVisual.cropData != null && cropVisual.cropData.stage == CropStage.Mature)
                {
                    harvestedCount++;
                    string cropName = cropVisual.cropData.type.ToString();

                    // Añadir al inventario
                    if (inventory != null)
                        inventory.AddItem(cropName, 1);

                    // Destruir la planta cosechada
                    Destroy(cropVisual.gameObject);
                    soil.ClearCropAt(x, z);
                }
            }
        }

        if (harvestedCount > 0)
        {
            Debug.Log($"🌾 Cosechaste {harvestedCount} cultivos.");
        }
        else
        {
            Debug.Log("⚠️ No hay cultivos maduros para cosechar.");
        }
    }
}
