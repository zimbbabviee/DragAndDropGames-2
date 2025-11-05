using UnityEngine;
using UnityEngine.EventSystems;

// CHANGES FOR ANDROID
public class DropPlaceScript : MonoBehaviour, IDropHandler
{
    private float placeZRot, vehicleZRot, rotDiff;
    private Vector3 placeSiz, vehicleSiz;
    private float xSizeDiff, ySizeDiff;
    public ObjectScript objScript;
    private bool isOccupied = false;

    void Start()
    {
        if (objScript == null)
        {
            objScript = Object.FindFirstObjectByType<ObjectScript>();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // CHANGES FOR ANDROID
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;


        if (eventData.pointerDrag.tag.Equals(tag))
        {
            placeZRot =
                 eventData.pointerDrag.GetComponent<RectTransform>().transform.eulerAngles.z;

            vehicleZRot =
                GetComponent<RectTransform>().transform.eulerAngles.z;

            rotDiff = Mathf.Abs(placeZRot - vehicleZRot);
            Debug.Log("Rotation difference: " + rotDiff);

            placeSiz = eventData.pointerDrag.GetComponent<RectTransform>().localScale;
            vehicleSiz = GetComponent<RectTransform>().localScale;
            xSizeDiff = Mathf.Abs(placeSiz.x - vehicleSiz.x);
            ySizeDiff = Mathf.Abs(placeSiz.y - vehicleSiz.y);
            Debug.Log("X size difference: " + xSizeDiff);
            Debug.Log("Y size difference: " + ySizeDiff);

            if ((rotDiff <= 5 || (rotDiff >= 355 && rotDiff <= 360)) &&
                (xSizeDiff <= 0.07 && ySizeDiff <= 0.07))
            {
                Debug.Log("Correct place");
                objScript.rightPlace = true;
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition =
                    GetComponent<RectTransform>().anchoredPosition;

                eventData.pointerDrag.GetComponent<RectTransform>().localRotation =
                    GetComponent<RectTransform>().localRotation;

                eventData.pointerDrag.GetComponent<RectTransform>().localScale =
                    GetComponent<RectTransform>().localScale;

                if (!isOccupied)
                {
                    isOccupied = true;

                    GameManager gameManager = FindFirstObjectByType<GameManager>();
                    if (gameManager != null)
                    {
                        gameManager.OnVehiclePlaced();
                    }
                }

                switch (eventData.pointerDrag.tag)
                {
                    case "Garbage":
                        objScript.effects.PlayOneShot(objScript.audioCli[2]);
                        break;
                    case "Medicine":
                        objScript.effects.PlayOneShot(objScript.audioCli[3]);
                        break;
                    case "Fire":
                        objScript.effects.PlayOneShot(objScript.audioCli[4]);
                        break;
                    case "bus":
                        objScript.effects.PlayOneShot(objScript.audioCli[6]);
                        break;
                    case "masina":
                        objScript.effects.PlayOneShot(objScript.audioCli[13]);
                        break;
                    case "betonomeshalka":
                        objScript.effects.PlayOneShot(objScript.audioCli[7]);
                        break;
                    case "drugajamashina":
                        objScript.effects.PlayOneShot(objScript.audioCli[14]);
                        break;
                    case "ekslavators":
                        objScript.effects.PlayOneShot(objScript.audioCli[12]);
                        break;
                    case "e6":
                        objScript.effects.PlayOneShot(objScript.audioCli[13]);
                        break;
                    case "policija":
                        objScript.effects.PlayOneShot(objScript.audioCli[9]);
                        break;
                    case "traktors1":
                        objScript.effects.PlayOneShot(objScript.audioCli[10]);
                        break;
                    default:
                        Debug.Log("Unknown tag detected");
                        break;
                }
            }
        }
    }
}

