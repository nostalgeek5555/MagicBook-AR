using UnityEditor;

[CustomEditor(typeof(ContentNode)), CanEditMultipleObjects]
public class CustomEditorContentNode : Editor
{
    public SerializedProperty
        contentID,
        contentType,
        contentText,
        contentTextFont,
        contentImage,
        contentImageWatermarkTxt,
        subjectImage,
        contentVideoImage,
        contentVideoStatusText,
        contentVideoPlayer,
        contentThumbnailImageLayer,
        contentVideoButton,
        contentVideoName,
        contentVideoURL,
        contentVideoButtonPanelAnimator,
        contentQuestionNumberText,
        contentQuestionText,
        contentAnswerToggleGroup,
        contentAnswerPrefab,
        contentQuestionID,
        contentQuestionQuest,
        contentQuestionAnswerID,
        contentQuestionAllAnswers,
        contentQuestionScore;

    private void OnEnable()
    {
        contentID = serializedObject.FindProperty("contentID");
        contentType = serializedObject.FindProperty("contentType");
        contentText = serializedObject.FindProperty("contentText");
        contentTextFont = serializedObject.FindProperty("fontAsset");
        contentImage = serializedObject.FindProperty("contentImage");
        subjectImage = serializedObject.FindProperty("subjectImage");
        contentImageWatermarkTxt = serializedObject.FindProperty("imageWatermarkText");
        contentVideoImage = serializedObject.FindProperty("contentVideoImage");
        contentVideoStatusText = serializedObject.FindProperty("videoStatusText");
        contentVideoPlayer = serializedObject.FindProperty("videoPlayer");
        contentThumbnailImageLayer = serializedObject.FindProperty("thumbnailImageLayer");
        contentVideoButton = serializedObject.FindProperty("videoButton");
        contentVideoName = serializedObject.FindProperty("fileName");
        contentVideoURL = serializedObject.FindProperty("URLvideo");
        contentVideoButtonPanelAnimator = serializedObject.FindProperty("videoButtonPanelAnimator");

        contentQuestionNumberText = serializedObject.FindProperty("questionNumberText");
        contentQuestionText = serializedObject.FindProperty("questionText");
        contentAnswerToggleGroup = serializedObject.FindProperty("answerToggleGroup");
        contentAnswerPrefab = serializedObject.FindProperty("answerPrefab");
        contentQuestionID = serializedObject.FindProperty("questionID");
        contentQuestionQuest = serializedObject.FindProperty("questionContent");
        contentQuestionAnswerID = serializedObject.FindProperty("matchAnswerID");
        contentQuestionAllAnswers = serializedObject.FindProperty("_allAnswers");
        contentQuestionScore = serializedObject.FindProperty("thisQuestionScore");
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
                EditorGUILayout.PropertyField(contentTextFont);
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
                EditorGUILayout.LabelField("Question Content", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(contentQuestionNumberText);
                EditorGUILayout.PropertyField(contentQuestionText);
                EditorGUILayout.PropertyField(contentAnswerToggleGroup);
                EditorGUILayout.PropertyField(contentAnswerPrefab);
                EditorGUILayout.PropertyField(contentQuestionID);
                EditorGUILayout.PropertyField(contentQuestionQuest);
                EditorGUILayout.PropertyField(contentQuestionAnswerID);
                EditorGUILayout.PropertyField(contentQuestionAllAnswers);
                EditorGUILayout.PropertyField(contentQuestionScore);
                break;

            case ContentPartSO.ContentType.Subject:
                EditorGUILayout.LabelField("Subject Content", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(subjectImage);

                break;
            default:
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
