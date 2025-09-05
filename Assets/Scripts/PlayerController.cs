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
    [SerializeField] float horizontalSensitivity = 20;
    [SerializeField] float verticalSensitivity = -20;
    [SerializeField] private float verticalClamp = 80;

    [Header("Components")]
    [SerializeField] GameObject rotationComponent;
    [SerializeField] Camera camera;
    [SerializeField] CharacterController characterController;


    private float speed;

    private Vector3 movementVector = Vector3.zero;

    private Vector2 velocity;
    private Vector2 mouseInput;
    private float pitch;

    private List<GameObject> targets = new List<GameObject>();

    private IInteractable interactable;

    void Start()
    {
        speed = walkingSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        MovePlayer();
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

            characterController.SimpleMove(relativeMovement * speed);
        }
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
        mouseInput = value.Get<Vector2>();
    }

    private void OnInteract(InputValue value)
    {
        if (interactable != null)
        {
            interactable.Interact(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            interactable = other.GetComponent<IInteractable>();
            interactionPrompt.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            interactable = other.GetComponent<IInteractable>();
            interactionPrompt.enabled = false;
        }
    }
}
