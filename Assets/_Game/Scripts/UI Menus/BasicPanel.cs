using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BasicPanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private GameObject _selectedWhenActive;
    [SerializeField] private GameObject _selectedWhenInactive;
    [SerializeField] private UnityEvent _eventOnActive;

    public void SetGroupActive(bool active)
    {
        _group.alpha = active ? 1 : 0;
        _group.blocksRaycasts = active;
        _group.interactable = active;
        SetSelected(active ? _selectedWhenActive : _selectedWhenInactive);
        if (active) _eventOnActive.Invoke();
    }

    private static void SetSelected(GameObject obj)
    {
        if (obj) CGSC.MouseKeyboardManager.UpdateSelected(obj);
    }
}
