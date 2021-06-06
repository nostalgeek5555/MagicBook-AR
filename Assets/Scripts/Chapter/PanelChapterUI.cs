using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Lean.Pool;
using DG.Tweening;
using System.Linq;

public class PanelChapterUI : MonoBehaviour
{
    public static PanelChapterUI Instance;
    public GameObject thisPanel;
    public Image whiteBG;
    public Image chapterBG;

    public GameObject chapterContainer;
    public Transform chapterNodeParent;
    public GameObject chapterNodePrefab;
    public Button nextButtonToChapter;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        if (PanelController.Instance.panelController == null)
        {
            GameObject panelController = transform.parent.gameObject;
            PanelController.Instance.panelController = panelController;
            PanelController.Instance.RegisterPanel(PanelController.Instance.panelController.transform, "filler");
        }

        nextButtonToChapter.onClick.RemoveAllListeners();
        nextButtonToChapter.onClick.AddListener(() =>
        {
            UIManager.Instance.coverPanel.SetActive(false);
            chapterContainer.SetActive(true);
            StartCoroutine(InitAllChapterNode());
        });
        //StartCoroutine(InitAllChapterNode());
    }


    public IEnumerator InitAllChapterNode()
    {

        DespawnAllChapterNode();

        Sequence sequence = DOTween.Sequence();

        if (GameManager.Instance.allChapterData.Count > 0)
        {
            ChapterNode chapterNode;
            for (int i = 0; i < GameManager.Instance.allChapterList.Count; i++)
            {
                //ChapterSO chapterSO = GameManager.Instance.allChapterData[GameManager.Instance.allChapterList[i].chapterName];
                ChapterSO chapterSO = GameManager.Instance.allChapterList[i];
                chapterNode = LeanPool.Spawn(chapterNodePrefab, chapterNodeParent).GetComponent<ChapterNode>();
                chapterNode.InitChapterNode(chapterSO);

                sequence.Join(chapterNode.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 3, 1));
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    public void DespawnAllChapterNode()
    {
        if (chapterNodeParent.childCount > 0)
        {
            for (int i = chapterNodeParent.childCount - 1; i >= 0; i--)
            {
                LeanPool.Despawn(chapterNodeParent.GetChild(i).gameObject);
            }
        }
    }
}
