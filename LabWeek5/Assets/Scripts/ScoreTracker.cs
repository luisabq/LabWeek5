using UnityEngine;

// Tracks the current score.
public class ScoreTracker : MonoBehaviour
{
    // Current total score.
    public int Score { get; private set; }

    // Listen for meteor kills.
    private void OnEnable() => GameEvents.OnMeteorDestroyed += HandleMeteorDestroyed;
    // Stop listening when disabled.
    private void OnDisable() => GameEvents.OnMeteorDestroyed -= HandleMeteorDestroyed;

    private void HandleMeteorDestroyed(MeteorHealth meteor, int scoreValue)
    {
        // Add the meteor's score value.
        Score += scoreValue;
    }
}