using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Return : MonoBehaviour
{



    private void OnTriggerExit2D(Collider2D collision)
    {

        Debug.Log(collision.tag + "¿øÀ§Ä¡");
        collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        collision.transform.localPosition = Vector2.zero;
    }
}
