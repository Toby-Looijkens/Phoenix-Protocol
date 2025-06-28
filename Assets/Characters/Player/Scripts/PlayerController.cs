using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkingSpeed = 3f;
    [SerializeField] private float sprintSpeed = 5f;
    [SerializeField] private float rotationSpeed = 1000f;
    [SerializeField] private float pushStrength = 40f;
    //[SerializeField] private float deceleration = 5f;

    [Header("Components")]
    [SerializeField] GameObject interactComponent;

    private float speed;

    private Vector2 movementVector = Vector2.zero;
    private Rigidbody2D rigidBody;

    private Quaternion rotationGoal;

    private List<GameObject> targets = new List<GameObject>();

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        speed = walkingSpeed;
    }

    void Update()
    {
        MovePlayer();
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
        rotationGoal = Quaternion.LookRotation(interactComponent.transform.forward, (new Vector3(mousePosition.x, mousePosition.y, 0) - interactComponent.transform.position));
        rotationGoal = Quaternion.Euler(0, 0, rotationGoal.eulerAngles.z + 90);
        interactComponent.transform.rotation = Quaternion.RotateTowards(interactComponent.transform.rotation, rotationGoal, rotationSpeed * Time.deltaTime);
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
}
