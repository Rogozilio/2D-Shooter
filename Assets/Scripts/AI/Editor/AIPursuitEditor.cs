using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(AIPursuit), true)]
public class AIPursuitEditor : Editor
{
    AIPursuit aiPursuit;

    SerializedProperty isActiveRandomStep;
    SerializedProperty radius;
    SerializedProperty speed;
    SerializedProperty delay;
    public void OnEnable()
    {
        aiPursuit = target as AIPursuit;

        isActiveRandomStep = serializedObject.FindProperty("isActiveRandomStep");
        radius = serializedObject.FindProperty("radius");
        speed = serializedObject.FindProperty("speed");
        delay = serializedObject.FindProperty("delay");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        aiPursuit.isActiveRandomStep
                = GUILayout.Toggle(isActiveRandomStep.boolValue, "IsActiveRandomStep");

        if (aiPursuit.isActiveRandomStep)
        {
            aiPursuit.radius = EditorGUILayout.Slider("Radius", radius.floatValue, 1f, 10f);
            aiPursuit.speed = EditorGUILayout.Slider("Speed", speed.floatValue, 0.1f, 10f);
            aiPursuit.delay = EditorGUILayout.Slider("Delay", delay.floatValue, 2f, 10f);
        }

        isActiveRandomStep.boolValue = aiPursuit.isActiveRandomStep;
        radius.floatValue            = aiPursuit.radius;
        speed.floatValue             = aiPursuit.speed;
        delay.floatValue             = aiPursuit.delay;

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
            SetObjectDirty(aiPursuit.gameObject);
    }
    public void SetObjectDirty(GameObject obj)
    {
        EditorUtility.SetDirty(obj);
        EditorSceneManager.MarkSceneDirty(obj.scene);
    }
}
