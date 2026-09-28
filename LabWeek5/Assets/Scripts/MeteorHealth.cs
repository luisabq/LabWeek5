using UnityEngine;

// Stores health and death behavior for meteors.
public class MeteorHealth : MonoBehaviour, IDamageable
{
    // Starting health.
    [SerializeField] private int maxHitPoints = 1;
    // Score awarded when destroyed.
    [SerializeField] private int scoreValue = 1;
    // Marks this as the larger meteor type.
    [SerializeField] private bool isBigMeteor = false;

    // Meteors belong to the enemy team.
    public Team Team => Team.Enemy;
    // Exposes the meteor size type.
    public bool IsBigMeteor => isBigMeteor;

    // Current health at runtime.
    private int currentHitPoints;

    private void Awake()
    {
        // Start full health.
        currentHitPoints = maxHitPoints;
    }

    public void TakeDamage(int amount)
    {
        // Reduce health and destroy on zero.
        currentHitPoints -= amount;
        if (currentHitPoints > 0) return;

        GameEvents.RaiseMeteorDestroyed(this, scoreValue);
        Destroy(gameObject);
    }
}