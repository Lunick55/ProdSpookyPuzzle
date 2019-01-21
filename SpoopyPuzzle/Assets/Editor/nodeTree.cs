using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class nodeEditor : EditorWindow
{
	string tempString = "Hello World!";

	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;

	Rect window1;
	Rect window2;

	[MenuItem("Window/Node Editor")]
	static void Startup()
	{
		nodeEditor window = (nodeEditor)EditorWindow.GetWindow<nodeEditor>();

		window.window1 = new Rect(10, 10, 100, 100);
		window.window2 = new Rect(210, 210, 100, 100);

		window.Show();
	}

	private void OnGUI()
	{
		DrawNodeCurve(window1, window2);



		GUILayout.BeginArea(new Rect(0, 0, 500, 300), GUI.skin.GetStyle("Box"));
			GUILayout.Label("Base Settings", EditorStyles.boldLabel);
			tempString = EditorGUILayout.TextField("Text Field", tempString);

			groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
			myBool = EditorGUILayout.Toggle("Toggle", myBool);
			myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
			EditorGUILayout.EndToggleGroup();
		GUILayout.EndArea();

		//creates the 2 node windows
		BeginWindows();
		window1 = GUI.Window(1, window1, DrawNodeWindow, "Window1");
		
		window2 = GUI.Window(2, window2, DrawNodeWindow, "Window2");
		EndWindows();

		
	}

	void DrawNodeWindow(int id)
	{
		GUI.DragWindow();
	}

	void DrawNodeCurve(Rect start, Rect end)
	{
		Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height/2, 0);
		Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);

		Vector3 startTan = startPos + Vector3.right * 50;
		Vector3 endTan = endPos + Vector3.left * 50;

		Color shadowCol = new Color(0, 0, 0, 0.06f);

		for (int i = 0; i < 3; i++)
		{
			Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i+1) * 5);
		}
		Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);

	}
}
