using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lean.Pool;
using System.Linq;

public class PanelChapterUI : MonoBehaviour
{
    public static PanelChapterUI Instance;
    public GameObject thisPanel;
    public Image whiteBG;
    public Image chapterBG;
    public Transform chapterNodeParent;
    public GameObject chapterNodePrefab;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (PanelController.Instance.panelController == null)
        {
            GameObject panelController = transform.parent.gameObject;
            PanelController.Instance.panelController = panelController;
        }

        InitAllChapterNode();
    }

    public void InitAllChapterNode()
    {

        DespawnAllChapterNode();

        if (GameManager.Instance.allChapterData.Count > 0)
        {
            ChapterNode chapterNode;
            for (int i = 0; i < GameManager.Instance.allChapterList.Count; i++)
            {
                ChapterSO chapterSO = GameManager.Instance.allChapterData[GameManager.Instance.allChapterList[i].chapterName];
                chapterNode = LeanPool.Spawn(chapterNodePrefab, chapterNodeParent).GetComponent<ChapterNode>();
                chapterNode.InitChapterNode(chapterSO);
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
