using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gollde : MonoBehaviour
{
    
    public Vector2 BoxSize;

    public GameObject gallde;
    
    private void Update()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(transform.position, BoxSize, 0);


        foreach (Collider2D col in collider2Ds)
        {
            if (col.CompareTag("Ball"))//태그가 Ball인 오브젝트에게만
            {
                if(gallde.CompareTag("Player"))
                {
                    GameManager.Instance.AddEnScore();

                }
                else if(gallde.CompareTag("Enemy"))
                {
                    GameManager.Instance.AddMyScore();
                }
                
                
            }
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, BoxSize);
 
        
    }
}
