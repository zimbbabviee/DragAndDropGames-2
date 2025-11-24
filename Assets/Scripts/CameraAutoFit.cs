using UnityEngine;

public class CameraAutoFit : MonoBehaviour
{
    public float targetWidth = 1920f;
    public float targetHeight = 1080f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("CameraAutoFit: Camera component not found!");
            return;
        }

        AdjustCameraSize();
    }

    void AdjustCameraSize()
    {
        float targetAspect = targetWidth / targetHeight;
        float currentAspect = (float)Screen.width / (float)Screen.height;

        float baseOrthographicSize = targetHeight / 2f;

        if (currentAspect < targetAspect)
        {
            float adjustedSize = baseOrthographicSize * (targetAspect / currentAspect);
            cam.orthographicSize = adjustedSize;
        }
        else
        {
            cam.orthographicSize = baseOrthographicSize;
        }

        Debug.Log($"CameraAutoFit: Screen {Screen.width}x{Screen.height}, Aspect {currentAspect:F2}, Target {targetAspect:F2}, Size {cam.orthographicSize}");
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (Application.isPlaying && cam != null)
        {
            AdjustCameraSize();
        }
    }
#endif
}
