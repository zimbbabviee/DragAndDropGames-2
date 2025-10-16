<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropScript : MonoBehaviour, IPointerDownHandler, IBeginDragHandler,
    IDragHandler, IEndDragHandler
=======
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropScript : MonoBehaviour, IPointerDownHandler, IDragHandler,
   IBeginDragHandler, IEndDragHandler
>>>>>>> 45429f9bcd19d08c2d33868a5526b477a3dd2ff2
{
    private CanvasGroup canvasGro;
    private RectTransform rectTra;
    public ObjectScript objectScr;
<<<<<<< HEAD
    public Screen_Boundaries screenBou;

    // Start is called before the first frame update
=======

    // Start is called once before the first execution of Update after the MonoBehaviour is created
>>>>>>> 45429f9bcd19d08c2d33868a5526b477a3dd2ff2
    void Start()
    {
        canvasGro = GetComponent<CanvasGroup>();
        rectTra = GetComponent<RectTransform>();
    }

<<<<<<< HEAD
    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
=======
    // Update is called once per frame
    public void OnPointerDown(PointerEventData eventData)
    {
        if(Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
>>>>>>> 45429f9bcd19d08c2d33868a5526b477a3dd2ff2
        {
            Debug.Log("OnPointerDown");
            objectScr.effects.PlayOneShot(objectScr.audioCli[0]);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
<<<<<<< HEAD
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            ObjectScript.drag = true;
            ObjectScript.lastDragged = eventData.pointerDrag;
            canvasGro.blocksRaycasts = false;
            canvasGro.alpha = 0.6f;
            // rectTra.SetAsLastSibling();
            int lastIndex = transform.parent.childCount - 1;
            int position = Mathf.Max(0, lastIndex - 1);
            transform.SetSiblingIndex(position);
            Vector3 cursorWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenBou.screenPoint.z));
            rectTra.position = cursorWorldPos;

            screenBou.screenPoint = Camera.main.WorldToScreenPoint(rectTra.localPosition);

            screenBou.offset = rectTra.localPosition -
                Camera.main.ScreenToWorldPoint(
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                screenBou.screenPoint.z));
        }
=======

>>>>>>> 45429f9bcd19d08c2d33868a5526b477a3dd2ff2
    }

    public void OnDrag(PointerEventData eventData)
    {
<<<<<<< HEAD
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
        {
            Vector3 curSreenPoint =
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenBou.screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curSreenPoint) + screenBou.offset;
            rectTra.position = screenBou.GetClampedPosition(curPosition);
        }
=======

>>>>>>> 45429f9bcd19d08c2d33868a5526b477a3dd2ff2
    }

    public void OnEndDrag(PointerEventData eventData)
    {
<<<<<<< HEAD
        if (Input.GetMouseButtonUp(0))
        {
            ObjectScript.drag = false;
            ObjectScript.lastDragged = eventData.pointerDrag;
            canvasGro.blocksRaycasts = true;
            canvasGro.alpha = 1.0f;

            if (objectScr.rightPlace)
            {
                canvasGro.blocksRaycasts = false;
                ObjectScript.lastDragged = null;


            }

            objectScr.rightPlace = false;
        }
    }
}
=======

    }

}
>>>>>>> 45429f9bcd19d08c2d33868a5526b477a3dd2ff2
