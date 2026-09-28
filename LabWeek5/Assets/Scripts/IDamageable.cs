// Simple team IDs for damage filtering.
public enum Team
{
    Player,
    Enemy
}

// Anything that can receive damage implements this.
public interface IDamageable
{
    // Team ownership for hit checks.
    Team Team { get; }
    // Apply incoming damage.
    void TakeDamage(int amount);
}