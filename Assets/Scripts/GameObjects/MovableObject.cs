using UnityEngine;

namespace GameObjects
{
    public class MovableObject : MonoBehaviour
    {
        [SerializeField] protected Rigidbody2D rb;
        protected float speed;

        protected Vector2 direction = new Vector2(0, 0);
        protected float lastDirectionX = 1.0f;

        protected void FixedUpdate()
        {
            lastDirectionX = Mathf.Sign(direction.x);
            var movement = direction.normalized * speed;
            Vector2 targetPosition = rb.position + movement * Time.fixedDeltaTime;

            rb.MovePosition(targetPosition);
        }

        private Vector2 getDirectionByTarget()
        {
            Vector2 target = new Vector2();
            Vector2 targetPosition = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            return targetPosition - rb.position;
        }
    }
}
