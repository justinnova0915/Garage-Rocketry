using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SnapFixer : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;
    private Rigidbody rb;

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        socketInteractor = GetComponentInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>(); // Since it’s on a child
        rb = GetComponent<Rigidbody>();

        grabInteractable.selectExited.AddListener(OnReleased);
        socketInteractor.selectEntered.AddListener(OnSnappedTo);
        socketInteractor.selectExited.AddListener(OnUnsnapped);
    }

    void OnSnappedTo(SelectEnterEventArgs args)
    {
        // When this object snaps to another socket, disable its own socket
        socketInteractor.enabled = false;
        // Freeze its position to stop jittering or drifting
        rb.isKinematic = true;
    }

    void OnUnsnapped(SelectExitEventArgs args)
    {
        // When it’s removed from a socket, re-enable its own socket
        socketInteractor.enabled = true;
        // Let physics take over agai
        rb.isKinematic = false;
    }

    void OnReleased(SelectExitEventArgs args)
    {
        // If dropped and not snapped, ensure physics is active
        if (!socketInteractor.hasSelection)
        {
            rb.isKinematic = false;
        }
    }

    void OnDestroy()
    {
        grabInteractable.selectExited.RemoveListener(OnReleased);
        socketInteractor.selectEntered.RemoveListener(OnSnappedTo);
        socketInteractor.selectExited.RemoveListener(OnUnsnapped);
    }
}