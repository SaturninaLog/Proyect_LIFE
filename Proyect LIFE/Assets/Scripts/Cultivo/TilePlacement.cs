using UnityEngine;

public class TilePlacement : MonoBehaviour
{
    [Header("Settings")]
    public Camera playerCamera;
    public LayerMask placementMask = -1; // Solo "Ground" para colocación
    public Material previewMaterial;
    public float maxDistance = 10f;
    public float checkRadius = 0.4f;

    private GameObject prefabToPlace;
    private GameObject previewInstance;
    private Renderer previewRenderer;
    private bool isActive = false;

    void Update()
    {
        if (playerCamera == null)
        {
            Debug.LogError("playerCamera no asignado en TilePlacement!");
            return;
        }

        if (!isActive || prefabToPlace == null) return;

        ShowPreview();
        PlaceObject();
        RemoveObject(); // Eliminación de cualquier objeto construido
    }

    // Activar modo de colocación con un prefab
    public void Activate(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab nulo en Activate!");
            return;
        }

        prefabToPlace = prefab;
        isActive = true;

        // Destruir preview anterior si existe
        if (previewInstance != null)
            Destroy(previewInstance);

        // Crear preview
        previewInstance = Instantiate(prefabToPlace);
        previewRenderer = previewInstance.GetComponent<Renderer>();

        if (previewRenderer != null)
            previewRenderer.material = new Material(previewMaterial);

        // Desactivar collider del preview para no interferir
        Collider col = previewInstance.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // Ocultar inicialmente
        if (previewInstance != null)
            previewInstance.SetActive(false);

        Debug.Log("TilePlacement ACTIVADO con prefab: " + prefab.name);
    }

    // Desactivar modo
    public void Deactivate()
    {
        isActive = false;
        prefabToPlace = null;

        if (previewInstance != null)
            Destroy(previewInstance);
        previewInstance = null;
        previewRenderer = null;

        Debug.Log("TilePlacement DESACTIVADO");
    }

    void ShowPreview()
    {
        if (previewInstance == null) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, placementMask))
        {
            // Snap a grid (redondear a enteros)
            Vector3 pos = hit.point;
            pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
            previewInstance.transform.position = pos;
            previewInstance.SetActive(true); // Mostrar preview

            // Color según validez
            if (CanPlaceHere(pos))
                previewRenderer.material.color = new Color(0, 1, 0, 0.5f); // Verde: Válido
            else
                previewRenderer.material.color = new Color(1, 0, 0, 0.5f); // Rojo: Inválido
        }
        else
        {
            // No hay superficie: Ocultar preview
            previewInstance.SetActive(false);
        }
    }

    void PlaceObject()
    {
        if (Input.GetMouseButtonDown(0)) // Clic izquierdo
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance, placementMask))
            {
                Vector3 pos = hit.point;
                pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));

                if (CanPlaceHere(pos))
                {
                    // Colocar objeto
                    GameObject placedObj = Instantiate(prefabToPlace, pos, Quaternion.identity);

                    // Asignar tag para eliminación fácil (solo si no tiene tag ya)
                    if (string.IsNullOrEmpty(placedObj.tag) || placedObj.tag == "Untagged")
                        placedObj.tag = "PlacedObject";

                    // Activar collider si lo tiene
                    Collider placedCol = placedObj.GetComponent<Collider>();
                    if (placedCol != null) placedCol.enabled = true;

                    Debug.Log("Objeto colocado: " + prefabToPlace.name + " en " + pos);

                    // Opcional: Desactivar después de colocar uno
                    // Deactivate(); // Descomenta para un solo placement
                }
                else
                {
                    Debug.Log("No se puede colocar: Posición ocupada o inválida (e.g., sobre SoilTile).");
                }
            }
        }
    }

    // Verificar si se puede colocar (solo en Ground, sin overlaps)
    bool CanPlaceHere(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, checkRadius);

        foreach (Collider col in colliders)
        {
            GameObject colObj = col.gameObject;

            // Ignorar preview
            if (previewInstance != null && colObj == previewInstance)
                continue;

            // Ignorar Ground (tag o layer)
            if (colObj.CompareTag("Ground") || colObj.layer == LayerMask.NameToLayer("Ground"))
                continue;

            // Bloquear si hay SoilTile
            if (colObj.GetComponent<SoilTile>() != null)
            {
                // Debug.Log("Bloqueado: No se puede colocar sobre SoilTile."); // Opcional, quita si spamea
                return false;
            }

            // Bloquear si hay otro PlacedObject
            if (colObj.CompareTag("PlacedObject"))
            {
                // Debug.Log("Bloqueado: Posición ocupada por otro objeto."); // Opcional
                return false;
            }

            // Cualquier otro collider: Bloquear por seguridad
            return false;
        }

        return true; // Libre
    }

    // ELIMINAR CUALQUIER OBJETO CONSTRUIDO (PlacedObject o SoilTile) - CORREGIDO: Sin duplicado if
    void RemoveObject()
    {
        if (Input.GetMouseButtonDown(1)) // Clic derecho - SOLO UN IF
        {
            Debug.Log("Clic derecho detectado en TilePlacement");

            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast sin mask para detectar todo
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                GameObject obj = hit.collider.gameObject;
                Debug.Log("Raycast golpeó: " + obj.name + " (Tag: " + obj.tag + ")");

                // NO eliminar Ground (tag o layer)
                if (obj.CompareTag("Ground") || obj.layer == LayerMask.NameToLayer("Ground"))
                {
                    Debug.Log("No se puede eliminar el terreno base.");
                    return;
                }

                // Eliminar si es PlacedObject o SoilTile
                if (obj.CompareTag("PlacedObject") || obj.GetComponent<SoilTile>() != null)
                {
                    string objName = obj.name;
                    Destroy(obj);
                    Debug.Log("Objeto eliminado: " + objName);
                }
                else
                {
                    Debug.Log("No es un objeto eliminable: " + obj.name + " (agrega tag 'PlacedObject' si es construido).");
                }
            }
            else
            {
                Debug.Log("No se detectó ningún objeto para eliminar.");
            }
        }
    }
}
