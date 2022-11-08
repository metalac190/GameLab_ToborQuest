using System;
using UnityEngine;

public class ColliderGizmos : MonoBehaviour
{
#if UNITY_EDITOR

    [Header("Settings")]
    [SerializeField] private bool _showColliders = true;
    [SerializeField] private bool _solid;

    [Header("Colors")]
    [SerializeField] private Color _groundColor = Color.cyan;
    [SerializeField] private Color _wallColor = Color.yellow;
    [SerializeField] private Color _breakableColor = Color.magenta;
    [SerializeField] private Color _interactable = Color.magenta;
    [SerializeField] private Color _npcColor = Color.magenta;
    [SerializeField] private Color _defaultColor = Color.red;
    [SerializeField] private Mesh _capsuleMesh;

    private MeshCollider[] _meshColliders;
    private CapsuleCollider[] _capsuleColliders;
    private BoxCollider[] _boxColliders;
    private SphereCollider[] _sphereColliders;

    private void Start()
    {
        if (!_showColliders) return;
        Debug.LogError("DO NOT SHIP DEBUG COLLIDERS!", gameObject);
        FindAllColliders();
    }

    private void OnDrawGizmos()
    {
        try
        {
            if (!_showColliders) return;
            if (_meshColliders == null) FindAllColliders();
        
            foreach (var col in _meshColliders)
            {
                if (!SetColor(col.gameObject)) continue;
                var t = col.transform;
                if (_solid)Gizmos.DrawMesh(col.sharedMesh, t.position, t.rotation, t.lossyScale);
                else Gizmos.DrawWireMesh(col.sharedMesh, t.position, t.rotation, t.lossyScale);
            }
            foreach (var col in _capsuleColliders)
            {
                if (!SetColor(col.gameObject)) continue;
                var t = col.transform;
                Gizmos.matrix = Matrix4x4.TRS(t.position, t.rotation, t.lossyScale);
                if (_solid) Gizmos.DrawMesh(_capsuleMesh, col.center, Quaternion.identity, new Vector3(2 * col.radius, col.height * 0.5f, 2 * col.radius));
                else Gizmos.DrawWireMesh(_capsuleMesh, col.center, Quaternion.identity, new Vector3(2 * col.radius, col.height * 0.5f, 2 * col.radius));
            }
            foreach (var col in _boxColliders)
            {
                if (!SetColor(col.gameObject)) continue;
                var t = col.transform;
                Gizmos.matrix = Matrix4x4.TRS(t.position, t.rotation, t.lossyScale);
                if (_solid)Gizmos.DrawCube(col.center, col.size + Vector3.one * 0.001f);
                else Gizmos.DrawWireCube(col.center, col.size);
            }
            foreach (var col in _sphereColliders)
            {
                if (!SetColor(col.gameObject)) continue;
                var t = col.transform;
                Gizmos.matrix = Matrix4x4.TRS(t.position, t.rotation, t.lossyScale);
                if (_solid)Gizmos.DrawSphere(col.center, col.radius + 0.001f);
                else Gizmos.DrawWireSphere(col.center, col.radius);
            }
            
            bool SetColor(GameObject obj)
            {
                if (obj.GetComponent<InvisibleTrigger>()) return false;
                if (obj.transform.root.name.Equals("Player")) return false;
                if (obj.GetComponent<InteractablePad>())
                {
                    Gizmos.color = _interactable;
                    return true;
                }
                int layer = obj.layer;
                Gizmos.color = layer switch
                {
                    11 => _groundColor,     // Ground
                    12 => _wallColor,       // Wall
                    13 => _breakableColor,  // Breakable
                    16 => _npcColor,        // NPC
                    _ => _defaultColor
                };
                return true;
            }
        }
        catch (Exception e)
        {
            FindAllColliders();
        }

    }
    

    [Button]
    private void FindAllColliders()
    {
        _meshColliders = FindObjectsOfType<MeshCollider>();
        _capsuleColliders = FindObjectsOfType<CapsuleCollider>();
        _boxColliders = FindObjectsOfType<BoxCollider>();
        _sphereColliders = FindObjectsOfType<SphereCollider>();
    }

#endif
}
