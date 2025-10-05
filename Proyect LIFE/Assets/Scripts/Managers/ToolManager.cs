using UnityEngine;

public enum ToolType
{
    None,
    Build,
    Plant,
    Spray,
    Till,
    Harvest
}

public class ToolManager : MonoBehaviour
{
    [Header("Herramientas disponibles")]
    public TilePlacement buildTool;
    public PlantTool plantTool;
    public SprayTool sprayTool;
    public TillerTool tillTool;
    public HarvestTool harvestTool;

    [Header("Prefabs disponibles para construcción")]
    public GameObject soilPrefab;
    public GameObject housePrefab;
    public GameObject greenhousePrefab;

    [Header("Input - Teclas para activar herramientas")]
    public KeyCode buildKey = KeyCode.B;
    public KeyCode plantKey = KeyCode.P;
    public KeyCode sprayKey = KeyCode.R;
    public KeyCode tillKey = KeyCode.T;
    public KeyCode harvestKey = KeyCode.H;
    public KeyCode noneKey = KeyCode.Escape;

    [Header("Input - Teclas para cambiar prefab en Build")]
    public KeyCode soilKey = KeyCode.Alpha1;
    public KeyCode houseKey = KeyCode.Alpha2;
    public KeyCode greenhouseKey = KeyCode.Alpha3;

    private ToolType currentTool = ToolType.None;

    void Start()
    {
        SetTool(ToolType.None);
        Debug.Log("ToolManager iniciado sin herramienta activa.");
    }

    void Update()
    {
        HandleToolSwitchInput();

        // Solo si está activo Build, escuchar cambio de prefab
        if (currentTool == ToolType.Build)
            HandleBuildPrefabInput();
    }

    void HandleToolSwitchInput()
    {
        if (Input.GetKeyDown(buildKey))
        {
            SetTool(ToolType.Build);
        }
        else if (Input.GetKeyDown(plantKey))
        {
            SetTool(ToolType.Plant);
        }
        else if (Input.GetKeyDown(sprayKey))
        {
            SetTool(ToolType.Spray);
        }
        else if (Input.GetKeyDown(tillKey))
        {
            SetTool(ToolType.Till);
        }
        else if (Input.GetKeyDown(harvestKey))
        {
            SetTool(ToolType.Harvest);
        }
        else if (Input.GetKeyDown(noneKey))
        {
            SetTool(ToolType.None);
        }
    }

    void HandleBuildPrefabInput()
    {
        if (Input.GetKeyDown(soilKey))
        {
            SetBuildPrefab(soilPrefab);
        }
        else if (Input.GetKeyDown(houseKey))
        {
            SetBuildPrefab(housePrefab);
        }
        else if (Input.GetKeyDown(greenhouseKey))
        {
            SetBuildPrefab(greenhousePrefab);
        }
    }

    public void SetTool(ToolType newTool)
    {
        if (currentTool == newTool)
        {
            Debug.Log($"Herramienta ya activa: {newTool}");
            return;
        }

        // Desactivar la anterior
        switch (currentTool)
        {
            case ToolType.Build: buildTool?.Deactivate(); break;
            case ToolType.Plant: plantTool?.Deactivate(); break;
            case ToolType.Spray: sprayTool?.Deactivate(); break;
            case ToolType.Till: tillTool?.Deactivate(); break;
            case ToolType.Harvest: harvestTool?.Deactivate(); break;
        }

        // Activar la nueva
        currentTool = newTool;
        switch (currentTool)
        {
            case ToolType.Build:
                buildTool?.Activate(soilPrefab);
                Debug.Log("🧱 Build Tool activada");
                break;

            case ToolType.Plant:
                plantTool?.Activate();
                Debug.Log("🌱 Plant Tool activada");
                break;

            case ToolType.Spray:
                sprayTool?.Activate();
                Debug.Log("💧 Spray Tool activada");
                break;

            case ToolType.Till:
                tillTool?.Activate();
                Debug.Log("🪓 Till Tool activada");
                break;

            case ToolType.Harvest:
                harvestTool?.Activate();
                Debug.Log("🌾 Harvest Tool activada");
                break;

            case ToolType.None:
                Debug.Log("❌ Ninguna herramienta activa");
                break;
        }
    }

    public void SetBuildPrefab(GameObject newPrefab)
    {
        if (currentTool != ToolType.Build)
        {
            Debug.LogWarning("⚠️ No puedes cambiar el prefab si Build no está activo. Presiona 'B' primero.");
            return;
        }

        buildTool?.Activate(newPrefab);
        Debug.Log($"🔄 Prefab cambiado a: {newPrefab.name}");
    }
}
