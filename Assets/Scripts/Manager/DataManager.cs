using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using NaughtyAttributes;
using System.Linq;

public class DataManager : MonoBehaviour
{
    public const string saveFilename = "playerdata";
    public static DataManager Instance;
    public PlayerData playerData;
    public ChapterData chapterData;
    public List<ChapterData> chapterDataInPlayerData;
    public List<string> allChapterUnlocked;
    public List<string> allSubchapterUnlocked;
    public List<string> allVideoClipKeys;
    public List<VideoClip> videoClips;
    public Dictionary<string, VideoClip> allVideoClipData;
    public string folderName = "Videos";

    [Header("Current Chapter/Subchapter Data")]
    public int currentChapterID;
    public string currentChapterName;
    public int currentTotalSubchapter;
    public string currentSubchapterName;
    public string currentSubchapterTitle;
    public int currentSubchapterID;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        GetComponentInParent<GameManager>().GetResourcesData();
        LoadData();
        LoadResources();
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("chapter name " + currentChapterName);
        }
#if UNITY_EDITOR
        chapterDataInPlayerData = playerData.allChapterUnlocked.Values.ToList();
        allChapterUnlocked = playerData.chapterUnlocked.Keys.ToList();
        allSubchapterUnlocked = playerData.subchapterUnlocked.Keys.ToList();
#endif
    }

    [Button("Save Player Data")]
    public void SaveData()
    {
        string FullFilePath = Application.persistentDataPath + "/" + saveFilename + ".bin";
        BinaryFormatter Formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(FullFilePath, FileMode.Create);
        Formatter.Serialize(fileStream, playerData);
        fileStream.Close();

        Debug.Log("saved");
    }



    [Button("Load Player Data")]
    public void LoadData()
    {
        string FullFilePath = Application.persistentDataPath + "/" + saveFilename + ".bin";
        if (File.Exists(FullFilePath))
        {
            //load data if player data exist
            BinaryFormatter Formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(FullFilePath, FileMode.Open);
            playerData = (PlayerData)Formatter.Deserialize(fileStream);
            fileStream.Close();
        }

        else
        {
            //player default data
            ResetPlayerData();
        }
    }

    [Button("Delete data")]
    public void DeleteData()
    {
        string FullFilePath = Application.persistentDataPath + "/" + saveFilename + ".bin";
        if (File.Exists(FullFilePath))
        {
            File.Delete(FullFilePath);
            ResetPlayerData();
        }
    }


    public void ResetPlayerData()
    {
        playerData = new PlayerData();
        playerData.totalChapterUnlocked = 1;
        playerData.currentChapter = GameManager.Instance.allChapterList[0].chapterName;
        playerData.currentSubchapter = GameManager.Instance.allSubchapterList[0].subchapterName;
        playerData.totalChapter = GameManager.Instance.allChapterData.Count;
        playerData.chapterUnlocked.Add(playerData.currentChapter, true);
        chapterData = new ChapterData(0, playerData.currentChapter, true, GameManager.Instance.allChapterData[playerData.currentChapter].subchapterList.Count, 1);
        //Debug.Log("subchapter count " + playerData.allChapterUnlocked[playerData.currentChapter].subchapterNameList);
        for (int i = 0; i < GameManager.Instance.allChapterData[playerData.currentChapter].subchapterList.Count; i++)
        {
            string subchapterName = GameManager.Instance.allChapterData[playerData.currentChapter].subchapterList[i].subchapterName;
            chapterData.subchapterNameList.Add(subchapterName);
        }
        playerData.allChapterUnlocked.Add(GameManager.Instance.allChapterList[0].chapterName, chapterData);
        string subchapterKey = GameManager.Instance.allChapterList[0].chapterName + "|" + GameManager.Instance.allSubchapterList[0].subchapterName;
        playerData.subchapterUnlocked.Add(subchapterKey, true);
        Debug.Log("current chapter unlocked data " + subchapterKey + " status " + playerData.subchapterUnlocked[subchapterKey]);
        SaveData();
    }

    public void RefreshGlobalData()
    {
        currentChapterID = 0;
        currentChapterName = "";
        currentSubchapterID = 0;
        currentSubchapterName = "";
        currentSubchapterTitle = "";
        currentTotalSubchapter = 0;
    }

    

    public void LoadResources()
    {
        //load all video data
        videoClips = new List<VideoClip>(Resources.LoadAll<VideoClip>("Files/Video"));
        allVideoClipData = new Dictionary<string, VideoClip>();

        for (int i = 0; i < videoClips.Count; i++)
        {
            allVideoClipData.Add(videoClips[i].name, videoClips[i]);
            Debug.Log("video clip name " + videoClips[i].name);
        }

    }

    public enum PartType
    {
        Chapter, Subchapter
    }


#region Player, Chapter & Subchapter Data
    [Serializable]
    public class PlayerData
    {
        public string currentChapter;
        public string currentSubchapter;
        public int totalChapter;
        public int totalChapterUnlocked;

        public Dictionary<string, ChapterData> allChapterUnlocked = new Dictionary<string, ChapterData>();
        public Dictionary<string, bool> chapterUnlocked = new Dictionary<string, bool>();
        public Dictionary<string, bool> subchapterUnlocked = new Dictionary<string, bool>();
    }


    [Serializable]
    public class ChapterData
    {
        public int chapterID;
        public string chapterName;
        private bool chapterUnlocked = false;
        public int totalSubchapter = 0;
        public int totalSubchapterUnlocked = 1;
        public List<string> subchapterNameList = new List<string>();

        public bool ChapterUnlocked
        { get => chapterUnlocked; set => chapterUnlocked = value; }

        public ChapterData() { }
        public ChapterData(int _chapterID, string _chapterName, bool _chapterUnlocked, int _totalSubchapter, int _totalSubchapterUnlocked)
        {
            chapterID = _chapterID;
            chapterName = _chapterName;
            chapterUnlocked = _chapterUnlocked;
            totalSubchapter = _totalSubchapter;
            totalSubchapterUnlocked = _totalSubchapterUnlocked;
        }
    }
#endregion

    public enum FileType
    {
        Image, Video
    }

}
