using UnityEngine;
using UnityEngine.InputSystem;

public class newRocketPropelling : MonoBehaviour
{
    [Header("Thrust Settings")]
    public float thrustPower = 10f;      // Base thrust power, adjust as necessary
    public float rotationTorque = 5f;    // Torque applied for rotation

    [Header("Input Actions")]
    public InputAction thrustAction;
    public InputAction rotateAction;

    private Rigidbody rb;
    private ConstantForce cf;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cf = GetComponent<ConstantForce>();
        InitializeInput();
    }

    private void InitializeInput()
    {
        thrustAction.Enable();
        rotateAction.Enable();

        thrustAction.performed += context => UpdateThrust(context.ReadValue<float>());
        thrustAction.canceled += context => UpdateThrust(0);

        rotateAction.performed += context => UpdateRotation(context.ReadValue<float>());
        rotateAction.canceled += context => UpdateRotation(0);
    }

    private void UpdateThrust(float thrustInput)
    {
        if (cf != null) // Ensure ConstantForce component is not null
        {
            cf.force = transform.forward * thrustInput * thrustPower;
        }
        else
        {
            Debug.LogError("ConstantForce component is missing or not initialized.");
        }
    }

    private void UpdateRotation(float rotationInput)
    {
        if (rb != null) // Ensure Rigidbody component is not null
        {
            rb.AddTorque(transform.up * rotationInput * rotationTorque);
        }
        else
        {
            Debug.LogError("Rigidbody component is missing or not initialized.");
        }
    }

    private void OnDisable()
    {
        if (thrustAction != null)
        {
            thrustAction.Disable();
        }
        if (rotateAction != null)
        {
            rotateAction.Disable();
        }
    }
}
