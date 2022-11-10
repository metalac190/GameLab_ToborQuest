using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoundSystem;

public class cloudSound : StateMachineBehaviour
{
    [SerializeField] SFXEvent cloudSwoosh;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cloudSwoosh.Play();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cloudSwoosh.Play();
    }
}


