using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Vuforia;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    public Camera mainCamera;
    public Camera arCamera;
    public GameObject groundPlaneStage;
    public Canvas arOverlayCanvas;
    public Canvas masterCanvas;
    public GameObject instructionUI;
    public List<GameObject> allARContentList;
    public Dictionary<string, GameObject> allARContentGO;
    public string activeARContentID;
    
    
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        activeARContentID = DataManager.Instance.activeARContentID;
        UIManager.Instance.onArDisplayed = true;
        Debug.Log("current active ar id" + activeARContentID);
        AddAllContentARListToDictionary();
        SetActiveARContent();
    }


    public void AddAllContentARListToDictionary()
    {
        allARContentGO = new Dictionary<string, GameObject>();
        if (allARContentList.Count > 0)
        {
            for (int i = 0; i < allARContentList.Count; i++)
            {
                GameObject arContentGO = allARContentList[i];
                allARContentGO.Add(arContentGO.name, arContentGO);
            }
        }
    }

   //public void OnActiveARContent(bool mainCamActive)
   //{
   //     if (!mainCamActive)
   //     {
   //         PanelController.Instance.onArDisplayed = true;
   //         EnableDisableARCamera(true);
   //         masterCanvas.gameObject.SetActive(false);
   //         arOverlayCanvas.gameObject.SetActive(true);
            
   //         allARContentGO[activeARContentID].SetActive(true);

   //         for (int i = 0; i < allARContentList.Count; i++)
   //         {
   //             if (allARContentList[i].name != activeARContentID)
   //             {
   //                 allARContentList[i].SetActive(false);
   //             }
   //         }
   //     }

   //     else
   //     {
   //         PanelController.Instance.onArDisplayed = false;
   //         EnableDisableARCamera(false);
   //         arOverlayCanvas.gameObject.SetActive(false);
   //         allARContentGO[activeARContentID].SetActive(false);
   //         masterCanvas.gameObject.SetActive(true);
   //         //UIManager.Instance.generalCanvas.gameObject.SetActive(true);
   //     }
   //}

    public void SetActiveARContent()
    {
        
        allARContentGO[activeARContentID].SetActive(true);

        for (int i = 0; i < allARContentList.Count; i++)
        {
            if (allARContentList[i].name != activeARContentID)
            {
                allARContentList[i].SetActive(false);
            }
        }
    }

    public void EnableDisableARCamera(bool enable)
    {
        if (enable)
        {
            CameraDevice.Instance.Init();
            CameraDevice.Instance.Start();
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
        }

        else
        {
            CameraDevice.Instance.Deinit();
            CameraDevice.Instance.Stop();
            Debug.Log("ar camera status disabled");
        }
    }
    public void OnTrackableFound()
    {
        instructionUI.gameObject.SetActive(false);
    }

    public void OnTrackableLost()
    {
        instructionUI.gameObject.SetActive(true);
    }

    
   
}
