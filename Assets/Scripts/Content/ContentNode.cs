using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using DG.Tweening;
using Lean.Pool;

public class ContentNode : MonoBehaviour
{
    [SerializeField] ContentPartSO contentPartSO;
    [Header("Main Content Identifier")]
    public string contentID;
    public ContentPartSO.ContentType contentType;
    
    [Header("Text Content Type")]
    [SerializeField] private TextMeshProUGUI contentText;
    [SerializeField] private float fontSize;
    [SerializeField] private TMP_FontAsset fontAsset;

    [Header("Image Content Type")]
    public Image contentImage;
    [SerializeField] private bool imageSetNativeSize;
    public TextMeshProUGUI imageWatermarkText;

    [Header("Video Content Type")]
    public RawImage contentVideoImage;
    public TextMeshProUGUI videoStatusText;
    public VideoPlayer videoPlayer;
    public Image thumbnailImageLayer;
    public Button videoButton;
    public Animator videoButtonPanelAnimator;

    [Header("Subject Content Type")]
    public Image subjectImage;
    [SerializeField] private bool subjectImageSetNativeSize;

    [Header("Question Content Type")]
    public TextMeshProUGUI questionNumberText;
    public TextMeshProUGUI questionText;
    public ToggleGroup answerToggleGroup;
    public GameObject answerPrefab;

    public int questionID;
    public string questionContent;
    [SerializeField] private int matchAnswerID;
    public int _matchAnswerID { get => matchAnswerID; set => matchAnswerID = value; }
    public List<string> _allAnswers;
    public int thisQuestionScore = 0;




    [SerializeField]
    private string fileName;

    [SerializeField]
    private string URLvideo;


    public void InitContentNode(ContentPartSO _contentPartSO, int _contentOrder)
    {
        contentPartSO = _contentPartSO;
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
                imageSetNativeSize = _contentPartSO.imageSetNativeSize;

                if (imageSetNativeSize)
                {
                    SetImageStretch();
                }

                else
                {
                    //if (contentPartSO.customAnchorPoint)
                    //{
                    //    StretchImageWithoutPreserve();
                    //}

                    //else
                    //{
                        Debug.Log("set default image size");    
                        contentImage.rectTransform.sizeDelta = new Vector2(0, _contentPartSO.contentImageSize);
                        contentImage.rectTransform.anchorMin = new Vector2(0, 0);
                        contentImage.rectTransform.anchorMax = new Vector2(1, 1);
                        contentImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                        contentImage.rectTransform.anchoredPosition = new Vector2(0, 0);
                    //}
                    
                }
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

                if (_contentPartSO.fontAsset != null)
                {
                    contentText.font = _contentPartSO.fontAsset;
                }
                

                break;
            
            case ContentPartSO.ContentType.Video:
                StartCoroutine(OnTouchVideoDisplay(5, ButtonPanelAnim.Show.ToString(), ButtonPanelAnim.Hide.ToString(), DoNext));
                StartCoroutine(RequestVideo(URLvideo, PendingVideoOnReady));
                break;
            
            case ContentPartSO.ContentType.AR:
                break;

            case ContentPartSO.ContentType.Subject:
                subjectImage.gameObject.SetActive(true);
                subjectImage.sprite = _contentPartSO.contentImage;
                SetImageNative();

                break;
            case ContentPartSO.ContentType.Question:

                if (answerToggleGroup.transform.childCount > 0)
                {
                    Debug.Log("toggle child " + answerToggleGroup.transform.childCount);
                    for (int i = answerToggleGroup.transform.childCount - 1; i >= 0; i--)
                    {
                        LeanPool.Despawn(answerToggleGroup.transform.GetChild(i).gameObject);
                        Debug.Log("despawn");
                    }
                }

                questionID = _contentPartSO.questionID;
                questionContent = _contentPartSO.question;
                matchAnswerID = _contentPartSO.matchAnswerID;
                _allAnswers = _contentPartSO.allAnswers;

                questionNumberText.text = questionID + ".";
                questionText.text = questionContent;

                AnswerQuestionContent answerQuestionContent;
                for (int i = 0; i < _allAnswers.Count; i++)
                {
                    answerQuestionContent = LeanPool.Spawn(answerPrefab, answerToggleGroup.transform).GetComponent<AnswerQuestionContent>();
                    answerQuestionContent.AnswerInit(i, _allAnswers[i]);
                }

                break;
            default:
                break;
        }
    }

    #region Image Content Type
    public void SetImageStretch()
    {
        contentImage.SetNativeSize();
        contentImage.preserveAspect = true;
        
        RectTransform imageParentRect = contentImage.transform.parent.GetComponent<RectTransform>();
        imageParentRect.sizeDelta = contentImage.rectTransform.sizeDelta;

        contentImage.rectTransform.anchorMin = new Vector2(0, 0);
        contentImage.rectTransform.anchorMax = new Vector2(1, 1);
        contentImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        contentImage.rectTransform.anchoredPosition = new Vector2(0, 0);
        contentImage.rectTransform.sizeDelta = new Vector2(0, 0);
    }

    public void StretchImageWithoutPreserve()
    {
        contentImage.SetNativeSize();
        contentImage.preserveAspect = false;

        RectTransform imageParentRect = contentImage.transform.parent.GetComponent<RectTransform>();
        imageParentRect.sizeDelta = contentImage.rectTransform.sizeDelta;

        contentImage.rectTransform.anchorMin = new Vector2(0, 0);
        contentImage.rectTransform.anchorMax = new Vector2(1, 1);
        contentImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        contentImage.rectTransform.anchoredPosition = new Vector2(0, 0);
        contentImage.rectTransform.sizeDelta = new Vector2(0, 0);
    }

    

    #endregion



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

    #region Subject Content Type
    public void SetImageNative()
    {
        subjectImage.SetNativeSize();
        subjectImage.preserveAspect = true;

        RectTransform imageParentRect = subjectImage.transform.parent.GetComponent<RectTransform>();
        imageParentRect.sizeDelta = subjectImage.rectTransform.sizeDelta;

        subjectImage.rectTransform.position = new Vector2(0, 0);
        subjectImage.rectTransform.localPosition = new Vector2(0, 0);
        subjectImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        subjectImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        subjectImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        
    }
    #endregion
}
