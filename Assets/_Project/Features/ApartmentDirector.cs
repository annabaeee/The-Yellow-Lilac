using UnityEngine;
using System.Collections;
using System;

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
    [SerializeField] private Renderer paintingToWarp;
    private Material paintingMaterial;
    [SerializeField] private float fadeDuration;

    [Header("Audio")]
    [SerializeField] private AudioSource melodyAudioSource;
    [SerializeField] private AudioSource whisperAudioSource;

    [Header("Effects")]
    [SerializeField] private GameObject shadowEffect;
    [SerializeField] private GameObject vasePrefab;
    [SerializeField] private Transform vaseSpawnPoint;

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
        paintingMaterial = paintingToWarp.material;
        paintingMaterial.SetFloat("_FloatAmount", 1f);
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
        StartCoroutine(WarpPainting(paintingMaterial, fadeDuration));
    }

    private IEnumerator WarpPainting(Material paintingMaterial, float fadeDuration)
    {
        float time = 0;
        paintingMaterial.SetFloat("_FloatAmount", 1f);
        while (time < fadeDuration)
        {
            paintingMaterial.SetFloat("_FloatAmount", Mathf.Lerp(1f, 0f, time / fadeDuration));
            time += Time.deltaTime;
            yield return null;
        }
        paintingMaterial.SetFloat("_FloatAmount", 0f);
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
