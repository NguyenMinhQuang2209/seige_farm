using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactible),true)]
public class InteractibleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Interactible item = (Interactible)target;
        if(item.GetComponent<EventOnlyInteract>() != null)
        {
            item.promptMessage = EditorGUILayout.TextField("Prompt Message", item.promptMessage);
            if (item.GetComponent<InteractibleEvent>() == null)
            {
                item.useEvent = true;
                item.gameObject.AddComponent<InteractibleEvent>();
            }
        }
        else
        {
            base.OnInspectorGUI();
            if (item.useEvent)
            {
                if(item.GetComponent<InteractibleEvent>() == null)
                {
                    item.gameObject.AddComponent<InteractibleEvent>();
                }
            }
            else
            {
                if (item.GetComponent<InteractibleEvent>() != null)
                {
                    DestroyImmediate(item.GetComponent<InteractibleEvent>());
                }
            }
        }
    }
}
