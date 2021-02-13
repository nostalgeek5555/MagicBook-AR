using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Content", menuName = "Scriptable Object/Content/ContentParts")]
public class ContentPartSO : ScriptableObject
{
    public ContentType contentType;
    public string contentName;
    public Sprite contentImage;
    public string contentText;
    public string videoURL;

    public enum ContentType
    {
        Text, Image, Video, Question, AR
    }
}
