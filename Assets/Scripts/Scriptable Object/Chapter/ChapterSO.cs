using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Chapter", menuName = "Scriptable Object/Chapter")]
public class ChapterSO : ScriptableObject
{
    public ChapterType chapterType;
    public ChapterFillerType chapterFillerType;
    public int chapterID;
    public bool chapterUnlocked;
    public string chapterName;
    public string chapterTitle;
    public float fontSize;

    [ReorderableList] public List<SubchapterSO> subchapterList;

    public enum ChapterType
    {
       Filler, Content
    }

    public enum ChapterFillerType
    {
        Guide, ConceptMap, Review, Glosarium, About
    }
}
