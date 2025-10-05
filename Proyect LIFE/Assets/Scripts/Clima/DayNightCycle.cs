using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Configuración del tiempo")]
    public float min;
    public float grados;
    public float timeSpeed = 1f;

    [Header("Luz solar y lunar")]
    public Light sol;
    public Light luna;

    [Header("Intensidades de luz")]
    public float intensidadDia = 1f;
    public float intensidadNoche = 0.1f;
    public float intensidadLuna = 0.3f;

    [Header("Iluminación ambiental")]
    public Color luzDia = new Color(1f, 0.95f, 0.8f);
    public Color luzNoche = new Color(0.25f, 0.3f, 0.45f);

    [Header("Skyboxes")]
    public Material skyboxDia;
    public Material skyboxNoche;

    private bool esDeDia = true;

    void Update()
    {
        // Simulación del paso del tiempo
        min += timeSpeed * Time.deltaTime;
        if (min >= 1440f) min = 0f;

        // Rotación solar
        grados = min / 4f;
        sol.transform.localEulerAngles = new Vector3(grados, -90f, 0f);

        bool ahoraEsDia = grados < 180f;

        // Cambiar entre día y noche
        if (ahoraEsDia != esDeDia)
        {
            esDeDia = ahoraEsDia;
            CambiarSkybox(esDeDia);
        }

        // Transiciones suaves de luz e iluminación
        if (ahoraEsDia)
        {
            sol.intensity = Mathf.Lerp(sol.intensity, intensidadDia, Time.deltaTime * 0.5f);
            luna.intensity = Mathf.Lerp(luna.intensity, 0f, Time.deltaTime * 0.5f);
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, luzDia, Time.deltaTime * 0.5f);
        }
        else
        {
            sol.intensity = Mathf.Lerp(sol.intensity, intensidadNoche, Time.deltaTime * 0.5f);
            luna.intensity = Mathf.Lerp(luna.intensity, intensidadLuna, Time.deltaTime * 0.5f);
            RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, luzNoche, Time.deltaTime * 0.5f);
        }

        sol.enabled = ahoraEsDia;
        luna.enabled = !ahoraEsDia;

        // Forzar actualización del entorno
        DynamicGI.UpdateEnvironment();
    }

    void CambiarSkybox(bool dia)
    {
        if (dia && skyboxDia != null)
        {
            RenderSettings.skybox = skyboxDia;
        }
        else if (!dia && skyboxNoche != null)
        {
            RenderSettings.skybox = skyboxNoche;
        }

        // 🔄 Forzar que Unity actualice la iluminación global
        DynamicGI.UpdateEnvironment();
    }
}
