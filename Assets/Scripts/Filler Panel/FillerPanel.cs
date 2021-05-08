using UnityEngine;

public class FillerPanel : MonoBehaviour
{
    public static FillerPanel Instance;


    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnDisable()
    {
        PanelController.Instance.ActivateDeactivateSinglePanel(transform, DataManager.Instance._chapterFillerType.ToString(), false);
    }

    public void ActivateChildrenPanel()
    {
        PanelController.Instance.ActivateDeactivateSinglePanel(transform, DataManager.Instance._chapterFillerType.ToString(), true);
    }
}
