using UnityEngine;

public class SoilTile : MonoBehaviour
{
    [Header("Propiedades del suelo")]
    public float fertility = 0.5f;        // calidad del suelo (0–1)
    public int moistureLevel = 0;         // 0 = seco, 1 = medio, 2 = húmedo
    public float minFertilityToPlant = 0.2f;

    [Header("Crops")]
    public GameObject cropPrefab;         // prefab de la planta
    public int gridSize = 3;              // 3x3 grid
    private CropVisual[,] cropsGrid;
    public bool hasCrop = false;
    public Crop currentCrop;

    private Renderer rend;

    [Header("Espaciado de semillas")]
    public float cellSpacing = 0.4f;      // Distancia entre semillas

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (rend != null) rend.material = new Material(rend.material);

        // Inicializar grilla
        cropsGrid = new CropVisual[gridSize, gridSize];
        UpdateVisuals();
    }

    void Update()
    {
        UpdateVisuals();

        // Convertimos moistureLevel a valor float normalizado (0–1)
        float normalizedMoisture = Mathf.Clamp01(moistureLevel / 2f);

        // Hacer crecer todas las semillas en la grilla
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                if (cropsGrid[x, z] != null && cropsGrid[x, z].cropData != null)
                {
                    cropsGrid[x, z].Grow(Time.deltaTime, normalizedMoisture, fertility);
                }
            }
        }
    }

    // 🌱 Plantar semilla en la posición de la grilla
    public void PlantSeed(CropType type)
    {
        if (fertility < minFertilityToPlant)
        {
            Debug.Log("⚠️ Suelo no lo suficientemente fértil para plantar.");
            return;
        }

        // Buscar la primera celda vacía
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                if (cropsGrid[x, z] == null)
                {
                    Vector3 localPos = GetGridPosition(x, z);
                    GameObject cropObj = Instantiate(cropPrefab, transform.position + localPos, Quaternion.identity, transform);

                    CropVisual cv = cropObj.GetComponent<CropVisual>();
                    cv.SetCrop(new Crop(type));

                    cropsGrid[x, z] = cv;
                    Debug.Log($"🌱 Semilla plantada en {x},{z}");
                    return; // Ya plantamos, salimos
                }
            }
        }

        Debug.Log("⚠️ Todas las posiciones de la grilla están ocupadas.");
    }

    // Obtener la posición local de la grilla
    private Vector3 GetGridPosition(int x, int z)
    {
        float start = -cellSpacing * (gridSize - 1) / 2f;
        return new Vector3(start + x * cellSpacing, 0.5f, start + z * cellSpacing);
    }

    // 💧 Subir un nivel de humedad
    public void ApplyWater()
    {
        if (moistureLevel < 2)
        {
            moistureLevel++;
            Debug.Log($"💧 Humedad aumentada a nivel {moistureLevel}");
        }
    }

    // 🌱 Incrementar fertilidad
    public void IncreaseFertility(float amount)
    {
        fertility = Mathf.Clamp01(fertility + amount);
        Debug.Log($"🌱 Fertilidad ahora: {fertility}");
    }

    public void UpdateVisuals()
    {
        if (rend == null) return;

        Color baseColor = new Color(0.6f, 0.45f, 0.3f);

        // Oscurecer por humedad
        float moistureFactor = 0f;
        switch (moistureLevel)
        {
            case 0: moistureFactor = 0f; break;
            case 1: moistureFactor = 0.2f; break;
            case 2: moistureFactor = 0.4f; break;
        }

        Color darkened = new Color(
            Mathf.Clamp01(baseColor.r - moistureFactor),
            Mathf.Clamp01(baseColor.g - moistureFactor),
            Mathf.Clamp01(baseColor.b - moistureFactor)
        );

        // Ligeramente más verde si hay fertilidad
        Color fertilityTint = new Color(0f, 0.15f, 0f);
        darkened = Color.Lerp(darkened, darkened + fertilityTint, fertility * 0.5f);

        rend.material.color = darkened;
    }
}
