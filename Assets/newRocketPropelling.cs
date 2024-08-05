using UnityEngine;
using UnityEngine.InputSystem;

public class newRocketPropelling : MonoBehaviour
{
    [Header("Thrust Settings")]
    public float weight = 1f;            // Weight of the rocket
    public float desiredTWR = 1.5f;      // Desired thrust-to-weight ratio

    private float maxThrust;             // Maximum thrust power, calculated based on TWR
    public float thrustIncreaseRate = 10f; // Rate at which thrust increases per second
    private float currentThrust = 0f;    // Current thrust level

    [Header("Rotation Settings")]
    public float maxRotationTorque = 5f;   // Maximum torque for rotation
    public float rotationAcceleration = 2f; // How quickly the rotation speed ramps up
    public float aerodynamicDrag = 0.5f;    // Custom aerodynamic drag coefficient

    private Rigidbody rb;
    private InputAction increaseThrustAction;
    private InputAction decreaseThrustAction;
    private InputAction rotateLeftAction;
    private InputAction rotateRightAction;

    private float currentRotationSpeed = 0f; // Current speed of rotation

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Calculate max thrust based on desired TWR
        maxThrust = desiredTWR * rb.mass * Physics.gravity.magnitude;

        // Set higher angular drag to simulate aerodynamic resistance
        rb.angularDrag = aerodynamicDrag;
        rb.mass = weight; // Set the mass of the rocket according to its weight

        // Initialize thrust control actions
        increaseThrustAction = new InputAction("IncreaseThrust", InputActionType.Button, "<Keyboard>/w");
        decreaseThrustAction = new InputAction("DecreaseThrust", InputActionType.Button, "<Keyboard>/s");

        // Initialize rotation actions
        rotateLeftAction = new InputAction("RotateLeft", InputActionType.Button, "<Keyboard>/a");
        rotateRightAction = new InputAction("RotateRight", InputActionType.Button, "<Keyboard>/d");

        // Enable all actions
        increaseThrustAction.Enable();
        decreaseThrustAction.Enable();
        rotateLeftAction.Enable();
        rotateRightAction.Enable();
    }

    private void Update()
    {
        if (increaseThrustAction.IsPressed())
        {
            AdjustThrust(thrustIncreaseRate * Time.deltaTime);
        }
        if (decreaseThrustAction.IsPressed())
        {
            AdjustThrust(-thrustIncreaseRate * Time.deltaTime);
        }

        float rotationInput = 0f;
        if (rotateLeftAction.IsPressed())
        {
            rotationInput = -1f;
        }
        if (rotateRightAction.IsPressed())
        {
            rotationInput = 1f;
        }
        
        AdjustRotation(rotationInput);
    }

    private void FixedUpdate()
    {
        // Calculate thrust-to-weight ratio
        float twr = currentThrust / (rb.mass * Physics.gravity.magnitude);

        // Apply current thrust as a force in the direction the rocket is facing
        rb.AddForce(transform.forward * currentThrust);

        // Debug log for thrust-to-weight ratio and acceleration
        Debug.Log($"Thrust-to-Weight Ratio: {twr}, Current Thrust: {currentThrust}");

        // Apply rotation with damping to simulate aerodynamic resistance
        ApplyDampedRotation();
    }

    private void AdjustThrust(float delta)
    {
        currentThrust += delta;
        currentThrust = Mathf.Clamp(currentThrust, 0, maxThrust);
    }

    private void AdjustRotation(float input)
    {
        // Gradually increase or decrease the rotational speed based on thrust-to-weight ratio
        float rotationFactor = Mathf.Lerp(0.5f, 1.5f, currentThrust / maxThrust);
        currentRotationSpeed = Mathf.MoveTowards(currentRotationSpeed, input * maxRotationTorque * rotationFactor, rotationAcceleration * Time.deltaTime);
    }

    private void ApplyDampedRotation()
    {
        // Apply the calculated torque with current rotation speed
        rb.AddTorque(Vector3.forward * currentRotationSpeed);
    }

    private void OnDisable()
    {
        // Disable all actions when the object is disabled
        increaseThrustAction.Disable();
        decreaseThrustAction.Disable();
        rotateLeftAction.Disable();
        rotateRightAction.Disable();
    }
}
