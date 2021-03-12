using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SubchapterNode : MonoBehaviour
{
    [Header("Subchapter Data")]
    public int subchapterID;
    public bool subchapterUnlocked;
    public string subchapterName;
    public string subchapterTitle;

    [Header("Subchapter Node UI")]
    public Button subchapterButton;
    public TextMeshProUGUI subchapterTitleTxt;
    
    public void InitSubchapter(SubchapterSO _subchapterSO)
    {
        subchapterID = _subchapterSO.subchapterID;
        subchapterName = _subchapterSO.subchapterName;
        subchapterTitle = _subchapterSO.subchapterTitle;
        subchapterTitleTxt.text = subchapterTitle;

        if (DataManager.Instance != null)
        {
            if (DataManager.Instance.playerData != null)
            {
                string currentSubchapterKey = DataManager.Instance.currentChapterName + "|" + _subchapterSO.subchapterName;
                if (DataManager.Instance.playerData.subchapterUnlocked.ContainsKey(currentSubchapterKey))
                {
                    subchapterUnlocked = DataManager.Instance.playerData.subchapterUnlocked[currentSubchapterKey];
                    if (subchapterUnlocked)
                    {
                        subchapterButton.interactable = true;
                        subchapterButton.onClick.RemoveAllListeners();
                        subchapterButton.onClick.AddListener(() =>
                        {
                            OpenContentPanel();
                        });
                    }

                    else
                    {
                        subchapterButton.interactable = false;
                    }
                }

                else
                {
                    subchapterUnlocked = false;
                    subchapterButton.interactable = false;
                }
            }
        }
    }

    private void OpenContentPanel()
    {
        DataManager.Instance.currentSubchapterID = subchapterID;
        DataManager.Instance.currentSubchapterName = subchapterName;
        DataManager.Instance.currentSubchapterTitle = subchapterTitle;
        PanelController.Instance.ActiveDeactivePanel("content", "subchapter");
    }
}
