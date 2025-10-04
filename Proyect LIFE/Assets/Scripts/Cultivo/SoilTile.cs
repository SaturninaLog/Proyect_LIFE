using UnityEngine;

public class SoilTile : MonoBehaviour
{
    public float fertility = 1f;  // calidad del suelo (0–1)
    public float moisture = 1f;   // humedad (0–1)
    public bool hasCrop = false;
    public Crop currentCrop;

    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateVisuals();
    }

    void Update()
    {
        // Para demo: refrescar color siempre (en un proyecto real se optimiza llamando solo cuando cambian los valores)
        UpdateVisuals();
    }

    public void PlantSeed(Crop crop)
    {
        if (!hasCrop)
        {
            hasCrop = true;
            currentCrop = crop;
            Debug.Log("Semilla plantada en " + gameObject.name);
        }
    }

    void UpdateVisuals()
    {
        if (rend == null) return;

        // Color base: marrón (tierra)
        Color baseColor = new Color(0.4f, 0.25f, 0.1f);

        // Ajustar humedad: más azul = más húmedo
        float blueTint = Mathf.Clamp01(moisture);

        // Ajustar fertilidad: más verde = más fértil
        float greenTint = Mathf.Clamp01(fertility);

        // Combinar colores
        Color finalColor = baseColor + new Color(0, greenTint * 0.5f, blueTint * 0.5f);

        rend.material.color = finalColor;
    }
}