using UnityEditor;

[CustomEditor(typeof(ContentNode)), CanEditMultipleObjects]
public class CustomEditorContentNode : Editor
{
    public SerializedProperty
        contentID,
        contentType,
        contentText,
        contentImage,
        contentImageWatermarkTxt,
        contentVideoImage,
        contentVideoStatusText,
        contentVideoPlayer,
        contentThumbnailImageLayer,
        contentVideoButton,
        contentVideoName,
        contentVideoURL,
        contentVideoButtonPanelAnimator;

    private void OnEnable()
    {
        contentID = serializedObject.FindProperty("contentID");
        contentType = serializedObject.FindProperty("contentType");
        contentText = serializedObject.FindProperty("contentText");
        contentImage = serializedObject.FindProperty("contentImage");
        contentImageWatermarkTxt = serializedObject.FindProperty("imageWatermarkText");
        contentVideoImage = serializedObject.FindProperty("contentVideoImage");
        contentVideoStatusText = serializedObject.FindProperty("videoStatusText");
        contentVideoPlayer = serializedObject.FindProperty("videoPlayer");
        contentThumbnailImageLayer = serializedObject.FindProperty("thumbnailImageLayer");
        contentVideoButton = serializedObject.FindProperty("videoButton");
        contentVideoName = serializedObject.FindProperty("fileName");
        contentVideoURL = serializedObject.FindProperty("URLvideo");
        contentVideoButtonPanelAnimator = serializedObject.FindProperty("videoButtonPanelAnimator");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(contentID);
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(contentType);

        ContentPartSO.ContentType _contentType = (ContentPartSO.ContentType)contentType.enumValueIndex;

        switch(_contentType)
        {
            case ContentPartSO.ContentType.Text:
                EditorGUILayout.LabelField("Text Content", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(contentText);
                break;

            case ContentPartSO.ContentType.Image:
                EditorGUILayout.LabelField("Image Content", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(contentImage);
                EditorGUILayout.PropertyField(contentImageWatermarkTxt);
                break;

            case ContentPartSO.ContentType.Video:
                EditorGUILayout.LabelField("Video Content", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(contentVideoImage);
                EditorGUILayout.PropertyField(contentVideoPlayer);
                EditorGUILayout.PropertyField(contentVideoStatusText);
                EditorGUILayout.PropertyField(contentThumbnailImageLayer);
                EditorGUILayout.PropertyField(contentVideoButton);

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(contentVideoName);
                EditorGUILayout.PropertyField(contentVideoURL);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.PropertyField(contentVideoButtonPanelAnimator);
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
