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
        GUILayout.Label(m_settings.GetType().Name, EditorStyles.boldLabel);

		PropertyInfo[] props = m_settings.GetType().GetProperties();
		string[] propLabels = new string[props.Length];
	    for (int i = 0; i != props.Length; i++) {
		    propLabels[i] = props[i].Name;
	    }
	    m_indexInList = EditorGUILayout.Popup(m_indexInList, propLabels);
	    PropertyInfo currentInfo = props[m_indexInList];
	    object currentObject = m_settings.GetType().GetProperty(currentInfo.Name).GetValue(m_settings, null);
	    PropertyInfo[] actualParams = currentObject.GetType().GetProperties();
	    foreach (PropertyInfo info in actualParams) {
		    if (info.PropertyType == typeof(int)) {
			    info.SetValue(currentObject, int.Parse(EditorGUILayout.TextField(info.Name, currentObject.GetType().GetProperty(info.Name).GetValue(currentObject, null).ToString())), null);
		    }
		    if (info.PropertyType == typeof(bool)) {
			    info.SetValue(currentObject, EditorGUILayout.Toggle(info.Name, (bool)currentObject.GetType().GetProperty(info.Name).GetValue(currentObject, null)), null);
		    }
	    }
		if (GUILayout.Button("Save Settings")) {
			string filePath = Path.Combine(Application.streamingAssetsPath, "settings_difficuly_" + m_settingsLevel + ".jsn");
			string json = JsonConvert.SerializeObject(m_settings, Formatting.Indented);
			StreamWriter outStream = System.IO.File.CreateText(filePath);
			outStream.WriteLine(json);
			outStream.Close();
        }
    }

}