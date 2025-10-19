using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject[] vehicles;
    public GameObject[] dropPlaces;

    public float minDistanceBetweenVehicles = 100f;
    public float minDistanceBetweenDropPlaces = 100f;
    public bool randomizeRotation = false;
    public bool randomizeScale = false;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;

    public float spawnMinX = -800f;
    public float spawnMaxX = 800f;
    public float spawnMinY = -400f;
    public float spawnMaxY = 400f;

    private List<Vector2> vehiclePositions = new List<Vector2>();
    private List<Vector2> dropPlacePositions = new List<Vector2>();

    void Start()
    {
        if (vehicles.Length == 0)
        {
            Debug.LogWarning("CarSpawner: No vehicles assigned!");
        }

        if (dropPlaces.Length == 0)
        {
            Debug.LogWarning("CarSpawner: No drop places assigned!");
        }

        RandomizePositions();

        ObjectScript objectScript = FindFirstObjectByType<ObjectScript>();
        if (objectScript != null)
        {
            objectScript.UpdateStartCoordinates();
            Debug.Log("CarSpawner: Updated ObjectScript start coordinates after randomization");
        }
        else
        {
            Debug.LogWarning("CarSpawner: ObjectScript not found on scene!");
        }
    }

    void RandomizePositions()
    {
        vehiclePositions.Clear();
        dropPlacePositions.Clear();

        RandomizeVehicles();
        RandomizeDropPlaces();

        Debug.Log($"CarSpawner: Randomized {vehicles.Length} vehicles and {dropPlaces.Length} drop places");
    }

    void RandomizeVehicles()
    {
        for (int i = 0; i < vehicles.Length; i++)
        {
            if (vehicles[i] == null)
            {
                Debug.LogWarning($"CarSpawner: Vehicle at index {i} is null!");
                continue;
            }

            RectTransform vehicleRect = vehicles[i].GetComponent<RectTransform>();
            if (vehicleRect == null)
            {
                Debug.LogError($"CarSpawner: Vehicle '{vehicles[i].name}' doesn't have RectTransform!");
                continue;
            }

            Vector2 newPosition = GetRandomPositionOnScreen(vehiclePositions, minDistanceBetweenVehicles);
            vehicleRect.anchoredPosition = newPosition;
            vehiclePositions.Add(newPosition);

            if (randomizeRotation)
            {
                float randomRotation = Random.Range(0f, 360f);
                vehicleRect.localRotation = Quaternion.Euler(0, 0, randomRotation);
            }

            if (randomizeScale)
            {
                float randomScale = Random.Range(minScale, maxScale);
                vehicleRect.localScale = Vector3.one * randomScale;
            }

            Debug.Log($"CarSpawner: Randomized vehicle '{vehicles[i].name}' to {newPosition}");
        }
    }

    void RandomizeDropPlaces()
    {
        for (int i = 0; i < dropPlaces.Length; i++)
        {
            if (dropPlaces[i] == null)
            {
                Debug.LogWarning($"CarSpawner: Drop place at index {i} is null!");
                continue;
            }

            RectTransform dropPlaceRect = dropPlaces[i].GetComponent<RectTransform>();
            if (dropPlaceRect == null)
            {
                Debug.LogError($"CarSpawner: Drop place '{dropPlaces[i].name}' doesn't have RectTransform!");
                continue;
            }

            Vector2 newPosition = GetRandomPositionOnScreen(dropPlacePositions, minDistanceBetweenDropPlaces);
            dropPlaceRect.anchoredPosition = newPosition;
            dropPlacePositions.Add(newPosition);

            if (randomizeRotation)
            {
                float randomRotation = Random.Range(0f, 360f);
                dropPlaceRect.localRotation = Quaternion.Euler(0, 0, randomRotation);
            }

            if (randomizeScale)
            {
                float randomScale = Random.Range(minScale, maxScale);
                dropPlaceRect.localScale = Vector3.one * randomScale;
            }

            Debug.Log($"CarSpawner: Randomized drop place '{dropPlaces[i].name}' to {newPosition}");
        }
    }

    Vector2 GetRandomPositionOnScreen(List<Vector2> existingPositions, float minDistance)
    {
        Vector2 randomPosition;
        int maxAttempts = 50;
        int attempts = 0;

        do
        {
            float randomX = Random.Range(spawnMinX, spawnMaxX);
            float randomY = Random.Range(spawnMinY, spawnMaxY);
            randomPosition = new Vector2(randomX, randomY);
            attempts++;

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning($"CarSpawner: Could not find position with {minDistance} distance after {maxAttempts} attempts");
                break;
            }
        }
        while (!IsPositionValid(randomPosition, existingPositions, minDistance));

        return randomPosition;
    }

    bool IsPositionValid(Vector2 position, List<Vector2> existingPositions, float minDistance)
    {
        foreach (Vector2 existingPos in existingPositions)
        {
            if (Vector2.Distance(position, existingPos) < minDistance)
            {
                return false;
            }
        }
        return true;
    }

    public void RandomizeAgain()
    {
        RandomizePositions();

        ObjectScript objectScript = FindFirstObjectByType<ObjectScript>();
        if (objectScript != null)
        {
            objectScript.UpdateStartCoordinates();
            Debug.Log("CarSpawner: Updated ObjectScript start coordinates after re-randomization");
        }
    }
}
