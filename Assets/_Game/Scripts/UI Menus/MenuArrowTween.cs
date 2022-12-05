using UnityEngine;

public class MenuArrowTween : MonoBehaviour
{
    [SerializeField] private bool playWhenEnabled = true;
    [SerializeField] private int bouncingDistance = 50;
    [SerializeField] private float bouncingTime = 1.5f;    
    
    private int tweenID;
    private RectTransform _transform;
    private RectTransform Transform
    {
        get
        {
            if (!_transform) _transform = GetComponent<RectTransform>();
            return _transform;
        }
    }
    private Vector3 initPos;

    private void Awake()
    {
        initPos = Transform.localPosition;
    }

    private void OnEnable()
    {
        Transform.localPosition = initPos;
        if (playWhenEnabled) PlayTween();
    }

    [Button]
    public void PlayTween()
    {
        LeanTween.cancel(Transform);
        tweenID = LeanTween.moveX(Transform, bouncingDistance, bouncingTime)
            .setEaseOutCubic()
            .setLoopPingPong()
            .setIgnoreTimeScale(true).id;
    }

    [Button]
    public void StopTween()
    {
        LeanTween.cancel(Transform);
        Transform.localPosition = initPos;
    }

}
