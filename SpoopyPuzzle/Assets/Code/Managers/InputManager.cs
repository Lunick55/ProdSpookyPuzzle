using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

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
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            EventManager.FireEvent("UP");
            Debug.Log("UP");
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            EventManager.FireEvent("LEFT");
            Debug.Log("LEFT");
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            EventManager.FireEvent("DOWN");
            Debug.Log("DOWN");
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            EventManager.FireEvent("RIGHT");
            Debug.Log("RIGHT");
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            EventManager.FireEvent("UPup");
            Debug.Log("UPup");
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            EventManager.FireEvent("LEFTup");
            Debug.Log("LEFTup");
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            EventManager.FireEvent("DOWNup");
            Debug.Log("DOWNup");
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            EventManager.FireEvent("RIGHTup");
            Debug.Log("RIGHTup");
        }
    }
}
