using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using System.Linq;

public class ContentPanelUI : MonoBehaviour
{
    public GameObject contentImagePrefab;
    public GameObject contentTextPrefab;
    public GameObject contentVideoPrefab;
    public Transform contentNodeParent;

    private void OnEnable()
    {
        InitAllContents();
    }

    
    public void InitAllContents()
    {
        DespawnAllContents();

        if (GameManager.Instance.allSubchapterData.Count > 0)
        {
            if (UIManager.Instance.currentSubchapterName != "")
            {
                Debug.Log("current subchapter name " + UIManager.Instance.currentSubchapterName);
                SubchapterSO subchapterSO = GameManager.Instance.allSubchapterData[UIManager.Instance.currentChapterName + "|" + UIManager.Instance.currentSubchapterName];
                ContentNode contentNode;
                for (int i = 0; i < subchapterSO.subchapterContents.Count; i++)
                {
                    ContentPartSO contentPartSO = subchapterSO.subchapterContents[i];

                    switch (contentPartSO.contentType)
                    {
                        case ContentPartSO.ContentType.Image:
                            contentNode = LeanPool.Spawn(contentImagePrefab, contentNodeParent).GetComponent<ContentNode>();
                            contentNode.InitContentNode(contentPartSO, i);
                            Debug.Log("spawn image " + contentPartSO.contentType);
                            break;
                        case ContentPartSO.ContentType.Text:
                            contentNode = LeanPool.Spawn(contentTextPrefab, contentNodeParent).GetComponent<ContentNode>();
                            contentNode.InitContentNode(contentPartSO, i);
                            Debug.Log("spawn text " + contentPartSO.contentType);
                            break;
                        case ContentPartSO.ContentType.Video:
                            contentNode = LeanPool.Spawn(contentVideoPrefab, contentNodeParent).GetComponent<ContentNode>();
                            contentNode.InitContentNode(contentPartSO, i);
                            Debug.Log("spawn video " + contentPartSO.contentType);
                            break;
                        case ContentPartSO.ContentType.Question:
                            break;
                        case ContentPartSO.ContentType.AR:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public void DespawnAllContents()
    {
        if (contentNodeParent.childCount > 0)
        {
            for (int i = contentNodeParent.childCount - 1; i >= 0; i--)
            {
                LeanPool.Despawn(contentNodeParent.GetChild(i).gameObject);
            }
        }
    }
}
