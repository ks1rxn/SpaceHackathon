using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class LevelSettingsEditor : EditorWindow {
	private int m_settingsLevel;
	private object m_settings;

	private int m_indexInList;

    [MenuItem("Hard Rockets/Level Settings Editor/Easy")]
    public static void ShowWindowEasy() {
		ShowWindow(1);
    }

	[MenuItem("Hard Rockets/Level Settings Editor/Medium")]
    public static void ShowWindowMedium() {
		ShowWindow(2);
    }

	[MenuItem("Hard Rockets/Level Settings Editor/Hard")]
    public static void ShowWindowHard() {
		ShowWindow(3);
    }

	private static void ShowWindow(int level) {
		string filePath = Path.Combine(Application.streamingAssetsPath, "settings_difficuly_" + level + ".jsn");
	    if (File.Exists(filePath)) {
		    string dataAsJson = File.ReadAllText(filePath);
		    LevelSettings settings = JsonConvert.DeserializeObject<LevelSettings>(dataAsJson);
		    LevelSettingsEditor window = (LevelSettingsEditor) EditorWindow.GetWindow(typeof(LevelSettingsEditor));
		    window.SetSettings(settings, level);
	    } else {
		    LevelSettings settings = new LevelSettings();
			string json = JsonConvert.SerializeObject(settings, Formatting.Indented);
			StreamWriter outStream = System.IO.File.CreateText(filePath);
			outStream.WriteLine(json);
			outStream.Close();
			LevelSettingsEditor window = (LevelSettingsEditor) EditorWindow.GetWindow(typeof(LevelSettingsEditor));
		    window.SetSettings(settings, level);
	    }
	}

	public void SetSettings(object settings, int level) {
		m_settings = settings;
		m_settingsLevel = level;
	}

	private void OnGUI() {
	    if (m_settings == null) {
		    return;
	    }
        GUILayout.Label("Difficulty Level " + m_settingsLevel, EditorStyles.boldLabel);

		PropertyInfo[] props = m_settings.GetType().GetProperties();
		string[] propLabels = new string[props.Length];
	    for (int i = 0; i != props.Length; i++) {
		    propLabels[i] = props[i].Name;
	    }
	    m_indexInList = EditorGUILayout.Popup(m_indexInList, propLabels, GUILayout.Width(220));
	    PropertyInfo currentInfo = props[m_indexInList];
	    object currentObject = m_settings.GetType().GetProperty(currentInfo.Name).GetValue(m_settings, null);
	    FieldInfo[] actualParams = currentObject.GetType().GetFields();
	    foreach (FieldInfo info in actualParams) {
		    if (info.FieldType == typeof(int)) {
				 GUILayout.BeginHorizontal();
				 GUILayout.Label(info.Name, GUILayout.Width(200));
				 info.SetValue(currentObject, EditorGUILayout.IntField("", (int)currentObject.GetType().GetField(info.Name).GetValue(currentObject), GUILayout.Width(70)));
				 GUILayout.EndHorizontal();
		    }
			if (info.FieldType == typeof(float)) {
				GUILayout.BeginHorizontal();
				GUILayout.Label(info.Name, GUILayout.Width(200));
			    info.SetValue(currentObject, EditorGUILayout.FloatField("", (float)currentObject.GetType().GetField(info.Name).GetValue(currentObject), GUILayout.Width(70)));
				GUILayout.EndHorizontal();
		    }
		    if (info.FieldType == typeof(bool)) {
				GUILayout.BeginHorizontal();
				GUILayout.Label(info.Name, GUILayout.Width(200));
			    info.SetValue(currentObject, EditorGUILayout.Toggle("", (bool)currentObject.GetType().GetField(info.Name).GetValue(currentObject)));
				GUILayout.EndHorizontal();
		    }
	    }
		if (GUILayout.Button("Save Settings", GUILayout.Width(220))) {
			string filePath = Path.Combine(Application.streamingAssetsPath, "settings_difficuly_" + m_settingsLevel + ".jsn");
			string json = JsonConvert.SerializeObject(m_settings, Formatting.Indented);
			StreamWriter outStream = System.IO.File.CreateText(filePath);
			outStream.WriteLine(json);
			outStream.Close();
        }
    }

}