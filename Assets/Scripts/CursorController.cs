using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorController : MonoBehaviour, IPointerClickHandler
{
    public int controllerId;
    public float speed = 5.0f;
    private RectTransform rectTransform;
    private Canvas canvas;
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>() ?? FindObjectOfType<EventSystem>();
    }

    void Update()
    {
        float h = Input.GetAxis($"Horizontal{controllerId}") * speed;
        float v = Input.GetAxis($"Vertical{controllerId}") * speed;
        rectTransform.anchoredPosition += new Vector2(h, v) * Time.deltaTime;

        if (Input.GetButtonDown($"Submit{controllerId}"))
        {
            Click();
        }
    }

    private void Click()
    {
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = rectTransform.position;  
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<IPointerClickHandler>() != null)
            {
                ExecuteEvents.Execute(result.gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       
    }
}
