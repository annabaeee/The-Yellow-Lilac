using UnityEngine;
using UnityEngine.UI;

public class AbyssController : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform lilacTransform;
    [SerializeField] private Image yellowOverlay;
    [SerializeField] private Transform cameraTransform;

    [Header("Effect Settings")]
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float minDistance = 2f;

    // --- NEW VARIABLE ---
    [Tooltip("Controls the curve of the effects. 1 = linear. >1 = starts slow, ramps up fast. <1 = starts fast, ramps up slow.")]
    [SerializeField] private float effectExponent = 2.0f;

    private Vector3 cameraOriginalLocalPos;

    void Start()
    {
        if (yellowOverlay != null)
        {
            yellowOverlay.color = new Color(
                yellowOverlay.color.r,
                yellowOverlay.color.g,
                yellowOverlay.color.b,
                0f
            );
        }
        else
        {
            Debug.LogError("YellowOverlay UI Image is not assigned!");
        }

        if (cameraTransform != null)
        {
            cameraOriginalLocalPos = cameraTransform.localPosition;
        }
        else
        {
            Debug.LogError("Camera Transform is not assigned!");
        }

        if (playerTransform == null || lilacTransform == null)
        {
            Debug.LogError("Player or Lilac transform is not assigned!");
        }
    }

    void Update()
    {
        if (playerTransform == null || lilacTransform == null) return;

        // 1. Calculate distance
        float distance = Vector3.Distance(playerTransform.position, lilacTransform.position);

        // 2. Calculate effect intensity (0.0 to 1.0)
        float intensity = Mathf.InverseLerp(maxDistance, minDistance, distance);
        intensity = Mathf.Clamp01(intensity); // Ensure value stays between 0 and 1

        // Apply the exponent to make the curve gradual
        float gradualIntensity = Mathf.Pow(intensity, effectExponent);

        // 3. Apply the yellow screen effect
        ApplyYellowFade(gradualIntensity); // Use the new gradual value
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Recalculate intensity here as well
        float distance = Vector3.Distance(playerTransform.position, lilacTransform.position);
        float intensity = Mathf.InverseLerp(maxDistance, minDistance, distance);
        intensity = Mathf.Clamp01(intensity);

        // Apply the exponent to make the curve gradual
        float gradualIntensity = Mathf.Pow(intensity, effectExponent);
    }

    private void ApplyYellowFade(float intensity)
    {
        if (yellowOverlay == null) return;
        Color newColor = yellowOverlay.color;
        newColor.a = intensity;
        yellowOverlay.color = newColor;
    }
}