using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropScript : MonoBehaviour, IPointerDownHandler, IDragHandler,
   IBeginDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGro;
    private RectTransform rectTra;
    public ObjectScript objectScr;
    public Screen_Boundaries screenBou;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasGro = GetComponent<CanvasGroup>();
        rectTra = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    public void OnPointerDown(PointerEventData eventData)
    {
        if(Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            Debug.Log("OnPointerDown");
            objectScr.effects.PlayOneShot(objectScr.audioCli[0]);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            objectScr.lastDragged = null;
            canvasGro.blocksRaycasts = false;
            canvasGro.alpha = 0.6f;
            rectTra.SetAsFirstSibling();
            rectTra.SetAsLastSibling();
            Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenBou.screenPoint.z));
            rectTra.position = cursorWorldPos;
            screenBou.screenPoint = Camera.main.WorldToScreenPoint(rectTra.localPosition);

            screenBou.offset = rectTra.localPosition - Camera.main.ScreenToViewportPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                screenBou.screenPoint.z));

        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

}
