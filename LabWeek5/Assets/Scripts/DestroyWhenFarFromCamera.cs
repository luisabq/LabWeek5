using UnityEngine;

// Destroys objects once they drift too far from the camera.
public class DestroyWhenFarFromCamera : MonoBehaviour
{
    // Max allowed distance from the camera.
    [SerializeField] private float maxDistance = 20f;

    // Cached main camera.
    private Camera cam;

    private void Start()
    {
        // Grab the camera once.
        cam = Camera.main;
    }

    private void Update()
    {
        if (cam == null) return;

        // Remove the object when it is far enough away.
        Vector2 offset = transform.position - cam.transform.position;
        if (offset.sqrMagnitude > maxDistance * maxDistance)
        {
            Destroy(gameObject);
        }
    }
}