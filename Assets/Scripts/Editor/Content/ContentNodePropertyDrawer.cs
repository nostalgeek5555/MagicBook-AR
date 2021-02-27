using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ContentPartSO))]
public class ContentNodePropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        EditorGUI.BeginChangeCheck();

        var contentType = new Rect(position.x, position.y, 30, position.height);
        var contentImage = new Rect(position.x, position.y + 30, 50, position.height);
        var contentText = new Rect(position.x, position.y + 60, position.width, position.height);
        var contentURL = new Rect(position.x, position.y + 90, position.width, position.height);

        EditorGUI.LabelField(contentType, "Content Type : ");
        EditorGUI.PropertyField(contentType, property.FindPropertyRelative("ContentType"), GUIContent.none);

    }
}
