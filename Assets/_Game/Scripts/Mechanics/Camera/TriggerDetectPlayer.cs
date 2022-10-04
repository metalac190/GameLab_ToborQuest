using System;
using UnityEngine;

public class TriggerDetectPlayer : MonoBehaviour
{
    public Action OnPlayerEnter = delegate { };

    private static bool IsPlayer(Component o) => o.gameObject.name.Equals("Tobor");
    
    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other)) OnPlayerEnter?.Invoke();
    }
}
