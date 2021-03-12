using UnityEditor;

[CustomEditor(typeof(ContentPartSO)), CanEditMultipleObjects]
public class CustomEditorContentSO : Editor
{
    public SerializedProperty
        contentType,
        contentImage,
        imageWatermark,
        contentText,
        textType,
        alignmentOptions,
        videoName,
        videoURL;

    private void OnEnable()
    {
        contentType = serializedObject.FindProperty("contentType");
        contentImage = serializedObject.FindProperty("contentImage");
        imageWatermark = serializedObject.FindProperty("imageWatermarkText");
        contentText = serializedObject.FindProperty("contentText");
        textType = serializedObject.FindProperty("textType");
        alignmentOptions = serializedObject.FindProperty("alignmentOptions");
        videoName = serializedObject.FindProperty("videoName");
        videoURL = serializedObject.FindProperty("videoURL");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(contentType);

        ContentPartSO.ContentType _contentType = (ContentPartSO.ContentType)contentType.enumValueIndex;

        switch (_contentType)
        {
            case ContentPartSO.ContentType.Text:
                EditorGUILayout.PropertyField(contentText);
                EditorGUILayout.PropertyField(textType);
                EditorGUILayout.PropertyField(alignmentOptions);
                break;

            case ContentPartSO.ContentType.Image:
                EditorGUILayout.PropertyField(contentImage);
                EditorGUILayout.PropertyField(imageWatermark);
                break;

            case ContentPartSO.ContentType.Video:
                EditorGUILayout.PropertyField(videoName);
                EditorGUILayout.PropertyField(videoURL);
                break;

            case ContentPartSO.ContentType.AR:
                break;

            case ContentPartSO.ContentType.Question:
                break;

            default:
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
