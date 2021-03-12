using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using DG.Tweening;

public class ContentNode : MonoBehaviour
{
    [Header("Main Content Identifier")]
    public string contentID;
    public ContentPartSO.ContentType contentType;
    
    [Header("Text Content Type")]
    public TextMeshProUGUI contentText;

    [Header("Image Content Type")]
    public Image contentImage;
    public TextMeshProUGUI imageWatermarkText;

    [Header("Video Content Type")]
    public RawImage contentVideoImage;
    public TextMeshProUGUI videoStatusText;
    public VideoPlayer videoPlayer;
    public Image thumbnailImageLayer;
    public Button videoButton;
    public Animator videoButtonPanelAnimator;
    
    [SerializeField]
    private string fileName;

    [SerializeField]
    private string URLvideo;


    public void InitContentNode(ContentPartSO _contentPartSO, int _contentOrder)
    {
        transform.SetSiblingIndex(_contentOrder);
        contentID = _contentPartSO.name;
        contentType = _contentPartSO.contentType;
        fileName = _contentPartSO.videoName;

        if (contentImage != null)
        {
            contentImage.gameObject.SetActive(false);
        }

        if (contentText != null)
        {
            contentText.gameObject.SetActive(false);
        }

        switch (contentType)
        {
            case ContentPartSO.ContentType.Image:
                contentImage.gameObject.SetActive(true);
                contentImage.sprite = _contentPartSO.contentImage;
                imageWatermarkText.text = _contentPartSO.imageWatermarkText;
                //float imagewidth = contentImage.rectTransform.sizeDelta.x;
                //contentImage.rectTransform.sizeDelta = new Vector2(imagewidth, 400);


                //contentImage.SetNativeSize();
                break;
            
            case ContentPartSO.ContentType.Text:
                contentText.gameObject.SetActive(true);
                contentText.text = _contentPartSO.contentText;
                contentText.autoSizeTextContainer = true;
                contentText.alignment = _contentPartSO.alignmentOptions;
                break;
            
            case ContentPartSO.ContentType.Video:
                StartCoroutine(OnTouchVideoDisplay(5, ButtonPanelAnim.Show.ToString(), ButtonPanelAnim.Hide.ToString(), DoNext));
                StartCoroutine(RequestVideo(URLvideo, PendingVideoOnReady));
                break;
            
            case ContentPartSO.ContentType.AR:
                break;

            default:
                break;
        }
    }

    #region Video Content Type

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
            //create video thumbnail based on video played
            StartCoroutine(CreateThumbnail(0.5f, 0.1f));
            contentVideoImage.gameObject.SetActive(false);
            contentImage.gameObject.SetActive(true);
            thumbnailImageLayer.gameObject.SetActive(true);

            //on click video when video is playing/paused
            videoButton.onClick.RemoveAllListeners();
            videoButton.onClick.AddListener(() =>
            {
                if (videoPlayer.isPaused || !videoPlayer.isPlaying)
                {
                    SetVideoStatus(VideoStatus.Play);
                }

                else if (videoPlayer.isPlaying)
                {
                    SetVideoStatus(VideoStatus.Pause);
                }

            });
        }
    }

    private IEnumerator CreateThumbnail(float thumbnailFrameTime, float waitTime)
    {
        int width = videoPlayer.texture.width;
        int height = videoPlayer.texture.height;
        videoPlayer.time = thumbnailFrameTime;
        videoPlayer.SetDirectAudioMute(0, true);
        videoPlayer.Play();
        RenderTexture renderTexture = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        yield return new WaitForSeconds(waitTime);
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        RenderTexture.active = videoPlayer.targetTexture;
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();
        contentImage.sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f));
        videoPlayer.Stop();
        videoPlayer.SetDirectAudioMute(0, false);
        RenderTexture.active = null;
        //Debug.Log("create thumbnail");
    }
    
    private void SetVideoStatus(VideoStatus videoStatus)
    {
        thumbnailImageLayer.gameObject.SetActive(false);
        contentImage.gameObject.SetActive(false);
        contentVideoImage.gameObject.SetActive(true);
        videoStatusText.gameObject.SetActive(true);
        videoStatusText.text = videoStatus.ToString();

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1f);
        sequence.AppendCallback(() =>
        {
            videoStatusText.gameObject.SetActive(false);
        });

        switch (videoStatus)
        {
            case VideoStatus.Play:
                videoPlayer.Play();
                break;

            case VideoStatus.Pause:
                videoPlayer.Pause();
                break;

            case VideoStatus.Stop:
                videoPlayer.Stop();
                break;

            default:
                break;
        }
    }

    private IEnumerator OnTouchVideoDisplay(float waitTime, string animationName, string nextAnimation, Action<string> OnWaitFinish)
    {
        if (!videoButtonPanelAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            Debug.Log("show panel");
            videoButtonPanelAnimator.SetTrigger(animationName);
            yield return new WaitForSeconds(waitTime);
            OnWaitFinish(nextAnimation);
        }
    }

    private void DoNext(string animationName)
    {
        videoButtonPanelAnimator.SetTrigger(animationName);
        Debug.Log("hide panel");
    }

    public void TouchVideoButton()
    {
        StartCoroutine(OnTouchVideoDisplay(5, ButtonPanelAnim.Show.ToString(), ButtonPanelAnim.Hide.ToString(), DoNext));
    }

    public void PlayVideo()
    {
        SetVideoStatus(VideoStatus.Play);
    }

    public void PauseVideo()
    {
        SetVideoStatus(VideoStatus.Pause);
    }

    public void StopVideo()
    {
        if (videoPlayer.isPlaying)
        {
            SetVideoStatus(VideoStatus.Stop);
        }
    }

    

    public enum VideoStatus
    {
        Play, Pause, Stop
    }

    #endregion

    public enum ButtonPanelAnim
    {
        Show, Hide
    }
}
