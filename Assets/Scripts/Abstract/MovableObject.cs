using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody2D rb;
    
    [SerializeField]
    protected float speed = 1;

    protected Vector2 direction = new Vector2(0, 0);
    protected Vector2 movement;

    protected virtual void Update()
    {
        movement = direction * speed;
    }
    
    protected void FixedUpdate() 
    {
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }   
}
