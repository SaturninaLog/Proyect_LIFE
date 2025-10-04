using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToolType { None, Build, Plant, Water, Harvest }

public class ToolManager : MonoBehaviour
{
    public ToolType currentTool = ToolType.None;

    [Header("Herramientas")]
    public TilePlacement buildTool;   // Colocar terreno
    public PlantTool plantTool;       // Plantar semillas
    // Futuras herramientas -> WaterTool, HarvestTool, etc.

    void Update()
    {
        HandleInput();
        UpdateTools();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentTool = ToolType.Build;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentTool = ToolType.Plant;
        if (Input.GetKeyDown(KeyCode.Alpha0)) currentTool = ToolType.None; // Sin herramienta
    }

    void UpdateTools()
    {
        // Habilitar/deshabilitar scripts según herramienta actual
        buildTool.enabled = (currentTool == ToolType.Build);
        plantTool.enabled = (currentTool == ToolType.Plant);
        // lo mismo con futuras herramientas
    }
}

