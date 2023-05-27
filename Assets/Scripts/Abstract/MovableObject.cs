using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float speed = 1;

    protected Vector2 direction = new Vector2(0, 0);
    protected float lastDirectionX = 1.0f;

    protected void FixedUpdate()
    {
        lastDirectionX = Mathf.Sign(direction.x);
        var movement = direction * speed;
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
