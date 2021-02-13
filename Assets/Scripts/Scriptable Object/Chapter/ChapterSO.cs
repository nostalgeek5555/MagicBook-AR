using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Chapter", menuName = "Scriptable Object/Chapter")]
public class ChapterSO : ScriptableObject
{
    public int chapterID;
    public bool chapterUnlocked;
    public string chapterName;
    public string chapterTitle;

    [ReorderableList] public List<SubchapterSO> subchapterList;
}
