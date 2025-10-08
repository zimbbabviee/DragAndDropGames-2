using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject carPrefab;   
    public int carCount = 10;      

    [Header("Postion")]
    public Vector3 minPosition;    
    public Vector3 maxPosition;   

    void Start()
    {
   
        for (int i = 0; i < carCount; i++)
        {
            SpawnCar();
        }
    }

    void SpawnCar()
    {
  
        Vector3 randomPos = new Vector3(
            Random.Range(minPosition.x, maxPosition.x),
            Random.Range(minPosition.y, maxPosition.y),
            Random.Range(minPosition.z, maxPosition.z)
        );

        Instantiate(carPrefab, randomPos, Quaternion.identity);

     
        Debug.Log("Spawning car at " + randomPos);
    }
}
