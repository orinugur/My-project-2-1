using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.SocialPlatforms.Impl;

//[System.Serializable]
//public class Node
//{
//    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

//    public bool isWall;
//    public Node ParentNode;

//    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
//    public int x, y, G, H;
//    public int F { get { return G + H; } }
//}
public class GameManager : Singleton<GameManager>
{
    public TextMeshProUGUI ScoreTxt;
    public TextMeshProUGUI Timer;
    public TextMeshProUGUI Wine;
    public float TimeSecond;
    //public static GameObject Ballrigid;
    public  GameObject Ball;
    public static int MyScore;
    public static int EnScore;
    public int Seconds;
    public Rigidbody2D Brd; 
    public GameObject[] AllPlayer;
    public GameObject BlackB;
    public GameObject Win;
    public GameObject PlayerManager;
    public bool GameOver=false;
   public static bool NewGameSet=false;
    public AudioSource AudioSource;
    public GameObject Arrow;
    //public Vector2Int bottomLeft, topRight, startPos, targetPos;
    //public List<Node> FinalNodeList;
    //public bool allowDiagonal, dontCrossCorner;


    private void Awake()
    {
        NewGameSet = false;
        GameOver = false;
        Brd = Ball.GetComponent<Rigidbody2D>(); // Ball에 부착된 Rigidbody2D 컴포넌트 가져오기
        MyScore = 0;
        EnScore = 0;
        ScoreTxt.text = MyScore + " : " + EnScore;
        TimeSecond = 60;
        //Ball.transform.position = Brd.transform.position;
        Ball.transform.localPosition = Vector3.zero;
        //Brd.transform.Translate(0, 0, 0);        
        StartCoroutine(NewGame());
    }
    private void Update()
    {
        if (!GameOver && NewGameSet==true)
        {
            TimeSecond = TimeSecond - Time.deltaTime;
            DisplayTimer();
        }
        if (GameOver==true)
        {
            if(Input.anyKey)
            {
                StartCoroutine(GameOverScene());
            }
        }



    }
    public void AddMyScore()
    {
        Resetball();
        MyScore++;
        ScoreTxt.text = MyScore + " : " + EnScore;
        BallScript ballScript = Ball.GetComponent<BallScript>();
        ballScript.StopAllCoroutines();
        ballScript.StartCoroutine(ballScript.bibigi());
        ballScript.StartCoroutine(ballScript.puser());
    }
    public void AddEnScore()
    {
        Resetball();
        EnScore++;
        ScoreTxt.text = MyScore + " : " + EnScore;
        BallScript ballScript = Ball.GetComponent<BallScript>();
        ballScript.StopAllCoroutines();
        ballScript.StartCoroutine(ballScript.bibigi());
        ballScript.StartCoroutine(ballScript.puser());
    }
    public void Resetball()
    {
        Ball.transform.localPosition = Vector3.zero;
        //Brd.transform.Translate(0, 0, 0);
        Brd.velocity = new Vector2(0f, 0f);
        foreach (var player in AllPlayer)
        {
            player.transform.localPosition = Vector3.zero;
            
        }

    }



    public void DisplayTimer()
    {
        Seconds = Mathf.FloorToInt(TimeSecond);
        if (Seconds <=0)
        {
            TimeOver();
           
        }
        if(Seconds >= 0)
        {
            Timer.text = "TIMER : " + Seconds;
        }
        
       
    }
    public void TimeOver()
    {
        Saver.ScoreUpdata();
        Saver.Save3();
        Arrow.SetActive(false);
        Ball.SetActive(false);

        foreach (var player in AllPlayer)
        {
            player.SetActive(false);
        }
        GameOver = true;
        Win.SetActive(true);
        


    }
    IEnumerator GameOverScene()
    {


        Rigidbody2D rd = BlackB.GetComponent<Rigidbody2D>();
        rd.constraints = RigidbodyConstraints2D.None;
        rd.gravityScale = 6f;
        StartCoroutine(StartFade(AudioSource, 2f, 0));
        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene("Main");
        Debug.Log("메인불러오기");
    }


   IEnumerator NewGame()
    {

        foreach(var player in AllPlayer)
        {
        player.GetComponent<AutoMove>().enabled = false;
        }
        PlayerMove plaeyrmoveM = PlayerManager.GetComponent<PlayerMove>();
        plaeyrmoveM.enabled = false;
        BallScript BallS = Ball.GetComponent<BallScript>();
        BallS.enabled = false;
        yield return new WaitForSecondsRealtime(5f);

        foreach (var player in AllPlayer)
        {
            player.GetComponent<AutoMove>().enabled = true;
        }
        plaeyrmoveM.enabled = true;
        BallS.enabled = true;
        NewGameSet = true;
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
