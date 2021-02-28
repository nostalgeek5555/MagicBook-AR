using UnityEngine;
using Lean.Pool;

public class ContentPanelUI : MonoBehaviour
{
    public GameObject contentBG;
    public GameObject contentImagePrefab;
    public GameObject contentTextPrefab;
    public GameObject contentVideoPrefab;
    public Transform contentNodeParent;

    private void OnEnable()
    {
        contentBG.SetActive(true);
        InitAllContents();
    }

    private void OnDisable()
    {
        contentBG.SetActive(false);
    }


    public void InitAllContents()
    {
        DespawnAllContents();

        if (GameManager.Instance.allSubchapterData.Count > 0)
        {
            if (UIManager.Instance.currentSubchapterName != "")
            {
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
