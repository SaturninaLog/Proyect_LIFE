using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCielo : MonoBehaviour
{
    //Variables
    public float scrollX = 0.1f;
    public float scrollY = 0.1f;
    public DayNightCycle dnScript;

    void Update()
    {
        float speed = dnScript.timeSpeed;
        float offsetX = Time.time * scrollX / 100 * speed;
        float offsetY = Time.time * scrollY / 100 * speed;
        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}
