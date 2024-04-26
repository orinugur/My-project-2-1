using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Newtonsoft.Json;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.UIElements.Experimental;

public class Main : MonoBehaviour
{

    public GameObject Titel;
    public AudioSource AudioSource;
    public TextMeshProUGUI VsLog1;
    public TextMeshProUGUI VsLog2;
    public GameObject VslogBoard;
    public bool BoardPopUp=false;
    public AudioSource MainAudios;
    private void StartGame()
    {
        Rigidbody2D rd= Titel.GetComponent<Rigidbody2D>();
        rd.constraints = RigidbodyConstraints2D.FreezePositionY;
        
    }


    public void PrsedButon()
    {
        MainAudios.Play();
        Rigidbody2D rd = Titel.GetComponent<Rigidbody2D>();
        rd.constraints = RigidbodyConstraints2D.None;
        rd.gravityScale = 2f;
        StartCoroutine(Started());
    }
    public void historic() //전적검색기능
    {
        VslogBoard.SetActive(true);
        MainAudios.Play();
        AudioSource VsAu = VslogBoard.GetComponent<AudioSource>();
        VsAu.Play();
        BoardPopUp = true;
        string SaveDataFileName = "/JsonData/SaveData.json";
        try
        {

            var fileData = File.ReadAllText(Application.dataPath + SaveDataFileName);
            var list = JsonConvert.DeserializeObject<List<Log>>(fileData);
            VsLog1.text = "PL:EN\n";
            VsLog2.text = "PL:EN\n";
            List<Log> list2 = new List<Log>();
            for (int i = 0; i < list.Count; i++)
            {
                list2.Add(list[i]);
                Debug.Log("List tostring" + list[i].ToString());
                Debug.Log("List " + list[i].Player +" " + list[i].Enmey);
            }
            for(int i = 0; i < list2.Count; i++)
            {
                if (i <= 4)
                {
                    VsLog1.text += list2[i].Player + ":" + list2[i].Enmey + "\n";
                }
                else if (i >4)
                {
                    VsLog2.text += list2[i].Player + ":" + list2[i].Enmey + "\n";
                }
            }
        }
        

        catch (Exception ex)
        {
            Saver.Saving();
            Debug.LogError("Error while reading file: " + ex.Message);

        }



        //foreach(Log log in list)
        //{
        //    Debug.Log(log);
        //}


    }
    public void ExitGame()
    {
        MainAudios.Play();
        Application.Quit();
    }
    public void Start()
    {
        MainAudios.Play();
        StartGame();

    }

    private void Update()
    {
        if (BoardPopUp == true)
        {
            if (Input.anyKey)
            {
                VslogBoard.SetActive(false);
                BoardPopUp = false;
            }
        }

    }
    
    public IEnumerator Started()
    {
        StartCoroutine(StartFade(AudioSource,2f,0));
        yield return new WaitForSecondsRealtime(1.5f);
        
        SceneManager.LoadScene("Game");
    }

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }



}
