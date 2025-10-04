using UnityEngine;
using System.Collections.Generic;

public enum WeatherType
{
    Rain,
    Storm,
    Drought
}

public class WeatherSystem : MonoBehaviour
{
    [Header("Configuración del clima")]
    public WeatherType currentWeather = WeatherType.Rain;
    public float weatherDuration = 30f;    // Duración de cada evento climático
    private float weatherTimer;

    [Header("Velocidad de transición")]
    public float transitionSpeed = 0.25f;  // Qué tan rápido cambian las condiciones

    [Header("Efectos del clima")]
    public float rainMoistureTarget = 2f;
    public float rainFertilityChange = 0.02f;

    public float stormMoistureTarget = 2f;
    public float stormFertilityChange = -0.01f;

    public float droughtMoistureTarget = 0f;
    public float droughtFertilityChange = -0.03f;

    [Header("Referencias visuales (opcional)")]
    public ParticleSystem rainParticles;
    public ParticleSystem stormParticles;
    public Light sunLight;

    private List<SoilTile> soilTiles = new List<SoilTile>();

    void Start()
    {
        weatherTimer = weatherDuration;
        FindAllSoilTiles();
        ApplyWeatherVisuals();
    }

    void Update()
    {
        weatherTimer -= Time.deltaTime;

        // Aplicar efectos gradualmente cada frame
        SmoothWeatherEffects();

        if (weatherTimer <= 0)
        {
            ChangeWeather();
            weatherTimer = weatherDuration;
        }
    }

    void FindAllSoilTiles()
    {
        soilTiles.Clear();
        soilTiles.AddRange(FindObjectsOfType<SoilTile>());
    }

    void ChangeWeather()
    {
        currentWeather = (WeatherType)Random.Range(0, 3);
        Debug.Log($"🌦️ Cambio de clima a: {currentWeather}");
        ApplyWeatherVisuals();
    }

    void SmoothWeatherEffects()
    {
        foreach (var tile in soilTiles)
        {
            if (tile == null) continue;

            int targetMoisture = 0;
            float fertilityDelta = 0f;

            switch (currentWeather)
            {
                case WeatherType.Rain:
                    targetMoisture = Mathf.RoundToInt(rainMoistureTarget);
                    fertilityDelta = rainFertilityChange;
                    break;

                case WeatherType.Storm:
                    targetMoisture = Mathf.RoundToInt(stormMoistureTarget);
                    fertilityDelta = stormFertilityChange;
                    break;

                case WeatherType.Drought:
                    targetMoisture = Mathf.RoundToInt(droughtMoistureTarget);
                    fertilityDelta = droughtFertilityChange;
                    break;
            }

            // Humedad con transición suave
            float newMoisture = Mathf.Lerp(tile.moistureLevel, targetMoisture, Time.deltaTime * transitionSpeed);
            tile.moistureLevel = Mathf.RoundToInt(newMoisture);

            // Fertilidad cambia lentamente
            tile.IncreaseFertility(fertilityDelta * Time.deltaTime);
            tile.UpdateVisuals();
        }
    }

    void ApplyWeatherVisuals()
    {
        // Desactivar todas las partículas
        if (rainParticles) rainParticles.Stop();
        if (stormParticles) stormParticles.Stop();

        // Efectos visuales y de luz según el clima
        switch (currentWeather)
        {
            case WeatherType.Rain:
                if (rainParticles) rainParticles.Play();
                if (sunLight) sunLight.color = Color.Lerp(sunLight.color, new Color(0.6f, 0.7f, 1f), 0.5f);
                break;

            case WeatherType.Storm:
                if (stormParticles) stormParticles.Play();
                if (sunLight) sunLight.color = Color.Lerp(sunLight.color, new Color(0.4f, 0.4f, 0.6f), 0.5f);
                break;

            case WeatherType.Drought:
                if (sunLight) sunLight.color = Color.Lerp(sunLight.color, new Color(1f, 0.9f, 0.6f), 0.5f);
                break;
        }
    }
}
