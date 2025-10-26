using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class ApartmentDirector : MonoBehaviour
{
    public static ApartmentDirector Instance { get; private set; }
    [Header("Notes")]
    private int currentNote = 0;
    [SerializeField] private InteractableObject[] notes;

    [Header("Book")]
    [SerializeField] private Renderer bookRenderer;
    [SerializeField] private Material bookNewMaterial;

    [Header("Lighting")]
    [SerializeField] private Light[] apartmentLight;
    [SerializeField] private Color newLightColor;
    [SerializeField] private float lightChangeDuration = 2.0f;

    [Header("Painting")]
    [SerializeField] private Renderer[] paintingsToWarp;
    private Material[] paintingMaterials;
    [SerializeField] private float fadeDuration;

    [Header("Audio")]
    [SerializeField] private AudioSource melodyAudioSource;
    [SerializeField] private AudioSource whisperAudioSource;

    [Header("Effects")]
    [SerializeField] private GameObject shadowEffect;
    [SerializeField] private float shadowMoveDistance = 10f;
    [SerializeField] private float shadowMoveSpeed = 2f;
    private Vector3 shadowOriginalPosition;
    private UnityEngine.Video.VideoPlayer shadowVideoPlayer;
    [SerializeField] private GameObject vasePrefab;
    [SerializeField] private GameObject pizzaBox;

    [Header("Camera")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Vector3 targetLookDirection = Vector3.forward;
    [SerializeField] private float lookThreshold = 0.9f;

    [Header("Scene")]
    [SerializeField]
    private string abyssSceneName = "AbyssScene";

    public void TriggerNoteShift(int noteID)
    {
        switch (noteID)
        {
            case 0:
                StartCoroutine(FirstShiftSequence());
                break;
            case 1:
                StartCoroutine(SecondShiftSequence());
                break;
            case 2:
                StartCoroutine(ThirdShiftSequence());
                break;
            case 3:
                StartCoroutine(FourthShiftSequence());
                break;
            case 4:
                break;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        paintingMaterials = new Material[paintingsToWarp.Length];

        for (int i = 0; i < paintingsToWarp.Length; i++)
        {
            paintingMaterials[i] = paintingsToWarp[i].material;
        }

        foreach (Material paintingMaterial in paintingMaterials)
        {
            paintingMaterial.SetFloat("_FloatAmount", 1f);
        }

        if (shadowEffect != null)
        {
            shadowEffect.SetActive(false); // Start invisible
            shadowOriginalPosition = shadowEffect.transform.position; // Store original position
            shadowVideoPlayer = shadowEffect.GetComponent<UnityEngine.Video.VideoPlayer>();
            if (shadowVideoPlayer == null)
            {
                Debug.LogWarning("ShadowEffect is missing a VideoPlayer component!");
            }
        }
        else
        {
            Debug.LogError("ShadowEffect GameObject is not assigned in the inspector!");
        }

        if (playerCamera == null)
        {
            Debug.LogError("PlayerCamera is not assigned in the inspector! Please assign it.");
        }

        if (vasePrefab != null)
        {
            vasePrefab.SetActive(false);
        }

        if (pizzaBox != null)
        {
            pizzaBox.SetActive(true);
        }
    }

    private IEnumerator FirstShiftSequence()
    {
        Debug.Log("first shift sequence started");
        bookRenderer.material = bookNewMaterial;
        yield return null;
    }

    private IEnumerator SecondShiftSequence()
    {
        Debug.Log("second shift sequence started");
        StartCoroutine(FadeLightColor(apartmentLight, newLightColor, lightChangeDuration));
        yield return new WaitForSeconds(3f);
        StartCoroutine(WarpPainting(paintingMaterials, fadeDuration));
    }

    private IEnumerator ThirdShiftSequence()
    {
        Debug.Log("third shift sequence started. waiting for player to look");
        if (melodyAudioSource != null)
        {
            melodyAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("ThirdShiftAudioSource is not assigned!");
        }
        Vector3 normalizedTargetDir = targetLookDirection.normalized;

        while (true)
        {
            if (playerCamera == null)
            {
                Debug.LogError("PlayerCamera is not set. Aborting ThirdShift.");
                yield break;
            }

            Vector3 cameraForward = playerCamera.forward;
            float dot = Vector3.Dot(cameraForward, normalizedTargetDir);

            if (dot > lookThreshold)
            {
                Debug.Log("Player is looking in the target direction. Starting shadow effect.");
                break;
            }

            yield return null;
        }

        shadowEffect.transform.position = shadowOriginalPosition;
        shadowEffect.SetActive(true);
        if (shadowVideoPlayer != null)
        {
            shadowVideoPlayer.Play();
        }

        StartCoroutine(MoveShadowEffect());
    }
    private IEnumerator FourthShiftSequence()
    {
        Debug.Log("Fourth shift sequence started.");
        if (whisperAudioSource != null)
        {
            whisperAudioSource.Play();
        }

        if (vasePrefab != null)
        {
            vasePrefab.SetActive(true);
        }
        else
        {
            Debug.LogWarning("VasePrefab is not assigned in the inspector for the fourth shift!");
        }

        if (pizzaBox != null)
        {
            pizzaBox.SetActive(false);
        }

        yield return null;
    }

    private IEnumerator FifthShiftSequence()
    {
        Debug.Log("Fifth and final shift sequence started. Loading abyss.");

        SceneManager.LoadScene(abyssSceneName);
        yield return null;
    }

    public void TriggerFinalShift()
    {
        StartCoroutine(FifthShiftSequence());
    }

    private IEnumerator WarpPainting(Material[] paintingMaterials, float fadeDuration)
    {
        float time = 0;
        foreach (Material paintingMaterial in paintingMaterials)
        {
            paintingMaterial.SetFloat("_FloatAmount", 1f);
        }
        while (time < fadeDuration)
        {
            foreach (Material paintingMaterial in paintingMaterials)
            {
                paintingMaterial.SetFloat("_FloatAmount", Mathf.Lerp(1f, 0f, time / fadeDuration));
                time += Time.deltaTime;
                yield return null;
            }
        }
        foreach (Material paintingMaterial in paintingMaterials)
        {
            paintingMaterial.SetFloat("_FloatAmount", 0f);
        }
    }

    private IEnumerator FadeLightColor(Light[] lights, Color targetColor, float duration)
    {
        float time = 0;
        Color startColor = lights[0].color;
        while (time < duration)
        {
            foreach (Light light in lights)
            {
                light.color = Color.Lerp(startColor, targetColor, time / duration);
            }
            time += Time.deltaTime;
            yield return null;
        }
        foreach (Light light in lights)
        {
            light.color = targetColor;
        }
    }

    private IEnumerator MoveShadowEffect()
    {
        Vector3 startPos = shadowOriginalPosition;
        Vector3 targetPos = startPos + shadowEffect.transform.right * shadowMoveDistance;

        float distanceToTarget = Vector3.Distance(shadowEffect.transform.position, targetPos);
        while (distanceToTarget > 0.01f)
        {
            shadowEffect.transform.position = Vector3.MoveTowards(
                shadowEffect.transform.position,
                targetPos,
                shadowMoveSpeed * Time.deltaTime
            );

            distanceToTarget = Vector3.Distance(shadowEffect.transform.position, targetPos);

            yield return null;
        }

        Debug.Log("Shadow effect reached target. Disabling.");
        shadowEffect.transform.position = targetPos;

        if (shadowVideoPlayer != null)
        {
            shadowVideoPlayer.Stop();
        }

        shadowEffect.SetActive(false);
        shadowEffect.transform.position = shadowOriginalPosition;
    }

    public void NextNote()
    {
        currentNote++;
        EnableNoteByID(currentNote);
    }

    private void EnableNoteByID(int targetNoteID)
    {
        if (notes == null || notes.Length == 0)
        {
            Debug.LogWarning("No interactables assigned to the note manager");
                return;
        }
        foreach(InteractableObject note  in notes)
        {
            if (note ==  null)
            {
                Debug.LogWarning("A null interactable was found in the array");
                continue;
            }
            bool shouldBeActive = (note.noteID == targetNoteID);
            note.gameObject.SetActive(shouldBeActive); 
        }
    }
}
