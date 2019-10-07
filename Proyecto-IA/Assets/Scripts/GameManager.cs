using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum userType { singleUser,client,server  }
    [SerializeField] userType _userType;
    [SerializeField] int _ip;
    [SerializeField] int _port;
    void Start()
    { 
    }
    void Update()
    {
        
    }
}
