using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{   //creating a minor line of code to activate the jump particle effect-
    //-only when it is called instead of constantly 
    //The particle effect will get plugged into the inspector where it says jumpEffect
    //to call the function in the code where Tobor is launched just type CreateRings

    public ParticleSystem jumpEffect;
    
    void CreateRings()
    {
        jumpEffect.Play();
    }
}
