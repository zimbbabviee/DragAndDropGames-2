using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// CHANGES FOR ANDROID
public class FlyingObjectsControllerScript : MonoBehaviour
{
    [HideInInspector]
    public float speed = 1f;
    public float fadeDuration = 1.5f;
    public float waveAmplitude = 25f;
    public float waveFrequency = 1f;
    private ObjectScript objectScript;
    private Screen_Boundaries scrreenBoundriesScript;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private bool isFadingOut = false;
    private bool isExploading = false;
    private Image image;
    private Color originalColor;
    private bool isActive = true;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        rectTransform = GetComponent<RectTransform>();

        image = GetComponent<Image>();
        originalColor = image.color;
        objectScript = FindFirstObjectByType<ObjectScript>();
        scrreenBoundriesScript = FindFirstObjectByType<Screen_Boundaries>();
        StartCoroutine(FadeIn());
    }

    public void StopMoving()
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
            return;

        float waveOffset = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;
        rectTransform.anchoredPosition += new Vector2(-speed * Time.deltaTime, waveOffset * Time.deltaTime);
        // <-
        if (speed > 0 && transform.position.x < (scrreenBoundriesScript.minX + 80) && !isFadingOut)
        {
            StartCoroutine(FadeOutAndDestroy());
            isFadingOut = true;
        }

        // ->
        if (speed < 0 && transform.position.x > (scrreenBoundriesScript.maxX - 80) && !isFadingOut)
        {
            StartCoroutine(FadeOutAndDestroy());
            isFadingOut = true;
        }

        // Ja neko nevelk un kursors pieskaras bumbai
        Vector2 inputPosition;
        if(!TryGetInputPosition(out inputPosition))
            return;


        if (CompareTag("Bomb") && !isExploading &&
            RectTransformUtility.RectangleContainsScreenPoint(
                rectTransform, inputPosition, Camera.main))
        {
            Debug.Log("Bomb hit by cursor (without dragging)");
            TriggerExplosion();
        }

        if(ObjectScript.drag && !isFadingOut &&
            RectTransformUtility.RectangleContainsScreenPoint(
                rectTransform, inputPosition, Camera.main))
        {
            Debug.Log("Obstacle hit by drag");
           if(ObjectScript.lastDragged != null)
            {
                ZaudejumsScript zaudejumsScript = FindFirstObjectByType<ZaudejumsScript>();
                if (zaudejumsScript != null)
                {
                    zaudejumsScript.ShowGameOver();
                }

                StartCoroutine(ShrinkAndDestroy(ObjectScript.lastDragged, 0.5f));
                ObjectScript.lastDragged = null;
                ObjectScript.drag = false;
            }

            StartToDestroy();
        }
    }

    bool TryGetInputPosition(out Vector2 position)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if(Input.touchCount > 0)
        {
            position = Input.GetTouch(0).position;
            return true;
        }
        else
        {
            position = Vector2.zero;
            return false;
        }
#else
        // Desktop/Editor - используем позицию мыши
        position = Input.mousePosition;
        // Возвращаем true только если мышь в пределах экрана
        return position.x >= 0 && position.x <= Screen.width &&
               position.y >= 0 && position.y <= Screen.height;
#endif
    }

    public void TriggerExplosion()
    {
        isExploading = true;
        objectScript.effects.PlayOneShot(objectScript.audioCli[11], 5f);

        Animator animator = GetComponentInChildren<Animator>();
        if(animator != null)
        {
            animator.SetTrigger("explode");
        }

        image.color = Color.red;
        StartCoroutine(RecoverColor(0.4f));

        StartCoroutine(Vibrate());
        StartCoroutine(WaitBeforeExpload());
    }

    IEnumerator WaitBeforeExpload()
    {
        float radius = 0f;
        if (TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider))
            {
            radius = circleCollider.radius * transform.lossyScale.x;
        }
        ExploadAndDestroy(radius);
        yield return new WaitForSeconds(1f);
        ExploadAndDestroy(radius);
        Destroy(gameObject);
    }
    void ExploadAndDestroy(float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach(var hitCollider in hitColliders)
        {
            if(hitCollider != null && hitCollider.gameObject != gameObject)
            {
                FlyingObjectsControllerScript obj =
                    hitCollider.gameObject.GetComponent<FlyingObjectsControllerScript>();
                if (obj != null &&  obj.isExploading)
                {
                    obj.StartToDestroy();
                }
            }
        }
    }

    public void StartToDestroy()
    {
        if (!isFadingOut)
        {
            StartCoroutine(FadeOutAndDestroy());
            isFadingOut = true;

            image.color = Color.cyan;
            StartCoroutine(RecoverColor(0.5f));

            objectScript.effects.PlayOneShot(objectScript.audioCli[5]);

            StartCoroutine(Vibrate());
        }
    }
    IEnumerator Vibrate()
    {
#if UNITY_ANDROID
        Handheld.Vibrate();
#endif

        Vector2 originalPosition = rectTransform.anchoredPosition;
        float duration = 0.3f;
        float elpased = 0f;
        float intensity = 5f;

        while (elpased < duration)
        {
            rectTransform.anchoredPosition =
                originalPosition + Random.insideUnitCircle * intensity;
            elpased += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = originalPosition;
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    IEnumerator FadeOutAndDestroy()
    {
        float t = 0f;
        float startAlpha = canvasGroup.alpha;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f;
        Destroy(gameObject);
    }

    IEnumerator ShrinkAndDestroy(GameObject target, float duration)
    {
        Vector3 orginalScale = target.transform.localScale;
        Quaternion orginalRotation = target.transform.rotation;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            target.transform.localScale = Vector3.Lerp(orginalScale, Vector3.zero, t / duration);
            float angle = Mathf.Lerp(0f, 360f, t / duration);
            target.transform.rotation = Quaternion.Euler(0f, 0f, angle);

            yield return null;
        }
        Destroy(target);
    }

    IEnumerator RecoverColor(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        image.color = originalColor;
    }


}
