//using JetBrains.Annotations;
using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using Unity.VisualScripting;
using UnityEngine;
//using static UnityEngine.GraphicsBuffer;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class AutoMove : MonoBehaviour
{

    enum stat
    {
        GetBall, Patrol, enemy,
    }

    public bool grogi = false; //그로기 상태 여부 체크
    public bool Atk = false; //그로기 상태 여부 체크
                             //ㄴ 흠 플레이어가 조작하는게 아닌 ai 조작인데 그로기걸리면 그냥 인보크 넣어서 못움직이게 해버리는게 나을지도모르겠는데
    public bool slide = false;//슬라이딩중인지
    public bool Enemy = false;
    public bool GetBall = false;
    public bool BoxInBall = false;
    public float speed = 20.0f;//기본스피드
    public float kickspeed = 20f;
    public bool ENK;

    public GameObject gallzone;
    public GameObject player;
    public float radius = 8;
    public Vector2 BoxSize = new Vector2(3.5f, 4.5f);
    public Collider2D[] Ptco;
    public Collider2D[] Kickco;
    public Vector2 EnemyPo;
    public Animator anima;
    public CapsuleCollider2D cd;

    public Transform pointA; // 벡터 A의 위치
    public Transform pointB; // 벡터 B의 위치
    //private LineRenderer lineRenderer;



    void OnEnable()
    {
        Ptco = Physics2D.OverlapCircleAll(transform.position, radius);
        Kickco = Physics2D.OverlapBoxAll(transform.position, BoxSize, 0);
        Rigidbody2D rd = player.GetComponent<Rigidbody2D>();
        anima = player.GetComponent<Animator>();
        cd = player.GetComponent<CapsuleCollider2D>();
        grogi = false;
        Atk = false;
        slide = false;
        Enemy = false;
        rd.simulated = true;
        cd.enabled = true;
        rd.velocity = new Vector2(0f, 0f);
        gameObject.tag = "Player";
        cd.isTrigger = false;
        anima.SetBool("slide", false);
        anima.SetBool("kick", false);
        anima.SetBool("walk", false);
        anima.SetBool("idle", true);

    }

    private void Update()
    {
        Rigidbody2D rd = player.GetComponent<Rigidbody2D>();
        Ptco = Physics2D.OverlapCircleAll(transform.position, radius);
        Kickco = Physics2D.OverlapBoxAll(transform.position, BoxSize, 0);

        SearchingBall();
        SearchingEnemy();

        if (GetBall == true && Enemy == true)
        {
            EnemyAndBall(); //공을들고 적 골대로갈때 적을 만날시 회피기동하는 함수
            //Debug.Log(Enemy.name + "EnemyAndBall");
        }
        else if (GetBall == true && Enemy == false)
        {
            InBall();   //적이없을때 직선으로 골대로 달려가는 함수
            //Debug.Log(Enemy.name + "InBall");
        }
        //else if (GetBall == false && BoxInBall == true)//공을 못 얻었는데 영역안에 공이 있을시
        else if (BoxInBall == true)//공을 못 얻었는데 영역안에 공이 있을시
          {

            foreach (Collider2D col in Ptco)
            {
                if (col.CompareTag("Ball"))//태그영역안에 공이있을시에
                {
                    //Debug.Log(Enemy.name + "GetBall = F, inBall");

                    transform.position = Vector3.MoveTowards(transform.position, col.transform.position, speed * Time.deltaTime);
                    if (transform.position.x < col.transform.position.x)
                    {
                        transform.localScale = new Vector2(1, 1);
                    }
                    else if (transform.position.x > col.transform.position.x)
                    {
                        transform.localScale = new Vector2(-1, 1);
                    }
                    anima.SetBool("walk", true);
                    anima.SetBool("idle", false);
                    break;
                }
            }
        }
        else//복귀
        {
            GoHome();
            anima.SetBool("walk", true);
        }
        if (transform.localPosition == Vector3.zero)
        {
            anima.SetBool("slide", false);
            anima.SetBool("kick", false);
            anima.SetBool("walk", false);
            anima.SetBool("idle", true);

            if (name == "Enemy")
            {
                transform.localScale = new Vector3(-1, 1);
            }
            else if (name == "Player")
            {
                transform.localScale = new Vector3(1, 1);

            }


        }

        anima.SetBool("idle", true);
    }

    //흠 이거를 조작하는 방법이 두개가있는데
    // 하나는 swicth로 받아와서 조작하는 방법
    //하나는 for문을 통해서 받아온뒤에 해당 매서드만 실행하고 break 를 해버려서 하위 매서드를 실행하지 않는 방식
    //둘중하나로 갈수있는데 뭐로가는게 좋을까
    //이건 문법고수 GPT한태 물어볼까요? ㅋㅋ
    // ㅇㅋ
    //앞으로 적을꺼는 if을통해서
    //Enemy+Ball 이 true일때
    //Ball만 true 일때
    //Enemy만 true 일때
    //총 3가지 상태에 따른 대응되는 메서드를 구성하고서
    //if문의 실행순서를 상단에 적은순서대로 출력하게한다

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);//탐색영역

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, BoxSize);//킥가능영역
    }


    public void SearchingBall()
    {
        Kickco = Physics2D.OverlapBoxAll(transform.position, BoxSize, 0); //kickco라는 킥범위안에 컬라이더를 수집하는 배열에 든값을 초기화한다.
        foreach (Collider2D col in Kickco)
        {
            if (col.CompareTag("Ball"))//만약 Ball이 라는 태그의 컬라이더가 존재한다면 GetBall상태를 true로 만들고 forcach을종료
            {
                //Debug.Log(col.tag);
                GetBall = true;
                break;
            }
            else
            {
                GetBall = false;
            }
        }
    }

    public void SearchingEnemy()
    {
        Ptco = Physics2D.OverlapCircleAll(transform.position, radius);
        BoxInBall = false;
        Enemy = false;

        foreach (Collider2D col in Ptco)
        {
            if (col.CompareTag("Ball"))
            {
                BoxInBall = true;
            }

            if (col.CompareTag("Enemy"))
            {
                Enemy = true;
                EnemyPo = col.transform.position;
                //break;
            }
            //else
            //{
            //    Enemy = false;
            //}
        }


    }

    public void InBall()
    {
        //if (transform.position.x < gallzone.transform.position.x)
        //{
        //    transform.localScale = new Vector2(1, 1);
        //}
        //else if (transform.position.x > gallzone.transform.position.x)
        //{
        //    transform.localScale = new Vector2(-1, 1);
        //}
        if (transform.localPosition.x < gallzone.transform.position.x)

        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (transform.localPosition.x > gallzone.transform.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
        }


        transform.position = Vector3.MoveTowards(transform.position, gallzone.transform.position, speed * Time.deltaTime);
        //Debug.Log(transform.position);
        anima.SetBool("walk", true);
        anima.SetBool("idle", false);
        float distance = Vector3.Distance(transform.position, gallzone.transform.position);
        //Debug.Log(distance);
        if (distance < 25)
        {
            kick();
            //Debug.Log(name+"kick실행");
        }
    }
    public void EnemyAndBall()
    {

        //if(tag=="player"){
            foreach (Collider2D col in Ptco)
            {
                if (tag == "player" && col.CompareTag("Enemy"))

                {
                    //transform.position=Vector2.Lerp(col.transform.position, Gallzone.transform.position, 1);


                    Vector2 gap = col.transform.position + gallzone.transform.position;

                    if (col.transform.position.y > transform.position.y)//상대의 y값이 나보다 위일때 
                    {
                        gap.y = -1;
                    }
                    else if (col.transform.position.y < transform.position.y)//상대의 y값이 나보다 낮을때 나는 위로감
                    {
                        gap.y = 1;
                    }

                    ///////////////////////////////////////////////////////////////////////
                    if (col.transform.position.x > transform.position.x)
                    {

                        gap.x = -1;
                        //transform.localScale = new Vector2(1f, 1f);

                    }
                    else if (col.transform.position.x < transform.position.x)
                    {
                        //transform.localScale = new Vector2(-1f, 1f);
                        gap.x = 1;
                    }
                    //////////////////////////////////////////////////////////////////////////////
                    if (gap.y > 0)
                    {
                        gap.y = 0.7f;
                    }
                    else if (gap.y < 0)
                    {
                        gap.y = -0.7f;
                    }
                    ///////////////////////////////////////////////////////////////////////////
                    if (gap.x > 0)
                    {
                        gap.x = 0.7f;
                    }
                    else if (gap.x < 0)
                    {
                        gap.x = 0.7f;
                    }

                    Vector2 moveMovement = gap * speed * Time.deltaTime;
                    if (transform.position.x < moveMovement.x)
                    {
                        transform.localScale = new Vector2(1, 1);
                    }
                    else if (transform.position.x > moveMovement.x)
                    {
                        transform.localScale = new Vector2(-1, 1);
                    }
                    //Debug.Log($"{gap.x},{gap.y}");
                    //Debug.Log(moveMovement);
                    //Debug.Log("EnemyAndBall");
                    anima.SetBool("walk", true);
                    anima.SetBool("idle", false);
                    transform.Translate(moveMovement);

                    float distance = Vector3.Distance(transform.position, gallzone.transform.position);
                    //Debug.Log(distance);
                    if (distance < 25)
                    {
                        kick();
                        //Debug.Log(name+"kick실행");
                    }
                }
            //}

        }

        //else if (tag=="Enemy")
        //{ 
        //}
    }


    public void BoxinballAndEnemy()
    {
        string[] tags = { "Enemy", "Player", "Ball" };
        bool BoxinBall = false;
        int Count = 0;
        Vector2 BallVector = Vector2.zero;
        foreach (Collider2D col in Kickco)
        {
            if (col.CompareTag("Ball"))
            {
                BoxinBall = true;
                BallVector = col.transform.position;
                Count++;

            }
            else if (col.CompareTag("Player"))
            {

                Count++;
            }
            else if (col.CompareTag("Enemy"))
            {
                Count++;
            }
        }
        if (Count >= 3 && BoxinBall)
        {
            transform.position = Vector3.MoveTowards(transform.position, BallVector, speed * Time.deltaTime);
            if (transform.position.x < BallVector.x)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else if (transform.position.x > BallVector.x)
            {
                transform.localScale = new Vector2(-1, 1);
            }
        }
    }

    public void kick()
    {
        anima.SetBool("kick", true);
        foreach (Collider2D col in Kickco)
        {
            if (col.CompareTag("Ball"))//태그가 Ball인 오브젝트에게만
            {
                Rigidbody2D colRigidbody = col.GetComponent<Rigidbody2D>();
                Vector2 Togall = (gallzone.transform.position - transform.position).normalized;
                //Debug.Log(col.name);
                Vector2 force = Togall * kickspeed;
                colRigidbody.AddForce(force);//, ForceMode2D.Impulse);
                //Debug.Log("킥실행으로인한벨로시티변화");
                cd.isTrigger = true;
            }
        }
        grogi = true;

        if (CompareTag("Enemy"))
        {
            ENK = true;
        }
        tag = "Kicking";
        Atk = true;
        Invoke("kickExit", 1f);
    }
    void kickExit()
    {

        anima.SetBool("kick", false);
        tag = "Player";
        if (ENK == true)
        {
            tag = "Enemy";
        }
        //cd.enabled = true;
        //rd.simulated = true;
        //태그를 수정하는 식으로 해결했기때문에 불필요함이제
        grogi = false;
        Atk = false;
        cd.isTrigger = false;

    }

    IEnumerator puser()
    {
        Vector3 AP = transform.localPosition;
        yield return new WaitForSecondsRealtime(2f);
        if (AP == transform.localPosition)
        {
            Debug.Log(player.name + "집간다");
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
            if (transform.position.x < transform.localPosition.x)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else if (transform.position.x > transform.localPosition.x)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            yield return new WaitForSecondsRealtime(1.5f);
        }
    }

    void GoHome()
    {
        Rigidbody2D rd = player.GetComponent<Rigidbody2D>();

        Vector3 zero = Vector3.zero;
        if (transform.localPosition.x > zero.x) // 월드 좌표 x가 현재 포지션 x보다 작을 때
        {
            transform.localScale = new Vector2(-1, 1); // 좌우 반전
            //Debug.Log(transform.position + " 월드 포지션이 현재 포지션보다 클 때" + transform.localPosition);
        }
        else if (transform.localPosition.x < zero.x) // 월드 좌표 x가 현재 포지션 x보다 큰 때
        {
            transform.localScale = new Vector2(1, 1); // 좌우 반전 해제
            //Debug.Log(transform.position + " 월드 포지션이 현재 포지션보다 작을 때" + transform.localPosition);
        }



        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
        rd.velocity = new Vector3(0f, 0f, 0f);





    }
}

