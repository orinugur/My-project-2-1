using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
//using UnityEngine.UIElements;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class BallScript : MonoBehaviour
{
    enum BallState
    {
        onhead, offhead, kick
    }
    //public GameObject Boom;
    public ParticleSystem Boom;
    public GameObject Ball;
    public float radius = 3.5f;
    public bool onhead = false;
    public Collider2D[] colliders1;
    public List<Collider2D> colliders2 = new List<Collider2D>();
    public Vector2 AfterP;
    public int Count;
    public int WallCount;
    void Update()
    {
        

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        // Collider2D[]colliders = Physics2D.OverlapSphere(transform.position, radius);
        string[] tagToDetect = { "Player", "Enemy"};
        string[] Allplayer = { "Player", "Enemy", "Kicking" };
        string[] WallAndPlayer= {"Player", "Enemy", "Kicking","Wall"};
        colliders1 = colliders;
        colliders2.Clear();
        Count = 0;
        WallCount= 0;
        foreach (Collider2D col in colliders)
        {
            foreach (string tag in Allplayer)
            {
                if (col.tag == tag)
                {
                    colliders2.Add(col); 
                }
                  
            }   
        }
        foreach (Collider2D col2 in colliders2) //Enemy,Enemy,Kicking 상태를 조회
        {
            foreach (string tag in tagToDetect) //Enemy,Enemy 상태의 플레이어만 조회(Kicking상태의플레이어의 머리에 공이붙지 않기위한조치)
                if (colliders2.Count < 2 && col2.CompareTag(tag)) //수집한 콜라이더의 카운트가 2보다작고 (혼자일때) 콜라이더의 태그가 Enemy or Enemy일때만 머리에 붙음 
                {
                    AudioSource get = GetComponent<AudioSource>();
                    get.Play();
                    Vector2 position2 = col2.transform.position;
                    position2.y += 1f;
                    transform.position = position2;//머리위로올라간다.
                    //Vector2 currentVelocity = GetComponent<Rigidbody2D>().velocity;
                    //float mspeed = 0.1f; //점차감속
                    //GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(currentVelocity, Vector2.zero, mspeed);
                    onhead = true;
                    if (col2.CompareTag("Player"))
                    {
                        AutoMove AutoMoving = col2.GetComponent<AutoMove>();
                        AutoMoving.GetBall = true;
                    }
                    else if (col2.CompareTag("Enemy"))
                    {
                        AutoMove EnemyMove = col2.GetComponent<AutoMove>();
                        EnemyMove.GetBall = true;

                    }
                }
            
            if(colliders2.Count>=3)
            {
                foreach(Collider2D col in colliders2)
                {
                    Rigidbody2D colRigidbody = col.GetComponent<Rigidbody2D>();
                    Vector2 Booming = (col.transform.position - transform.position).normalized;
                    Vector2 force = Booming * 30f;
                    colRigidbody.AddForce(force);
                    float mspeed = 0.1f; //점차감속
                    GetComponent<Rigidbody2D>().velocity = Vector2.Lerp(colRigidbody.transform.position, Vector2.zero, mspeed*Time.deltaTime);
                    //공의속도가 점차감소하는

                    //, ForceMode2D.Impulse);
                    //Rigidbody2D rd = Ball.GetComponent<Rigidbody2D>();
                    //Vector2 Booming1 = (col.transform.position - Ball.transform.position).normalized;
                    //Vector2 force1 = Booming * 30f;
                    //rd.AddForce(force1);
                    //Debug.Log("터진다");
                    //colRigidbody.velocity = Vector2.Lerp(colRigidbody.velocity, Vector2.zero, 1f); //점차감속
                }
                StartCoroutine(Boombing());
            }
 
        }
        foreach(Collider2D col in colliders1)
        {
            foreach (string tag in WallAndPlayer)
            {
                if (col.CompareTag(tag))
                {
                    Count++;
                }
            }

        }

        if (Count >= 3)
        {
            StartCoroutine(Boombing());
            Baom();
        }
        StartCoroutine(puser());
        StartCoroutine(bibigi());

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void PuseBool()
    {
        
    }
    public IEnumerator puser()
    {
        Vector3 Af = transform.localPosition;
        yield return new WaitForSecondsRealtime(2f);
        if (Af == transform.localPosition)
        {
            Baom();
            yield return new WaitForSecondsRealtime(1.5f);
        }
    }
    public IEnumerator Boombing()
    {
        Boom.transform.position = transform.position;
        AudioSource BoomAudio = Boom.GetComponent<AudioSource>();
        BoomAudio.Play();
        Boom.Play();

        yield return new WaitForSecondsRealtime(1.5f);
        Boom.Stop();
    }

    public void Baom()
    {
        foreach (Collider2D col in colliders1)
        {
            if (!col.CompareTag("Wall") && !col.CompareTag("Ball"))
            {
                Rigidbody2D rd = Ball.GetComponent<Rigidbody2D>();
                Rigidbody2D colrd = col.GetComponent<Rigidbody2D>();
                //Vector2 Booming = (col.transform.position - Ball.transform.position).normalized;
                Vector2 Booming = (col.transform.position - Ball.transform.position);
                Debug.Log(Booming);
                Vector2 force = Booming * 20f;
                rd.AddForce(force);
                colrd.AddForce(force);
                //rd.velocity = Vector2.Lerp(rd.transform.position, Vector2.zero, 0.5f * Time.deltaTime);
                //colrd.velocity = Vector2.Lerp(rd.transform.position, Vector2.zero, 0.5f * Time.deltaTime);


                StartCoroutine(Boombing());
            }
        }
    }

    public IEnumerator bibigi()
    {
        Vector3 startPos = transform.position;
        yield return new WaitForSeconds(2f);
        float distance = Vector3.Distance(startPos, transform.position);
        if(distance <3)
        {
            Baom();
            //Debug.Log("비비기");
            yield return new WaitForSeconds(0.2f);
        }

    }


    public void OnDisable()
    {
        Boom.Stop();
    }

}
