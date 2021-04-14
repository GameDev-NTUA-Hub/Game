using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Movement
    public float speed = 10.0f;
    public float gravity = 9.87f;
    public float jumpHeight = 10.0f;
    private CharacterController characterController;
    private Vector3 velocity;
    private float gravityVelocity = 0.0f;

    //Rotation

    public Transform cameraT;
    public float sensitivity = 5.0f;
    public float upLimit = -50.0f;
    public float downLimit = 50.0f;
    private float yRotation;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        yRotation = transform.localEulerAngles.y;
    }

    void Update()
    {
        Move();
        Rotate();
    }

    void Rotate() //TODO: LERP X CAMERA ROTATION
    {
        float horizontalRotation = 0;
        float verticalRotation = 0;

        if(Input.GetKey(KeyCode.Mouse0))
        {
            horizontalRotation = Input.GetAxis("Mouse X");
            verticalRotation = Input.GetAxis("Mouse Y");
        }

        float desiredYRotation = horizontalRotation * sensitivity;
        yRotation = Mathf.LerpAngle(yRotation, desiredYRotation, 0.01f);

        transform.Rotate(0, yRotation, 0);
        
        cameraT.Rotate(-verticalRotation * sensitivity, 0, 0);

        Vector3 currentRotation = cameraT.localEulerAngles;
        
        if(currentRotation.x > 180) currentRotation.x -= 360;
        currentRotation.x = Mathf.Clamp(currentRotation.x, upLimit, downLimit);
        cameraT.localRotation = Quaternion.Euler(currentRotation);
    }

    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 movement = transform.forward * vertical + transform.right * horizontal;
        movement = movement.normalized; //or movement.Normalize()

        Vector3 desiredVelocity = movement * speed;
        velocity = Vector3.Lerp(velocity, desiredVelocity, 0.05f);

    //JUMP+GRAVITY CODE
        if (characterController.isGrounded)
        {
            //Input.GetKey(KeyCode.Space) //Happens WHILE the key is pressed
            //Input.GetKeyDown(KeyCode.A) //Happens ONLY on the frame the key is pressed
            //Input.GetKeyUp(KeyCode.W)   //Happens ONLY on the frame the key is RELEASED
            if(Input.GetKey(KeyCode.Space))
            {
                gravityVelocity = jumpHeight;
            }
            else
            {
                gravityVelocity = 0;
            }
            
        }
        else
        {
            gravityVelocity -= (gravity * Time.deltaTime);
        }

        velocity.y = gravityVelocity;
    //END JUMP+GRAVITY CODE

        characterController.Move (velocity * Time.deltaTime);
    }
}
