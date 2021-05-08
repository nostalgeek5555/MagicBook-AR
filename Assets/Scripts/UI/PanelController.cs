using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class PanelController : MonoBehaviour
{
    public static PanelController Instance;
    public GameObject panelController;
    public GameObject panelFiller;

    [Header("Page Navigation")]
    public Scene currentScene;
    public Button backButton;

    [SerializeField]
    private string currentChapterName;
    private string currentSubchapterName;
    public string chapterName { get => currentChapterName; set => currentChapterName = value; }
    public string subchapterName { get => currentSubchapterName; set => currentSubchapterName = value; }

    [Header("On AR content display")]
    public bool onArDisplayed = false;

    //[Header("")]
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Refresh();
    }

    public void ActiveDeactivePanel(string activePanel, string deactivePanel)
    {
        if (panelController.transform.childCount > 0)
        {
            for (int i = 0; i < panelController.transform.childCount; i++)
            {
                if (panelController.transform.GetChild(i).tag == activePanel)
                {
                    panelController.transform.GetChild(i).gameObject.SetActive(true);
                }

                if (panelController.transform.GetChild(i).tag == deactivePanel)
                {
                    panelController.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    public void ActivateDeactivateSinglePanel(Transform panelParent, string panelName, bool active)
    {
        if (panelParent.childCount > 0)
        {
            for (int i = 0; i < panelParent.childCount; i++)
            {
                if (panelParent.GetChild(i).tag == panelName)
                {
                    panelParent.GetChild(i).gameObject.SetActive(active);
                    Debug.Log("opened panel " + panelController.transform.GetChild(i).gameObject.tag);
                    break;
                }
            }
        }
    }

    public void RegisterPanel(Transform panelParent, string panelName)
    {
        if (panelParent.childCount > 0)
        {
            for (int i = 0; i < panelParent.childCount; i++)
            {
                if (panelController.transform.GetChild(i).tag == panelName)
                {
                    panelFiller = panelController.transform.GetChild(i).gameObject;
                    break;
                }
            }
        }
    }

    public void OnBack()
    {
        if (!UIManager.Instance.onArDisplayed)
        {
            if (UIManager.Instance.chapterPanel.activeInHierarchy == true)
            {
                //Debug.Log("back to main menu");
                SceneManager.LoadScene(0);
                Refresh();
            }

            else if (UIManager.Instance.subchapterPanel.activeInHierarchy == true)
            {
                //Debug.Log("back to chapter panel");
                UIManager.Instance.subchapterPanel.SetActive(false);
                UIManager.Instance.chapterPanel.SetActive(true);
                Refresh();
            }

            else if (UIManager.Instance.contentPanel.activeInHierarchy == true)
            {
                //Debug.Log("back to chapter panel");
                UIManager.Instance.contentPanel.SetActive(false);
                UIManager.Instance.subchapterPanel.SetActive(true);
                Refresh();
            }

            else if (UIManager.Instance.fillerPanel.activeInHierarchy == true)
            {
                UIManager.Instance.fillerPanel.SetActive(false);
                UIManager.Instance.chapterPanel.SetActive(true);
                Refresh();
            }
        }

        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void Refresh()
    {
        currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex == 0)
        {
            backButton.gameObject.SetActive(false);
        }

        else if (currentScene.buildIndex != 0)
        {
            backButton.gameObject.SetActive(true);
            UIManager.Instance.onArDisplayed = false;

        }
    }
}
