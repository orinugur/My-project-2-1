//using System.Collections;
//using System.Collections.Generic;
////using System.Drawing;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.Windows;


//public class PlayerATK : MonoBehaviour
//{
//    public Vector2 BoxSize;
//    public Transform pos;
//    public float speed = 20;
//    private Vector2 inputMovement = Vector2.zero;
//    // Update is called once per frame


//    private void OnDrawGizmos()
//{
//    Gizmos.color = Color.blue;
//    Gizmos.DrawWireCube(pos.position, BoxSize);
//}

//private PlayerMove playerMoveScript;
//public void Onmove(InputValue inputValue)
//{
//    inputMovement = inputValue.Get<Vector2>();
//    //Vector2 inputp = inputMovement * speed * Time.deltaTime;

//}
//public void Update()
//{

//    Rigidbody2D rd = GetComponent<Rigidbody2D>();
//    Debug.Log(inputMovement);
//    Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, BoxSize, 0);
//    if (playerMoveScript.Atk == true)
//        foreach (Collider2D col in collider2Ds)
//        {
//            Rigidbody2D colRigidbody = col.GetComponent<Rigidbody2D>();
//            Debug.Log(col.name);
//            colRigidbody.velocity = new Vector2(0f, 0f) + inputMovement * speed * Time.deltaTime;
//            Debug.Log(inputMovement);
//        }
//}

//    //PlaeyrMove로부터 값 받아오기 실패..
//    //플랜B로 이동..
//}
