using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 1.0f;
    [SerializeField] float gravity = 0.0f;
    [SerializeField] float jumpSpeed = 0.0f;
	private Transform myTrans;
    float vSpeed = 0;

    CharacterController cc;

    private bool canMovePlayer = true;
	private bool canJump = false;
	private bool canInteract = true;

    private Vector3 inputDir = Vector3.zero;

	private GameObject pickup;
	private bool isHoldingPickup = false;

	private GameObject personTalkingTo;

	enum Interaction
	{
		NONE,
		PICKUP,
		TALK
	};

	Interaction currInteract;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("UP", HandleUp);
        EventManager.StartListening("DOWN", HandleDown);
        EventManager.StartListening("LEFT", HandleLeft);
        EventManager.StartListening("RIGHT", HandleRight);
		EventManager.StartListening("E_down", HandleInteraction);
		EventManager.StartListening("SPACE", HandleSpace);
		EventManager.StartListening("MENU_open", DisableInteraction);
		EventManager.StartListening("MENU_close", EnableInteraction);

		cc = GetComponent<CharacterController>();
		myTrans = this.transform;

		currInteract = Interaction.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (canMovePlayer)
        {
            //sets the left and right movement
            inputDir = new Vector3(inputDir.x, 0.0f, inputDir.z);

            //apply speed
            inputDir = inputDir * speed;

            //check if grounded
            if (cc.isGrounded)
            {
                vSpeed = 0;

                //jump thing. 
                if (canJump)
                {
                    vSpeed = jumpSpeed;
                }
            }
			else
			{
				canJump = false;
			}

            //apply accelerating gravity
            inputDir.y = (vSpeed -= (gravity * Time.deltaTime)); 

            //move the object
            cc.Move(inputDir * Time.deltaTime); // this might need to get out of the if statement
        }

        inputDir = Vector3.zero;
    }


	//Interaction Handler. Depending on current Interaction type execute specific functions
	void HandleInteraction()
	{
		if (canInteract)
		{ 
			if (currInteract == Interaction.NONE)
			{
				Debug.Log("No Interaction");
				return;
			}
			else if (currInteract == Interaction.PICKUP)
			{
				Debug.Log("Pickup Interaction");
				PickupObject();
			}
			else if (currInteract == Interaction.TALK)
			{
				Debug.Log("Talk Interaction");
				TalkToSomeone();
			}
		}
	}

	//Pickups up the object currently in "pickup" if there is one
	void PickupObject()
	{
		if (pickup == null)
		{
			Debug.LogWarning("You are somehow trying to pickup nothing.");
			return;
		}
		if (isHoldingPickup)
		{
			//Debug.Log("Drop: " + pickup.name);
			//We are holding something, so drop it where we are.
			pickup.transform.parent = null;
			pickup.transform.position = new Vector3(myTrans.position.x, myTrans.position.y, myTrans.position.z);
			pickup.GetComponent<Collider>().enabled = true;
			isHoldingPickup = false;
			return;
		}

		Transform pickupTrans = pickup.transform;

		//Debug.Log("Pick up: " + pickup.name);
		//TODO: define pickupTrans on pickup, make it a member variable
		//We are able to pick something up, so snag it
		pickupTrans.parent = this.transform;
		pickupTrans.position = new Vector3(myTrans.position.x, myTrans.position.y + 0.5f, myTrans.position.z);
		pickup.GetComponent<Collider>().enabled = false;
		isHoldingPickup = true;
	}

	void TalkToSomeone()
	{
		if (personTalkingTo == null)
		{
			Debug.LogWarning("You are somehow trying to talk to nothing.");
			return;
		}

		EventManager.FireEvent(personTalkingTo.name + "_enable");
		//have a conversation somehow.
		//keyboard should be disabled, but mouse is fine
		//click on interactable bubbles
		//
	}

	private void SetInteraction(Interaction newInteract)
	{
		Debug.Log("Interaction is: " + newInteract);
		currInteract = newInteract;
	}

    private void OnTriggerEnter(Collider col)
    {
		if (col.tag == "friend")
		{
			Debug.Log("Friend Located!");
			personTalkingTo = col.gameObject;
			SetInteraction(Interaction.TALK);
		}

    }

	private void OnTriggerStay(Collider col)
	{
		if (col.tag == "pickup" && !isHoldingPickup)
		{
			//Debug.Log("I can pick it up");
			SetInteraction(Interaction.PICKUP);
			pickup = col.gameObject;
		}
	}

	private void OnTriggerExit(Collider col)
	{
		if (isHoldingPickup)
		{
			//TODO: fix bug...something is causing this to fire when jumping
			Debug.Log("Exit while holding: " + col.name);
			SetInteraction(Interaction.PICKUP);
		}
		else if (col.tag == "friend")
		{
			Debug.Log("Bye friend!");

			//remove this at some point, since moving while talking SHOULD be disabled
			//personTalkingTo.GetComponent<DialogueController>().enabled = false;
			personTalkingTo = null;
			SetInteraction(Interaction.NONE);
		}
		else if (col.tag == "pickup")
		{
			SetInteraction(Interaction.NONE);
			pickup = null;
		}
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
	void HandleSpace()
	{
		if (cc.isGrounded)
		{
			canJump = true;
		}
	}

	void DisableInteraction()
	{
		canInteract = false;
		canMovePlayer = false;
	}

	void EnableInteraction()
	{
		canInteract = true;
		canMovePlayer = true;
	}
}
