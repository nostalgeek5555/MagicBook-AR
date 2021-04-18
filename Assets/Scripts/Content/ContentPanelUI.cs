using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Pool;
using DG.Tweening;
public class ContentPanelUI : MonoBehaviour
{
    [Header("Content Data & UI")]
    [SerializeField]
    private string currentSubchapterTitle;
    public TextMeshProUGUI contentTitleText;
    public GameObject contentBG;
    public GameObject contentImagePrefab;
    public GameObject contentTextPrefab;
    public GameObject contentVideoPrefab;
    public GameObject contentSubjectPrefab;
    public Transform contentNodeParent;
    public ScrollRect scrollRect;
    public GameObject buttonsInContentLayout;
    public Button nextButton;
    [SerializeField] private int previousChapterID = 0;
    [SerializeField] private int nextChapterID = 0;
    [SerializeField] private string nextChapterName;
    [SerializeField] private string nextSubchapterName;
    [SerializeField] private int prevTotalSubchapter = 0;
    [SerializeField] private int nextTotalSubchapter = 0;

    [Header("New Chapter Popup")]
    public Image blackLayer;
    public GameObject newChapterPopup;
    public Button closePopupButton;
    public Button popupBackButton;
    public Button popupNextButton;
    public TextMeshProUGUI newChapterNameText;
    public TextMeshProUGUI nextChapterLoadText;
    public TextMeshProUGUI allChapterCompleteText;

