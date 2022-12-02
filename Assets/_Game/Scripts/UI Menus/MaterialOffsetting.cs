using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialOffsetting : MonoBehaviour
{
    public float speed = 0.1f;
    public bool moveToLeft = true;
    private Image posterImage;
    private Material imageMaterial;
    private bool toggle = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (!moveToLeft)
            speed *= -1;
        posterImage = GetComponent<Image>();
        imageMaterial = posterImage.material;
        //Debug.Log("Random number is " + Random.Range(-100, 100));
        imageMaterial.mainTextureOffset = new Vector2(Random.Range(-100, 100), 0);
        //posterImage.material.mainTextureOffset = new Vector2(Random.Range(0, 100), 0);
    }

    private void OnDisable()
    {
        toggle = !toggle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(toggle)
            imageMaterial.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
        else
            imageMaterial.mainTextureOffset -= new Vector2(speed * Time.deltaTime, 0);
    }
}
