using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class DialogueJSONCreator : EditorWindow {
	public DialogueCollection dialogueCollection;
	private string dialoguePath = "/Resources/Dialogue/";

	[MenuItem("JSON Tools/Dialogue Creator")]
	public static void ShowWindow() {
		EditorWindow.GetWindow(typeof(DialogueJSONCreator)).Show();
	}

	public void OnGUI() {
		GUILayout.Label("Dialogue Creator", EditorStyles.largeLabel);

		if (GUILayout.Button("New Dialogue")) {
			dialogueCollection = new DialogueCollection();
		}

		if (GUILayout.Button("Save data")) {
			SaveDialogue();
		}
		if (GUILayout.Button("Load data")) {
			LoadDialogue();
		}

		if (dialogueCollection != null) {
			SerializedObject serializedObject = new SerializedObject(this);
			SerializedProperty serializedProperty = serializedObject.FindProperty("dialogueCollection");
			EditorGUILayout.PropertyField(serializedProperty, true);

			serializedObject.ApplyModifiedProperties();
		}
	}

	public void SaveDialogue() {
		if (dialogueCollection == null) return;
		string filePath = EditorUtility.SaveFilePanel(
			"Save Dialogue File", Application.dataPath + dialoguePath, "shenanigans", "json");

		if (string.IsNullOrEmpty(filePath)) {
			return;
		}

		string json = JsonUtility.ToJson(dialogueCollection);

		File.WriteAllText(filePath, json);
	}

	public void LoadDialogue() {
		string filePath = EditorUtility.OpenFilePanel(
			"Load Dialogue File", Application.dataPath + dialoguePath, "json");

		if (string.IsNullOrEmpty(filePath)) {
			return;
		}

		if (File.Exists(filePath)) {
			string jsonString = File.ReadAllText(filePath);
			dialogueCollection = JsonUtility.FromJson<DialogueCollection>(jsonString);
		} else {
			dialogueCollection = new DialogueCollection();
		}
	}
}