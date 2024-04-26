////using JetBrains.Annotations;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;

////using Unity.VisualScripting;
//using UnityEngine;
////using static UnityEngine.GraphicsBuffer;
////using static UnityEngine.RuleTile.TilingRuleOutput;

//public class EnemyMove : MonoBehaviour
//{
//    public bool grogi = false; //�׷α� ���� ���� üũ
//    public bool Atk = false; //�׷α� ���� ���� üũ
//                             //�� �� �÷��̾ �����ϴ°� �ƴ� ai �����ε� �׷α�ɸ��� �׳� �κ�ũ �־ �������̰� �ع����°� ���������𸣰ڴµ�
//    public bool slide = false;//�����̵�������
//    public bool Player = false;
//    public bool GetBall = false;
//    public bool BoxInBall = false;
//    public float speed = 20.0f;//�⺻���ǵ�
//    public float kickspeed = 20f;

//    public GameObject Gallzone;
//    public GameObject Enemy;
//    public float radius = 8;
//    public Vector2 BoxSize = new Vector2(3.5f, 4.5f);
//    public Collider2D[] Ptco;
//    public Collider2D[] Kickco;
//    public Vector2 EnemyPo;
//    public Animator anima;
//    public CapsuleCollider2D cd;

//    void OnEnable()
//    {
//        Ptco = Physics2D.OverlapCircleAll(transform.position, radius);
//        Kickco = Physics2D.OverlapBoxAll(transform.position, BoxSize, 0);
//        Rigidbody2D rd = Enemy.GetComponent<Rigidbody2D>();
//        anima = Enemy.GetComponent<Animator>();
//        cd = Enemy.GetComponent<CapsuleCollider2D>();
//        grogi = false;
//        Atk = false;
//        slide = false;
//        Player = false;
//        rd.simulated = true;
//        cd.enabled = true;
//        rd.velocity = new Vector2(0f, 0f);
//        gameObject.tag = "Enemy";
//        cd.isTrigger = false;
//        anima.SetBool("slide", false);
//        anima.SetBool("kick", false);
//        anima.SetBool("walk", false);
//        anima.SetBool("idle", true);

//    }

//    private void Update()
//    {
//        Rigidbody2D rd = Enemy.GetComponent<Rigidbody2D>();
//        Ptco = Physics2D.OverlapCircleAll(transform.position, radius);
//        Kickco = Physics2D.OverlapBoxAll(transform.position, BoxSize, 0);

//        SearchingBall();
//        SearchingEnemy();
//        if (GetBall == true && Player == true)
//        {
//            EnemyAndBall(); //������� �� ���ΰ��� ���� ������ ȸ�Ǳ⵿�ϴ� �Լ�
//            //Debug.Log(Enemy.name + "EnemyAndBall");
//        }
//        else if (GetBall == true && Player == false)
//        {
//            InBall();   //���̾����� �������� ���� �޷����� �Լ�
//            //Debug.Log(Enemy.name + "InBall");
//        }
//        else if (GetBall == false && BoxInBall == true)//���� �� ����µ� �����ȿ� ���� ������
//        {

//            foreach (Collider2D col in Ptco)
//            {
//                if (col.CompareTag("Ball"))//�±׿����ȿ� ���������ÿ�
//                {
//                    //Debug.Log(Enemy.name + "GetBall = F, inBall");

//                    transform.position = Vector3.MoveTowards(transform.position, col.transform.position, speed * Time.deltaTime);
//                    if (transform.position.x < col.transform.position.x)
//                    {
//                        transform.localScale = new Vector2(1, 1);
//                    }
//                    else if (transform.position.x > col.transform.position.x)
//                    {
//                        transform.localScale = new Vector2(-1, 1);
//                    }
//                    anima.SetBool("walk", true);
//                    anima.SetBool("idle", false);
//                    break;
//                }
//            }
//        }
//        else//����
//        {
//            GoHome();
//        }

//        if (transform.localPosition == Vector3.zero)
//        {
//            anima.SetBool("slide", false);
//            anima.SetBool("kick", false);
//            anima.SetBool("walk", false);
//            anima.SetBool("idle", true);
//        }

//        anima.SetBool("idle", true);
//    }

