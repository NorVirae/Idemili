using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private float velocity = 0.0f;
    private float lastButtonPressedTime;
    private float lastGroundTime;
    public float jumpGracePeriod = 3f;
    public float jumpHeight = 10f;
    public float maximumSpeed = 6f;

    public Transform cameraTransform;

    public float rotationSpeed = 700f;

    float ySpeed = -1.5f;

    private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }
    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        var inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        movementDirection.Normalize();

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2;

        }
        Debug.Log(inputMagnitude + "ORA");

        if (characterController.isGrounded)
        {
            lastGroundTime = Time.time;

        }


        if (Input.GetButtonDown("Jump"))
        {
            lastButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundTime <= jumpGracePeriod)
        {
            ySpeed = -1.5f;
            if (Time.time - lastButtonPressedTime <= jumpGracePeriod)
            {
                ySpeed = jumpHeight;
            }
            else
            {

            }

        }


        ySpeed += Physics.gravity.y * Time.deltaTime;
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;




        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 speed = movementDirection * maximumSpeed;
        speed.y = ySpeed * Time.deltaTime;
        characterController.Move(speed);

    }
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

 

}
