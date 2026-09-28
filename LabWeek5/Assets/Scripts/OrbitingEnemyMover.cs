using UnityEngine;

// Makes a meteor orbit around the player.
public class OrbitingEnemyMover : MonoBehaviour
{
    // Smallest allowed orbit radius.
    [SerializeField] private float minOrbitRadius = 3f;
    // Largest allowed orbit radius.
    [SerializeField] private float maxOrbitRadius = 6f;
    // Base orbit speed.
    [SerializeField] private float orbitSpeed = 0.4f;
    // Slowest allowed orbit multiplier.
    [SerializeField] private float minSpeedMultiplier = 0.5f;
    // Fastest allowed orbit multiplier.
    [SerializeField] private float maxSpeedMultiplier = 1.5f;
    // How quickly the meteor turns to face the player.
    [SerializeField] private float facingTurnSpeed = 3f;
    // How quickly the meteor moves toward its orbit point.
    [SerializeField] private float followLerpSpeed = 2f;

    // Cached player target.
    private Transform player;
    // Current orbit angle in radians.
    private float angle;
    // Chosen orbit radius for this meteor.
    private float orbitRadius;
    // Orbit direction: clockwise or counterclockwise.
    private float orbitDirection = 1f;

    private void Start()
    {
        // Find the player once when the meteor spawns.
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Start at a random point around the orbit.
        angle = Random.Range(0f, Mathf.PI * 2f);

        // Give each meteor its own path around the player.
        orbitRadius = Random.Range(minOrbitRadius, maxOrbitRadius);
        orbitDirection = Random.value < 0.5f ? -1f : 1f;
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 toPlayer = player.position - transform.position;
        float distanceSqr = toPlayer.sqrMagnitude;

        // Closer meteors orbit faster.
        float speedMultiplier = Mathf.Clamp(
            20f / Mathf.Max(distanceSqr, 0.01f),
            minSpeedMultiplier,
            maxSpeedMultiplier);

        angle += orbitDirection * orbitSpeed * speedMultiplier * Time.deltaTime;

        // Move toward the current orbit point.
        Vector3 orbitOffset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * orbitRadius;
        Vector3 targetPosition = player.position + orbitOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followLerpSpeed);

        // Turn to face the player.
        float targetAngleDeg = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngleDeg);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * facingTurnSpeed);
    }
}