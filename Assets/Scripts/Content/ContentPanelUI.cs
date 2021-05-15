using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Pool;
using DG.Tweening;
using UnityEngine.SceneManagement;
public class ContentPanelUI : MonoBehaviour
{
    public static ContentPanelUI Instance;
    public ContentPanelType contentPanelType;

    [Header("Content Data & UI")]
    [SerializeField]
    private string currentSubchapterTitle;
    public TextMeshProUGUI contentTitleText;
    public GameObject contentBG;
    public GameObject contentImagePrefab;
    public GameObject contentTextPrefab;
    public GameObject contentVideoPrefab;
    public GameObject contentSubjectPrefab;
    public GameObject contentQuestionPrefab;
    public GameObject contentARPrefab;
    public Transform contentNodeParent;
    public ScrollRect scrollRect;
    //public GameObject buttonsInContentLayout;
    public List<GameObject> buttonsInContentLayout;
    public Button nextButton;
    public Button confirmButton;
    public GameObject nextButtonBG;
    public GameObject confirmButtonBG;
    [SerializeField] private int previousChapterID = 0;
    [SerializeField] private int nextChapterID = 0;
    [SerializeField] private string nextChapterName;
    [SerializeField] private string nextSubchapterName;
    [SerializeField] private int prevTotalSubchapter = 0;
    [SerializeField] private int nextTotalSubchapter = 0;
    public List<ContentNode> subchapterContentNodes = new List<ContentNode>();

    [Header("New Chapter Popup")]
    public Image blackLayer;
    public GameObject newChapterPopup;
    public Button closePopupButton;
    public Button popupBackButton;
    public Button popupNextButton;
    public Button popupHomeButton;
    public TextMeshProUGUI newChapterNameText;
    public TextMeshProUGUI nextChapterLoadText;
    public TextMeshProUGUI allChapterCompleteText;

    [Header("Complete Evaluation Popup")]
    public GameObject completeEvaluationPopup;
    public TextMeshProUGUI scoreText;
    public Button closeEvaPopupButton;
    public Button popupEvaBackButton;
    public Button popupEvaHomeButton;



    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        contentPanelType = DataManager.Instance.panelType;
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

