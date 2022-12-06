using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medal3DObjectHelper : MonoBehaviour {

    [SerializeField] private GameObject renderParent;
    [SerializeField] private GameObject bronzeMedalArt;
    [SerializeField] private GameObject silverMedalArt;
    [SerializeField] private GameObject goldMedalArt;
	[SerializeField] private GameObject platinumMedalArt;
	[SerializeField] private GameObject authorMedalArt;

    private void Awake() {
        renderParent.SetActive(false);
    }

    public void SetMedal(MedalType medalType) {
        renderParent.SetActive(true);

	    //turn off all medal object children
	    authorMedalArt.SetActive(false);
        platinumMedalArt.SetActive(false);
        goldMedalArt.SetActive(false);
        silverMedalArt.SetActive(false);
        bronzeMedalArt.SetActive(false);

        //turn on the specific medal by child index
        switch(medalType) {
            case MedalType.Bronze:
                bronzeMedalArt.SetActive(true);
                break;
            case MedalType.Silver:
                silverMedalArt.SetActive(true);
                break;
            case MedalType.Gold:
                goldMedalArt.SetActive(true);
	            break;
            case MedalType.Platinum:
                platinumMedalArt.SetActive(true);
                break;
            case MedalType.Author:
	            authorMedalArt.SetActive(true);
	            break;
        }
    }
}