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
		public string[] responses;
		public Rect[] responsesRect;

		public int[] connections;
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
			nodes[0].responses = new string[3];
			nodes[0].connections = new int[3];
			nodes[0].responsesRect = new Rect[3];
			nodes[0].prompt = "Hi! \n I don't \n have speech!";
			nodes[0].responses[0] = "Oh";
			nodes[0].responses[1] = "Ah";
			nodes[0].responses[2] = "Hm";
			nodes[0].connections[0] = 1;
			nodes[0].connections[1] = 1;
			nodes[0].connections[2] = 1;
			nodes[0].promptRect = new Rect(-37.5f, -80, 75, 75);
			nodes[0].responsesRect[0] = new Rect(-100, 0, 100, 50);
			nodes[0].responsesRect[1] = new Rect(0, 0, 100, 50);
			nodes[0].responsesRect[2] = new Rect(100, 0, 100, 50);

			nodes[1] = new DialogueNode();
			nodes[1].responses = new string[3];
			nodes[1].connections = new int[3];
			nodes[1].responsesRect = new Rect[3];
			nodes[1].prompt = "Fix me \n please?";
			nodes[1].responses[0] = "No.";
			nodes[1].responses[1] = "Repeat?";
			nodes[1].responses[2] = "Will do.";
			nodes[1].connections[0] = 1;
			nodes[1].connections[1] = 0;
			nodes[1].connections[2] = 2;
			nodes[1].promptRect = new Rect(-37.5f, -80, 75, 75);
			nodes[1].responsesRect[0] = new Rect(-100, 0, 100, 50);
			nodes[1].responsesRect[1] = new Rect(0, 0, 100, 50);
			nodes[1].responsesRect[2] = new Rect(100, 0, 100, 50);
		}
	}

	private void OnGUI()
	{
		worldToScreenPos = cam.WorldToScreenPoint(refObj.transform.position);

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
		Rect[] rRect = new Rect[node.responsesRect.Length];
		System.Array.Copy(node.responsesRect, rRect, node.responsesRect.Length);

		//convert the root point/objects position to screen coords
		pRect = new Rect(worldToScreenPos.x + pRect.x, Screen.height - worldToScreenPos.y + pRect.y, pRect.width, pRect.height);

		//Create a box for each button
		for (int i = 0; i < rRect.Length; i++)
		{
			rRect[i] = new Rect(worldToScreenPos.x + rRect[i].x - rRect[i].width/2, Screen.height - worldToScreenPos.y, rRect[i].width, rRect[i].height);
		}
		//rRect[0] = new Rect(worldToScreenPos.x + rRect[0].x, Screen.height - worldToScreenPos.y, 100, 50);
		//rRect[1] = new Rect(worldToScreenPos.x + rRect[1].x, Screen.height - worldToScreenPos.y, 100, 50);
		//rRect[2] = new Rect(worldToScreenPos.x + rRect[2].x, Screen.height - worldToScreenPos.y, 100, 50);

		//This just makes the main text box visible
		GUI.Label(pRect, node.prompt, testStyle);

		//WE STARTIN TO AUTOMATE BABY
		//check each available button to see if it was pressed
		for (int i = 0; i < rRect.Length; i++)
		{
			if (GUI.Button(rRect[i], node.responses[i]))
			{
				currNode = nodes[currNode].connections[i];
			}
		}
		//if (GUI.Button(rRect[0], node.responses[0]))
		//{
		//	currNode = nodes[currNode].connections[0];
		//}
		//else if (GUI.Button(rRect[1], node.responses[1]))
		//{
		//	currNode = nodes[currNode].connections[1];
		//}
		//else if (GUI.Button(rRect[2], node.responses[2]))
		//{
		//	currNode = nodes[currNode].connections[2];
		//}
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
