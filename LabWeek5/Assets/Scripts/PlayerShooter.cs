using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// Fires the player's laser with a cooldown.
public class PlayerShooter : MonoBehaviour
{
    // Projectile prefab to spawn.
    [SerializeField] private GameObject laserPrefab;
    // Time between shots.
    [SerializeField] private float cooldown = 1f;

    // Input action for firing.
    private InputAction fireAction;
    // Blocks firing until the cooldown ends.
    private bool canShoot = true;

    private void Awake()
    {
        // Bind fire to the space bar.
        fireAction = new InputAction("Fire", binding: "<Keyboard>/space");
    }

    private void OnEnable()
    {
        // Start listening for fire input.
        fireAction.performed += OnFire;
        fireAction.Enable();
    }

    private void OnDisable()
    {
        // Stop listening for fire input.
        fireAction.performed -= OnFire;
        fireAction.Disable();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        // Ignore input while the weapon is cooling down.
        if (!canShoot) return;

        // Spawn one laser just above the player.
        Instantiate(laserPrefab, transform.position + Vector3.up, Quaternion.identity);
        canShoot = false;
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        // Wait, then allow the next shot.
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }
}