using UnityEngine;

public class CropVisual : MonoBehaviour
{
    public Crop cropData;  // Referencia al Crop lógico
    public Renderer rend;

    void Awake()
    {
        if (rend == null) rend = GetComponent<Renderer>();
    }

    void Update()
    {
        if (cropData == null) return;

        switch (cropData.stage)
        {
            case CropStage.Seed:
                rend.material.color = Color.yellow;
                transform.localScale = Vector3.one * 0.3f;
                break;
            case CropStage.Growing:
                rend.material.color = Color.green;
                transform.localScale = Vector3.one * 0.4f;
                break;
            case CropStage.Mature:
                rend.material.color = new Color(0.6f, 0.3f, 0.1f);
                transform.localScale = Vector3.one * 0.5f;
                break;
            case CropStage.Dead:
                rend.material.color = Color.gray;
                break;
        }
    }

    // Este método permite asignar el Crop desde SoilTile
    public void SetCrop(Crop crop)
    {
        cropData = crop;
    }

    public void Grow(float deltaTime, float soilMoisture, float soilFertility)
    {
        // Solo crecer si cropData existe
        cropData?.Grow(deltaTime, soilMoisture, soilFertility);
    }
}
