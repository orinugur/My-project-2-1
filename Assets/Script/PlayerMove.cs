//using JetBrains.Annotations;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
using SpriteGlow;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Experimental.GraphView.GraphView;
//using static UnityEditor.PlayerSettings;

public class PlayerMove : MonoBehaviour
{
    enum stat
    {
        grogi,idle,Rec,Moving,onhead
    }

    public bool grogi = false; //�׷α� ���� ���� üũ
    public bool Atk =false; //�׷α� ���� ���� üũ
    public bool playing;//playing������ �ƴ���
    public float speed = 20.0f;//�⺻���ǵ�

    public Animator animator; //�ִϸ�����
    private Vector2 inputMovement = Vector2.zero;
    public GameObject[] Players; //�÷��̾��� ����� �Ҵ�� �迭�� ���ӿ�����Ʈ
    public int selectedPlayerIndex = 0; //� �÷��̾ ��������
    public GameObject currentPlaying;//���� ���õ� �÷��̾�
    public GameObject ball;
    public GameObject Arrow;
    public Vector2 BoxSize;
    //public Transform pos;
    public float kickspeed = 35;
    public Vector3 japo;

    Rigidbody2D rb;
    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        grogi = false;//�׷α� ���µ��� false

        ChangePlayer(0);//���� ���۽� ù��° �÷��̾ �����ϵ���
        Arrow.SetActive(true);
        Arrow.transform.position = currentPlaying.transform.position + japo;

    }
    private void LateUpdate()
    {
        Arrow.transform.position = currentPlaying.transform.position + japo;
    }
    void Update()
    {
        
        Animator anima = currentPlaying.GetComponent<Animator>();
        Rigidbody2D rd = currentPlaying.GetComponent<Rigidbody2D>();
        Vector2 moveMovement = inputMovement * speed * Time.deltaTime;//�̵��ӵ�

      

        if (currentPlaying != null&&grogi==false)//���õ��÷��̾ �׷α���°��ƴҽ�
        {
            currentPlaying.transform.Translate(moveMovement);//�̵�
            
            if (Input.GetKeyDown(KeyCode.J))//�׷α���°� �ƴҶ� j�� �Է½�(��Ŭ)
            {
                grogi = true;   //�׷α� ���� Ȱ��ȭ
                anima.SetBool("slide", true);//�����̵�ִϸ��̼� Ȱ��ȭ
                if (currentPlaying.transform.localScale.x > 0)//�������� �̵����϶�
                {
                    rd.velocity = new Vector2(35f, 0f);
                    
                }
                else
                {
                    rd.velocity = new Vector2(-35f, 0f);//�������� �̵����϶�
                   
                }
                Debug.Log("slide");
                Invoke("SliedExit", 0.6f);//�����̵� ���ӽð� ����
                Debug.Log("rec����");
                Invoke("Rec", 1.6f);//1.6�ʵ� rec�� �����ϸ� grogi�� false����
            }
        }
        if(grogi==false)//�׷αⰡ Ǯ���� ���ӵ� ����
        {
            rd.velocity=new Vector2(0f,0f);
        }

        // 1~7�Է½ÿ� �ش��ϴ� ��ȣ�� �÷��̾ ���� �÷��̾�� ����
        //������ �÷��̾��� automove�� ��Ȱ��ȭ, ������ ������ �÷��̾��� automove�� Ȱ��ȭ
        //if (grogi == false)
        //{
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ChangePlayer(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                ChangePlayer(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                ChangePlayer(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                ChangePlayer(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                ChangePlayer(4);
            //else if (Input.GetKeyDown(KeyCode.Alpha6))
            //    ChangePlayer(5);
            //else if (Input.GetKeyDown(KeyCode.Alpha7))
            //    ChangePlayer(6);
        //}
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(currentPlaying.transform.position, BoxSize, 0);

    }

    private void OnKick(InputValue inputValue)
    {
        //Animator anima = currentPlaying.GetComponent<Animator>();

            kick();
        
    }

 
    private void OnMove(InputValue inputValue)
    {
        Animator anima = currentPlaying.GetComponent<Animator>();
        //���õ� �÷��̾��� �ִϸ�����
        inputMovement = inputValue.Get<Vector2>();
        //Vector2 sc = currentPlaying.transform.localScale;
       
        
        
        

        //��ǲ�޾ƿ� ��(vector2�� x,y��)
        if (grogi==true)//�׷αⰡ ���������� walk�� ���� idel �ִϸ��̼��� ��µǵ��� ����
        {
            anima.SetBool("walk", false);
        }
        if (grogi == false)
        {
            if (inputMovement.x < 0)//�Է¹���x�� ���� 0�����������(�����̵�)
            {
                currentPlaying.transform.localScale = new Vector2(-1, 1);
             
                //sc.x = currentPlaying.transform.localScale.x * -1f;
                //currentPlaying.transform.localScale = sc;
                //Debug.Log("inputMovement.x < 0,walk");
                anima.SetBool("walk", true);
                anima.SetBool("idle", false);
            }
            else if (inputMovement.x > 0)//�Է¹���x�� ���� 0���� Ŭ ���(�������̵�)
            {
                currentPlaying.transform.localScale = new Vector2(1, 1);
                //sc.x = Mathf.Abs(currentPlaying.transform.localScale.x) * 1f;
                //currentPlaying.transform.localScale = sc;
                //x�������� ����� �Ͽ� ������ ��������
                anima.SetBool("walk", true);
                anima.SetBool("idle", false);
                //Debug.Log("inputMovement.x > 0,walk");
            }
            else if (inputMovement.x == 0 && inputMovement.y == 0)
            {//x��y�� �Ѵ� 0�ϰ��
                //Debug.Log("inputMovement.x == 0,idle");
                anima.SetBool("idle", true);
                anima.SetBool("walk", false);
            }   
        }
    } 
      
    void ChangePlayer(int index) //�÷��̾� ����
    {
        selectedPlayerIndex = index;
        //������Ʈ���� ��ǲ�޾ƿ� Ű������ index�� ���������ѵڿ�
        //�迭���� �ش� ��ȣ�� ������ ȣ����
        if (selectedPlayerIndex < 0 || selectedPlayerIndex >= Players.Length)
        {//�÷��̾� �迭�� ��� �Է��� �ҽ� ����� �޼��� ���
            Debug.LogError("Invalid Enemy index");
            return;
        }

        for (int i = 0; i < Players.Length; i++)
        //���õ� �÷��̾��� AutoMove�� ��Ȱ��ȭ
        {
            if (i == selectedPlayerIndex)
            {
                //����Ʈ �÷��̾ �����ϱ��� ���� ���̴� �÷��̾��� AutoMove�� Ȱ��ȭ
                AutoMove beforeautoMoveScript = currentPlaying.GetComponent<AutoMove>();
                SpriteGlowEffect glower = currentPlaying.GetComponent<SpriteGlowEffect>();

                //��?�ٵ� �̷� start�� ���ʿ� currentPlaying�� ���� null ���ĵ� �����ȳ���?
                if (beforeautoMoveScript != null)
                {
                        beforeautoMoveScript.enabled = true;
                        glower.EnableInstancing = true;
                }

                currentPlaying = Players[selectedPlayerIndex];
                // ���� �÷��̾ �迭�� �÷��̾�� ����
                AutoMove autoMoveScript = Players[selectedPlayerIndex].GetComponent<AutoMove>();
                SpriteGlowEffect Afterglower = currentPlaying.GetComponent<SpriteGlowEffect>();
                Afterglower.EnableInstancing = false;
                if (autoMoveScript != null)
                {
                    autoMoveScript.enabled = false;
                }
                // ���õ� �÷��̾��� AutoMove ��ũ��Ʈ�� ��Ȱ��ȭ
            }
            //AutoMove script = currentPlaying.GetComponent<AutoMove>();
            //script.grogi = false;

        }
        grogi = false;

    }

    void Rec() //�׷α�ȸ��
    {
        grogi = false;
        Animator anima = currentPlaying.GetComponent<Animator>();
    }
    
    void SliedExit()
    {
        Rigidbody2D rd = currentPlaying.GetComponent<Rigidbody2D>();
        Animator anima = currentPlaying.GetComponent<Animator>();
        anima.SetBool("slide", false);//�����̵� ����� �����ϰ�
        //anima.SetBool("idle", true);//idle������κ���
        rd.velocity = new Vector2(0f, 0f);
    }
    void kick()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(currentPlaying.transform.position, BoxSize, 0);
        Rigidbody2D rd = currentPlaying.GetComponent<Rigidbody2D>();
        CapsuleCollider2D cd = currentPlaying.GetComponent<CapsuleCollider2D>();
        //rd.simulated = false;
        //cd.enabled = false;//���������� �ö��̴��� ���� ���� �ɸ��� �Ӹ����ξȺٵ���
        //�̰Ŵ� �±׸� �ٲ۴� ������� �ذ�
        Animator anima = currentPlaying.GetComponent<Animator>();
        anima.SetBool("kick", true);
        foreach (Collider2D col in collider2Ds)
        {
            if (col.CompareTag("Ball"))//�±װ� Ball�� ������Ʈ���Ը�
            {
                Rigidbody2D colRigidbody = col.GetComponent<Rigidbody2D>();
                //Debug.Log(col.name);
                colRigidbody.velocity = new Vector2(0f, 0f) + inputMovement * kickspeed;//��ǲ����+ű���ǵ常ŭ��
                //Debug.Log(inputMovement);
                //Debug.Log("ű�����������Ѻ��ν�Ƽ��ȭ");
                cd.isTrigger= true;
            }
        }
        grogi = true;
        currentPlaying.tag = "Kicking";
        Atk = true;
        Invoke("kickExit", 1f);
        //currentPlaying.tag = "Player";
    }
    void kickExit()
    {
        CapsuleCollider2D cd = currentPlaying.GetComponent<CapsuleCollider2D>();
        Rigidbody2D rd = currentPlaying.GetComponent<Rigidbody2D>();
        Animator anima = currentPlaying.GetComponent<Animator>();
        
        anima.SetBool("kick", false);
        currentPlaying.tag = "Player";
        //cd.enabled = true;
        //rd.simulated = true;
        //�±׸� �����ϴ� ������ �ذ��߱⶧���� ���ʿ�������
        grogi = false;
        Atk = false;
        cd.isTrigger = false;

    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(currentPlaying.transform.position, BoxSize);
        
    //}

}