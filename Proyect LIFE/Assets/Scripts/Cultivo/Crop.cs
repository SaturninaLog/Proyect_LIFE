using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CropType { Corn, Wheat, Rice }

public enum CropStage { Seed, Growing, Mature, Dead }

[System.Serializable]
public class Crop
{
    public CropType type;
    public CropStage stage;

    public float growthTime;         // tiempo total para madurar
    public float currentGrowth = 0f; // progreso actual
    public float waterNeed;          // agua necesaria
    public float soilFertilityNeed;  // fertilidad mínima

    public int marketValue;          // valor de venta

    public Crop(CropType type)
    {
        this.type = type;
        stage = CropStage.Seed;

        // Datos iniciales según el tipo de cultivo
        switch (type)
        {
            case CropType.Corn:
                growthTime = 10f;
                waterNeed = 0.4f;
                soilFertilityNeed = 0.5f;
                marketValue = 5;
                break;
            case CropType.Wheat:
                growthTime = 15f;
                waterNeed = 0.3f;
                soilFertilityNeed = 0.4f;
                marketValue = 8;
                break;
            case CropType.Rice:
                growthTime = 20f;
                waterNeed = 0.7f;
                soilFertilityNeed = 0.6f;
                marketValue = 12;
                break;
        }
    }

    public void Grow(float deltaTime, float soilMoisture, float soilFertility)
    {
        // Si ya está muerta o madura, no hacer nada
        if (stage == CropStage.Dead || stage == CropStage.Mature) return;

        // La semilla inicia siempre en Seed
        if (stage == CropStage.Seed)
        {
            // Verificar condiciones mínimas del suelo
            if (soilMoisture < waterNeed || soilFertility < soilFertilityNeed)
            {
                stage = CropStage.Dead; // Si no cumple, muere inmediatamente
                return;
            }
            else
            {
                stage = CropStage.Growing; // Si cumple, pasa a Growing
            }
        }

        // Ajustar crecimiento según fertilidad (menos fertilidad = crecimiento más lento)
        float growthMultiplier = Mathf.Clamp01(soilFertility / soilFertilityNeed);
        currentGrowth += deltaTime * growthMultiplier;

        if (currentGrowth >= growthTime)
        {
            stage = CropStage.Mature;
        }
        else
        {
            stage = CropStage.Growing;
        }
    }


}

