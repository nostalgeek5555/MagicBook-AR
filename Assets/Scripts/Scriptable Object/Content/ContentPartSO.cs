using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Content", menuName = "Scriptable Object/Content/ContentParts")]
public class ContentPartSO : ScriptableObject
{
    public ContentType contentType;
    public TextType textType;
    public TextAlignmentOptions alignmentOptions;
    public float fontSize;
    public TMP_FontAsset fontAsset;
    public Graphic graphic;
    public Sprite contentImage;
    public bool imageSetNativeSize;
    public string imageWatermarkText;
    public string contentText;
    public string videoName;
    public string videoURL;

    [SerializeField]
    public Dictionary<string, List<Tuple<int, int>>> keyValuePairs;

    public enum ContentType
    {
        Text, Image, Video, Question, AR
    }

    public enum TextType
    {
        Title, Line, Paragraph, Watermark
    }

}
