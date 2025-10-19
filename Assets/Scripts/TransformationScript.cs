using UnityEngine;

public class TransformationScript : MonoBehaviour
{
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public float scaleSpeed = 0.005f;
    public float rotationSpeed = 10f;

    void Update()
    {
        if (ObjectScript.lastDragged != null)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                ObjectScript.lastDragged.GetComponent<RectTransform>().transform.Rotate(
                    0, 0, Time.deltaTime * rotationSpeed);
            }

            if (Input.GetKey(KeyCode.X))
            {
                ObjectScript.lastDragged.GetComponent<RectTransform>().transform.Rotate(
                    0, 0, -Time.deltaTime * rotationSpeed);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y < maxScale)
                {
                    ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale =
                        new Vector3(
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x,
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y + scaleSpeed,
                        1f);
                }
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y > minScale)
                {
                    ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale =
                        new Vector3(
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x,
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y - scaleSpeed,
                        1f);
                }

            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x > minScale)
                {
                    ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale =
                        new Vector3(
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x - scaleSpeed,
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y,
                        1f);
                }
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x < maxScale)
                {
                    ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale =
                        new Vector3(
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.x + scaleSpeed,
                        ObjectScript.lastDragged.GetComponent<RectTransform>().transform.localScale.y,
                        1f);
                }
            }

        }
    }
}