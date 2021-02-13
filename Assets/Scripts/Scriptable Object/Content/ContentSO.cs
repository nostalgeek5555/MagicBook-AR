using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Content", menuName = "Scriptable Object/Content/ContentContainer")]
public class ContentSO : ScriptableObject
{
    public int contentID;
    public string contentTitle;

    [ReorderableList] public List<ContentPartSO> allContentParts;    
}
