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
    public SubchapterData subchapterData;
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
        playerData.chapterUnlocked.Add(playerData.currentChapter, true);
        ChapterData chapterData = new ChapterData(0, playerData.currentChapter, true, GameManager.Instance.allChapterData[playerData.currentChapter].subchapterList.Count, 1);
        //Debug.Log("subchapter count " + playerData.allChapterUnlocked[playerData.currentChapter].subchapterNameList);

        for (int i = 0; i < GameManager.Instance.allChapterData[playerData.currentChapter].subchapterList.Count; i++)
        {
            Debug.Log("new subchapter added name " + GameManager.Instance.allChapterData[playerData.currentChapter].subchapterList[i].subchapterName);
            string subchapterName = GameManager.Instance.allChapterData[playerData.currentChapter].subchapterList[i].subchapterName;
            chapterData.subchapterNameList.Add(subchapterName);
        }
        playerData.allChapterUnlocked = new Dictionary<string, ChapterData>();
        playerData.allChapterUnlocked.Add(GameManager.Instance.allChapterList[0].chapterName, chapterData);
        Debug.Log("subchapter count " + playerData.allChapterUnlocked[playerData.currentChapter].subchapterNameList.Count);
        string subchapterUnlocked = GameManager.Instance.allChapterList[0].chapterName + "|" + GameManager.Instance.allSubchapterList[0].subchapterName;
        playerData.subchapterUnlocked.Add(subchapterUnlocked, true);
        Debug.Log("current chapter unlocked data " + playerData.allChapterUnlocked[playerData.currentChapter].totalSubchapterUnlocked);
        SaveData();
    }

    public void CheckCurrentSubchapterUnlocked()
    {
        string currentKeyName = currentChapterName + "|" + currentSubchapterName;
        if (!playerData.subchapterUnlocked.ContainsKey(currentKeyName))
        {
            playerData.subchapterUnlocked.Add(currentKeyName, true);
            playerData.allChapterUnlocked[currentChapterName].totalSubchapterUnlocked++;
            SaveData();
            Debug.Log("unlock new subchapter " + playerData.subchapterUnlocked.Keys.ToList());
        }
    }

    public void UnlockNextChapterSubchapter()
    {
        Debug.Log("current chapter name " + currentChapterName);
        //if (playerData.allChapterUnlocked[currentChapterName].totalSubchapterUnlocked < currentTotalSubchapter - 1)
        //{
        //    if (currentSubchapterID < playerData.allChapterUnlocked[currentChapterName].totalSubchapter - 1)
        //    {
        //        int nextSubchapterID = currentChapterID++;
        //        Debug.Log("next subchapter id " + nextSubchapterID);
        //        string nextSubchapterName = playerData.allChapterUnlocked[currentChapterName].subchapterNameList[nextSubchapterID];
        //        Debug.Log("next subchapter name " + nextSubchapterName);
        //        string nextKeyName = currentChapterName + "|" + nextSubchapterName;
        //        if (!playerData.subchapterUnlocked.ContainsKey(nextKeyName))
        //        {
        //            playerData.subchapterUnlocked.Add(nextKeyName, true);
        //            playerData.allChapterUnlocked[currentChapterName].totalSubchapterUnlocked++;
        //            SaveData();
        //            currentSubchapterName = playerData.allChapterUnlocked[currentChapterName].subchapterNameList[playerData.allChapterUnlocked[currentChapterName].totalSubchapterUnlocked];
        //            Debug.Log("unlock new subchapter " + currentSubchapterName);
        //        }
        //    }
           
        //}

        //else
        //{
        //    string nextChapterName = GameManager.Instance.allcurrentChapter[currentChapterID++];
        //    playerData.chapterUnlocked.Add(nextChapterName, true);
        //    ChapterData chapterData = new ChapterData(currentChapterID, nextChapterName, true, GameManager.Instance.allChapterData[nextChapterName].subchapterList.Count, 1);
        //    playerData.allChapterUnlocked.Add(nextChapterName, chapterData);

        //    PanelController.Instance.ActiveDeactivePanel("chapter", "subchapter");
        //    SaveData();
        //    Debug.Log("unlock new chapter " + currentChapterName);
        //    return PartType.Chapter.ToString();
        //}
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
        public Dictionary<string, SubchapterData> subchapterData = new Dictionary<string, SubchapterData>();

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

    [Serializable]
    public class SubchapterData
    {
        public int subchapterID;
        public string subchapterName;
        private bool subchapterUnlocked = false;

        public bool SubchapterUnlocked { get => subchapterUnlocked; set => subchapterUnlocked = value; }

        public SubchapterData(int _subchapterID, string _subchapterName, bool _subchapterUnlocked)
        {
            subchapterID = _subchapterID;
            subchapterName = _subchapterName;
            subchapterUnlocked = _subchapterUnlocked;
        }
    }
#endregion

    public enum FileType
    {
        Image, Video
    }

}
