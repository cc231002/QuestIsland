using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float movementPerSecond = 5f;
    [SerializeField]
    float rotationPerSecond = 20f;
    [SerializeField]
    GameObject camera;
    [SerializeField]
    InputActionReference rotateCamera;

    [SerializeField]
    Animator animator;
    Vector3 rotationVector = Vector3.zero;
    float inputDistance = 0;
    void Start()
    {
        rotateCamera.action.Enable();
        rotateCamera.action.performed += OnCameraRotation;
        rotateCamera.action.canceled += OnCameraRotationStop;
    }

    void OnCameraRotation(InputAction.CallbackContext callbackContext)
    {
        Vector2 inputVector = callbackContext.ReadValue<Vector2>();
        inputDistance = Vector3.Magnitude(inputVector);
        rotationVector = new Vector3(0, inputVector.x, 0);
    }
    
    void OnCameraRotationStop(InputAction.CallbackContext callbackContext)
    {
        rotationVector = Vector3.zero;
    }
    void Update()
    {
        Vector3 translation = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) {
            translation = transform.forward;
        }
        if (Input.GetKey(KeyCode.S)) {
            translation += -transform.forward;
        }
        if (Input.GetKey(KeyCode.D)) {
            translation += transform.right;
        }
        if (Input.GetKey(KeyCode.A)) {
            translation += -transform.right;
        }

        if (translation != Vector3.zero)
        {
            var previousRotation = camera.transform.rotation;
            var previousPostion = camera.transform.position;
            transform.forward = camera.transform.forward;
            camera.transform.rotation = previousRotation;
            camera.transform.position = previousPostion;
           // animator.SetBool("Walking", true);
        }
        else
        {
         //   animator.SetBool("Walking", false);
        }
        // Move the object
        transform.position += translation * movementPerSecond * Time.deltaTime;
        //transform.Rotate(rotationVector * rotationPerSecond * Time.deltaTime);
        camera.transform.RotateAround(transform.position, rotationVector,  inputDistance * rotationPerSecond * Time.deltaTime);

        //Debug.Log("My forward direction: " + transform.forward);
        //Debug.Log("My right direction: " + transform.right);
        //Debug.Log("My up direction: " + transform.up);


    }
}
 