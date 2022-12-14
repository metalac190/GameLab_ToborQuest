using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class MedalUIHelper : ScriptableObject {

    [SerializeField] private Sprite bronzeMedalSprite;
    [SerializeField] private Sprite silverMedalSprite;
    [SerializeField] private Sprite goldMedalSprite;
	[SerializeField] private Sprite platinumMedalSprite;
	[SerializeField] private Sprite authorMedalSprite;

    public void SetMedalUI(Image medalImage, MedalType medalType) {
        medalImage.enabled = true;
        switch(medalType) {
            case MedalType.None:
                medalImage.enabled = false;
                break;
            case MedalType.Bronze:
                medalImage.sprite = bronzeMedalSprite;
                break;
            case MedalType.Silver:
                medalImage.sprite = silverMedalSprite;
                break;
            case MedalType.Gold:
                medalImage.sprite = goldMedalSprite;
	            break;
            case MedalType.Platinum:
                medalImage.sprite = platinumMedalSprite;
                break;
            case MedalType.Author:
	            medalImage.sprite = authorMedalSprite;
	            break;
        }
    }
}