using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Lean.Pool;
public class ContentPanelUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [ReadOnly]
    [SerializeField]
    private string currentSubchapterTitle;

    public DataManager.SubchapterData subchapterData;
    public TextMeshProUGUI contentTitleText;
    public GameObject contentBG;
    public GameObject contentImagePrefab;
    public GameObject contentTextPrefab;
    public GameObject contentVideoPrefab;
    public Transform contentNodeParent;
    public ScrollRect scrollRect;
    public Button nextButton;

    private void OnEnable()
    {
        if (UIManager.Instance != null)
        {
            currentSubchapterTitle = DataManager.Instance.currentSubchapterTitle;
            contentTitleText.text = currentSubchapterTitle;
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

        if (GameManager.Instance.allSubchapterData.Count > 0)
        {
            if (DataManager.Instance.currentSubchapterName != "")
            {
                SubchapterSO subchapterSO = GameManager.Instance.allSubchapterData[DataManager.Instance.currentChapterName + "|" + DataManager.Instance.currentSubchapterName];
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
                nextButton.transform.SetAsLastSibling();
                Debug.Log("next button index " + nextButton.transform.GetSiblingIndex());
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

    public void OnDrag(PointerEventData eventData)
    {
        ((IDragHandler)scrollRect).OnDrag(eventData);

        if (scrollRect.verticalNormalizedPosition <= 0 && scrollRect.verticalNormalizedPosition >= 0.8)
        {
            nextButton.interactable = true;
            Debug.Log("current button next interactable " + nextButton.interactable);
        }

        else
        {
            nextButton.interactable = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ((IBeginDragHandler)scrollRect).OnBeginDrag(eventData);

        if (scrollRect.verticalNormalizedPosition <= 0 && scrollRect.verticalNormalizedPosition <= 0.8)
        {
            nextButton.interactable = true;
            Debug.Log("current button next interactable " + nextButton.interactable);
        }

        else
        {
            nextButton.interactable = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ((IEndDragHandler)scrollRect).OnEndDrag(eventData);

        if (scrollRect.verticalNormalizedPosition <= 0 && scrollRect.verticalNormalizedPosition <= 0.8)
        {
            nextButton.interactable = true;
            Debug.Log("current button next interactable " + nextButton.interactable);
        }

        
    }

    public void UnlockChapterSubchapter()
    {
        Debug.Log("chapter name " + GetInstance().currentChapterName);
        //if (GetInstance() != null)
        //{
        //    if (GetInstance().playerData != null)
        //    {
        //        if (GetInstance().playerData.allChapterUnlocked[GetInstance().currentChapterName].totalSubchapterUnlocked < GetInstance().currentTotalSubchapter - 1)
        //        {
        //            string keyName = GetInstance().currentChapterName + "|" + GetInstance().currentSubchapterName;
        //            if (!GetInstance().playerData.subchapterUnlocked.ContainsKey(keyName))
        //            {
        //                GetInstance().playerData.subchapterUnlocked.Add(keyName, true);
        //                GetInstance().playerData.allChapterUnlocked[GetInstance().currentChapterName].totalSubchapterUnlocked++;
        //                GetInstance().SaveData();
        //                GetInstance().currentSubchapterName = GetInstance().playerData.allChapterUnlocked[GetInstance().currentChapterName].subchapterNameList[GetInstance().playerData.allChapterUnlocked[GetInstance().currentChapterName].totalSubchapterUnlocked];
        //                scrollRect.verticalNormalizedPosition = 0;
        //                InitAllContents();
        //                Debug.Log("unlock next subchapter " + GetInstance().playerData.subchapterUnlocked);
        //            }
        //        }

        //        else
        //        {
        //            if (DataManager.Instance.currentChapterID < GameManager.Instance.allChapterList.Count - 1)
        //            {
        //                string nextChapterName = GameManager.Instance.allcurrentChapter[DataManager.Instance.currentChapterID++];
        //                DataManager.Instance.playerData.chapterUnlocked.Add(nextChapterName, true);
        //                DataManager.ChapterData chapterData = new DataManager.ChapterData(DataManager.Instance.currentChapterID, nextChapterName, true, GameManager.Instance.allChapterData[nextChapterName].subchapterList.Count, 1);
        //                DataManager.Instance.playerData.allChapterUnlocked.Add(nextChapterName, chapterData);

        //                PanelController.Instance.ActiveDeactivePanel("chapter", "subchapter");
        //                DataManager.Instance.SaveData();

        //                Debug.Log("next chapter unlocked " + nextChapterName);
        //            }
        //        }
        //    }
        //}
    }

    private static DataManager GetInstance()
    {
        return DataManager.Instance;
    }
}
