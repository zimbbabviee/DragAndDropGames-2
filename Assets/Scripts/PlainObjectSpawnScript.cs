using UnityEngine;

//CHANGES NOT NEEDED FOR ANDROID
public class PlainObjectSpawnScript : MonoBehaviour
{
    Screen_Boundaries screenBoundriesScript;
    public GameObject[] cludsPrefabs;
    public GameObject[] objectPrefabs;
    public Transform spawnPoint;

    public float cloudSpawnInterval = 2f;
    public float objectSpawnInterval = 3f;
    private float minY, maxY;
    public float cloudMinSpeed = 50f;
    public float cloudMaxSpeed = 150f;
    public float objectMinSpeed = 50f;
    public float objectMaxSpeed = 200f;

    public int flyingObjectsSortingOrder = -1;

    private bool isGameActive = true;

    void Start()
    {
        screenBoundriesScript = FindFirstObjectByType<Screen_Boundaries>();
        minY = screenBoundriesScript.minY;
        maxY = screenBoundriesScript.maxY;

        Canvas parentCanvas = spawnPoint.GetComponentInParent<Canvas>();
        if (parentCanvas != null)
        {
            parentCanvas.sortingOrder = flyingObjectsSortingOrder;
        }

        InvokeRepeating(nameof(SpawnCloud), 0f, cloudSpawnInterval);
        InvokeRepeating(nameof(SpawnObject), 0f, objectSpawnInterval);
    }

    public void StopSpawning()
    {
        isGameActive = false;
        CancelInvoke(nameof(SpawnCloud));
        CancelInvoke(nameof(SpawnObject));
    }

    void SpawnCloud()
    {
        if (cludsPrefabs.Length == 0)
            return;

        GameObject cloudPrefab = cludsPrefabs[Random.Range(0, cludsPrefabs.Length)];
        float y = Random.Range(minY, maxY);
        Vector3 spawnPosition = new Vector3(spawnPoint.position.x, y, spawnPoint.position.z);
        GameObject cloud =
            Instantiate(cloudPrefab, spawnPosition, cloudPrefab.transform.rotation, spawnPoint);
        float movementSpeed = Random.Range(cloudMinSpeed, cloudMaxSpeed);
        FlyingObjectsControllerScript controller =
            cloud.GetComponent<FlyingObjectsControllerScript>();
        controller.speed = movementSpeed;

    }

    void SpawnObject()
    {
        if (objectPrefabs.Length == 0)
            return;

        GameObject objectPrefab = objectPrefabs[Random.Range(0, objectPrefabs.Length)];
        float y = Random.Range(minY, maxY);

        Vector3 spawnPosition = new Vector3(-spawnPoint.position.x, y, spawnPoint.position.z);

        GameObject flyingObject =
            Instantiate(objectPrefab, spawnPosition, objectPrefab.transform.rotation, spawnPoint);
        float movementSpeed = Random.Range(objectMinSpeed, objectMaxSpeed);
        FlyingObjectsControllerScript controller =
            flyingObject.GetComponent<FlyingObjectsControllerScript>();
        controller.speed = -movementSpeed;
    }
}
