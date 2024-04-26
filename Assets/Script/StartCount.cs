using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartCount : MonoBehaviour
{
    public int Seconds;
    public float TimeSecond;
    public TextMeshProUGUI StartingCount;
    public GameObject Count;
    public string[] Ment = { "3", "2", "1", "0", "Go!"};
    // Update is called once per frame
    //private void Awake()
    //{
    //    StartCoroutine(DisplayScale());
    //}
    public AudioClip[] Audio; 
    AudioSource soundSource;

    void Update()
    {
        transform.localScale = transform.localScale * 0.95f;
        TimeSecond = TimeSecond + Time.deltaTime;
        if (Seconds >= Ment.Length)
            {
                Count.SetActive(false);
            }

    }
    private void Start()
    {
        soundSource = GetComponent<AudioSource>();
        StartCoroutine(DisplayTimer());
    }
    IEnumerator DisplayTimer()
    {
        //        AudioSource StartingBeep = GetComponent<AudioSource>();

        Seconds = Mathf.FloorToInt(TimeSecond);

        if (Seconds < Ment.Length)
        {
            StartingCount.text = Ment[Seconds];
        }

        if (Seconds <=3)
        {
            soundSource.clip = Audio[0];
            soundSource.Play();
            
        }
        if(Seconds > 3)
        {
            soundSource.clip = Audio[1];
            soundSource.Play();
        }
        Debug.Log(Seconds + "  " + Ment.Length);
        
        yield return new WaitForSeconds(1f);
        if(Seconds <= 3)
        {
            transform.localScale = Vector3.one;
        }
        
        StartCoroutine(DisplayTimer());
    }

    public void OnDisable()
    
    {
        //AudioSource whistle = GetComponent<AudioSource>();
//        soundSource.Stop();
        
    }
        //IEnumerator DisplayScale()
        //{
        //    yield return new WaitForSeconds(Seconds);
        //    transform.localScale = Vector3.one;
        //    yield return new WaitForSeconds(Seconds);
        //    transform.localScale = Vector3.one;
        //    yield return new WaitForSeconds(Seconds);
        //    transform.localScale = Vector3.one;
        //    yield return new WaitForSeconds(Seconds);
        //    transform.localScale = Vector3.one;
        //    yield return new WaitForSeconds(Seconds);
        //    transform.localScale = Vector3.one;
        //    yield return new WaitForSeconds(Seconds);
        //    transform.localScale = Vector3.one;
        //}
    }
