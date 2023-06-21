using System.Collections.Generic;
using UnityEngine;

namespace GameObjects.AnimalForFood
{
    public class MouseSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] food;
        [SerializeField]
        public Transform[] spawnPoints;
        [SerializeField]
        public int maxAmountFoodOnScene;
        [SerializeField]
        private GameObject worldBoundary;
        [SerializeField]
        private float spawnStartTime;

        private float timeBetweenSpawn;
        private List<GameObject> currentFood;

        private Vector2 minPoint;
        private Vector2 maxPoint;

        // TODO переписать на анимацию исчезания (отлета у птицы) при сильном удалении от стартовой позиции
        private void Start() 
        {
            timeBetweenSpawn = spawnStartTime;
            currentFood = new List<GameObject>();
            PolygonCollider2D polygonCollider = worldBoundary.GetComponent<PolygonCollider2D>();
            foreach (var point in polygonCollider.points)
            {
                minPoint.x = Mathf.Min(minPoint.x, point.x);
                minPoint.y = Mathf.Min(minPoint.y, point.y);
                maxPoint.x = Mathf.Max(maxPoint.x, point.x);
                maxPoint.y = Mathf.Max(maxPoint.y, point.y);
            }
        }

        private void Update() 
        {
            if (timeBetweenSpawn <= 0)
            {
                if (currentFood.Count > 0) 
                {
                    List<GameObject> newFoodList = new List<GameObject>();
                    foreach (var item in currentFood)
                    {
                        if (item != null)
                        {
                            Vector2 position = item.transform.position;
                            if (position.x < minPoint.x || position.y < minPoint.y || position.x > maxPoint.x ||
                                position.y > maxPoint.y)
                            {
                                Destroy(item);
                            }
                            else
                            {
                                newFoodList.Add(item);
                            }
                        }
                    }
                    currentFood = newFoodList;
                }

                if (currentFood.Count < maxAmountFoodOnScene) 
                {
                    // TODO: сделать что то с двумя мышками в одной клетке
                    int foodInd = Random.Range(0, food.Length);
                    int positionInd = Random.Range(0, spawnPoints.Length);
                    currentFood.Add(Instantiate(food[foodInd], spawnPoints[positionInd].transform.position, Quaternion.identity));
                }

                timeBetweenSpawn = spawnStartTime;
            } else {
                timeBetweenSpawn -= Time.deltaTime;
            }
        }
    }
}
