using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    Material backgroundMaterial;
    Vector2 offset;

    public int xVelo, yVelo, speed;

    private void Awake()
    {
        backgroundMaterial = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        offset = new Vector2(xVelo, yVelo);

        backgroundMaterial.mainTextureOffset += offset * (Time.deltaTime / 50) * speed;

        xVelo = (int)Input.GetAxis("Horizontal");
    }
}
