using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public GameObject[] vehicles;
    [HideInInspector]
    public Vector2[] startCoordinates;
    public Canvas can;
    public AudioSource effects;
    public AudioClip[] audioCli;
    [HideInInspector]
    public bool rightPlace = false;
    public static GameObject lastDragged = null;
    public static bool drag = false;


    void Awake()
    {
        startCoordinates = new Vector2[vehicles.Length];
        Debug.Log(vehicles.Length);
        Debug.Log(startCoordinates.Length);
        for (int i = 0; i < vehicles.Length; i++)
        {
            startCoordinates[i] = vehicles[i].GetComponent<RectTransform>().localPosition;
            Debug.Log(vehicles[i].GetComponent<RectTransform>().localPosition);
        }
    }

    public void UpdateStartCoordinates()
    {
        if (startCoordinates == null || startCoordinates.Length != vehicles.Length)
        {
            startCoordinates = new Vector2[vehicles.Length];
        }

        for (int i = 0; i < vehicles.Length; i++)
        {
            if (vehicles[i] != null)
            {
                startCoordinates[i] = vehicles[i].GetComponent<RectTransform>().localPosition;
                Debug.Log($"Updated start coordinates for {vehicles[i].name}: {startCoordinates[i]}");
            }
        }
    }

}
