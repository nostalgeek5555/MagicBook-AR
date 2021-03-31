using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(ContentPartSO)), CanEditMultipleObjects]
public class CustomEditorContentSO : Editor
{
    public SerializedProperty
        contentType,
        contentImage,
        imageSetNativeSize,
        imageWatermark,
        contentText,
        textType,
        alignmentOptions,
        fontSize,
        fontAsset,
        videoName,
        videoURL;

    private void OnEnable()
    {
        contentType = serializedObject.FindProperty("contentType");
        contentImage = serializedObject.FindProperty("contentImage");
        imageSetNativeSize = serializedObject.FindProperty("imageSetNativeSize");
        imageWatermark = serializedObject.FindProperty("imageWatermarkText");
        contentText = serializedObject.FindProperty("contentText");
        textType = serializedObject.FindProperty("textType");
        fontSize = serializedObject.FindProperty("fontSize");
        fontAsset = serializedObject.FindProperty("fontAsset");
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
                EditorGUILayout.PropertyField(imageSetNativeSize);
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
