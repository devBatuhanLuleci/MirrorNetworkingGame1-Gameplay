using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    public enum InputState {Idle,Dragging};
    public InputState inputState;

    private void Awake()
    {
       
        instance = this;
    }

}
