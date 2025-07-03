using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;


public class OnScreenStickAreaController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    RectTransform _canvasRectTransform;
    [SerializeField]
    RectTransform _stickTransform;
    [SerializeField]
    RectTransform _backgroundTransform;
    OnScreenStick _stickScript;
    Vector2 _startPos;


    public void OnDrag(PointerEventData eventData)
    {
        _stickScript.OnDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, eventData.position, eventData.pressEventCamera, out localPoint)) {
            _stickTransform.anchoredPosition = localPoint;
            _backgroundTransform.anchoredPosition = _stickTransform.anchoredPosition;
            _stickScript.OnPointerDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _stickScript.OnPointerUp(eventData);
        _stickTransform.anchoredPosition = _startPos;
        _backgroundTransform.anchoredPosition = _startPos;
    }

    void Awake()
    {
        _canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        _stickScript = _stickTransform.GetComponent<OnScreenStick>();
        _startPos = _stickTransform.anchoredPosition;
    }
}
