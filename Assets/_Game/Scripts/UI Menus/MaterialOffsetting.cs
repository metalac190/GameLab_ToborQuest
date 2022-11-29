using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialOffsetting : MonoBehaviour
{
    public float speed = 0.1f;
    public bool moveToRight = true;
    private Image posterImage;
    private Material imageMaterial;

    // Start is called before the first frame update
    void Awake()
    {
        if (!moveToRight)
            speed *= -1;
        posterImage = GetComponent<Image>();
        imageMaterial = posterImage.material;
        imageMaterial.mainTextureOffset = new Vector2(Random.Range(-100, 100), 0);
        //posterImage.material.mainTextureOffset = new Vector2(Random.Range(0, 100), 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        imageMaterial.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }
}
