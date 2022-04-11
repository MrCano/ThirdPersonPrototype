using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonPrototype
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; //Lock the cursor to the game window so player cant click off game.
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

