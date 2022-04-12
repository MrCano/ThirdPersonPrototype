using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonPrototype
{
    public class BaseThirdPersonController : MonoBehaviour
    {
        private CharacterController charController;
        private InputManager userInput;
        private Camera cam;

        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float turnSmoothing = 0.2f;
        [SerializeField] private float jumpForce = 3f;
        [SerializeField] private float gravityModifier = 2f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.2f;
        [SerializeField] private LayerMask groundLayer;

        private float gravity = -9.81f;
        private float turnSmoothSpeed;

        //Used for UI debug
        [HideInInspector] public Vector3 direction;
        [HideInInspector] public Vector3 moveDir;
        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public bool isGrounded;

        // Awake is called before Start
        void Awake()
        {
            //Get components on awake
            charController = GetComponent<CharacterController>();
            userInput = GetComponent<InputManager>();
            cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            MovePlayer();
        }

        private void MovePlayer()
        {
            ///// Grounded Check /////

            //Check if the player is grounded
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

            //Reset velocity when grounded and hold player to ground
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -1f;
            }

            ///// Horizontal Movement /////

            //Set a Vector3 based on user's input (using Input.GetAxisRaw("") for each axis)
            direction = new Vector3(userInput.xInput, 0f, userInput.zInput).normalized;

            //If there is user input, get angles and move player
            if (direction.magnitude >= 0.1f)
            {
                //Create an angle based on the user's input and camera direction
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
                //Create a new angle from the player's current angle & targetAngle that smooths rotation
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothSpeed, turnSmoothing);
                //Rotate player using the above angle
                transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                //Create a Vector3 holding the move direction with camera angle taken into account
                moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

                //Move player
                charController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);

            }

            ///// Vertical Movement /////

            //Add vertical velocity when the jump button is pressed and player is grounded (using Input.GetButtonDown(""))
            if (userInput.JumpPressed() && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -3f * gravity);
            }

            //Add gravity to vertical velocity
            velocity.y += gravity * gravityModifier * Time.deltaTime;

            //Move the player using velocity
            charController.Move(velocity * Time.deltaTime);
        }

    }
}