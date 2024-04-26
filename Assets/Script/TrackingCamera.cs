using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrackingCamera : MonoBehaviour
{
    float minCamSize = 32;//최대 축소 사이즈
    float maxCamSize = 42;//최대 확대 사이즈
    float defaultCamSize = 32;//기본 사이즈
    //float sensitivity = 0.05f;//마우스 휠 민감도    
    float camSpeed = 10;//카메라가 얼마나 빨리 목표 위치로 이동할지. 1~20 권장
    float canRange = 55;//카메라가 얼마나 에임 위치로 멀리 이동할지. 1~20이하 권장
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

