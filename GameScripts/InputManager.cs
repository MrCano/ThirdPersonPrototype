using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonPrototype
{
    public class InputManager : MonoBehaviour
    {
        [HideInInspector]
        public float xInput;
        [HideInInspector]
        public float zInput;


        void Update()
        {
            xInput = Input.GetAxisRaw(ConstantManager.HORIZONTAL);
            zInput = Input.GetAxisRaw(ConstantManager.VERTICAL);
        }

        public bool JumpPressed()
        {
            if (Input.GetButtonDown(ConstantManager.JUMP))
            {
                return true;
            }
            else
                return false;
        }

    }
}