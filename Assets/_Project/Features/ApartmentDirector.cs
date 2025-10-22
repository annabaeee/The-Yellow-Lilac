using UnityEngine;
using System.Collections;

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
    [SerializeField] private Light apartmentLight;
    [SerializeField] private Color newLightColor;
    [SerializeField] private float lightChangeDuration = 2.0f;

    [Header("Painting")]
    [SerializeField] private Renderer[] paintingsToWarp;

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
        yield return null;
    }

    private IEnumerator FadeLightColor(Light light, Color targetColor, float duration)
    {
        float time = 0;
        Color startColor = light.color;
        while (time < duration)
        {
            light.color = Color.Lerp(startColor, targetColor, time/duration);
            time += Time.deltaTime;
            yield return null;
        }
        light.color = targetColor;
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
