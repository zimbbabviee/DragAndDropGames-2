using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class Screen_Boundaries : MonoBehaviour
{
    [HideInInspector]
    public Vector3 screenPoint, offset;
    public float minX, maxX, minY, maxY;
   // public float padding = 0.02f;

    public Rect worldBounds = new Rect(-960, -540, 1920, 1080);
    [Range(0f, 0.5f)]
    public float padding = 0.2f;

    public Camera targetCamera;

    public float minCamX { get; private set; }
    public float maxCamX { get; private set; }
    public float minCamY { get; private set; }
    public float maxCamY { get; private set; }

    float lastOrthoSize;
    float lastAspect;
    Vector3 lastCamPos;

    void Awake()
    {
       /* Vector3 lowerLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 upperRight =
            Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        float widthReduction = (upperRight.x - lowerLeft.x) * padding;
        float heightReduction = (upperRight.y - lowerLeft.y) * padding;

        minX = lowerLeft.x + widthReduction;
        maxX = upperRight.x - widthReduction;
        minY = lowerLeft.y + heightReduction;
        maxY = upperRight.y - heightReduction;*/

        if(targetCamera = null)
        {
            targetCamera = Camera.main;
        }
        RecalculateBounds();
    }

     void Update()
    {
        if(targetCamera == null)
        {
            return;
        }
        bool changed = false;

        if (targetCamera.orthographic)
        {
            if (!Mathf.Approximately(targetCamera.orthographicSize, lastOrthoSize))
                changed = true;

            if (targetCamera.transform.position != lastCamPos)
                changed = true;

            if (changed)
            {
                RecalculateBounds();
            }
        }
    }

    public void RecalculateBounds()
    {
        if (targetCamera == null)
            return;

        float wbMinX = worldBounds.xMin;
        float wbMaxX = worldBounds.xMax;
        float wbMinY = worldBounds.yMin;
        float wbMaxY = worldBounds.yMax;

        if (targetCamera.orthographic)
        {
            float halfH = targetCamera.orthographicSize;
            float halfW = halfH * targetCamera.aspect;

            if(halfW * 2f >= (wbMaxX - wbMinX))
            {
                minCamX = maxCamX = (wbMinX + wbMaxX) * 0.5f;
            }
            else
            {
                minCamX = wbMinX + halfW;
                maxCamX = wbMaxX - halfW;
            }

            if(halfH * 2f >= (wbMaxX - wbMaxY))
            {
                minCamY = maxCamY = (wbMinY + wbMaxY) * 0.5f;
            }
            else
            {
                minCamY = wbMinY + halfH;
                maxCamY = wbMaxY - halfH;
            }
        }
        lastOrthoSize = targetCamera.orthographicSize;
        lastAspect = targetCamera.aspect;
        lastCamPos = targetCamera.transform.position;
    }
    public Vector2 GetClampedPosition(Vector3 curPosition)
    {
        float shrinkW = worldBounds.width * padding;
        float shrinkH = worldBounds.height * padding;
        float wbMinX = worldBounds.xMin + shrinkW;
        float wbMaxX = worldBounds.xMax - shrinkW;
        float wbMinY = worldBounds.xMin + shrinkH;
        float wbMaxY = worldBounds.xMax - shrinkH;

        float cx = Mathf.Clamp(curPosition.x, wbMinX, wbMaxX);
        float cy = Mathf.Clamp(curPosition.y, wbMinY, wbMaxY);
        return new Vector2(cx, cy);
    }
        public Vector3 GetClampedCameraPosition(Vector3 desiredCamCenter)
        {

            float cx = Mathf.Clamp(desiredCamCenter.x, minCamX, maxCamX);
            float cy = Mathf.Clamp(desiredCamCenter.y, minCamY, maxY);
            return new Vector3(cx, cy, desiredCamCenter.z);
        }
        /*return new Vector2(
            Mathf.Clamp(position.x, minX, maxX),
            Mathf.Clamp(position.y, minY, maxY)
        );*/
}