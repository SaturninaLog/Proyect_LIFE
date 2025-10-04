using UnityEngine;

public class TilePlacement : MonoBehaviour
{
    public Camera playerCamera;
    public LayerMask placementMask;
    public Material previewMaterial;
    public float maxDistance = 10f;
    public float checkRadius = 0.4f;

    private GameObject prefabToPlace;
    private GameObject previewInstance;
    private Renderer previewRenderer;
    private bool isActive = false;

    void Update()
    {
        if (!isActive || prefabToPlace == null) return;

        ShowPreview();
        PlaceObject();
        RemoveTile(); 
    }

    public void Activate(GameObject prefab)
    {
        prefabToPlace = prefab;
        isActive = true;

        if (previewInstance != null)
            Destroy(previewInstance);

        previewInstance = Instantiate(prefabToPlace);
        previewRenderer = previewInstance.GetComponent<Renderer>();

        if (previewRenderer != null)
            previewRenderer.material = new Material(previewMaterial);

        Collider col = previewInstance.GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    public void Deactivate()
    {
        isActive = false;
        prefabToPlace = null;

        if (previewInstance != null)
            Destroy(previewInstance);
    }

    void ShowPreview()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxDistance, placementMask))
        {
            Vector3 pos = hit.point;
            pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));
            previewInstance.transform.position = pos;

            if (CanPlaceHere(pos))
                previewRenderer.material.color = new Color(0, 1, 0, 0.5f); // Verde
            else
                previewRenderer.material.color = new Color(1, 0, 0, 0.5f); // Rojo
        }
    }

    void PlaceObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxDistance, placementMask))
            {
                Vector3 pos = hit.point;
                pos = new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), Mathf.Round(pos.z));

                if (CanPlaceHere(pos))
                {
                    Instantiate(prefabToPlace, pos, Quaternion.identity);
                    Debug.Log("Objeto colocado: " + prefabToPlace.name);
                }
                else
                {
                    Debug.Log("Bloqueado: ya hay algo o un cultivo en esa casilla.");
                }
            }
        }
    }


    // Chunk para que el jugador pueda contruir solo en parcelas con la etiqueta "Ground"
    // Tambien para que solo pueda contruir si no hay un prefab encima
    bool CanPlaceHere(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, checkRadius);

        foreach (Collider col in colliders)
        {
            // ✅ Ignorar el preview
            if (previewInstance != null && col.gameObject == previewInstance)
                continue;

            // ✅ Ignorar el suelo base si tiene layer o tag específico
            if (col.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                col.gameObject.CompareTag("Ground"))
                continue;

            // ✅ Si detectamos un SoilTile → bloquear construcción encima
            SoilTile soil = col.GetComponent<SoilTile>();
            if (soil != null)
            {
                return false; // No se puede colocar sobre un suelo ya construido
            }

            // ✅ Si detectamos cualquier otro objeto colocado → bloquear
            return false;
        }

        return true; // ✅ Zona libre
    }

    // REMOVER TILES
    void RemoveTile()
    {
        if (Input.GetMouseButtonDown(1)) // clic derecho
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ✅ Sin LayerMask para detectar todo
            if (Physics.Raycast(ray, out hit, 10f))
            {
                GameObject obj = hit.collider.gameObject;

                // Evitar borrar el terreno base
                if (obj.CompareTag("Ground")) return;

                // ✅ Borrar solo si es colocable
                if (obj.CompareTag("PlacedObject") || obj.GetComponent<SoilTile>() != null)
                {
                    Destroy(obj);
                    Debug.Log("Objeto eliminado: " + obj.name);
                }
                else
                {
                    Debug.Log("No es un objeto eliminable: " + obj.name);
                }
            }
        }
    }





}
