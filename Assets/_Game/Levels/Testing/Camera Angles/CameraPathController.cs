using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class CameraPathController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private List<CameraPathSection> _sections;

    public void SetActiveCamera(CameraPathSection activeCam, CinemachineSmoothPath path, float offset)
    {
        var dolly = _camera.GetCinemachineComponent<CinemachineTrackedDolly>();
        dolly.m_Path = path;
        dolly.m_AutoDolly.m_PositionOffset = offset;
        
        bool after = true;
        foreach (var cam in _sections)
        {
            if (cam.Equals(activeCam))
            {
                after = false;
                continue;
            }
            cam.UpdatePoint(after);
        }
    }

    [Button]
    private void AutoFillSections()
    {
        _sections = GetComponentsInChildren<CameraPathSection>().ToList();
    }
}
