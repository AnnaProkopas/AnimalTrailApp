using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameObjects.Car
{
    public class CarSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] cars;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform collapsePoint;
    
        [CanBeNull] private GameObject currentCar;

        void Update()
        {
            if (currentCar == null || Norm(currentCar.transform.position - collapsePoint.position) < 0.1 ) 
            {
                if (currentCar != null) Destroy(currentCar);
                int carInd = Random.Range(0, cars.Length);
                currentCar = Instantiate(cars[carInd], spawnPoint.position, Quaternion.identity);
                currentCar.GetComponent<Car>().SetDirection(collapsePoint.position);
            }
        }

        private float Norm(Vector3 v)
        {
            return Mathf.Max(Mathf.Abs(v.x), Mathf.Abs(v.y));
        }
    }
}
