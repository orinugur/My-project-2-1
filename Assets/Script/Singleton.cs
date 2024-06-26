using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {//인스턴스가 null이면
            if (instance == null)
            {
                //만약 씬에 이미 해당파파일의 인스턴스가 있으면
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    GameObject singltonObject = new GameObject();
                    instance = singltonObject.AddComponent<T>();
                    DontDestroyOnLoad(singltonObject);
                }
            }

            return instance;
        }


    }

}
