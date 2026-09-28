using UnityEngine;

// Spawns regular meteors and occasional big meteors.
public class MeteorSpawner : MonoBehaviour
{
    // Prefab for normal meteors.
    [SerializeField] private GameObject meteorPrefab;
    // Prefab for big meteors.
    [SerializeField] private GameObject bigMeteorPrefab;
    // Time between normal spawns.
    [SerializeField] private float spawnInterval = 2f;
    // Delay before the first spawn.
    [SerializeField] private float firstSpawnDelay = 1f;
    // Normal kills needed before a big meteor appears.
    [SerializeField] private int meteorsPerBigMeteor = 5;
    // Extra distance above the screen when spawning.
    [SerializeField] private float spawnMargin = 2f;

    // Tracks normal meteor kills.
    private int meteorKillCount;

    private void OnEnable()
    {
        // Listen for kill and game-over events.
        GameEvents.OnMeteorDestroyed += HandleMeteorDestroyed;
        GameEvents.OnPlayerDied += StopSpawning;
    }

    private void OnDisable()
    {
        // Stop listening when disabled.
        GameEvents.OnMeteorDestroyed -= HandleMeteorDestroyed;
        GameEvents.OnPlayerDied -= StopSpawning;
    }

    private void Start()
    {
        // Start the repeating spawn cycle.
        InvokeRepeating(nameof(SpawnMeteor), firstSpawnDelay, spawnInterval);
    }

    public void StopSpawning()
    {
        // Stop all scheduled spawns.
        CancelInvoke();
    }

    private void SpawnMeteor()
    {
        // Spawn one regular meteor.
        Instantiate(meteorPrefab, RandomSpawnPosition(), Quaternion.identity);
    }

    private void HandleMeteorDestroyed(MeteorHealth meteor, int scoreValue)
    {
        // Big meteors only trigger the zoom reset event.
        if (meteor.IsBigMeteor)
        {
            GameEvents.RaiseBigMeteorDestroyed();
            return;
        }

        // Count normal kills toward the next big meteor.
        meteorKillCount++;
        if (meteorKillCount < meteorsPerBigMeteor) return;

        meteorKillCount = 0;
        Instantiate(bigMeteorPrefab, RandomSpawnPosition(), Quaternion.identity);
        GameEvents.RaiseBigMeteorSpawned();
    }

    // Pick a spawn point just above the visible camera area.
    private Vector3 RandomSpawnPosition()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            return new Vector3(Random.Range(-8f, 8f), 7.5f, 0f);
        }

        Vector3 center = cam.transform.position;

        // Measure the visible half-height at gameplay depth.
        float halfHeight = cam.orthographic
            ? cam.orthographicSize
            : Mathf.Abs(center.z) * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * cam.aspect;

        return new Vector3(
            center.x + Random.Range(-halfWidth, halfWidth),
            center.y + halfHeight + spawnMargin,
            0f);
    }
}