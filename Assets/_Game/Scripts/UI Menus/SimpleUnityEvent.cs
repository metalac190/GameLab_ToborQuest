using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleUnityEvent : MonoBehaviour
{
	[SerializeField] private UnityEvent _event;
	
	public void Invoke() => _event.Invoke();
}
