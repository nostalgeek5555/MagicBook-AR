using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Subchapter", menuName = "Scriptable Object/Subchapter")]
public class SubchapterSO : ScriptableObject
{
    public ContentPanelUI.ContentPanelType panelType;

    public int subchapterID;
    public bool subchapterUnlocked;
    public string subchapterName;
    public string subchapterTitle;
    [ShowAssetPreview] public Sprite subchapterIcon;

    [ReorderableList] public List<ContentPartSO> subchapterContents;
}
