using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange; //Closest interactable object in range
    public GameObject interactionIcon; //Icon to show when an interactable object is in range
    
    private void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        print("Interact pressed");
        if (!context.performed) return;
        interactableInRange?.Interact();
        if (interactableInRange != null && !interactableInRange.CanInteract())
        {
            interactionIcon.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out IInteractable interactable) || !interactable.CanInteract()) return;
        interactableInRange = interactable;
        interactionIcon.SetActive(true);
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out IInteractable interactable) || interactable != interactableInRange) return;
        interactableInRange = null;
        interactionIcon.SetActive(false);
    }
}
