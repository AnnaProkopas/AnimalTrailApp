using JetBrains.Annotations;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject car;
    [SerializeField]
    private GameObject worldBoundary;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float speed = 4.0f;
    
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
            currentCar = Instantiate(car, spawnPoint.position, Quaternion.identity);
        }
    }
}
