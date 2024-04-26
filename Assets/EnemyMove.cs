//using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

//using Unity.VisualScripting;
using UnityEngine;
//using static UnityEngine.GraphicsBuffer;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyMove : MonoBehaviour
{
    public bool grogi = false; //그로기 상태 여부 체크
    public bool Atk = false; //그로기 상태 여부 체크
                             //ㄴ 흠 플레이어가 조작하는게 아닌 ai 조작인데 그로기걸리면 그냥 인보크 넣어서 못움직이게 해버리는게 나을지도모르겠는데
    public bool slide = false;//슬라이딩중인지
    public bool Player = false;
    public bool GetBall = false;
    public bool BoxInBall = false;
    public float speed = 20.0f;//기본스피드
    public float kickspeed = 20f;
    public float Maxyspeed = 20.0f;
    public float Maxxspeed = 20.0f;

    public GameObject Gallzone;
    public GameObject Enemy;
    public float radius = 8;
    public Vector2 BoxSize = new Vector2(3.5f, 4.5f);
    public Collider2D[] Ptco;
    public Collider2D[] Kickco;
    public Vector2 EnemyPo;
    public Animator anima;
    public CapsuleCollider2D cd;

    void OnEnable()
    {
        Ptco = Physics2D.OverlapCircleAll(transform.position, radius);
        Kickco = Physics2D.OverlapBoxAll(transform.position, BoxSize, 0);
        Rigidbody2D rd = Enemy.GetComponent<Rigidbody2D>();
        anima = Enemy.GetComponent<Animator>();
        cd = Enemy.GetComponent<CapsuleCollider2D>();
        grogi = false;
        Atk = false;
        slide = false;
        Player = false;
        rd.simulated = true;
        cd.enabled = true;
        rd.velocity = new Vector2(0f, 0f);
        gameObject.tag = "Enemy";
        cd.isTrigger = false;
        anima.SetBool("slide", false);
        anima.SetBool("kick", false);
        anima.SetBool("walk", false);
        anima.SetBool("idle", true);

    }

    private void Update()
    {
        Rigidbody2D rd = Enemy.GetComponent<Rigidbody2D>();
        Ptco = Physics2D.OverlapCircleAll(transform.position, radius);
        Kickco = Physics2D.OverlapBoxAll(transform.position, BoxSize, 0);

        SearchingBall();
        SearchingEnemy();
        if (GetBall == true && Player == true)
        {
            EnemyAndBall(); //공을들고 적 골대로갈때 적을 만날시 회피기동하는 함수
            //Debug.Log(Enemy.name + "EnemyAndBall");
        }
        else if (GetBall == true && Player == false)
        {
            InBall();   //적이없을때 직선으로 골대로 달려가는 함수
            //Debug.Log(Enemy.name + "InBall");
        }
        else if (GetBall == false && BoxInBall == true)//공을 못 얻었는데 영역안에 공이 있을시
        {

            foreach (Collider2D col in Ptco)
            {
                if (col.CompareTag("Ball"))//태그영역안에 공이있을시에
                {
                    //Debug.Log(Enemy.name + "GetBall = F, inBall");

                    //transform.position = Vector3.MoveTowards(transform.position, col.transform.position, speed * Time.deltaTime);


                    // Rigidbody 컴포넌트를 얻어옵니다.
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();

                    // 목표 위치와 현재 위치의 차이를 계산합니다.
                    Vector3 direction = col.transform.position - transform.position;

                    // 만약 방향이 0이 아니면 정규화하여 방향을 구합니다.
                    if (direction != Vector3.zero)
                    {
                        direction.Normalize();
                    }

                    // 힘을 계산합니다. 속도와 방향을 곱해줍니다.
                    Vector3 force = direction * speed;

                    // Rigidbody에 힘을 추가합니다.
                    rb.AddForce(force * Time.deltaTime);






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
        }

        if (transform.localPosition == Vector3.zero)
        {
            anima.SetBool("slide", false);
            anima.SetBool("kick", false);
            anima.SetBool("walk", false);
            anima.SetBool("idle", true);
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

        foreach (Collider2D col in Ptco)
        {
            if (col.CompareTag("Ball"))
            {
                BoxInBall = true;
            }

            if (col.CompareTag("Player"))
            {
                Player = true;
                EnemyPo = col.transform.position;
                break;
            }
            else
            {
                Player = false;
            }
        }


    }

    public void InBall()
    {
        if (transform.position.x < Gallzone.transform.position.x)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (transform.position.x > Gallzone.transform.position.x)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        //transform.position = Vector3.MoveTowards(transform.position, Gallzone.transform.position, speed * Time.deltaTime);
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // 목표 위치와 현재 위치의 차이를 계산합니다.
        Vector3 direction = Gallzone.transform.position - transform.position;

        // 만약 방향이 0이 아니면 정규화하여 방향을 구합니다.
        if (direction != Vector3.zero)
        {
            direction.Normalize();
        }

        // 힘을 계산합니다. 속도와 방향을 곱해줍니다.
        Vector3 force = direction * speed;

        // Rigidbody에 힘을 추가합니다.
        rb.AddForce(force * Time.deltaTime);
        //Debug.Log(transform.position);
        anima.SetBool("walk", true);
        anima.SetBool("idle", false);
        float distance = Vector3.Distance(transform.position, Gallzone.transform.position);
        //Debug.Log(distance);
        if (distance < 25)
        {
            kick();
            //Debug.Log(name+"kick실행");
        }
    }
    public void EnemyAndBall()
    {
        Rigidbody2D rd = new Rigidbody2D();
        foreach (Collider2D col in Ptco)
        {
            if (col.CompareTag("Player"))
            {
                //transform.position=Vector2.Lerp(col.transform.position, Gallzone.transform.position, 1);


                Vector2 gap = col.transform.position + Gallzone.transform.position;
                Vector2 moveMovement = gap * speed * Time.deltaTime;
                if (transform.position.x < moveMovement.x)
                {
                    transform.localScale = new Vector2(1, 1);
                    
                }
                else if (transform.position.x > moveMovement.x)
                {
                    transform.localScale = new Vector2(-1, 1);
                }
                Debug.Log($"{gap.x},{gap.y}");
                Debug.Log(moveMovement);
                anima.SetBool("walk", true);
                anima.SetBool("idle", false);
                //transform.Translate(moveMovement);
                rd.AddForce(moveMovement);
                float distance = Vector3.Distance(transform.position, Gallzone.transform.position);
                Debug.Log(distance);
                if (distance < 25)
                {
                    kick();
                    Debug.Log(name + "kick실행");
                }
            }


        }
    }



    public void BoxinballAndEnemy()
    {
        string[] tags = { "Enemy", "Player", "Ball" };
        bool BoxinBall = false;
        int Count = 0;
        Vector3 BallVector = Vector2.zero;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

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
            // 목표 위치와 현재 위치의 차이를 계산합니다.
            Vector3 direction = BallVector - transform.position;

            // 만약 방향이 0이 아니면 정규화하여 방향을 구합니다.
            if (direction != Vector3.zero)
            {
                direction.Normalize();
            }

            // 힘을 계산합니다. 속도와 방향을 곱해줍니다.
            Vector3 force = direction * speed;

            // Rigidbody에 힘을 추가합니다.
            rb.AddForce(force * Time.deltaTime);

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
                Vector2 Togall = (Gallzone.transform.position - transform.position).normalized;
                //Debug.Log(col.name);
                Vector2 force = Togall * kickspeed;
                colRigidbody.AddForce(force);//, ForceMode2D.Impulse);
                //Debug.Log("킥실행으로인한벨로시티변화");
                cd.isTrigger = true;
            }
        }
        grogi = true;
        tag = "Kicking";
        Atk = true;
        Invoke("kickExit", 1f);
    }
    void kickExit()
    {

        anima.SetBool("kick", false);
        tag = "Enemy";
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
            Debug.Log(Enemy.name + "집간다");
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
        Rigidbody2D rd = Enemy.GetComponent<Rigidbody2D>();
        //Debug.Log("복귀시작");
        //Debug.Log(Enemy.name + "집간다");
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
        Vector3 direction = Vector3.zero - transform.localPosition;
        if (direction != Vector3.zero)
        {
            direction.Normalize();
        }
        // 힘을 계산합니다. 속도와 방향을 곱해줍니다.
        Vector3 force = direction * speed;

        // Rigidbody에 힘을 추가합니다. 
        rd.AddForce(force * Time.deltaTime);

        
        if (transform.position.x < transform.localPosition.x)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (transform.position.x > transform.localPosition.x)
        {
            transform.localScale = new Vector2(-1, 1);
        }



    }
}

