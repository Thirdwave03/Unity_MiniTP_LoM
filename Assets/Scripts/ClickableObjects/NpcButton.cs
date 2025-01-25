using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NpcButton : MonoBehaviour
{
    [SerializeField] private UnityEvent onClick;

    public void InvokeOnClick()
    {
        onClick?.Invoke();
    }
}
