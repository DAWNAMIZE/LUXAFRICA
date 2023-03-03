using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Vector2 posA;
    private Vector2 offset;
    private Image innerCircle;
    public Vector2 direction;
    [SerializeField] private float sensitivity = 30f;
    public bool isDragging = false;
    public static VirtualJoystick Main;

    private void Awake()
    {
        if (Main == null)
        {
            Main = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }


    private void Start()
    {
        innerCircle = transform.GetChild(0).GetComponent<Image>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        posA = eventData.position;
        isDragging = true;

    }

    public void OnDrag(PointerEventData eventData)
    {
        offset = eventData.position - posA;

        float x = offset.x / sensitivity;
        float y = offset.y / sensitivity;
        direction = offset.magnitude >= sensitivity ? Vector2.ClampMagnitude(offset, 1) : new Vector2(x, y);

        isDragging = true;

        innerCircle.rectTransform.anchoredPosition = new Vector2(direction.x * GetComponent<RectTransform>().sizeDelta.x / 3f, direction.y * GetComponent<RectTransform>().sizeDelta.y / 3f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        posA = Vector3.zero;
        direction = posA;
        innerCircle.GetComponent<RectTransform>().anchoredPosition = posA;
        isDragging = false;
    }

}
