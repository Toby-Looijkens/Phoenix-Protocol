using System.Collections.Generic;
using System.Threading;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] TMP_Text interactionPrompt;

    [Header("Camera")]
    [SerializeField] private Vector2 acceleration = new Vector2(2000, 2000);
    [SerializeField] Vector2 sensitivity = new Vector2(20, -20);
    [SerializeField] private float verticalClamp = 80;

    [Header("Components")]
    [SerializeField] GameObject rotationComponent;
    [SerializeField] Camera camera;


    private float speed;

    private Vector3 movementVector = Vector3.zero;
    private Rigidbody rigidBody;

    private Vector2 velocity;
    private Vector2 currentRotation;
    private Vector2 rotationVector;

    private List<GameObject> targets = new List<GameObject>();

    private IInteractable interactable;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        speed = walkingSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MovePlayer();
        MoveCamera();
    }

    private void MovePlayer()
    {
        if (movementVector != Vector3.zero)
        {
            //Get camera normals
            Vector3 forward = camera.transform.forward;
            Vector3 right = camera.transform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            //Movement based on where player is looking
            Vector3 forwardRelative = forward * movementVector.y;
            Vector3 rightRelative = right * movementVector.x;

            Vector3 relativeMovement = forwardRelative + rightRelative;

            transform.position += relativeMovement * speed * Time.deltaTime;
        } 
    }

    private void MoveCamera()
    {
        Vector2 scaledRotationVector = rotationVector * sensitivity;

        currentRotation += scaledRotationVector * Time.deltaTime;
        currentRotation.y = Mathf.Clamp(currentRotation.y, -verticalClamp, verticalClamp);
        camera.transform.localEulerAngles = new Vector3(currentRotation.y, currentRotation.x, 0);
    }

    private void OnMove(InputValue value)
    {
        movementVector = value.Get<Vector2>();
    }

    private void OnSprint(InputValue value)
    {
        if (value.Get<float>() > 0)
        {
            speed = sprintSpeed;
        } else
        {
            speed = walkingSpeed;
        }
    }

    private void OnLook(InputValue value)
    {
        rotationVector = value.Get<Vector2>();
    }

    private void OnInteract(InputValue value)
    {
        if (interactable != null)
        {
            interactable.Interact(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            interactable = collision.GetComponent<IInteractable>();
            interactionPrompt.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            interactable = null;
            interactionPrompt.enabled = false;
        }
    }
}
