<<<<<<< HEAD
using System.Collections;
using System.Collections.Generic;
=======
>>>>>>> 45429f9bcd19d08c2d33868a5526b477a3dd2ff2
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
<<<<<<< HEAD
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
}
=======
    public GameObject lastDragged = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
>>>>>>> 45429f9bcd19d08c2d33868a5526b477a3dd2ff2
