using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Content", menuName = "Scriptable Object/Content/ContentParts")]
public class ContentPartSO : ScriptableObject
{
    public ContentType contentType;

    [Header("Text Content Type")]
    public TextType textType;
    public TextAlignmentOptions alignmentOptions;
    public float fontSize;
    public TMP_FontAsset fontAsset;

    [Header("Image Content Type")]
    public Graphic graphic;
    public Sprite contentImage;
    public float contentImageSize;
    public bool imageSetNativeSize;
    public bool preserveAspect;
    public bool customAnchorPoint;
    public float leftAnchor, rightAnchor, topAnchor, bottomAnchor;
    public string imageWatermarkText;
    public string contentText;

    [Header("Video Content Type")]
    public string videoName;
    public string videoURL;

    [Header("Question Content Type")]
    public int questionID;
    [TextArea]
    public string question;
    public int matchAnswerID;
    [ReorderableList] public List<string> allAnswers;

    [SerializeField]
    public Dictionary<string, List<Tuple<int, int>>> keyValuePairs;

    public enum ContentType
    {
        Text, Image, Video, Question, AR, Subject
    }

    public enum TextType
    {
        Title, Line, Paragraph, Watermark
    }

}
