using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Reflection;
using TMPro;

public class Saver : MonoBehaviour
{
    static string SaveFolder = "/JsonData/";
    static string SaveDataFileName = "/JsonData/SaveData.json";
    public static int Player;
    public static int Enemy;
    public Log Log;

    [ContextMenu("To Json Data")]
    void SaveLogDataToJson()
    {
        string jsonData = JsonUtility.ToJson(Log);
        string path = Application.dataPath + "/Log.json";
        File.WriteAllText(path, jsonData);
    }
    [ContextMenu("To Json Data")]
    void LoadLogDataFromJson()
    {
        string jsonData = JsonUtility.ToJson(Log);
        string path = Application.dataPath + "/Log.json";
        File.WriteAllText(path, jsonData);
    }

    
    public static void Save()
    {

        string folderPath = Application.dataPath + SaveFolder;
        Log log = new Log();
        log.Player = GameManager.MyScore;
        log.Enmey = GameManager.EnScore;
        var data = JsonConvert.SerializeObject(log, Formatting.Indented);
        File.WriteAllText(Application.dataPath + SaveDataFileName, data);
    }
    public static void Saving()
    {
        var list = new List <Log>();
        Log log = new Log();
        list.Insert(0, log);
        Debug.Log("로그추가후");
        // 리스트를 다시 JSON으로 직렬화
        var newData = JsonConvert.SerializeObject(list, Formatting.Indented);
        // 파일에 쓰기
        File.WriteAllText(Application.dataPath + SaveDataFileName, newData);
    }
    ////////////////////////////////
    public static void Save3()
    {
        try
        {
            var fileData = File.ReadAllText(Application.dataPath + SaveDataFileName);
            var list = JsonConvert.DeserializeObject<List<Log>>(fileData);

            // 역직렬화한 데이터가 null이 아닌 경우에만 진행
            if (list != null)
            {
                Debug.Log(list);
                Debug.Log("save3");
                Log log = new Log();
                log.Player = GameManager.MyScore;
                log.Enmey = GameManager.EnScore;
                //foreach(var item in list)
                //{
                //    Debug.Log("리스트 카운트 : "+ list.Count  +"player"+item.Player + "Enemy"+ item.Enmey);
                //}
                if (list.Count >= 10)
                {
                    list.RemoveAt(list.Count - 1);
                }
                // 리스트에 로그 추가
                list.Insert(0, log);
                //Debug.Log("로그추가후");

                // 리스트를 다시 JSON으로 직렬화
                var newData = JsonConvert.SerializeObject(list, Formatting.Indented);

                // 파일에 쓰기
                File.WriteAllText(Application.dataPath + SaveDataFileName, newData);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error while reading file: " + ex.Message);
            Saving();
        }
    }

    //public static void Save3()
    //{
    //    var fileData = File.ReadAllText(Application.dataPath + SaveDataFileName);
    //    // JSON 데이터를 List<Log>로 역직렬화
    //    //var list = JsonConvert.DeserializeObject<List<Log>>(fileData);
    //    var linkedlist = JsonConvert.DeserializeObject<LinkedList<Log>>(fileData);

    //    // 역직렬화한 데이터가 null이 아닌 경우에만 진행
    //    if (linkedlist != null)
    //    {
    //        Debug.Log(linkedlist.Count);
    //        if (linkedlist.Count < 10)
    //        {
    //            linkedlist.RemoveLast();
    //            Log log = new Log();
    //            log.Player = GameManager.MyScore;
    //            log.Enmey = GameManager.EnScore;

    //            // 리스트에 로그 추가
    //            //list.Insert(0, log);
    //            linkedlist.AddFirst(log); 

    //            // 리스트를 다시 JSON으로 직렬화
    //            var newData = JsonConvert.SerializeObject(linkedlist, Formatting.Indented);

    //            // 파일에 쓰기
    //            File.WriteAllText(Application.dataPath + SaveDataFileName, newData);

    //            Debug.Log("save3");
    //        }
    //    }
    //}
    ///////////////////////////



    public static void Load()
    {
        var flleData = File.ReadAllText(Application.dataPath + SaveDataFileName);
        var data = JsonConvert.DeserializeObject<Log>(flleData);
        List<Log> list = new List<Log>();
        Debug.Log(data.Player +data.Enmey);
        

    }

    public static void ScoreUpdata()
    {
        Player = GameManager.MyScore;
        Enemy = GameManager.EnScore;
    }
       
    

}



public class Log
{
    public int Player;
    public int Enmey;

}