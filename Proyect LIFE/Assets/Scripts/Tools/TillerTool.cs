using UnityEngine;

public class TillerTool : MonoBehaviour
{
    [Header("Configuración")]
    public Camera playerCamera;        // Cámara del jugador (FPS)
    public float maxDistance = 10f;    // Alcance del arado
    public int fertilityAmount = 1;    // Cuánto aumenta la fertilidad por uso
    public LayerMask soilMask;         // Solo afectar a SoilTile

    private bool isActive = false;

    void Update()
    {
        if (!isActive) return;

        HandleTilling();
    }

    public void Activate()
    {
        isActive = true;
        Debug.Log("🟫 TillerTool ACTIVADO");
    }

    public void Deactivate()
    {
        isActive = false;
        Debug.Log("🟫 TillerTool DESACTIVADO");
    }

    void HandleTilling()
    {
        // Raycast desde el centro de la pantalla
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, soilMask))
        {
            SoilTile soil = hit.collider.GetComponent<SoilTile>();
            if (soil != null)
            {
                // Arar al mantener clic izquierdo
                if (Input.GetMouseButton(0))
                {
                    soil.IncreaseFertility(fertilityAmount);
                }
            }
        }
    }
}
