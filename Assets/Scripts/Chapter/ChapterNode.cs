using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChapterNode : MonoBehaviour
{
    public int chapterID;
    public string chapterName;
    public string chapterTitle;
    public Button chapterButton;
    public TextMeshProUGUI chapterNameText;
    public ChapterSO chapterSO;
    
    
    public void InitChapterNode(ChapterSO _chapterSO)
    {
        chapterSO = _chapterSO;
        chapterID = chapterSO.chapterID;
        chapterName = chapterSO.chapterName;
        chapterTitle = chapterSO.chapterTitle;
        chapterNameText.text = chapterTitle;
        Debug.Log("chapter title " + chapterTitle);

        chapterButton.onClick.RemoveAllListeners();
        chapterButton.onClick.AddListener(() =>
        {
            OpenSubchapterPanel();
        });
    }
    
    private void OpenSubchapterPanel()
    {
        PanelController.Instance.chapterName = chapterName;
        UIManager.Instance.currentChapterName = chapterName;
        PanelController.Instance.ActiveDeactivePanel("subchapter", "chapter");
        Debug.Log("current chapter name " + chapterName);
    }
}
