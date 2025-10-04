using UnityEngine;

public enum ToolType { None, Build, Plant, Spray, Tiller }

public class ToolManager : MonoBehaviour
{
    [Header("Herramientas")]
    public TilePlacement buildTool;
    public PlantTool plantTool;
    public SprayTool sprayTool;
    public TillerTool tillerTool;

    [Header("Prefabs disponibles")]
    public GameObject soilPrefab;
    public GameObject housePrefab;
    public GameObject greenhousePrefab;

    [Header("Input - Teclas para activar herramientas")]
    public KeyCode buildKey = KeyCode.B;
    public KeyCode plantKey = KeyCode.P;
    public KeyCode sprayKey = KeyCode.R;
    public KeyCode tillerKey = KeyCode.T;
    public KeyCode noneKey = KeyCode.Escape;

    [Header("Input - Teclas para cambiar prefab en Build (solo si Build activo)")]
    public KeyCode soilKey = KeyCode.Alpha1;
    public KeyCode houseKey = KeyCode.Alpha2;
    public KeyCode greenhouseKey = KeyCode.Alpha3;

    private ToolType currentTool = ToolType.None;

    void Start()
    {
        SetTool(ToolType.None);
        Debug.Log("ToolManager iniciado: Ninguna herramienta activa.");
    }

    void Update()
    {
        HandleToolSwitchInput();

        // Detectar prefabs solo si Build está activo
        if (currentTool == ToolType.Build)
            HandleBuildPrefabInput();
    }

    private void HandleToolSwitchInput()
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
        else if (Input.GetKeyDown(tillerKey))
        {
            SetTool(ToolType.Tiller);
        }
        else if (Input.GetKeyDown(noneKey))
        {
            SetTool(ToolType.None);
        }
    }

    private void HandleBuildPrefabInput()
    {
        if (Input.GetKeyDown(soilKey))
            SetBuildPrefab(soilPrefab);
        else if (Input.GetKeyDown(houseKey))
            SetBuildPrefab(housePrefab);
        else if (Input.GetKeyDown(greenhouseKey))
            SetBuildPrefab(greenhousePrefab);
    }

    public void SetTool(ToolType newTool)
    {
        if (currentTool == newTool) return;

        // Desactivar la herramienta actual
        switch (currentTool)
        {
            case ToolType.Build: buildTool?.Deactivate(); break;
            case ToolType.Plant: plantTool?.Deactivate(); break;
            case ToolType.Spray: sprayTool?.Deactivate(); break;
            case ToolType.Tiller: tillerTool?.Deactivate(); break;
        }

        // Activar la nueva herramienta
        currentTool = newTool;
        switch (currentTool)
        {
            case ToolType.Build:
                if (buildTool != null)
                    buildTool.Activate(soilPrefab);
                break;
            case ToolType.Plant:
                plantTool?.Activate();
                break;
            case ToolType.Spray:
                sprayTool?.Activate();
                break;
            case ToolType.Tiller:
                tillerTool?.Activate();
                break;
            case ToolType.None:
                break;
        }
    }

    public void SetBuildPrefab(GameObject newPrefab)
    {
        if (currentTool != ToolType.Build) return;
        buildTool?.Activate(newPrefab);
    }
}
