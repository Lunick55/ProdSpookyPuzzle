using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Possible thoughts on dialogue system:
-normal text is a button, press to advance until responses
	-format is entirly response, with normal text being psuedo buttons
-or press 'e' to continue conversation until button prompt requires mouse
	 
	 
*/

public class DialogueController : MonoBehaviour
{
	[SerializeField] GameObject wPoint;
	[SerializeField] Camera cam;
	[SerializeField] GameObject pPrompt;
	//[SerializeField] Button[] buttonOptions;

	//messing around with gui styles
	GUIStyle testStyle;

	class DialogueNode
	{
		public string prompt;
		public string[] responses;
		public int[] connections;
	};

	DialogueNode[] nodes;
	int currNode = 0;

	private void Start()
	{
		//testStyle.normal.textColor = Color.blue;
		//testStyle.normal.background = GUIStyle("box");

		nodes = new DialogueNode[3];

		nodes[0] = new DialogueNode();
		nodes[0].responses = new string[3];
		nodes[0].connections = new int[3];
		nodes[0].prompt = "Hello! \n Test? \n Test!";
		nodes[0].responses[0] = "Hell";
		nodes[0].responses[1] = "ooooo";
		nodes[0].responses[2] = "World";
		nodes[0].connections[0] = 1;
		nodes[0].connections[1] = 1;
		nodes[0].connections[2] = 1;

		nodes[1] = new DialogueNode();
		nodes[1].responses = new string[3];
		nodes[1].connections = new int[3];
		nodes[1].prompt = "Level 2 \n mah doo";
		nodes[1].responses[0] = "Loop 4ever";
		nodes[1].responses[1] = "Restart Pls";
		nodes[1].responses[2] = "Bye";
		nodes[1].connections[0] = 1;
		nodes[1].connections[1] = 0;
		nodes[1].connections[2] = 2;
	}

	private void OnGUI()
	{
		if (nodes[currNode] != null)
		{
			DisplayNode(nodes[currNode]);
		}
		else
		{
			ResetCurrNode();
			this.enabled = false;
		}
	}

	void DisplayNode(DialogueNode node)
	{
		//convert the root point/objects position to screen coords
		Vector3 screenPos = cam.WorldToScreenPoint(wPoint.transform.position);

		//make some boxes
		Rect testRect = new Rect(screenPos.x - 37.5f, Screen.height - screenPos.y - 80, 75, 75);
		Rect reply1Rect = new Rect(screenPos.x - 150, Screen.height - screenPos.y, 100, 50);
		Rect reply2Rect = new Rect(screenPos.x - 50, Screen.height - screenPos.y, 100, 50);
		Rect reply3Rect = new Rect(screenPos.x + 50, Screen.height - screenPos.y, 100, 50);

		GUI.Label(testRect, node.prompt, "box");

		//node.responses[0].transform.position = screenPos + new Vector3(-100, 15, 0);
		//node.responses[1].transform.position = screenPos + new Vector3(0, 0, 0);
		//node.responses[2].transform.position = screenPos + new Vector3(100, 15, 0);

		if (GUI.Button(reply1Rect, node.responses[0]))
		{
			currNode = nodes[currNode].connections[0];
		}
		else if (GUI.Button(reply2Rect, node.responses[1]))
		{
			currNode = nodes[currNode].connections[1];
		}
		else if (GUI.Button(reply3Rect, node.responses[2]))
		{
			currNode = nodes[currNode].connections[2];
		}
	}

	void ResetCurrNode()
	{
		currNode = 0;
	}
}
