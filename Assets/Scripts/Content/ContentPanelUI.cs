using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Lean.Pool;
public class ContentPanelUI : MonoBehaviour
{
    [ReadOnly]
    [SerializeField]
    private string currentSubchapterTitle;

    public TextMeshProUGUI contentTitleText;
    public GameObject contentBG;
    public GameObject contentImagePrefab;
    public GameObject contentTextPrefab;
    public GameObject contentVideoPrefab;
    public Transform contentNodeParent;
    public ScrollRect scrollRect;
    public GameObject buttonsInContentLayout;
    public Button nextButton;

    private void OnEnable()
    {
        if (UIManager.Instance != null)
        {
            currentSubchapterTitle = DataManager.Instance.currentSubchapterTitle;
        }
        nextButton.interactable = false;
        contentBG.SetActive(true);
        ScrollToTop(scrollRect);
        InitAllContents();
    }

    private void OnDisable()
    {
        contentBG.SetActive(false);
        nextButton.interactable = false;
    }

    public static void ScrollToTop (ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 1);
    }

    public static void ScrollToBottom (ScrollRect scrollRect)
    {
        scrollRect.normalizedPosition = new Vector2(0, 0);
    }

    

    public void InitAllContents()
    {
        DespawnAllContents();

        contentTitleText.text = currentSubchapterTitle;

        if (GameManager.Instance.allSubchapterData.Count > 0)
        {
            if (GetInstance().currentSubchapterName != "")
            {
                SubchapterSO subchapterSO = GameManager.Instance.allSubchapterData[GetInstance().currentChapterName + "|" + GetInstance().currentSubchapterName];
                ContentNode contentNode;
                for (int i = 0; i < subchapterSO.subchapterContents.Count; i++)
                {
                    ContentPartSO contentPartSO = subchapterSO.subchapterContents[i];

                    switch (contentPartSO.contentType)
                    {
                        case ContentPartSO.ContentType.Image:
                            contentNode = LeanPool.Spawn(contentImagePrefab, contentNodeParent).GetComponent<ContentNode>();
                            contentNode.InitContentNode(contentPartSO, i);

                            break;
                        case ContentPartSO.ContentType.Text:
                            contentNode = LeanPool.Spawn(contentTextPrefab, contentNodeParent).GetComponent<ContentNode>();
                            contentNode.InitContentNode(contentPartSO, i);

                            break;
                        case ContentPartSO.ContentType.Video:
                            contentNode = LeanPool.Spawn(contentVideoPrefab, contentNodeParent).GetComponent<ContentNode>();
                            contentNode.InitContentNode(contentPartSO, i);

                            break;
                        case ContentPartSO.ContentType.Question:
                            break;
                        case ContentPartSO.ContentType.AR:
                            break;
                        default:
                            break;
                    }
                }

                buttonsInContentLayout.transform.SetAsLastSibling();
                ScrollToTop(scrollRect);
            }
        }
    }

    public void DespawnAllContents()
    {
        if (contentNodeParent.childCount > 0)
        {
            for (int i = contentNodeParent.childCount - 1; i >= 0; i--)
            {
                if (contentNodeParent.GetChild(i).GetComponent<ContentNode>() != null)
                {
                    LeanPool.Despawn(contentNodeParent.GetChild(i).gameObject);
                }
                
            }
        }
    }

    public void OnScrolling()
    {
        Debug.Log("on drag");
        if (scrollRect.verticalNormalizedPosition <= 0.3)
        {
            nextButton.interactable = true;
            Debug.Log("current button next interactable " + nextButton.interactable);
        }

        else
        {
            nextButton.interactable = false;
        }
    }
    

    public void UnlockNextChapterSubchapter()
    {
        if (GetInstance() != null)
        {
            if (GetInstance().playerData != null)
            {
                if (GetInstance().currentSubchapterID < GetInstance().currentTotalSubchapter - 1)
                {
                    GetInstance().currentSubchapterID++;
                    int nextSubchapterID = GetInstance().currentSubchapterID;
                    string nextSubchapterName = GetInstance().playerData.allChapterUnlocked[GetInstance().currentChapterName].subchapterNameList[nextSubchapterID];
                    string nextKeyName = GetInstance().currentChapterName + "|" + nextSubchapterName;
                    GetInstance().currentSubchapterName = nextSubchapterName;
                    currentSubchapterTitle = GameManager.Instance.allSubchapterData[nextKeyName].subchapterTitle;
                    if (!GetInstance().playerData.subchapterUnlocked.ContainsKey(nextKeyName))
                    {
                        GetInstance().playerData.subchapterUnlocked.Add(nextKeyName, true);
                        GetInstance().playerData.allChapterUnlocked[GetInstance().currentChapterName].totalSubchapterUnlocked++;
                        GetInstance().SaveData();
                    }
                    InitAllContents();
                }

                else
                {
                    if (GetInstance().currentChapterID < GetInstance().playerData.totalChapter - 1)
                    {
                        GetInstance().currentSubchapterID = 0;
                        GetInstance().currentChapterID++;
                        string nextChapterName = GameManager.Instance.allcurrentChapter[GetInstance().currentChapterID];
                        if (!GetInstance().playerData.chapterUnlocked.ContainsKey(nextChapterName))
                        {
                            if (GetInstance().playerData.totalChapterUnlocked < GameManager.Instance.allChapterList.Count)
                            {
                                
                                GetInstance().playerData.totalChapterUnlocked++;
                                GetInstance().playerData.chapterUnlocked.Add(nextChapterName, true);
                                GetInstance().chapterData = new DataManager.ChapterData(GetInstance().currentChapterID, nextChapterName, true, GameManager.Instance.allChapterData[nextChapterName].subchapterList.Count, 1);
                                for (int i = 0; i < GameManager.Instance.allChapterData[nextChapterName].subchapterList.Count; i++)
                                {
                                    string subchapterName = GameManager.Instance.allChapterData[nextChapterName].subchapterList[i].subchapterName;
                                    GetInstance().chapterData.subchapterNameList.Add(subchapterName);
                                }
                                GetInstance().playerData.allChapterUnlocked.Add(nextChapterName, GetInstance().chapterData);
                                string nextSubchapterName = GetInstance().playerData.allChapterUnlocked[nextChapterName].subchapterNameList[GetInstance().currentSubchapterID];
                                string nextKeyName = nextChapterName + "|" + nextSubchapterName;
                                GetInstance().playerData.subchapterUnlocked.Add(nextKeyName, true);
                                GetInstance().SaveData();
                                PanelController.Instance.ActiveDeactivePanel("subchapter", "content");
                            }
                        }
                        PanelController.Instance.ActiveDeactivePanel("subchapter", "content");
                    }

                    else
                    {
                        PanelController.Instance.ActiveDeactivePanel("subchapter", "content");
                    }
                }
               

            }
        }
    }

    private static DataManager GetInstance()
    {
        return DataManager.Instance;
    }
}
