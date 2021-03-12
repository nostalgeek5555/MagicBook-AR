using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChapterNode : MonoBehaviour
{
    [Header("Chapter Data")]
    [ReadOnly] public int chapterID;
    [ReadOnly] public string chapterName;
    public bool chapterUnlocked;
    public string chapterTitle;
    public int totalSubchapter;

    [Header("Chapter Node UI")]
    public Button chapterButton;
    public TextMeshProUGUI chapterNameText;

    private void Start()
    {
        for (int i = 0; i < DataManager.Instance.playerData.allChapterUnlocked[DataManager.Instance.playerData.currentChapter].subchapterNameList.Count; i++)
        {
            Debug.Log("subchapter name " + DataManager.Instance.playerData.allChapterUnlocked[DataManager.Instance.playerData.currentChapter].subchapterNameList[i]);
        }    
    }

    public void InitChapterNode(ChapterSO _chapterSO)
    {
        chapterID = _chapterSO.chapterID;
        chapterName = _chapterSO.chapterName;
        chapterTitle = _chapterSO.chapterTitle;
        totalSubchapter = _chapterSO.subchapterList.Count;
        chapterNameText.text = chapterTitle;
     
        if (DataManager.Instance.playerData.allChapterUnlocked.ContainsKey(chapterName))
        {
            chapterUnlocked = DataManager.Instance.playerData.chapterUnlocked[chapterName];

            if (chapterUnlocked)
            {
                chapterButton.interactable = chapterUnlocked;
                chapterButton.onClick.RemoveAllListeners();
                chapterButton.onClick.AddListener(() =>
                {
                    OpenSubchapterPanel();
                });
            }
            
            else
            {
                chapterButton.interactable = false;
            }
        }

        else
        {
            chapterUnlocked = false;
            chapterButton.interactable = false;
        }
        
    }
    
    private void OpenSubchapterPanel()
    {
        PanelController.Instance.chapterName = chapterName;
        DataManager.Instance.currentChapterID = chapterID;
        DataManager.Instance.currentChapterName = chapterName;
        Debug.Log("current chapter name " + DataManager.Instance.currentChapterName);
        DataManager.Instance.currentTotalSubchapter = totalSubchapter;
        PanelController.Instance.ActiveDeactivePanel("subchapter", "chapter");
        //Debug.Log("current chapter name " + chapterName);
    }
}
