using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer3D
{
    public class PlayerController : MonoBehaviour
    {
        private CharacterController charController;
        private InputManager input;
        //public Transform cam; //TODO: add for camera

        [SerializeField]
        private float moveSpeed = 5f;
        [SerializeField]
        private float TurnSmoothTime = 0.1f;
        [SerializeField]
        private float JumpSpeed = 5f;
        [SerializeField]
        private float gravity = -9.81f;

        private Vector3 moveDirection;
        private float turnSmoothVelocity;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        private bool isGrounded;

        // Start is called before the first frame update
        void Start()
        {
            charController = GetComponent<CharacterController>();
            input = GetComponent<InputManager>();

            //TODO: add for camera
            //cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            MovePlayer();
        }

        #region Movement

        public void MovePlayer()
        {
            float yStore = moveDirection.y;
            moveDirection = new Vector3(input.xInput, 0f, input.zInput).normalized;
            moveDirection.y = yStore;

            //Calculate Jump
            if (isGrounded)
            {
                moveDirection.y = -1f;
                if (input.HasJumped())
                {
                    moveDirection.y = JumpSpeed;
                }
            }
            AddGravity();

            if(moveDirection.magnitude >= 0.1f)
            {
                //Rotate Player based on input
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;//"cam.eulerAngles.y" //TODO: add for camera
                float smoothedAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, TurnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);

                //TODO: add for camera and change "moveDirection" to "moveDir.normalized"
                //Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                
                //Move Player
                charController.Move(moveDirection * moveSpeed * Time.deltaTime);
            }
            
        }

        public void CalculateJump()
        {
            
        }

        #endregion

        #region Physics

        private void AddGravity()
        {
            moveDirection.y += gravity * Time.deltaTime;
        }

        #endregion

    }

}
