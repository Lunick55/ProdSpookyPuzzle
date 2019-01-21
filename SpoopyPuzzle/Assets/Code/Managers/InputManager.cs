using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	bool canHandleInput = true;

    //private static InputManager inputManager;

    //public static InputManager instance
    //{
    //    get
    //    {
    //        if (!inputManager)
    //        {
    //            inputManager = FindObjectOfType(typeof(InputManager)) as InputManager;

    //            if (!inputManager)
    //            {
    //                Debug.LogError("There needs to be one active InputManager script on a GameObject in your scene.");
    //            }
    //            else
    //            {
    //                inputManager.Init();
    //            }
    //        }

    //        return inputManager;
    //    }
    //}

    void Init()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (canHandleInput)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				EventManager.FireEvent("SPACE");
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
			{
				EventManager.FireEvent("UP");
			}
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
			{
				EventManager.FireEvent("LEFT");
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
			{
				EventManager.FireEvent("DOWN");
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
			{
				EventManager.FireEvent("RIGHT");
			}

			if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
			{
				EventManager.FireEvent("UP_up");
			}
			if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
			{
				EventManager.FireEvent("LEFT_up");
			}
			if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
			{
				EventManager.FireEvent("DOWN_up");
			}
			if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
			{
				EventManager.FireEvent("RIGHT_up");
			}

			if (Input.GetKeyDown(KeyCode.E))
			{
				EventManager.FireEvent("E_down");
			}
		}
    }
}
