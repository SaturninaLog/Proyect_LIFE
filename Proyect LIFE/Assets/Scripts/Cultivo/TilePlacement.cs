using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePlacement : MonoBehaviour
{
    public Camera playerCamera;            // Cámara en primera persona
    public GameObject soilTilePrefab;      // Prefab del terreno
    public LayerMask placementMask;        // Suelo donde colocar
    public float maxDistance = 10f;        // Rango de colocación

    private GameObject previewTile;        // El "fantasma"
    public Material previewMaterial;       // Material semitransparente para preview

    void Update()
    {
        HandlePlacement();
    }

    void HandlePlacement()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, placementMask))
        {
            Vector3 position = hit.point;
            position = new Vector3(Mathf.Round(position.x), 0, Mathf.Round(position.z)); // ajusta a grid

            // Mostrar preview
            if (previewTile == null)
            {
                previewTile = Instantiate(soilTilePrefab, position, Quaternion.identity);
                SetPreviewMode(previewTile, true);
            }
            else
            {
                previewTile.transform.position = position;
            }

            // Confirmar colocación con clic izquierdo
            if (Input.GetMouseButtonDown(0))
            {
                GameObject placedTile = Instantiate(soilTilePrefab, position, Quaternion.identity);
                SetPreviewMode(placedTile, false); // versión real
            }
        }
    }

    void SetPreviewMode(GameObject tile, bool isPreview)
    {
        var rend = tile.GetComponent<Renderer>();
        if (rend != null)
        {
            if (isPreview)
            {
                rend.material = previewMaterial; // transparente
                tile.GetComponent<Collider>().enabled = false;
            }
            else
            {
                tile.GetComponent<SoilTile>().enabled = true; // habilita el script real
                tile.GetComponent<Collider>().enabled = true;
            }
        }
    }
}
