using UnityEngine;
using StarterAssets;

/// <summary>
/// Handles player interaction. Uses a SphereCast with a LayerMask for stable and precise object detection,
/// highlights them, and displays their information via the UIManager.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    // Component References
    private Camera _mainCamera;
    private StarterAssetsInputs _input;
    private FirstPersonController _firstPersonController;

    // Interaction Configuration
    [Header("Interaction Settings")]
    [SerializeField] private float _interactionDistance = 3f;
    [SerializeField] private float _sphereCastRadius = 0.1f;
    [SerializeField] private LayerMask _interactionLayers;

    // State Variables
    private bool _isShowingNote = false;
    private InteractableObject _currentInteractable = null;

    void Start()
    {
        _mainCamera = Camera.main;
        _input = GetComponent<StarterAssetsInputs>();
        _firstPersonController = GetComponent<FirstPersonController>();
    }

    void Update()
    {
        if (_isShowingNote)
        {
            // If we are currently showing a note, only check for input to close it.
            if (_input.interact)
            {
                CloseNote();
                _input.interact = false; // Consume input
            }
        }
        else
        {
            // If we are not showing a note, perform detection.
            HandleInteractionDetection();

            // And check for input to show a note.
            if (_currentInteractable != null && _input.interact)
            {
                ShowNote(_currentInteractable.noteText);
                _input.interact = false; // Consume input
            }
        }
    }

    /// <summary>
    /// Casts a sphere to detect interactable objects and updates their highlight state.
    /// </summary>
    private void HandleInteractionDetection()
    {
        InteractableObject detectedInteractable = null;

        // Perform the cast and only proceed if it hits something on the correct layer.
        if (Physics.SphereCast(_mainCamera.transform.position, _sphereCastRadius, _mainCamera.transform.forward, out RaycastHit hit, _interactionDistance, _interactionLayers))
        {
            // A hit occurred, now try to get the component from the collider.
            hit.collider.TryGetComponent<InteractableObject>(out detectedInteractable);
        }

        // If the detected object is different from the one we're currently highlighting, update the state.
        if (detectedInteractable != _currentInteractable)
        {
            // --- HIDE OLD PROMPT & OUTLINE ---
            // De-highlight the old object if it exists.
            if (_currentInteractable != null)
            {
                _currentInteractable.outlineObject?.SetActive(false);
                UIManager.Instance.HideLookPrompt();
            }

            // --- SHOW NEW PROMPT & OUTLINE ---
            // Highlight the new object and show its prompt if it exists.
            if (detectedInteractable != null)
            {
                detectedInteractable.outlineObject?.SetActive(true);
                UIManager.Instance.ShowLookPrompt(detectedInteractable.interactionPrompt);
            }

            // Update the current interactable.
            _currentInteractable = detectedInteractable;
        }
    }

    private void ShowNote(string text)
    {
        _isShowingNote = true;

        // Hide outline and look prompt while reading the note to prevent UI overlap.
        if (_currentInteractable != null)
        {
            _currentInteractable.outlineObject?.SetActive(false);
            UIManager.Instance.HideLookPrompt(); 
        }

        // This remains the same: it shows your main note canvas.
        UIManager.Instance.ShowInteractionPanel(text);
        SetPlayerControl(false);
    }

    private void CloseNote()
    {
        _isShowingNote = false;
        UIManager.Instance.HideInteractionPanel();
        SetPlayerControl(true);

        // After closing the note, re-enable the outline and prompt if we're still looking at the object.
        if (_currentInteractable != null)
        {
            _currentInteractable.outlineObject?.SetActive(true);
            UIManager.Instance.ShowLookPrompt(_currentInteractable.interactionPrompt);
        }
    }

    /// <summary>
    /// A helper method to enable or disable player movement and look controls.
    /// </summary>
    private void SetPlayerControl(bool hasControl)
    {
        Debug.LogError("Setting Control to " + hasControl);
        _firstPersonController.enabled = hasControl;
        _input.cursorLocked = hasControl;
        _input.cursorInputForLook = hasControl;
        Cursor.lockState = hasControl ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !hasControl;
    }
}