using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

// Handles camera shake and zoom effects.
public class CinemachineFxController : MonoBehaviour
{
    // Cinemachine camera to control.
    [SerializeField] private CinemachineCamera vcam;
    // Shake strength.
    [SerializeField] private float shakeAmplitude = 2f;
    // Shake speed.
    [SerializeField] private float shakeFrequency = 2f;
    // How long the shake lasts.
    [SerializeField] private float shakeDuration = 0.2f;
    // How far to zoom out for big meteors.
    [SerializeField] private float zoomOutMultiplier = 1.4f;
    // How long the zoom takes.
    [SerializeField] private float zoomDuration = 1f;

    // Noise source used for shake.
    private CinemachineBasicMultiChannelPerlin noise;
    // Main camera used to check projection mode.
    private Camera mainCam;
    // Default lens value to restore later.
    private float defaultLensSize;

    private void Awake()
    {
        // Cache effect references.
        noise = vcam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        mainCam = Camera.main;
        defaultLensSize = GetLensSize();

        if (noise != null)
        {
            noise.AmplitudeGain = 0f;
        }
    }

    private void OnEnable()
    {
        // Listen for camera-related gameplay events.
        GameEvents.OnPlayerSpawned += HandlePlayerSpawned;
        GameEvents.OnMeteorDestroyed += HandleMeteorDestroyed;
        GameEvents.OnBigMeteorSpawned += HandleBigMeteorSpawned;
        GameEvents.OnBigMeteorDestroyed += HandleBigMeteorDestroyed;
    }

    private void OnDisable()
    {
        // Stop listening when disabled.
        GameEvents.OnPlayerSpawned -= HandlePlayerSpawned;
        GameEvents.OnMeteorDestroyed -= HandleMeteorDestroyed;
        GameEvents.OnBigMeteorSpawned -= HandleBigMeteorSpawned;
        GameEvents.OnBigMeteorDestroyed -= HandleBigMeteorDestroyed;
    }

    private void HandlePlayerSpawned(Transform player)
    {
        // Hook the camera to the spawned player.
        var target = vcam.Target;
        target.TrackingTarget = player;
        vcam.Target = target;
    }

    // Small meteor hit: quick shake.
    private void HandleMeteorDestroyed(MeteorHealth meteor, int scoreValue) => StartCoroutine(Shake());
    // Big meteor appears: zoom out.
    private void HandleBigMeteorSpawned() => StartCoroutine(ZoomTo(defaultLensSize * zoomOutMultiplier));
    // Big meteor gone: return to normal zoom.
    private void HandleBigMeteorDestroyed() => StartCoroutine(ZoomTo(defaultLensSize));

    // True when the live camera is orthographic.
    private bool IsOrthographic => mainCam != null && mainCam.orthographic;

    private float GetLensSize()
    {
        // Read the active zoom value.
        return IsOrthographic ? vcam.Lens.OrthographicSize : vcam.Lens.FieldOfView;
    }

    private void SetLensSize(float size)
    {
        // Lens settings are a struct, so write back the edited copy.
        var lens = vcam.Lens;
        if (IsOrthographic)
        {
            lens.OrthographicSize = size;
        }
        else
        {
            lens.FieldOfView = size;
        }
        vcam.Lens = lens;
    }

    private IEnumerator Shake()
    {
        // Skip shake if no noise component is set.
        if (noise == null) yield break;

        noise.AmplitudeGain = shakeAmplitude;
        noise.FrequencyGain = shakeFrequency;
        yield return new WaitForSeconds(shakeDuration);
        noise.AmplitudeGain = 0f;
    }

    private IEnumerator ZoomTo(float targetSize)
    {
        // Blend from the current zoom to the target zoom.
        float startSize = GetLensSize();
        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;
            SetLensSize(Mathf.Lerp(startSize, targetSize, elapsed / zoomDuration));
            yield return null;
        }

        SetLensSize(targetSize);
    }
}