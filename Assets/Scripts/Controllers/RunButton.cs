using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RunButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressedRun;
    public static RunButton _RunBtnInstance;

    void Awake()
    {
        if(_RunBtnInstance != null && _RunBtnInstance != this)
        {
            Destroy(this);
        }
        else
        {
            _RunBtnInstance = this;
        }
    }

    private void Start() {
        isPressedRun = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        isPressedRun = true;
    }

    public void OnPointerUp(PointerEventData data)
    {
        isPressedRun = false;
    }
}
