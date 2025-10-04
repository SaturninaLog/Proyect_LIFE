using UnityEngine;

public class SprayTool : MonoBehaviour
{
    public Camera playerCamera;        // Cámara del jugador (FPS)
    public float maxDistance = 10f;    // Alcance del spray
    public float waterAmount = 0.1f;   // Cuánto aumenta la humedad por spray
    public LayerMask soilMask;         // Solo afectar a SoilTile

    private bool isActive = false;

    private float sprayCooldown = 0.5f; // medio segundo entre aumentos
    private float sprayTimer = 0f;


    void Update()
    {
        if (!isActive) return;
        sprayTimer += Time.deltaTime;
        HandleSpray();
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    void HandleSpray()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, soilMask))
        {
            SoilTile soil = hit.collider.GetComponent<SoilTile>();
            if (soil != null && Input.GetMouseButton(0) && sprayTimer >= sprayCooldown)
            {
                soil.ApplyWater();
                sprayTimer = 0f;
            }
        }
    }
}
