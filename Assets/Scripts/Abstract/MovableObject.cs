using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody2D rb;
    
    [SerializeField]
    protected float speed = 1;

    protected Vector2 direction = new Vector2(0, 0);
    protected Vector2? target = null;
    protected float lastDirectionX = 1.0f;

    protected void FixedUpdate()
    {
        Vector2 targetPosition;

        lastDirectionX = direction.x;
        if (target != null)
        {
            targetPosition = Vector2.MoveTowards(rb.position, target.Value, speed * Time.fixedDeltaTime);
            direction = targetPosition - rb.position;
        }
        else
        {
            var movement = direction * speed;
            targetPosition = rb.position + movement * Time.fixedDeltaTime;
        }

        rb.MovePosition(targetPosition);
    }   
}
