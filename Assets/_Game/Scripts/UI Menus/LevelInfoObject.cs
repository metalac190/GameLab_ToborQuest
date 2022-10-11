using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class LevelInfoObject : ScriptableObject
{
    [SerializeField] private SerializedScene levelScene;
    [SerializeField] private string levelInfo;
    [SerializeField] private Image levelImage;
    public float BestTime { get; set; }
    public bool ghostDataAvailable { get; set; }
}
