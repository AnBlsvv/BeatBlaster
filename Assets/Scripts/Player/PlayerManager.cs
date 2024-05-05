using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //Singleton
    #region Singleton

    public static PlayerManager _PMInstance;

    void Awake()
    {
        if(_PMInstance != null && _PMInstance != this)
        {
            Destroy(this);
        }
        else
        {
            _PMInstance = this;
        }
        Application.targetFrameRate = 100;
    }

    #endregion

    public GameObject player;
}
