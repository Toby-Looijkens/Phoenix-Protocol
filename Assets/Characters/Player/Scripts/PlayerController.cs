using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float rotationSpeed = 1000f;
    [SerializeField] private float pushStrength = 40f;
    [SerializeField] private FieldOfViewScript fov;
    //[SerializeField] private float deceleration = 5f;
    [SerializeField] TMP_Text interactionPrompt;

    [Header("Components")]
    [SerializeField] GameObject rotationComponent;
    [SerializeField] Camera camera;


    private float speed;

    private Vector2 movementVector = Vector2.zero;
    private Rigidbody2D rigidBody;

    private Quaternion rotationGoal;

    private List<GameObject> targets = new List<GameObject>();

    private IInteractable interactable;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        speed = walkingSpeed;
    }

    void Update()
    {
        camera.transform.position = transform.position + new Vector3(0, 0, -10);

        MovePlayer();
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        fov.SetAimDirection((new Vector3(mousePosition.x, mousePosition.y, 0) - transform.position));
        fov.SetOrigin(transform.position);
        RotateTowardsMouse();
    }

    private void MovePlayer()
    {
        if (movementVector != Vector2.zero)
        {
            rigidBody.linearVelocity = movementVector * speed;
        } 
        else
        {
            SlowDownPlayer(15f);
        }
    }

    private void RotateTowardsMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        rotationGoal = Quaternion.LookRotation(rotationComponent.transform.forward, (new Vector3(mousePosition.x, mousePosition.y, 0) - rotationComponent.transform.position));
        rotationGoal = Quaternion.Euler(0, 0, rotationGoal.eulerAngles.z + 90);
        rotationComponent.transform.rotation = Quaternion.RotateTowards(rotationComponent.transform.rotation, rotationGoal, rotationSpeed * Time.deltaTime);
    }

    private void SlowDownPlayer(float deceleration)
    {
        Vector3 speedVector = rigidBody.linearVelocity;
        Vector3 invertedSpeedVector = speedVector * -1 * deceleration * Time.deltaTime;

        if (Mathf.Abs(speedVector.x) >= 0 && Mathf.Abs(speedVector.x) <= Mathf.Abs(invertedSpeedVector.x))
        {
            speedVector.x = 0;
        }
        else
        {
            speedVector.x += invertedSpeedVector.x;
        }

        if (Mathf.Abs(speedVector.y) >= 0 && Mathf.Abs(speedVector.y) <= Mathf.Abs(invertedSpeedVector.y))
        {
            speedVector.y = 0;
        }
        else
        {
            speedVector.y += invertedSpeedVector.y;
        }

        rigidBody.linearVelocity = speedVector;
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
