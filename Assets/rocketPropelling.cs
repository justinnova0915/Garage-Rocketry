using UnityEngine;
using UnityEngine.InputSystem;

public class RocketController : MonoBehaviour
{
    ConstantForce thrust;
    public Vector3 rocketDirection;
    Vector3 copy;
    public float xlrate;

    // Input action for thrust
    public InputAction thrustAction;

    void Awake()
    {
        // Initialize the input action
        thrustAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/space");
        thrustAction.Enable();
    }

    void Start()
    {
        thrust = gameObject.GetComponent<ConstantForce>();
        copy = rocketDirection;
    }

    void Update()
    {
        // Increase acceleration when it exceeds 20f
        if (xlrate >= 20f)
        {
            xlrate += .025f;
        }

        // Check if the thrust action is being performed
        if (thrustAction.ReadValue<float>() > 0)
        {
            thrust.force = rocketDirection * xlrate;
            rocketDirection = copy;
        }
        else
        {
            thrust.force = Vector3.zero;
        }
    }

    private void OnEnable()
    {
        // Enable the action when the object is enabled
        thrustAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the action when the object is disabled
        thrustAction.Disable();
    }
}
