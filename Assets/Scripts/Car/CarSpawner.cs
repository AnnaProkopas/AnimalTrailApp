using JetBrains.Annotations;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] cars;
    [SerializeField] private GameObject worldBoundary;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float speed = 4.0f;
    
    [CanBeNull] private GameObject currentCar;
    private float maxPointX;

    void Start()
    {
        PolygonCollider2D polygonCollider = worldBoundary.GetComponent<PolygonCollider2D>();
        foreach (var point in polygonCollider.points)
        {
            maxPointX = Mathf.Max(maxPointX, point.x);
        }

        maxPointX += speed;
    }

    void Update()
    {
        if (currentCar == null || currentCar.transform.position.x  > maxPointX) 
        {
            if (currentCar != null) Destroy(currentCar);
            int carInd = Random.Range(0, cars.Length);
            currentCar = Instantiate(cars[carInd], spawnPoint.position, Quaternion.identity);
        }
    }
}
