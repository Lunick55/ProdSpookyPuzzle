using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    [SerializeField] float gravity = 0.0f;
    [SerializeField] float jumpSpeed = 0.0f;
    float vSpeed = 0;

    CharacterController cc;

    private bool canHandleInput = true;
    private Vector3 inputDir = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("UP", HandleUp);
        EventManager.StartListening("DOWN", HandleDown);
        EventManager.StartListening("LEFT", HandleLeft);
        EventManager.StartListening("RIGHT", HandleRight);

        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (canHandleInput)
        {
            //sets the left and right movement
            inputDir = new Vector3(inputDir.x, 0.0f, inputDir.z);

            //apply speed
            inputDir = inputDir * speed;

            //check if grounded
            if (cc.isGrounded)
            {
                Debug.Log("I'm on the ground");
                vSpeed = 0;

                //TEMP::jump thing. 
                //TODO: move to input manager
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    vSpeed = jumpSpeed;
                }
            }


            //apply accelerating gravity
            inputDir.y = (vSpeed -= (gravity * Time.deltaTime)); 

            //move the object
            cc.Move(inputDir * Time.deltaTime); // this might need to get out of the if statement
            Debug.Log(inputDir.y);
        }

        inputDir = Vector3.zero;
    }

    //sets directions based on input handler
    void HandleLeft()
    {
        inputDir.x = -1;
    }
    void HandleRight()
    {
        inputDir.x = 1;
    }
    void HandleUp()
    {
        inputDir.z = 1;
    }
    void HandleDown()
    {
        inputDir.z = -1;
    }
}
