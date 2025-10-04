using UnityEngine;

public enum ToolType { None, Build, Plant }

public class ToolManager : MonoBehaviour
{
    [Header("Herramientas")]
    public TilePlacement buildTool;
    public PlantTool plantTool;

    [Header("Prefabs disponibles")]
    public GameObject soilPrefab;
    public GameObject housePrefab;
    public GameObject greenhousePrefab;

    private ToolType currentTool = ToolType.None;

    // Cambia la herramienta activa
    public void SetTool(ToolType newTool)
    {
        // Desactivar herramienta anterior
        switch (currentTool)
        {
            case ToolType.Build:
                if (buildTool != null) buildTool.Deactivate();
                break;
            case ToolType.Plant:
                if (plantTool != null) plantTool.Deactivate();
                break;
        }

        // Activar la nueva herramienta
        currentTool = newTool;
        switch (currentTool)
        {
            case ToolType.Build:
                if (buildTool != null) buildTool.Activate(soilPrefab);
                Debug.Log("🧱 Herramienta: Construcción activada");
                break;

            case ToolType.Plant:
                if (plantTool != null) plantTool.Activate();
                Debug.Log("🌱 Herramienta: Plantar activada");
                break;

            case ToolType.None:
                Debug.Log("❌ Ninguna herramienta activa");
                break;
        }
    }

    // Cambiar qué objeto construir sin cambiar de herramienta
    public void SetBuildPrefab(GameObject newPrefab)
    {
        if (buildTool != null)
            buildTool.Activate(newPrefab);
    }
}
