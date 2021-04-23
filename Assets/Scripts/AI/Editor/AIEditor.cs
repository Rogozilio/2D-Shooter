using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(AI), true)]
public class AIEditor : Editor
{
    private AI ai;

    private SerializedProperty isActiveRandomStep;
    private SerializedProperty radius;
    private SerializedProperty speed;
    private SerializedProperty delay;
    public void OnEnable()
    {
        ai = target as AI;

        isActiveRandomStep = serializedObject.FindProperty("isActiveRandomStep");
        radius = serializedObject.FindProperty("radius");
        speed = serializedObject.FindProperty("speed");
        delay = serializedObject.FindProperty("delay");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ai.isActiveRandomStep
                = GUILayout.Toggle(isActiveRandomStep.boolValue, "IsActiveRandomStep");

        if (ai.isActiveRandomStep)
        {
            ai.radius = EditorGUILayout.Slider("Radius", radius.floatValue, 1f, 10f);
            ai.speed = EditorGUILayout.Slider("Speed", speed.floatValue, 0.1f, 10f);
            ai.delay = EditorGUILayout.Slider("Delay", delay.floatValue, 2f, 10f);
        }

        isActiveRandomStep.boolValue = ai.isActiveRandomStep;
        radius.floatValue            = ai.radius;
        speed.floatValue             = ai.speed;
        delay.floatValue             = ai.delay;

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
            SetObjectDirty(ai.gameObject);
    }
    public void SetObjectDirty(GameObject obj)
    {
        EditorUtility.SetDirty(obj);
        EditorSceneManager.MarkSceneDirty(obj.scene);
    }
}