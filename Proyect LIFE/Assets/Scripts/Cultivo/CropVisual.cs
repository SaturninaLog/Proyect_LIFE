using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropVisual : MonoBehaviour
{
    public Crop cropData; // contiene los datos lógicos (tipo, etapa, etc.)
    public Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        if (cropData == null)
            cropData = new Crop(CropType.Corn); // valor por defecto
    }

    void Update()
    {
        // Cambiar color o tamaño según el estado
        switch (cropData.stage)
        {
            case CropStage.Seed:
                rend.material.color = Color.yellow;
                transform.localScale = Vector3.one * 0.3f;
                break;
            case CropStage.Growing:
                rend.material.color = Color.green;
                transform.localScale = Vector3.one * 0.7f;
                break;
            case CropStage.Mature:
                rend.material.color = new Color(0.6f, 0.3f, 0.1f);
                transform.localScale = Vector3.one * 1.2f;
                break;
            case CropStage.Dead:
                rend.material.color = Color.gray;
                break;
        }
    }
}

