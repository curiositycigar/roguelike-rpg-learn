using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO C#中各种单例的实现
public class SingleInstanceMonoBehaviour : MonoBehaviour
{
    public static SingleInstanceMonoBehaviour instance;

    private void Awake()
    {
        instance = this;
    }
}
