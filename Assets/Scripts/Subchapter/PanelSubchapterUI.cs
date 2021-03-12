using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using System.Linq;
using DG.Tweening;

public class PanelSubchapterUI : MonoBehaviour
{
    public static PanelSubchapterUI Instance;
    public string currentChapterName { get; set; }
    public Transform subchapterNodeParent;
    public GameObject subchapterNodePrefab;

    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        StartCoroutine(InitAllSubchapterNode());
    }

    public IEnumerator InitAllSubchapterNode()
    {
        //StartCoroutine(CheckChapterChildCount());

        DespawnAllSubchapterNode();

        if (GameManager.Instance.allSubchapterData.Count > 0)
        {
            Sequence sequence = DOTween.Sequence();
            SubchapterNode subchapterNode;
            for (int i = 0; i < GameManager.Instance.allSubchapterData.Count; i++)
            {
                string currentSubchapterKey = GameManager.Instance.allSubchapterData.Keys.ElementAt(i);
                string chapterKey = currentSubchapterKey.Substring(0, currentSubchapterKey.IndexOf("|"));
                Debug.Log(currentSubchapterKey);
                Debug.Log(chapterKey);
                if (chapterKey == DataManager.Instance.currentChapterName)
                {
                    SubchapterSO subchapterSO = GameManager.Instance.allSubchapterData[currentSubchapterKey];
                    subchapterNode = LeanPool.Spawn(subchapterNodePrefab, subchapterNodeParent).GetComponent<SubchapterNode>();
                    subchapterNode.InitSubchapter(subchapterSO);

                    sequence.Join(subchapterNode.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f, 3, 1));
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    public void DespawnAllSubchapterNode()
    {
        if (subchapterNodeParent.childCount > 0)
        {
            for (int i = subchapterNodeParent.childCount - 1; i >= 0; i--)
            {
                LeanPool.Despawn(subchapterNodeParent.GetChild(i).gameObject);
            }
        }
    }

}
