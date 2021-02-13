using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubchapterNode : MonoBehaviour
{
    public SubchapterSO subchapterSO;
    public int subchapterID;
    public bool subchapterUnlocked;
    public string subchapterName;
    public string subchapterTitle;
    public Button subchapterButton;
    public TextMeshProUGUI subchapterTitleTxt;
    
    public void InitSubchapter(SubchapterSO _subchapterSO)
    {
        subchapterSO = _subchapterSO;
        subchapterID = subchapterSO.subchapterID;
        subchapterName = subchapterSO.subchapterName;
        subchapterTitle = subchapterSO.subchapterTitle;
        subchapterTitleTxt.text = subchapterName;

        subchapterButton.onClick.RemoveAllListeners();
        subchapterButton.onClick.AddListener(() =>
        {
            OpenContentPanel();
        });
    }

    private void OpenContentPanel()
    {
        UIManager.Instance.currentSubchapterName = subchapterName;
        PanelController.Instance.ActiveDeactivePanel("content", "subchapter");
        
    }
}
