using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using DG.Tweening;

public class ContentNode : MonoBehaviour
{
    public ContentPartSO contentPartSO;
    public Image contentImage;
    public RawImage contentVideoImage;
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI videoStatusText;
    public VideoPlayer videoPlayer;
    public Button videoButton;
    

    [SerializeField]
    private string fileName;

    [SerializeField]
    private string URLvideo;
    

    public void InitContentNode(ContentPartSO _contentPartSO, int _contentOrder)
    {
        transform.SetSiblingIndex(_contentOrder);
        contentPartSO = _contentPartSO;
        fileName = _contentPartSO.contentName;

        if (contentImage != null)
        {
            contentImage.gameObject.SetActive(false);
        }

        if (contentText != null)
        {
            contentText.gameObject.SetActive(false);
        }

        switch (contentPartSO.contentType)
        {
            case ContentPartSO.ContentType.Image:
                contentImage.gameObject.SetActive(true);
                contentImage.sprite = contentPartSO.contentImage;
                break;
            
            case ContentPartSO.ContentType.Text:
                contentText.gameObject.SetActive(true);
                contentText.text = contentPartSO.contentText;
                break;
            
            case ContentPartSO.ContentType.Video:
                Debug.Log("data path " + Application.persistentDataPath);
                StartCoroutine(RequestVideo(URLvideo, PendingVideoOnReady));
                break;
            
            case ContentPartSO.ContentType.AR:
                break;

            default:
                break;
        }
    }

    private IEnumerator RequestVideo(string URL, Action<bool> OnVideoReady)
    {
        videoPlayer.source = VideoSource.VideoClip;
        videoPlayer.clip = DataManager.Instance.allVideoClipData[fileName];

        CustomRenderTexture customRenderTexture = new CustomRenderTexture(Convert.ToInt32(videoPlayer.width), Convert.ToInt32(videoPlayer.height));
        customRenderTexture.Create();

        contentVideoImage.texture = customRenderTexture;
        videoPlayer.targetTexture = customRenderTexture;
        videoPlayer.playOnAwake = false;
        videoPlayer.Prepare();

        while (videoPlayer.isPrepared == false)
        {
            yield return null;
        }

        OnVideoReady(videoPlayer.isPrepared);

        //videoPlayer.source = VideoSource.Url;
        //URL = RequestURL(DataManager.Instance.folderName, fileName);
        //Debug.Log("URL = " + URL);
        //videoPlayer.url = URL;

    }


    private void PendingVideoOnReady(bool isVideoReady)
    {
        videoButton.gameObject.SetActive(true);
        videoStatusText.gameObject.SetActive(false);

        if (isVideoReady)
        {
            //on click video when video is playing/paused
            videoButton.onClick.RemoveAllListeners();
            videoButton.onClick.AddListener(() =>
            {
                if (videoPlayer.isPaused || !videoPlayer.isPlaying)
                {
                    SetVideoStatus(VideoStatus.Play);
                    videoPlayer.Play();
                }

                else
                {
                    SetVideoStatus(VideoStatus.Pause);
                    videoPlayer.Pause();
                }

            });
        }
    }

    //private string RequestURL(string folderName, string fileName)
    //{
    //    string path = folderName;
    //    Debug.Log("combined path " + Path.Combine(Application.persistentDataPath, folderName, fileName));
    //    string newDirPath;

    //    Debug.Log("path " + Path.Combine(Application.persistentDataPath, path));
    //    if (Directory.Exists(Path.Combine(Application.persistentDataPath, path)))
    //    {
    //        newDirPath = Path.Combine(Application.persistentDataPath, path);
    //        Debug.Log("path combined " + Path.Combine(Application.persistentDataPath, path));
    //        return newDirPath;
    //    }

    //    else 
    //    {
    //        newDirPath = Path.Combine(Application.persistentDataPath, path);
    //        Debug.Log("path combined " + Path.Combine(Application.persistentDataPath, path));
    //        Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, path));
    //        return newDirPath;
    //    }
        
    //}

    
    private void SetVideoStatus(VideoStatus videoStatus)
    {
        videoStatusText.gameObject.SetActive(true);
        Debug.Log("status text " + videoStatusText.gameObject.activeSelf);
        videoStatusText.text = videoStatus.ToString();

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() =>
        {
            videoStatusText.gameObject.SetActive(false);
        });
    }

    public enum VideoStatus
    {
        Play, Pause, Stop
    }
}