    public void AddCurrentContentNodes()
    {
        //subchapterContentNodes.Clear();

        for (int i = 0; i < scrollRect.content.childCount; i++)
        {
            if (scrollRect.content.transform.GetChild(i).GetComponent<ContentNode>() != null)
            {
                ContentNode currentContentNode = scrollRect.content.transform.GetChild(i).GetComponent<ContentNode>();
                subchapterContentNodes.Add(currentContentNode);
            }
        }
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

        ActivateDeactivateNavigationButton();

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
                            subchapterContentNodes.Add(contentNode);
                            break;

                        case ContentPartSO.ContentType.Text:
                            contentNode = LeanPool.Spawn(contentTextPrefab, contentNodeParent).GetComponent<ContentNode>();
                            contentNode.InitContentNode(contentPartSO, i);
                            subchapterContentNodes.Add(contentNode);
                            break;

                        case ContentPartSO.ContentType.Video:
                            contentNode = LeanPool.Spawn(contentVideoPrefab, contentNodeParent).GetComponent<ContentNode>();
                            contentNode.InitContentNode(contentPartSO, i);
                            subchapterContentNodes.Add(contentNode);
                            break;

                        case ContentPartSO.ContentType.Question:
                            contentNode = LeanPool.Spawn(contentQuestionPrefab, contentNodeParent).GetComponent<ContentNode>();
                            contentNode.InitContentNode(contentPartSO, i);
                            subchapterContentNodes.Add(contentNode);
                            break;

                        case ContentPartSO.ContentType.AR:
                            contentNode = LeanPool.Spawn(contentARPrefab, contentNodeParent).GetComponent<ContentNode>();
                            contentNode.InitContentNode(contentPartSO, i);
                            subchapterContentNodes.Add(contentNode);
                            break;

                        case ContentPartSO.ContentType.Subject:
                            contentNode = LeanPool.Spawn(contentSubjectPrefab, contentNodeParent).GetComponent<ContentNode>();
                            contentNode.InitContentNode(contentPartSO, i);
                            subchapterContentNodes.Add(contentNode);
                            break;

                        default:
                            break;
                    }
                }

                PanelController.Instance.backButton.interactable = true;

                if (DataManager.Instance.currentChapterType == ChapterSO.ChapterType.Content)
                {
                    nextButton.gameObject.SetActive(true);
                }

                else
                {
                    nextButton.gameObject.SetActive(false);
                }

                foreach (var button in buttonsInContentLayout)
                {
                    button.transform.SetAsLastSibling();
                }
                //buttonsInContentLayout.transform.SetAsLastSibling();
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
                    ContentNode removedContentNode = contentNodeParent.GetChild(i).GetComponent<ContentNode>();
                    LeanPool.Despawn(contentNodeParent.GetChild(i).gameObject);
                    subchapterContentNodes.Remove(removedContentNode);
                }
            }
        }
    }

    public void OnScrolling()
    {
        if (nextButton.gameObject.activeSelf)
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

                        //string _nextChapterName = GameManager.Instance.allChapterList[nextChapterID].chapterName;
                        string _nextChapterName = GameManager.Instance.sortedChapterList[nextChapterID].chapterName;
                        //Debug.Log("next unlocked chapter " + _nextChapterName);
                        int _nextTotalSubchapter = GameManager.Instance.allChapterData[_nextChapterName].subchapterList.Count;
                        //Debug.Log("next total subchapter " + _nextTotalSubchapter);

                        if (!GetInstance().playerData.chapterUnlocked.ContainsKey(_nextChapterName))
                        {
                            if (GetInstance().playerData.totalChapterUnlocked < /*GameManager.Instance.allChapterList.Count*/GameManager.Instance.sortedChapterList.Count)
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

                                currentSubchapterTitle = GameManager.Instance.allSubchapterData[nextKeyName].subchapterTitle;
                                nextChapterName = _nextChapterName;
                                nextSubchapterName = _nextSubchapterName;

                                Debug.Log("next chapter name " + nextChapterName);
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

            //popup home button
            popupHomeButton.onClick.RemoveAllListeners();
            popupHomeButton.onClick.AddListener(() =>
            {
                blackLayer.gameObject.SetActive(false);
                newChapterPopup.SetActive(false);
                PanelController.Instance.backButton.gameObject.SetActive(true);
                DespawnAllContents();
                SceneManager.LoadScene(0);
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

    public void ActivateDeactivateNavigationButton()
    {
        switch (contentPanelType)
        {
            case ContentPanelType.Subject:
                nextButtonBG.SetActive(true);
                confirmButtonBG.SetActive(false);
                break;

            case ContentPanelType.Evaluation:
                nextButtonBG.SetActive(false);
                confirmButtonBG.SetActive(true);
                break;

            default:
                break;
        }
    }


    public void OpenCompleteEvaluationPopup(int playerScore)
    {
        PanelController.Instance.backButton.gameObject.SetActive(false);
        blackLayer.gameObject.SetActive(true);
        completeEvaluationPopup.SetActive(true);
        scoreText.text = playerScore.ToString();

        CanvasGroup canvasGroup = completeEvaluationPopup.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(completeEvaluationPopup.transform.DOPunchScale(Vector3.one * 0.25f, 0.3f, 4, 1));
        sequence.Join(canvasGroup.DOFade(1, 0.2f));
        sequence.AppendCallback(() =>
        {
            closeEvaPopupButton.interactable = true;
            popupEvaBackButton.interactable = true;
            popupEvaHomeButton.interactable = true;

            //close popup
            closeEvaPopupButton.onClick.RemoveAllListeners();
            closeEvaPopupButton.onClick.AddListener(() =>
            {
                GetInstance().currentChapterID = previousChapterID;
                GetInstance().currentTotalSubchapter = prevTotalSubchapter;
                blackLayer.gameObject.SetActive(false);
                completeEvaluationPopup.SetActive(false);
                PanelController.Instance.backButton.gameObject.SetActive(true);
            });

            //popup back button 
            popupEvaBackButton.onClick.RemoveAllListeners();
            popupEvaBackButton.onClick.AddListener(() =>
            {
                blackLayer.gameObject.SetActive(false);
                completeEvaluationPopup.SetActive(false);
                PanelController.Instance.backButton.gameObject.SetActive(true);
                //DespawnAllContents();
                PanelController.Instance.ActiveDeactivePanel("subchapter", "content");
            });

            //popup home button
            popupEvaHomeButton.onClick.RemoveAllListeners();
            popupEvaHomeButton.onClick.AddListener(() =>
            {
                //GetInstance().currentSubchapterID = 0;
                //GetInstance().currentChapterID = nextChapterID;
                //GetInstance().currentChapterName = nextChapterName;
                //GetInstance().currentSubchapterName = nextSubchapterName;
                //GetInstance().currentTotalSubchapter = nextTotalSubchapter;
                blackLayer.gameObject.SetActive(false);
                completeEvaluationPopup.SetActive(false);
                SceneManager.LoadScene(0);

                Debug.Log("load next chapter " + GetInstance().currentChapterName);
                Debug.Log("load next subchapter " + GetInstance().currentSubchapterName);
                //InitAllContents();
            });

        });
    }

    public void OnClickConfirmButton()
    {
        if (scrollRect.content.childCount > 0)
        {
            int totalQuestionAnswered = 0;
            int totalPlayerScore = 0;

            //for (int i = 0; i < scrollRect.content.childCount; i++)
            //{
            //    ContentNode contentNode = scrollRect.content.transform.GetChild(i).GetComponent<ContentNode>();
            //    if (contentNode.contentType == ContentPartSO.ContentType.Question)
            //    {
            //        if (contentNode.questionAnswered)
            //        {
            //            totalPlayerScore += contentNode.thisQuestionScore;
            //            totalQuestionAnswered++;
            //            continue;
            //        }

            //        else
            //        {
            //            continue;
            //        }
            //    }
            //}

            for (int i = 0; i < subchapterContentNodes.Count; i++)
            {
                ContentNode currentContentNode = subchapterContentNodes[i];
                if (currentContentNode.contentType == ContentPartSO.ContentType.Question)
                {
                    if (currentContentNode.questionAnswered)
                    {
                        totalPlayerScore += currentContentNode.thisQuestionScore;
                        totalQuestionAnswered++;
                        continue;
                    }

                    else
                    {
                        continue;
                    }
                }
            }

            if (totalQuestionAnswered == subchapterContentNodes.Count)
            {
                Debug.Log("complete with score " + totalPlayerScore);
                OpenCompleteEvaluationPopup(totalPlayerScore);
            }
        }
    }

    private static DataManager GetInstance()
    {
        return DataManager.Instance;
    }

    public enum ContentPanelType
    {
        Subject, Evaluation
    }
}
