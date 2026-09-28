using UnityEngine;

// Deals damage when this object enters a trigger.
public class DamageOnTrigger : MonoBehaviour
{
    // Damage dealt on contact.
    [SerializeField] private int damage = 1;
    // Destroy this object after a hit.
    [SerializeField] private bool destroySelfOnHit = true;
    // Which team this object can hurt.
    [SerializeField] private Team targetTeam = Team.Enemy;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignore anything that cannot take damage.
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable == null || damageable.Team != targetTeam) return;

        damageable.TakeDamage(damage);

        if (destroySelfOnHit)
        {
            Destroy(gameObject);
        }
    }
}