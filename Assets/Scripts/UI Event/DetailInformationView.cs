using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class DetailInformationView : MonoBehaviour
{
    Vector2 imagesOriginalSize = Vector3.zero;
    [SerializeField] private RawImage screenImg;
    [SerializeField] private RawImage detailImg;
    [SerializeField] private VideoPlayer videoPlayer;
    private Coroutine slideTask;
    private float MaxX = 0;
    
    /// <summary>
    /// 
    /// </summary>
    public void Start()
    {
        //MainEventBus.Instance.Subscribe(this);
        GameEvents.Instance.OnRequestExhibitContents += ShowExhibition;
        GameEvents.Instance.OnRequestPauseGalleryVideo += OnVideoPause;
        GameEvents.Instance.OnRequestLobbyVideoStart += PlayVideo;
        imagesOriginalSize = transform.Find("GameObject/ImageMask/Image").GetComponent<Image>().rectTransform.sizeDelta;
        PrepareVideoResource();
        slideTask = null;
    }
    
    private void PrepareVideoResource()
    {
        //videoPlayer.url = Application.streamingAssetsPath + "/" + target + ".mp4";
        videoPlayer.url = "https://unn-metaverse-unity.s3.ap-northeast-2.amazonaws.com/unn-intro.mp4";//"https://test-metabus-unity.s3.ap-northeast-2.amazonaws.com/35thGallery/testvideo.mp4";
        videoPlayer.isLooping = true;
        Debug.Log("video url : " + videoPlayer.url);
        videoPlayer.Pause();
    }

    private void PlayVideo()
    {
        StartCoroutine(LoadVideoResource()); 
    }
    
    IEnumerator LoadVideoResource()
    {
        videoPlayer.Prepare();
        float time = 0.0f;
        while (true)
        {
            time += Time.deltaTime;
            if (videoPlayer.isPrepared) {
                break;
            }
            if (10.0f < time) {
                break;
            }   
            yield return null;
        }
        //if(rawImage != null)
        //    rawImage.texture = videoPlayer.texture;
        screenImg.texture = videoPlayer.texture;
        detailImg.texture = videoPlayer.texture;
        
        videoPlayer.Play();
    }

    private void OnDestroy()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.OnRequestExhibitContents -= ShowExhibition;
            GameEvents.Instance.OnRequestPauseGalleryVideo -= OnVideoPause;
            GameEvents.Instance.OnRequestLobbyVideoStart -= PlayVideo;
        }
    }

    private void OnVideoPause(bool pause)
    {
        if(pause)
            videoPlayer.Pause();
        else
            videoPlayer.Play();
    }

    /// <summary>
    /// 
    /// </summary>
    public void setImage(Sprite sprite,float ratio)
    {
        Image image = transform.Find("GameObject/ImageMask/Image").GetComponent<Image>();

        image.sprite = sprite;

        image.SetNativeSize();
        ResizeImageByItsSize(image,ratio);
        setScrollViewportBackgroudnHeight(image.GetComponent<RectTransform>().sizeDelta.y,
                                          "ImageMask/Background");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rawImage"></param>
    public void SetVideo(RawImage rawImage, ExhibitInfo info, float ratio)
    {
        /*
        float videoWidth    = 720f;
        float videoHeight   = 1107f;
        Vector2 targetSize  = new Vector2(videoWidth, videoHeight);

        rawImage.rectTransform.sizeDelta = new Vector2(videoWidth, videoHeight);

        //Debug.LogError(rawImage.rectTransform.sizeDelta);
        
        */
        
        float videoWidth    = 720f;
        float videoHeight   = 1280f;
        /*
        if (videoWidth / 1200f > videoHeight / 900f)
        {
            rawImage.rectTransform.sizeDelta = new Vector2(1200, videoHeight * (1200f/videoWidth));
            rawImage.transform.parent.GetComponent<RectTransform>().sizeDelta =
                new Vector2(1200, videoHeight * (1200f / videoWidth));
        }
        else
        {
            rawImage.rectTransform.sizeDelta = new Vector2(videoWidth * (900/videoHeight), 900f);
            rawImage.transform.parent.GetComponent<RectTransform>().sizeDelta =
                new Vector2(videoWidth * (900/videoHeight), 900);
        }
        */
        setScrollViewportBackgroudnHeight(rawImage.GetComponent<RectTransform>().sizeDelta.y,
                                          "VideoMask/Background");
    }

    /// <summary>
    /// 
    /// </summary>
    void setScrollViewportBackgroudnHeight(float height, string path)
    {
        Vector2 sizeDelta = transform.Find($"GameObject/{path}").GetComponent<RectTransform>().sizeDelta;
        transform.Find($"GameObject/{path}").GetComponent<RectTransform>().sizeDelta = new Vector2(sizeDelta.x, height);
    }

    /// <summary>
    /// 
    /// </summary>
    void ResizeImageByItsSize(Image target, float ratio)
    {
        Vector3 targetSize = target.rectTransform.sizeDelta;
        Vector3 defaultSize = imagesOriginalSize;

        float diffVal;
        float height = target.rectTransform.sizeDelta.x * 1/ratio;
        float width= target.rectTransform.sizeDelta.x;
            
        // /*높이 조정*/
        // if (defaultSize.y < targetSize.y)
        // {
        //     diffVal = defaultSize.y - targetSize.y;
        //     height = targetSize.y + diffVal;
        //     width = height / (1/ratio);
        //     target.rectTransform.sizeDelta = new Vector2(width, height);
        // }
        //
        // /*너비 조정*/
        // if (defaultSize.x < target.rectTransform.sizeDelta.x)
        // {
        //     diffVal = defaultSize.x - targetSize.x;
        //     width = targetSize.x + diffVal;
        //     height = targetSize.y + (targetSize.y * (diffVal / targetSize.x));
        //     target.rectTransform.sizeDelta = new Vector2(width, height);
        // }
        
        
        /*확대*/
        if (width / 1200f > height / 900f)
        {
            target.rectTransform.sizeDelta = new Vector2(1200, height * (1200f/width));
            target.transform.parent.GetComponent<RectTransform>().sizeDelta =
                new Vector2(1700, height * (1200f / width));
        }
        else
        {
            target.rectTransform.sizeDelta = new Vector2(width * (900/height), 900f);
            target.transform.parent.GetComponent<RectTransform>().sizeDelta =
                new Vector2(width * (900/height) + 500, 900);
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public void setTitle(string path, string text)
    {
        transform.Find($"GameObject/{path}/Scroll View/Title").GetComponent<TextMeshProUGUI>().text = text;
    }

    /// <summary>
    /// 
    /// </summary>
    public void setContents(string path, string str, RectTransform contentsViewArea)
    {
        TextMeshProUGUI _text = transform.Find($"GameObject/{path}/Scroll View/Viewport/Text").GetComponent<TextMeshProUGUI>();
        _text.text = str;
        _text.ForceMeshUpdate();

        ResizeScrollViewContentsArea(_text.fontSize, _text.textInfo.lineCount, contentsViewArea);
    }

    /// <summary>
    /// 
    /// </summary>
    void ResizeScrollViewContentsArea(float fontSize, int lines, RectTransform contentsViewArea)
    {
        float freeSpace = 1.15f;
        float height = (fontSize * freeSpace) * lines;
        contentsViewArea.sizeDelta = new Vector2(contentsViewArea.sizeDelta.x, height);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnClose()
    {
        if (transform.Find("GameObject").gameObject.activeSelf == false)
            return;

        ReactCommunicator.Instance.SendHeaderLayoutTypetoJS(ReactCommunicator.Instance.CurrentLocation);
        
        GameEvents.Instance.RequestBlockRaycast(false);
        //GameEvents.Instance.RequestPauseGalleryVideo(false);
        GameEvents.Instance.RequestSetActivePlayerInputSys(true);
        GameEvents.Instance.RequestPauseGalleryAudio(false);
        //MainEventBus.Instance.Publish(MainGameEventType.whenMouseEventResume);
        //MainEventBus.Instance.Publish(MainGameEventType.activatePlayerInputSys);
        // MainEventBus.Instance.Publish(MainGameEventType.unblockRaycastHit);
        // MainEventBus.Instance.Publish(MainGameEventType.resumeGalleryVideo);
        // MainEventBus.Instance.Publish(MainGameEventType.resumGalleryAudio);

        transform.Find("GameObject/ImageMask").gameObject.SetActive(false);
        transform.Find("GameObject/ImageMaskFuture").gameObject.SetActive(false);

        AudioPlayer ap = transform.Find("GameObject/Audio").GetComponent<AudioPlayer>();
        ap.OnStop();
        ap.gameObject.SetActive(false);

        VideoPlayer_2D vp = transform.Find("GameObject/VideoMask").GetComponent<VideoPlayer_2D>();
        //vp.Stop();
        vp.gameObject.SetActive(false);

        transform.Find("GameObject").gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnActivate()
    {
        transform.Find("GameObject").gameObject.SetActive(true);
        GameEvents.Instance.RequestBlockRaycast(true);
        //GameEvents.Instance.RequestPauseGalleryVideo(true);
        GameEvents.Instance.RequestSetActivePlayerInputSys(false);
        GameEvents.Instance.RequestPauseGalleryAudio(true);
        //MainEventBus.Instance.Publish(MainGameEventType.whenMouseEventPause);
        //MainEventBus.Instance.Publish(MainGameEventType.inactivatePlayerInputSys);
        //MainEventBus.Instance.Publish(MainGameEventType.blockRaycastHit);
        //MainEventBus.Instance.Publish(MainGameEventType.pauseGalleryVideo);
        //MainEventBus.Instance.Publish(MainGameEventType.pauseGalleryAudio);
    }

    public void OnNextBtn()
    {
        if (slideTask == null)
        {
            slideTask = StartCoroutine(SlideContextPosition(true));
        }
    }

    public void OnPrevBtn()
    {
        if (slideTask == null)
        {
            slideTask = StartCoroutine(SlideContextPosition(false));
        }
    }

    private IEnumerator SlideContextPosition(bool isNext)
    {
        Transform contentTransform = transform.Find("GameObject/ImageMaskFuture/Scroll View/Viewport/Content");//.localPosition = Vector3.zero;
        
        float currentX = contentTransform.localPosition.x;
        float distX = (isNext) ? -15.66f : 15.66f;

        if (Mathf.Abs(currentX) < 1f && !isNext)
        {
            Debug.Log("Can't slide back");
        }
        else if (isNext && (currentX - MaxX) < 1f)
        {
            Debug.Log("Can't slide forward");
        }
        else
        {
            int count = 0;
        
            while (count<50)
            {
                contentTransform.localPosition = new Vector3(currentX+distX, 0f, 0f);
                currentX = contentTransform.localPosition.x;
                yield return new WaitForFixedUpdate();
                count++;
            }
        }
        
        yield return null;
        slideTask = null;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void ShowExhibition(ExhibitionType type,string item, float ratio)
    {
        OnActivate();
        
        ExhibitInfo info = LocalRepository.Instance.FindExhibitInfo(item);
        string path = string.Empty;
        RectTransform contentsViewArea = null;

        Debug.Log("item : " + item);

        switch (type)
        {
            case ExhibitionType.image:
                {
                    //Texture2D texture = ResourcesManager.LoadResources<Texture2D>("Exhibits/" + data.str);
                    if (item.Contains("past"))
                    {
                        transform.Find("GameObject/ImageMask").gameObject.SetActive(true);
                        Texture2D texture = Resources.Load("Art/" + item) as Texture2D;
                        setImage(
                            Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                                new Vector2(0.5f, 0.5f)), float.Parse(info.ratio));
                        path = "ImageMask/Background";
                        contentsViewArea = transform.Find("GameObject/ImageMask/Background/Scroll View/Viewport/Text")
                            .GetComponent<RectTransform>();
                    }
                    else if(item.Contains("future"))
                    {
                        transform.Find("GameObject/ImageMaskFuture").gameObject.SetActive(true);

                        int artNum = int.Parse(item.Split("future")[1]);
                        Debug.Log("artNum " + artNum);
                        int contentsCnt = 0;
                        switch (artNum/10)
                        {
                            case 5 :
                                contentsCnt = 7;
                                break;
                            default:
                                contentsCnt = 5;
                                break;
                        }
                        MaxX = -(contentsCnt - 1) * 783;
                        for (int i = 1; i < contentsCnt + 1; i++)
                        {
                            Image image = transform.Find("GameObject/ImageMaskFuture/Scroll View/Viewport/Content/Image"+i.ToString()).GetComponent<Image>();

                            Texture2D tempTexture2D = Resources.Load("Art/" + item.Substring(0, item.Length - 1) + i.ToString()) as Texture2D;
                            Sprite tempSprite = Sprite.Create(tempTexture2D, new Rect(0, 0, tempTexture2D.width, tempTexture2D.height), new Vector2(0.5f, 0.5f));
                            image.sprite = tempSprite;
                        }

                        transform.Find("GameObject/ImageMaskFuture/Scroll View/Viewport/Content").localPosition = new Vector3(-783 * (artNum % 10 - 1), 0, 0);//Vector3.zero;
                    }
                }
                break;
            case ExhibitionType.video:
                {
                    VideoPlayer_2D vp = transform.Find("GameObject/VideoMask").GetComponent<VideoPlayer_2D>();
                    vp.gameObject.SetActive(true);

                    SetVideo(transform.Find("GameObject/VideoMask/Video").GetComponent<RawImage>(), info, ratio);

                    //vp.PlayLink("");
                    path = "VideoMask/Background";

                    contentsViewArea = transform.Find("GameObject/VideoMask/Background/Scroll View/Viewport/Text").GetComponent<RectTransform>();
                }
                break;
            case ExhibitionType.audio:
                {
                    AudioPlayer ap = transform.Find("GameObject/Audio").GetComponent<AudioPlayer>();
                    ap.gameObject.SetActive(true);
                    ap.OnPlay();
                    path = "Audio/Background";
                    contentsViewArea = transform.Find("GameObject/Audio/Background/Scroll View/Viewport/Text").GetComponent<RectTransform>();
                }
                break;
        }

        if (item.Contains("past"))
        {
            if (string.IsNullOrEmpty(path) == false &&
                contentsViewArea != null)
            {

                setTitle(path, info.title);
                setContents(path, info.introduction, contentsViewArea);
            }
        }
        
        ReactCommunicator.Instance.SendHeaderLayoutTypetoJS("NONE");
    }

    /// <summary>
    /// 
    /// </summary>
    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            OnClose();
        }
    }
}
