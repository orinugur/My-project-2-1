using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrackingCamera : MonoBehaviour
{
    float minCamSize = 32;//�ִ� ��� ������
    float maxCamSize = 42;//�ִ� Ȯ�� ������
    float defaultCamSize = 32;//�⺻ ������
    //float sensitivity = 0.05f;//���콺 �� �ΰ���    
    float camSpeed = 10;//ī�޶� �󸶳� ���� ��ǥ ��ġ�� �̵�����. 1~20 ����
    float canRange = 55;//ī�޶� �󸶳� ���� ��ġ�� �ָ� �̵�����. 1~20���� ����
    float camSize;
    Camera cam;
    public GameObject Ball;
    public GameObject Arrow;

    private void Awake()
    {
        camSize = defaultCamSize;
        cam = GetComponent<Camera>();
    }
    void FixedUpdate()
    {


        Vector3 playerPosition = Arrow.transform.position;
        Vector3 ballPosition = Ball.transform.position;
        Vector3 midPoint = (playerPosition + ballPosition) / 2;
        Vector3 camTargetPosition = new Vector3(midPoint.x, midPoint.y, -10f);
        this.transform.position = Vector3.Lerp(this.transform.position, camTargetPosition, Time.deltaTime * camSpeed);

        Zoom();
        cam.orthographicSize = camSize;
        
    }

    void Zoom()
    {

        float distance = Vector3.Distance(Arrow.transform.position, Ball.transform.position);
        Debug.Log(distance);
        if (distance < canRange)
        {
            camSize = camSize - (camSize * Time.deltaTime) - 1;
        }
        else
        {
            camSize = camSize + (camSize * Time.deltaTime) + 1;
        }
        camSize = Mathf.Clamp(camSize, minCamSize, maxCamSize);
    }
}

