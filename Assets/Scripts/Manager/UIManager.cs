using UnityEngine.SceneManagement;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Canvas generalCanvas;
    public Scene currentScene;

    [Header("All panel")]
    public GameObject chapterPanel;
    public GameObject subchapterPanel;
    public GameObject contentPanel;

    [Header("AR Content Displayed")]
    public bool onArDisplayed = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else 
        {
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Refresh();
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene(1);
    }

    public void OnExitApp()
    {
        Application.Quit();
    }

    public void Refresh()
    {
        currentScene = SceneManager.GetActiveScene();

        if (currentScene.buildIndex == 1)
        {
            if (chapterPanel == null && subchapterPanel == null)
            {
                chapterPanel = GameObject.FindGameObjectWithTag("chapter");
                subchapterPanel = GameObject.FindGameObjectWithTag("subchapter");
                contentPanel = GameObject.FindGameObjectWithTag("content");
            }
        }
        
        if (subchapterPanel != null)
        {
            subchapterPanel.SetActive(false);
        }

        if (contentPanel != null)
        {
            contentPanel.SetActive(false);
        }
        
    }


}
    