    private void OnEnable()
    {
        nextChapterName = "";
        nextSubchapterName = "";
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

        PanelController.Instance.backButton.gameObject.SetActive(true);
        //PanelController.Instance.backButton.interactable = false;
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

                        case ContentPartSO.ContentType.Subject:
                            contentNode = LeanPool.Spawn(contentSubjectPrefab, contentNodeParent).GetComponent<ContentNode>();
                            contentNode.InitContentNode(contentPartSO, i);
                            break;
                        default:
                            break;
                    }
                }

                PanelController.Instance.backButton.interactable = true;
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
        if (scrollRect.verticalNormalizedPosition <= 0.3)
        {
            nextButton.interactable = true;
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
                    Debug.Log("init all content");
                    InitAllContents();
                }

                else
                {
                    if (GetInstance().currentChapterID < GetInstance().playerData.totalChapter - 1)
                    {
                        previousChapterID = GetInstance().currentChapterID;
                        nextChapterID = GetInstance().currentChapterID + 1;
                        //Debug.Log("current chapter id " + GetInstance().currentChapterID);
                        //Debug.Log("next chapter id " + nextChapterID);
                        //GetInstance().currentChapterID++;

                        string _nextChapterName = GameManager.Instance.allChapterList[nextChapterID].chapterName;
                        //Debug.Log("next unlocked chapter " + _nextChapterName);
                        int _nextTotalSubchapter = GameManager.Instance.allChapterData[_nextChapterName].subchapterList.Count;
                        //Debug.Log("next total subchapter " + _nextTotalSubchapter);

                        if (!GetInstance().playerData.chapterUnlocked.ContainsKey(_nextChapterName))
                        {
                            if (GetInstance().playerData.totalChapterUnlocked < GameManager.Instance.allChapterList.Count)
                            {
                                prevTotalSubchapter = GetInstance().currentTotalSubchapter;
                                nextTotalSubchapter = _nextTotalSubchapter;
                                
                                GetInstance().playerData.totalChapterUnlocked++;
                                GetInstance().playerData.chapterUnlocked.Add(_nextChapterName, true);
                                GetInstance().chapterData = new DataManager.ChapterData(nextChapterID, _nextChapterName, true, GameManager.Instance.allChapterData[_nextChapterName].subchapterList.Count, 1);
                                for (int i = 0; i < GameManager.Instance.allChapterData[_nextChapterName].subchapterList.Count; i++)
                                {
                                    string subchapterName = GameManager.Instance.allChapterData[_nextChapterName].subchapterList[i].subchapterName;
                                    GetInstance().chapterData.subchapterNameList.Add(subchapterName);
                                }
                                GetInstance().playerData.allChapterUnlocked.Add(_nextChapterName, GetInstance().chapterData);
                                string _nextSubchapterName = GetInstance().playerData.allChapterUnlocked[_nextChapterName].subchapterNameList[0];
                                string nextKeyName = _nextChapterName + "|" + _nextSubchapterName;
                                GetInstance().playerData.subchapterUnlocked.Add(nextKeyName, true);
                                GetInstance().SaveData();
                                
                                nextChapterName = _nextChapterName;
                                nextSubchapterName = _nextSubchapterName;

                                //Debug.Log("next chapter name " + nextChapterName);
                                //Debug.Log("next subchapter name " + nextSubchapterName);
                                OpenNewChapterPopup();

                                //PanelController.Instance.ActiveDeactivePanel("subchapter", "content");
                            }
                        }

                        else
                        {
                            GetInstance().currentChapterID = previousChapterID;
                            PanelController.Instance.ActiveDeactivePanel("subchapter", "content");
                        }
                        
                    }

                    else
                    {
                        GetInstance().currentChapterID = previousChapterID;
                        PanelController.Instance.ActiveDeactivePanel("subchapter", "content");
                    }
                }
               

            }
        }
    }

    public void OpenNewChapterPopup()
    {
        PanelController.Instance.backButton.gameObject.SetActive(false);
        blackLayer.gameObject.SetActive(true);
        newChapterPopup.SetActive(true);
        closePopupButton.interactable = false;
        popupBackButton.interactable = false;
        popupNextButton.interactable = false;

        if (GetInstance().playerData.chapterUnlocked.Count == GameManager.Instance.allChapterData.Count)
        {
            nextChapterLoadText.gameObject.SetActive(false);
            allChapterCompleteText.gameObject.SetActive(true);
        }

        else
        {
            newChapterNameText.text = nextChapterName;
            nextChapterLoadText.gameObject.SetActive(true);
            allChapterCompleteText.gameObject.SetActive(false);
        }

        CanvasGroup canvasGroup = newChapterPopup.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(newChapterPopup.transform.DOPunchScale(Vector3.one * 0.25f, 0.3f, 4, 1));
        sequence.Join(canvasGroup.DOFade(1, 0.2f));
        sequence.AppendCallback(() =>
        {
            closePopupButton.interactable = true;
            popupBackButton.interactable = true;
            popupNextButton.interactable = true;

            //close popup
            closePopupButton.onClick.RemoveAllListeners();
            closePopupButton.onClick.AddListener(() =>
            {
                GetInstance().currentChapterID = previousChapterID;
                GetInstance().currentTotalSubchapter = prevTotalSubchapter;
                blackLayer.gameObject.SetActive(false);
                newChapterPopup.SetActive(false);
                PanelController.Instance.backButton.gameObject.SetActive(true);
            });

            //popup back button 
            popupBackButton.onClick.RemoveAllListeners();
            popupBackButton.onClick.AddListener(() =>
            {
                blackLayer.gameObject.SetActive(false);
                newChapterPopup.SetActive(false);
                PanelController.Instance.backButton.gameObject.SetActive(true);
                DespawnAllContents();
                PanelController.Instance.ActiveDeactivePanel("subchapter", "content");
            });

            //popup next button
            popupNextButton.onClick.RemoveAllListeners();
            popupNextButton.onClick.AddListener(() =>
            {
                GetInstance().currentSubchapterID = 0;
                GetInstance().currentChapterID = nextChapterID;
                GetInstance().currentChapterName = nextChapterName;
                GetInstance().currentSubchapterName = nextSubchapterName;
                GetInstance().currentTotalSubchapter = nextTotalSubchapter;
                blackLayer.gameObject.SetActive(false);
                newChapterPopup.SetActive(false);

                Debug.Log("load next chapter " + GetInstance().currentChapterName);
                Debug.Log("load next subchapter " + GetInstance().currentSubchapterName);
                InitAllContents();
            });

        });
    }

    private static DataManager GetInstance()
    {
        return DataManager.Instance;
    }
}
