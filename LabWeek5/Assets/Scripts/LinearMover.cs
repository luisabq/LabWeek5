using UnityEngine;

// Moves an object in a straight line.
public class LinearMover : MonoBehaviour
{
    // Travel direction.
    [SerializeField] private Vector3 direction = Vector3.down;
    // Travel speed.
    [SerializeField] private float speed = 2f;

    private void Update()
    {
        // Move every frame using world time.
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }
}