using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HanoiDisk : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int diskSize = 1;

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Image diskImage;
    private Color originalColor;
    private Vector2 originalPosition;
    private HanoiTower currentTower;
    private HanoiTower sourceTower;
    private Camera uiCamera;
    private Vector2 dragOffset;
    private bool isDraggingAllowed = false;
    private Transform originalParent;
    private int originalSiblingIndex;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        diskImage = GetComponent<Image>();

        if (diskImage != null)
        {
            originalColor = diskImage.color;
        }

        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        if (canvas != null)
        {
            uiCamera = canvas.worldCamera;
        }
        else
        {
            Debug.LogError("Canvas not found for HanoiDisk");
        }
    }

    public void SetTower(HanoiTower tower)
    {
        currentTower = tower;
    }

    public HanoiTower GetTower()
    {
        return currentTower;
    }

    private bool ScreenPointToWorld(Vector2 screenPoint, out Vector3 worldPoint)
    {
        worldPoint = Vector3.zero;
        if (uiCamera == null) return false;

        float z = Mathf.Abs(uiCamera.transform.position.z - transform.position.z);
        Vector3 sp = new Vector3(screenPoint.x, screenPoint.y, z);
        worldPoint = uiCamera.ScreenToWorldPoint(sp);
        return true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraggingAllowed = false;

        if (currentTower != null && !currentTower.IsTopDisk(this))
        {
            Debug.Log("Cannot drag: This disk is not on top!");
            return;
        }

        isDraggingAllowed = true;
        sourceTower = currentTower;
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        canvasGroup.alpha = 0.9f;
        canvasGroup.blocksRaycasts = false;

        rectTransform.localScale = Vector3.one * 1.1f;

        if (diskImage != null)
        {
            diskImage.color = new Color(originalColor.r * 1.2f, originalColor.g * 1.2f, originalColor.b * 1.2f, originalColor.a);
        }

        Vector3 worldPosition = rectTransform.position;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
        rectTransform.position = worldPosition;

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            uiCamera,
            out localPointerPosition))
        {
            dragOffset = rectTransform.localPosition - (Vector3)localPointerPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDraggingAllowed)
            return;

        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            uiCamera,
            out localPointerPosition))
        {
            rectTransform.localPosition = localPointerPosition + dragOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggingAllowed)
        {
            return;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        rectTransform.localScale = Vector3.one;

        if (diskImage != null)
        {
            diskImage.color = originalColor;
        }

        HanoiTower targetTower = null;

        if (eventData.pointerEnter != null)
        {
            targetTower = eventData.pointerEnter.GetComponent<HanoiTower>();
            if (targetTower == null)
            {
                targetTower = eventData.pointerEnter.GetComponentInParent<HanoiTower>();
            }
        }

        bool validPlacement = false;

        if (targetTower != null && targetTower.CanPlaceDisk(this))
        {
            if (currentTower != null)
            {
                currentTower.RemoveDisk(this);
            }

            targetTower.AddDisk(this);
            validPlacement = true;

            if (sourceTower != targetTower)
            {
                HanoiGameManager gameManager = Object.FindFirstObjectByType<HanoiGameManager>();
                if (gameManager != null)
                {
                    gameManager.OnDiskMoved();
                }
            }
        }
        else
        {
            Debug.Log("Invalid placement! Disk will fall down.");
            StartCoroutine(FallAndReturn());
        }

        isDraggingAllowed = false;
    }

    IEnumerator FallAndReturn()
    {
        float fallDuration = 0.5f;
        float elapsed = 0f;
        Vector3 startPos = rectTransform.position;

        // Calculate fall target (bottom of screen)
        Screen_Boundaries screenBounds = FindFirstObjectByType<Screen_Boundaries>();
        float bottomY = screenBounds != null ? screenBounds.worldBounds.yMin - 200 : -1000;
        Vector3 fallTarget = new Vector3(startPos.x, bottomY, startPos.z);

        // Fall down
        while (elapsed < fallDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fallDuration;
            // Use quadratic easing for more realistic fall
            float easedT = t * t;
            rectTransform.position = Vector3.Lerp(startPos, fallTarget, easedT);

            yield return null;
        }

        // Return to original tower
        if (sourceTower != null)
        {
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalSiblingIndex);
            sourceTower.UpdateDiskPositionsPublic();
        }
        else
        {
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}
