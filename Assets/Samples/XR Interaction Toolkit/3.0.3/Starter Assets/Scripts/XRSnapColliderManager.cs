using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSnapColliderManager : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private Collider activeSocketCollider; // ✅ Store the specific socket collider that is being used

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        socketInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }

        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.AddListener(OnSnappedIntoSocket);
            socketInteractor.selectExited.AddListener(OnRemovedFromSocket);
        }
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        if (activeSocketCollider != null)
        {
            activeSocketCollider.enabled = true; // ✅ Re-enable only the socket that was used
            Debug.Log(gameObject.name + " grabbed - socket collider re-enabled.");
        }
    }

    void OnReleased(SelectExitEventArgs args)
    {
        if (activeSocketCollider != null)
        {
            activeSocketCollider.enabled = true; // ✅ Ensure it stays enabled after release
            Debug.Log(gameObject.name + " released - socket collider enabled.");
        }
    }

    void OnSnappedIntoSocket(SelectEnterEventArgs args)
    {
        // ✅ Find which specific collider was used for snapping
        Collider socketCollider = args.interactableObject.transform.GetComponent<Collider>();

        if (socketCollider != null)
        {
            activeSocketCollider = socketCollider; // Store the specific socket that was used
            activeSocketCollider.enabled = false; // ❌ Disable ONLY that socket
            Debug.Log(gameObject.name + " snapped - socket collider disabled.");
        }
    }

    void OnRemovedFromSocket(SelectExitEventArgs args)
    {
        if (activeSocketCollider != null)
        {
            activeSocketCollider.enabled = true; // ✅ Re-enable ONLY the socket that was used
            Debug.Log(gameObject.name + " removed from socket - socket collider enabled.");
            activeSocketCollider = null; // Reset
        }
    }
}
