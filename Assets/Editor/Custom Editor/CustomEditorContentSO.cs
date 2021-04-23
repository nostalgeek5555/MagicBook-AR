using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ContentPartSO)), CanEditMultipleObjects]
public class CustomEditorContentSO : Editor
{
    public SerializedProperty
        contentType,
        contentImage,
        imageSize,
        imageSetNativeSize,
        preserveAspect,
        customAnchorPoint,
        leftAnchor,
        rightAnchor,
        topAnchor,
        bottomAnchor,
        imageWatermark,
        contentText,
        textType,
        alignmentOptions,
        fontSize,
        fontAsset,
        videoName,
        videoURL,
        questionID,
        question,
        matchAnswerID,
        allAnswers;

    private void OnEnable()
    {
        contentType = serializedObject.FindProperty("contentType");

        contentImage = serializedObject.FindProperty("contentImage");
        imageSize = serializedObject.FindProperty("contentImageSize");
        imageSetNativeSize = serializedObject.FindProperty("imageSetNativeSize");
        preserveAspect = serializedObject.FindProperty("preserveAspect");
        customAnchorPoint = serializedObject.FindProperty("customAnchorPoint");
        leftAnchor = serializedObject.FindProperty("leftAnchor");
        rightAnchor = serializedObject.FindProperty("rightAnchor");
        topAnchor = serializedObject.FindProperty("topAnchor");
        bottomAnchor = serializedObject.FindProperty("bottomAnchor");
        imageWatermark = serializedObject.FindProperty("imageWatermarkText");

        contentText = serializedObject.FindProperty("contentText");
        textType = serializedObject.FindProperty("textType");
        fontSize = serializedObject.FindProperty("fontSize");
        fontAsset = serializedObject.FindProperty("fontAsset");
        alignmentOptions = serializedObject.FindProperty("alignmentOptions");

        videoName = serializedObject.FindProperty("videoName");
        videoURL = serializedObject.FindProperty("videoURL");

        questionID = serializedObject.FindProperty("questionID");
        question = serializedObject.FindProperty("question");
        matchAnswerID = serializedObject.FindProperty("matchAnswerID");
        allAnswers = serializedObject.FindProperty("allAnswers");
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
                EditorGUILayout.PropertyField(fontSize);
                EditorGUILayout.PropertyField(fontAsset);
                break;

            case ContentPartSO.ContentType.Image:
                EditorGUILayout.PropertyField(contentImage);
                EditorGUILayout.BeginHorizontal();
                var sprite = contentImage.objectReferenceValue as Sprite;
                if (sprite != null)
                {
                    var texture = AssetPreview.GetAssetPreview(sprite);
                    GUILayout.Label(texture);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(imageSize);
                EditorGUILayout.PropertyField(imageSetNativeSize);
                EditorGUILayout.PropertyField(preserveAspect);
                EditorGUILayout.PropertyField(customAnchorPoint);
                EditorGUILayout.BeginHorizontal();
                //var customAnchorToggle = customAnchorPoint.objectReferenceValue;

                //if (customAnchorToggle)
                //{

                //}
                EditorGUILayout.PropertyField(leftAnchor);
                EditorGUILayout.PropertyField(rightAnchor);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(topAnchor);
                EditorGUILayout.PropertyField(bottomAnchor);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                EditorGUILayout.PropertyField(imageWatermark);
                break;

            case ContentPartSO.ContentType.Video:
                EditorGUILayout.PropertyField(videoName);
                EditorGUILayout.PropertyField(videoURL);
                break;

            case ContentPartSO.ContentType.AR:
                break;

            case ContentPartSO.ContentType.Question:
                EditorGUILayout.PropertyField(questionID);
                EditorGUILayout.PropertyField(question);
                EditorGUILayout.PropertyField(matchAnswerID);
                EditorGUILayout.PropertyField(allAnswers);
                break;

            case ContentPartSO.ContentType.Subject:
                EditorGUILayout.PropertyField(contentImage);
                EditorGUILayout.BeginHorizontal();
                var subjectImage = contentImage.objectReferenceValue as Sprite;
                if (subjectImage != null)
                {
                    var texture = AssetPreview.GetAssetPreview(subjectImage);
                    GUILayout.Label(texture);
                }
                EditorGUILayout.EndHorizontal();

                break;

            default:
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }


}
