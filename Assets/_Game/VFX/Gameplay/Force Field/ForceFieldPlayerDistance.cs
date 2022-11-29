using UnityEngine;

public class ForceFieldPlayerDistance : MonoBehaviour
{
    [SerializeField] private Material _forceFieldMat;
    [SerializeField] private float _distToShow = 4;
    [SerializeField] private float _distMultiplier = 0.25f;
    
    private Transform _player;
    private static readonly int SphereParameters = Shader.PropertyToID("_SphereParameters");

    private void Start()
    {
        _player = FindObjectOfType<MovementController>().transform;
    }

    private void OnValidate()
    {
        if (_distToShow <= 0)
        {
            _distMultiplier = 0.01f;
            return;
        }
        _distMultiplier = 1f / _distToShow;
    }

    private void OnDestroy()
    {
        _forceFieldMat.SetVector(SphereParameters, Vector4.zero);
    }

    private void Update()
    {
        var pos = (Vector4)_player.position;
        pos.w = _distMultiplier;
        _forceFieldMat.SetVector(SphereParameters, pos);
    }
}
