using UnityEngine;

// Handles player death.
public class PlayerHealth : MonoBehaviour, IDamageable
{
    // Prevents death from firing twice.
    private bool isDead;

    // Players belong to the player team.
    public Team Team => Team.Player;

    public void TakeDamage(int amount)
    {
        // Ignore extra hits after death.
        if (isDead) return;

        isDead = true;
        GameEvents.RaisePlayerDied();
        Destroy(gameObject);
    }
}