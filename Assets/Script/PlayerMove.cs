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

    public bool grogi = false; //그로기 상태 여부 체크
    public bool Atk =false; //그로기 상태 여부 체크
    public bool playing;//playing중인지 아닌지
    public float speed = 20.0f;//기본스피드

    public Animator animator; //애니메이터
    private Vector2 inputMovement = Vector2.zero;
    public GameObject[] Players; //플레이어의 목록이 할당됄 배열형 게임오브젝트
    public int selectedPlayerIndex = 0; //어떤 플레이어를 선택할지
    public GameObject currentPlaying;//현재 선택된 플레이어
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
        
        grogi = false;//그로기 상태들은 false

        ChangePlayer(0);//게임 시작시 첫번째 플레이어를 조작하도록
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
        Vector2 moveMovement = inputMovement * speed * Time.deltaTime;//이동속도

      

        if (currentPlaying != null&&grogi==false)//선택된플레이어가 그로기상태가아닐시
        {
            currentPlaying.transform.Translate(moveMovement);//이동
            
            if (Input.GetKeyDown(KeyCode.J))//그로기상태가 아닐때 j를 입력시(태클)
            {
                grogi = true;   //그로기 상태 활성화
                anima.SetBool("slide", true);//슬라이드애니메이션 활성화
                if (currentPlaying.transform.localScale.x > 0)//우측으로 이동중일때
                {
                    rd.velocity = new Vector2(35f, 0f);
                    
                }
                else
                {
                    rd.velocity = new Vector2(-35f, 0f);//좌측으로 이동중일때
                   
                }
                Debug.Log("slide");
                Invoke("SliedExit", 0.6f);//슬라이드 지속시간 설정
                Debug.Log("rec실행");
                Invoke("Rec", 1.6f);//1.6초뒤 rec를 실행하면 grogi가 false가됌
            }
        }
        if(grogi==false)//그로기가 풀릴시 가속도 해제
        {
            rd.velocity=new Vector2(0f,0f);
        }

        // 1~7입력시에 해당하는 번호의 플레이어를 현재 플레이어로 변경
        //선택한 플레이어의 automove를 비활성화, 이전에 선택한 플레이어의 automove를 활성화
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
        //선택된 플레이어의 애니메이터
        inputMovement = inputValue.Get<Vector2>();
        //Vector2 sc = currentPlaying.transform.localScale;
       
        
        
        

        //인풋받아온 값(vector2의 x,y값)
        if (grogi==true)//그로기가 켜져있을시 walk를 꺼서 idel 애니메이션이 출력되도록 유도
        {
            anima.SetBool("walk", false);
        }
        if (grogi == false)
        {
            if (inputMovement.x < 0)//입력받은x의 값이 0보다작을경우(왼쪽이동)
            {
                currentPlaying.transform.localScale = new Vector2(-1, 1);
             
                //sc.x = currentPlaying.transform.localScale.x * -1f;
                //currentPlaying.transform.localScale = sc;
                //Debug.Log("inputMovement.x < 0,walk");
                anima.SetBool("walk", true);
                anima.SetBool("idle", false);
            }
            else if (inputMovement.x > 0)//입력받은x의 값이 0보다 클 경우(오른쪽이동)
            {
                currentPlaying.transform.localScale = new Vector2(1, 1);
                //sc.x = Mathf.Abs(currentPlaying.transform.localScale.x) * 1f;
                //currentPlaying.transform.localScale = sc;
                //x스케일을 양수로 하여 우측을 보도록함
                anima.SetBool("walk", true);
                anima.SetBool("idle", false);
                //Debug.Log("inputMovement.x > 0,walk");
            }
            else if (inputMovement.x == 0 && inputMovement.y == 0)
            {//x와y가 둘다 0일경우
                //Debug.Log("inputMovement.x == 0,idle");
                anima.SetBool("idle", true);
                anima.SetBool("walk", false);
            }   
        }
    } 
      
    void ChangePlayer(int index) //플레이어 변경
    {
        selectedPlayerIndex = index;
        //업데이트에서 인풋받아온 키에따라 index의 값을수정한뒤에
        //배열에서 해당 번호의 선수를 호출함
        if (selectedPlayerIndex < 0 || selectedPlayerIndex >= Players.Length)
        {//플레이어 배열을 벗어난 입력을 할시 디버그 메세지 출력
            Debug.LogError("Invalid Enemy index");
            return;
        }

        for (int i = 0; i < Players.Length; i++)
        //선택된 플레이어의 AutoMove를 비활성화
        {
            if (i == selectedPlayerIndex)
            {
                //셀렉트 플레이어를 변경하기전 조작 중이던 플레이어의 AutoMove를 활성화
                AutoMove beforeautoMoveScript = currentPlaying.GetComponent<AutoMove>();
                SpriteGlowEffect glower = currentPlaying.GetComponent<SpriteGlowEffect>();

                //어?근데 이럼 start때 최초에 currentPlaying의 값은 null 일탠데 에러안나나?
                if (beforeautoMoveScript != null)
                {
                        beforeautoMoveScript.enabled = true;
                        glower.EnableInstancing = true;
                }

                currentPlaying = Players[selectedPlayerIndex];
                // 현재 플레이어를 배열의 플레이어로 변경
                AutoMove autoMoveScript = Players[selectedPlayerIndex].GetComponent<AutoMove>();
                SpriteGlowEffect Afterglower = currentPlaying.GetComponent<SpriteGlowEffect>();
                Afterglower.EnableInstancing = false;
                if (autoMoveScript != null)
                {
                    autoMoveScript.enabled = false;
                }
                // 선택된 플레이어의 AutoMove 스크립트를 비활성화
            }
            //AutoMove script = currentPlaying.GetComponent<AutoMove>();
            //script.grogi = false;

        }
        grogi = false;

    }

    void Rec() //그로기회복
    {
        grogi = false;
        Animator anima = currentPlaying.GetComponent<Animator>();
    }
    
    void SliedExit()
    {
        Rigidbody2D rd = currentPlaying.GetComponent<Rigidbody2D>();
        Animator anima = currentPlaying.GetComponent<Animator>();
        anima.SetBool("slide", false);//슬라이드 모션을 종료하고서
        //anima.SetBool("idle", true);//idle모션으로복귀
        rd.velocity = new Vector2(0f, 0f);
    }
    void kick()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(currentPlaying.transform.position, BoxSize, 0);
        Rigidbody2D rd = currentPlaying.GetComponent<Rigidbody2D>();
        CapsuleCollider2D cd = currentPlaying.GetComponent<CapsuleCollider2D>();
        //rd.simulated = false;
        //cd.enabled = false;//순간적으로 컬라이더를 꺼서 공이 케릭터 머리위로안붙도록
        //이거는 태그를 바꾼는 방식으로 해결
        Animator anima = currentPlaying.GetComponent<Animator>();
        anima.SetBool("kick", true);
        foreach (Collider2D col in collider2Ds)
        {
            if (col.CompareTag("Ball"))//태그가 Ball인 오브젝트에게만
            {
                Rigidbody2D colRigidbody = col.GetComponent<Rigidbody2D>();
                //Debug.Log(col.name);
                colRigidbody.velocity = new Vector2(0f, 0f) + inputMovement * kickspeed;//인풋방향+킥스피드만큼의
                //Debug.Log(inputMovement);
                //Debug.Log("킥실행으로인한벨로시티변화");
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
        //태그를 수정하는 식으로 해결했기때문에 불필요함이제
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