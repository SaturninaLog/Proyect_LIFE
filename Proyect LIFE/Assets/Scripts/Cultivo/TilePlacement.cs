using UnityEngine;

public class TilePlacement : MonoBehaviour
{
    public Camera playerCamera;
    public LayerMask placementMask;
    public Material previewMaterial;

    private GameObject currentPrefab;
    private GameObject previewInstance;
    private Renderer previewRenderer;
    private bool isActive = false;

    public void Activate(GameObject prefab)
    {
        currentPrefab = prefab;
        isActive = true;

        if (previewInstance != null) Destroy(previewInstance);

        previewInstance = Instantiate(currentPrefab);
        previewRenderer = previewInstance.GetComponent<Renderer>();
        if (!previewRenderer) previewRenderer = previewInstance.AddComponent<MeshRenderer>();

        previewRenderer.material = new Material(previewMaterial);

        Collider col = previewInstance.GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    public void Deactivate()
    {
        isActive = false;
        if (previewInstance != null) Destroy(previewInstance);
    }

    void Update()
    {
        if (!isActive || currentPrefab == null) return;

        // Ray desde el centro de la cámara
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 50f, placementMask))
        {
            Vector3 pos = new Vector3(Mathf.Round(hit.point.x), hit.point.y, Mathf.Round(hit.point.z));
            previewInstance.transform.position = pos;
            previewRenderer.material.color = new Color(0f, 1f, 0f, 0.5f);

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(currentPrefab, pos, Quaternion.identity);
            }
        }
        else
        {
            previewRenderer.material.color = new Color(1f, 0f, 0f, 0.5f);
        }
    }
}
