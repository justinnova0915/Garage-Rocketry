using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class XRChangeLayerOnGrab : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private InteractionLayerMask originalInteractionLayer;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        originalInteractionLayer = grabInteractable.interactionLayers;

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        Debug.Log(gameObject.name + " initialized with Interaction Layer: " + originalInteractionLayer);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log(gameObject.name + " Grabbed - Changing to HeldObjects");

        // Set interaction layer to HeldObjects
        grabInteractable.interactionLayers = InteractionLayerMask.GetMask("HeldObjects");

        Debug.Log(gameObject.name + " Current Interaction Layer: " + grabInteractable.interactionLayers);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log(gameObject.name + " Released - Reverting to " + originalInteractionLayer);

        // Restore the original interaction layer (Small/Medium/Large)
        grabInteractable.interactionLayers = originalInteractionLayer;

        Debug.Log(gameObject.name + " Current Interaction Layer after release: " + grabInteractable.interactionLayers);
    }
}