//    //�� �̰Ÿ� �����ϴ� ����� �ΰ����ִµ�
//    // �ϳ��� swicth�� �޾ƿͼ� �����ϴ� ���
//    //�ϳ��� for���� ���ؼ� �޾ƿµڿ� �ش� �ż��常 �����ϰ� break �� �ع����� ���� �ż��带 �������� �ʴ� ���
//    //�����ϳ��� �����ִµ� ���ΰ��°� ������
//    //�̰� ������� GPT���� ������? ����
//    // ����
//    //������ �������� if�����ؼ�
//    //Enemy+Ball �� true�϶�
//    //Ball�� true �϶�
//    //Enemy�� true �϶�
//    //�� 3���� ���¿� ���� �����Ǵ� �޼��带 �����ϰ�
//    //if���� ��������� ��ܿ� ����������� ����ϰ��Ѵ�

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, radius);//Ž������

//        Gizmos.color = Color.blue;
//        Gizmos.DrawWireCube(transform.position, BoxSize);//ű���ɿ���
//    }


//    public void SearchingBall()
//    {
//        Kickco = Physics2D.OverlapBoxAll(transform.position, BoxSize, 0); //kickco��� ű�����ȿ� �ö��̴��� �����ϴ� �迭�� �簪�� �ʱ�ȭ�Ѵ�.
//        foreach (Collider2D col in Kickco)
//        {
//            if (col.CompareTag("Ball"))//���� Ball�� ��� �±��� �ö��̴��� �����Ѵٸ� GetBall���¸� true�� ����� forcach������
//            {
//                //Debug.Log(col.tag);
//                GetBall = true;
//                break;
//            }
//            else
//            {
//                GetBall = false;
//            }
//        }
//    }

//    public void SearchingEnemy()
//    {
//        Ptco = Physics2D.OverlapCircleAll(transform.position, radius);
//        BoxInBall = false;

//        foreach (Collider2D col in Ptco)
//        {
//            if (col.CompareTag("Ball"))
//            {
//                BoxInBall = true;
//            }

//            if (col.CompareTag("Player"))
//            {
//                Player = true;
//                EnemyPo = col.transform.position;
//                break;
//            }
//            else
//            {
//                Player = false;
//            }
//        }


//    }

//    public void InBall()
//    {
//        if (transform.position.x < Gallzone.transform.position.x)
//        {
//            transform.localScale = new Vector2(1, 1);
//        }
//        else if (transform.position.x > Gallzone.transform.position.x)
//        {
//            transform.localScale = new Vector2(-1, 1);
//        }
//        transform.position = Vector3.MoveTowards(transform.position, Gallzone.transform.position, speed * Time.deltaTime);
//        //Debug.Log(transform.position);
//        anima.SetBool("walk", true);
//        anima.SetBool("idle", false);
//        float distance = Vector3.Distance(transform.position, Gallzone.transform.position);
//        //Debug.Log(distance);
//        if (distance < 25)
//        {
//            kick();
//            //Debug.Log(name+"kick����");
//        }
//    }
//    public void EnemyAndBall()
//    {
//        foreach (Collider2D col in Ptco)
//        {
//            if (col.CompareTag("Player"))
//            {
//                //transform.position=Vector2.Lerp(col.transform.position, Gallzone.transform.position, 1);


//                Vector2 gap = col.transform.position + Gallzone.transform.position;

//                if (col.transform.position.y > transform.position.y)//����� y���� ������ ���϶� 
//                {
//                    gap.y = -1;
//                }
//                else if (col.transform.position.y < transform.position.y)//����� y���� ������ ������ ���� ���ΰ�
//                {
//                    gap.y = 1;
//                }

//                ///////////////////////////////////////////////////////////////////////
//                if (col.transform.position.x > transform.position.x)
//                {

//                    gap.x = -1;
//                    //transform.localScale = new Vector2(1f, 1f);

//                }
//                else if (col.transform.position.x < transform.position.x)
//                {
//                    //transform.localScale = new Vector2(-1f, 1f);
//                    gap.x = 1;
//                }
//                //////////////////////////////////////////////////////////////////////////////
//                if (gap.y > 0)
//                {
//                    gap.y = 0.7f;
//                }
//                else if (gap.y < 0)
//                {
//                    gap.y = -0.7f;
//                }
//                ///////////////////////////////////////////////////////////////////////////
//                if (gap.x > 0)
//                {
//                    gap.x = 0.7f;
//                }
//                else if (gap.x < 0)
//                {
//                    gap.x = 0.7f;
//                }

