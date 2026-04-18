using System.Collections.Generic;
using UnityEngine;

public class PlayerTestController : MonoBehaviour
{
    Vector3 Movement;
    Vector2 LookingAround;
    Rigidbody rb;
    Transform ori;
    Camera cam;

    bool isGrounded;


    float MouseSpeed = 120f;

    [SerializeField] float speed = 10f;
    [SerializeField] float sprintSpeed = 20f;
    [SerializeField] float vertRotation = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        

        rb = GetComponent<Rigidbody>();
        cam = GameObject.FindWithTag("GameController").GetComponent<Camera>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        ori = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //MouseSpeed = 100f;
        Application.targetFrameRate = 80;
    }

    // Update is called once per frame

    
    void Update()
    {
           InputKey();       
    }

    void FixedUpdate()
    {
           LookAround();
           MovementH();
    }

    void InputKey()
    {
        Movement = Vector3.zero;
        

        var keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.aKey.isPressed) Movement.x = -1;
            if (keyboard.dKey.isPressed) Movement.x = 1;
            if (keyboard.wKey.isPressed) Movement.z = 1;
            if (keyboard.sKey.isPressed) Movement.z = -1;

            if (keyboard.spaceKey.wasPressedThisFrame && isGrounded)
            {
                isGrounded = false;
                rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            }
        }


        Movement = Movement.normalized;



      var mouse = UnityEngine.InputSystem.Mouse.current;
        if (mouse != null)
        {
            // delta is the change in mouse position since last frame
            LookingAround = mouse.delta.ReadValue();
        }
    }

    void MovementH()
    {

        // Debug.Log("moving");
        if (rb != null)
        {

            Vector3 movingDir = (ori.forward * Movement.z + ori.right * Movement.x).normalized;
            float smoothMovement = 10f;
            Vector3 curMovement = Vector3.zero;
            curMovement = Vector3.Lerp(curMovement, movingDir, smoothMovement);
            rb.MovePosition(rb.position + curMovement * speed * Time.fixedDeltaTime);
           
        }


    }

    void LookAround()
    {
        //  Debug.Log("rotating");
        vertRotation -= LookingAround.y * Time.fixedDeltaTime * MouseSpeed;
        vertRotation = Mathf.Clamp(vertRotation, -90f, 90f);


        transform.Rotate(0f, LookingAround.x * Time.fixedDeltaTime * MouseSpeed, 0f);
        //  Debug.Log("Horizontal Y Rotation: " + transform.eulerAngles.y);
        

        cam.transform.localRotation = Quaternion.Euler(vertRotation, 0f, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("isGrounded"))
        {
            isGrounded = true;

        }

        
    }
}
