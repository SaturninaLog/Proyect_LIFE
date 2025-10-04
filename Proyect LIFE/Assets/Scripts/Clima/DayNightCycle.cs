using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    // Start is called before the first frame update

    public float min, grados;
    public float timeSpeed = 1;
    public Light luna;
    

    // Update is called once per frame
    void Update()
    {
        //1 dia = 24 min

        min += timeSpeed * Time.deltaTime;
        
        //60 min = 1 hs = 24 hs

        if (min >= 1440) //Tiempo del dia - 1440 son 24hs
        {
            min = 0;
        }
        //360 grados / 1440 - 1 grado = 0.25min
        grados = min / 4;
        this.transform.localEulerAngles = new Vector3(grados, -90f, 0f);

        if(grados >= 180)
        {
            this.GetComponent<Light>().enabled = false;
            luna.enabled = true;
        }
        else
        {
            this.GetComponent<Light>().enabled = true;
            luna.enabled = false;
        }

    }
}
