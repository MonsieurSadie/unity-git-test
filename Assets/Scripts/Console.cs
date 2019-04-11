using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuntimeTools
{
  public class Console : MonoBehaviour {
    static bool logToEditor = true;

    static string logText;
    static bool showLogs = true;
    Vector2 scrollPosition = Vector2.zero;


    public static void Log(string text)
    {
      logText += "- " + text + "\n";
      if(logToEditor)
      {
        Debug.Log(text);
      }
    }
    
    
    void OnGUI () 
    {
#if UNITY_ANDROID
      GUI.skin.label.fontSize = 24;
      GUI.skin.button.fontSize = 30;
      GUI.skin.toggle.fontSize = 30;
#else
      GUI.skin.label.fontSize = 16;
      GUI.skin.button.fontSize = 18;
      GUI.skin.toggle.fontSize = 18;
#endif

      GUILayout.BeginHorizontal();
      showLogs = GUILayout.Toggle(showLogs, "Toggle logs");
      GUILayout.FlexibleSpace();
      if(showLogs && GUILayout.Button("Clear logs")) logText = "";
      GUILayout.EndHorizontal();

      if(showLogs) 
      {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
        GUILayout.Label(logText);
        GUILayout.EndScrollView();
      }
    }
  }
}
