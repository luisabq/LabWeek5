using System;
using UnityEngine;

// Central event hub for gameplay messages.
public static class GameEvents
{
    // Fired when the player is spawned.
    public static event Action<Transform> OnPlayerSpawned;
    // Fired when the player dies.
    public static event Action OnPlayerDied;
    // Fired when any meteor is destroyed.
    public static event Action<MeteorHealth, int> OnMeteorDestroyed;
    // Fired when a big meteor appears.
    public static event Action OnBigMeteorSpawned;
    // Fired when a big meteor is destroyed.
    public static event Action OnBigMeteorDestroyed;

    // Broadcast the spawned player transform.
    public static void RaisePlayerSpawned(Transform player) => OnPlayerSpawned?.Invoke(player);

    // Broadcast player death.
    public static void RaisePlayerDied() => OnPlayerDied?.Invoke();

    // Broadcast meteor destruction and score value.
    public static void RaiseMeteorDestroyed(MeteorHealth meteor, int scoreValue) =>
        OnMeteorDestroyed?.Invoke(meteor, scoreValue);

    // Broadcast big meteor spawn.
    public static void RaiseBigMeteorSpawned() => OnBigMeteorSpawned?.Invoke();

    // Broadcast big meteor destruction.
    public static void RaiseBigMeteorDestroyed() => OnBigMeteorDestroyed?.Invoke();
}