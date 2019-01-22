using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
TODO:
-add a tempCurrNode so that dialogue can loop from different points	 

-add more variables for the nodes class so there is more control
*/

public class DialogueController : MonoBehaviour
{
	private GameObject refObj;
	[SerializeField] private Camera cam;
	string ID;
	[SerializeField] DialogueNode[] nodes;
	[SerializeField] int currNode = 0;

	Vector3 worldToScreenPos;

	//messing around with gui styles
	GUIStyle testStyle;

	[System.Serializable]
	class DialogueNode
	{
		//Box for the prompt and the text to go inside
		public string prompt;
		public Rect promptRect;

		//Boxs for responses and the text inside them
		//public string[] responses;
		//public Rect[] responsesRect;
		//public string[] eventName;

		public Response[] responses;

		//what the next dialogue node is. -1 to terminate discussion
		//should be one for each button, or only one if no buttons
		public int[] connections;
	};

	[System.Serializable]
	class Response
	{
		public string response;
		public Rect responseRect;

		public string eventCall = " ";
	};

	private void Awake()
	{
		ID = this.name;
		refObj = this.gameObject;
		EventManager.StartListening(ID + "_enable", EnableCurrNode);
	}

	private void OnEnable()
	{
		EventManager.StartListening("E_down", ContinueDialogue);
		EventManager.FireEvent("MENU_open");
	}
	private void OnDisable()
	{
		EventManager.StopListening("E_down", ContinueDialogue);
		EventManager.FireEvent("MENU_close");
	}

	private void Start()
	{
		testStyle = new GUIStyle("box");
		testStyle.normal.textColor = Color.blue;
		testStyle.wordWrap = true;

		if (nodes.Length == 0)
		{
			nodes = new DialogueNode[2];

			nodes[0] = new DialogueNode();
			nodes[0].responses = new Response[3];//responses = new string[3];
			for (int i = 0; i < nodes[0].responses.Length; i++)
			{
				nodes[0].responses[i] = new Response();
			}
			nodes[0].connections = new int[3];
			//nodes[0].responsesRect = new Rect[3];
			nodes[0].prompt = "Hi! \n I don't \n have speech!";
			nodes[0].responses[0].response = "Oh";
			nodes[0].responses[1].response = "Ah";
			nodes[0].responses[2].response = "Hm";
			nodes[0].connections[0] = 1;
			nodes[0].connections[1] = 1;
			nodes[0].connections[2] = 1;
			nodes[0].promptRect = new Rect(-37.5f, -80, 75, 75);
			nodes[0].responses[0].responseRect = new Rect(-100, 0, 100, 50);
			nodes[0].responses[1].responseRect = new Rect(0, 0, 100, 50);
			nodes[0].responses[2].responseRect = new Rect(100, 0, 100, 50);

			nodes[1] = new DialogueNode();
			nodes[1].responses = new Response[3];//nodes[1].responses = new string[3];
			for (int i = 0; i < nodes[0].responses.Length; i++)
			{
				nodes[1].responses[i] = new Response();
			}
			nodes[1].connections = new int[3];
			//nodes[1].responsesRect = new Rect[3];
			nodes[1].prompt = "Fix me \n please?";
			nodes[1].responses[0].response = "No.";
			nodes[1].responses[1].response = "Repeat?";
			nodes[1].responses[2].response = "Will do.";
			nodes[1].connections[0] = 1;
			nodes[1].connections[1] = 0;
			nodes[1].connections[2] = 2;
			nodes[1].promptRect = new Rect(-37.5f, -80, 75, 75);
			nodes[1].responses[0].responseRect = new Rect(-100, 0, 100, 50);
			nodes[1].responses[1].responseRect = new Rect(0, 0, 100, 50);
			nodes[1].responses[2].responseRect = new Rect(100, 0, 100, 50);
		}
	}

	private void OnGUI()
	{
		//converts the reference objects position to screen coords
		worldToScreenPos = cam.WorldToScreenPoint(refObj.transform.position);

		//-1 should be a signal to end the current discussion
		if (currNode < nodes.Length && currNode != -1)
		{
			DisplayNode(nodes[currNode]);
		}
		else
		{
			Debug.Log(ID + " is done talking");
			this.enabled = false;
		}
	}

	void DisplayNode(DialogueNode node)
	{
		//create temporary boxes for offset purposes
		Rect pRect = node.promptRect;
		pRect = new Rect(worldToScreenPos.x + pRect.x, Screen.height - worldToScreenPos.y + pRect.y, pRect.width, pRect.height);

		Response[] rRect = new Response[node.responses.Length];
		//System.Array.Copy(node.responses, rRect, node.responses.Length);
		//rRect = (Response[])node.responses.Clone();

		//don't like it, but loads rRect array from nodes.responses
		//for each new element in nodes.responses, a new line must be added here to match
		for (int i = 0; i < rRect.Length; i++)
		{
			rRect[i] = new Response();
			rRect[i].response = node.responses[i].response;
			rRect[i].responseRect = node.responses[i].responseRect;
		}

		//Create a box for each button
		for (int i = 0; i < rRect.Length; i++)
		{
			rRect[i].responseRect = new Rect(worldToScreenPos.x + rRect[i].responseRect.x - rRect[i].responseRect.width/2, Screen.height - worldToScreenPos.y, rRect[i].responseRect.width, rRect[i].responseRect.height);
		}

		//This just makes the main text box visible
		GUI.Label(pRect, node.prompt, testStyle);

		//WE STARTIN TO AUTOMATE BABY
		//check each available button to see if it was pressed
		for (int i = 0; i < rRect.Length; i++)
		{
			if (GUI.Button(rRect[i].responseRect, node.responses[i].response))
			{
				currNode = nodes[currNode].connections[i];

				EventManager.FireEvent(node.responses[i].eventCall);
			}
		}

	}

	void ContinueDialogue()
	{
		if (nodes[currNode].responses.Length == 0)
		{
			currNode = nodes[currNode].connections[0];
		}
	}

	void EnableCurrNode()
	{
		currNode = 0;
		this.enabled = true;
	}
}
