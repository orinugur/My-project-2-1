using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Winer : MonoBehaviour
{


    [SerializeField]
    public TextMeshProUGUI HowsWiner;
    public GameObject BlackB;
    public GameObject Ment;

    // Update is called once per frame
    private void OnEnable()
    {
        BlackB.SetActive(true);
        AudioSource BBA = BlackB.GetComponent<AudioSource>();
        BBA.Play();
        Ment.SetActive(true);
    }
    void Update()
    {
        if (GameManager.MyScore> GameManager.EnScore)
        {
            HowsWiner.text = "Player Win!!";
        }
        else if (GameManager.MyScore == GameManager.EnScore)
        {
            HowsWiner.text = "DRAW";
        }
        else
        {
            HowsWiner.text = "Enemy Win!!";
        }
    }
}
