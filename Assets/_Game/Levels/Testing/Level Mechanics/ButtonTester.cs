using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTester : MonoBehaviour {

    [SerializeField] private GameObject art;

    private void Awake() {
        art.SetActive(false);
    }

    public void Show() {
        art.SetActive(true);
    }

    public void Hide() {
        art.SetActive(false);
    }
}