//                Vector2 moveMovement = gap * speed * Time.deltaTime;
//                if (transform.position.x < moveMovement.x)
//                {
//                    transform.localScale = new Vector2(1, 1);
//                }
//                else if (transform.position.x > moveMovement.x)
//                {
//                    transform.localScale = new Vector2(-1, 1);
//                }
//                Debug.Log($"{gap.x},{gap.y}");
//                Debug.Log(moveMovement);
//                anima.SetBool("walk", true);
//                anima.SetBool("idle", false);
//                transform.Translate(moveMovement);
//                float distance = Vector3.Distance(transform.position, Gallzone.transform.position);
//                Debug.Log(distance);
//                if (distance < 25)
//                {
//                    kick();
//                    Debug.Log(name + "kick����");
//                }
//            }


//        }
//    }


//    public void BoxinballAndEnemy()
//    {
//        string[] tags = { "Enemy", "Player", "Ball" };
//        bool BoxinBall = false;
//        int Count = 0;
//        Vector2 BallVector = Vector2.zero;
//        foreach (Collider2D col in Kickco)
//        {
//            if (col.CompareTag("Ball"))
//            {
//                BoxinBall = true;
//                BallVector = col.transform.position;
//                Count++;

//            }
//            else if (col.CompareTag("Player"))
//            {

//                Count++;
//            }
//            else if (col.CompareTag("Enemy"))
//            {
//                Count++;
//            }
//        }
//        if (Count >= 3 && BoxinBall)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, BallVector, speed * Time.deltaTime);
//            if (transform.position.x < BallVector.x)
//            {
//                transform.localScale = new Vector2(1, 1);
//            }
//            else if (transform.position.x > BallVector.x)
//            {
//                transform.localScale = new Vector2(-1, 1);
//            }
//        }
//    }

//    public void kick()
//    {
//        anima.SetBool("kick", true);
//        foreach (Collider2D col in Kickco)
//        {
//            if (col.CompareTag("Ball"))//�±װ� Ball�� ������Ʈ���Ը�
//            {
//                Rigidbody2D colRigidbody = col.GetComponent<Rigidbody2D>();
//                Vector2 Togall = (Gallzone.transform.position - transform.position).normalized;
//                //Debug.Log(col.name);
//                Vector2 force = Togall * kickspeed;
//                colRigidbody.AddForce(force);//, ForceMode2D.Impulse);
//                //Debug.Log("ű�����������Ѻ��ν�Ƽ��ȭ");
//                cd.isTrigger = true;
//            }
//        }
//        grogi = true;
//        tag = "Kicking";
//        Atk = true;
//        Invoke("kickExit", 1f);
//    }
//    void kickExit()
//    {

//        anima.SetBool("kick", false);
//        tag = "Enemy";
//        //cd.enabled = true;
//        //rd.simulated = true;
//        //�±׸� �����ϴ� ������ �ذ��߱⶧���� ���ʿ�������
//        grogi = false;
//        Atk = false;
//        cd.isTrigger = false;

//    }

//    IEnumerator puser()
//    {
//        Vector3 AP = transform.localPosition;
//        yield return new WaitForSecondsRealtime(2f);
//        if (AP == transform.localPosition)
//        {
//            Debug.Log(Enemy.name + "������");
//            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
//            if (transform.position.x < transform.localPosition.x)
//            {
//                transform.localScale = new Vector2(1, 1);
//            }
//            else if (transform.position.x > transform.localPosition.x)
//            {
//                transform.localScale = new Vector2(-1, 1);
//            }
//            yield return new WaitForSecondsRealtime(1.5f);
//        }
//    }

//    void GoHome()
//    {
//        Rigidbody2D rd = Enemy.GetComponent<Rigidbody2D>();

//        //Debug.Log("���ͽ���");
//        //Debug.Log(Enemy.name + "������");
//        transform.localPosition = Vector3.MoveTowards(transform.localPosition, Vector3.zero, speed * Time.deltaTime);
//        rd.velocity = new Vector3(0f, 0f, 0f);
//        if (transform.position.x < transform.localPosition.x)
//        {
//            transform.localScale = new Vector2(1, 1);
//        }
//        else if (transform.position.x > transform.localPosition.x)
//        {
//            transform.localScale = new Vector2(-1, 1);
//        }



//    }
//}

