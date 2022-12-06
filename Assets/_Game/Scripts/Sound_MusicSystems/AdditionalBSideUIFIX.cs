using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditionalBSideUIFIX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            var tochange = gameObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0);
        }
    }

}
