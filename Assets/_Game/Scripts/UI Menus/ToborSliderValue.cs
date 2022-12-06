using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToborSliderValue : MonoBehaviour
{
    private Slider toborSlider;

    private void Awake()
    {
        toborSlider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        toborSlider.value = CGSC.GetSceneLoadProgress;
    }
}
