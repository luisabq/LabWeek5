using UnityEngine;
using UnityEngine.InputSystem;

// Handles player movement with smooth acceleration.
public class PlayerMovement : MonoBehaviour
{
    // Max move speed.
    [SerializeField] private float maxSpeed = 8f;
    // How quickly the ship speeds up.
    [SerializeField] private float acceleration = 20f;
    // How quickly the ship slows down.
    [SerializeField] private float deceleration = 8f;
    // Visual tilt while moving sideways.
    [SerializeField] private float tiltAmount = 3f;

    // Input action for movement.
    private InputAction moveAction;
    // Current movement velocity.
    private Vector2 velocity;

    private void Awake()
    {
        // Build a simple WASD and arrow-key movement action.
        moveAction = new InputAction("Move");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");
    }

    // Start reading move input.
    private void OnEnable() => moveAction.Enable();
    // Stop reading move input.
    private void OnDisable() => moveAction.Disable();

    private void Update()
    {
        // Clamp diagonal input so it is not faster.
        Vector2 input = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);

        if (input.sqrMagnitude > 0.01f)
        {
            // Accelerate toward the current input direction.
            velocity = Vector2.MoveTowards(velocity, input * maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            // Slow to a stop when there is no input.
            velocity = Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.deltaTime);
        }

        // Move in world space.
        transform.Translate(velocity * Time.deltaTime, Space.World);

        // Tilt a little while strafing.
        transform.rotation = Quaternion.Euler(0f, 0f, -velocity.x * tiltAmount);
    }
}