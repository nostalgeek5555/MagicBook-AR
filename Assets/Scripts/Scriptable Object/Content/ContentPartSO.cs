using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "Content", menuName = "Scriptable Object/Content/ContentParts")]
public class ContentPartSO : ScriptableObject
{
    public ContentType contentType;
    public TextType textType;
    public TextAlignmentOptions alignmentOptions;
    public Sprite contentImage;
    public string contentText;
    public string videoName;
    public string videoURL;

    public enum ContentType
    {
        Text, Image, Video, Question, AR
    }

    public enum TextType
    {
        Title, Line, Paragraph
    }

}
