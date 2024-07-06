using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : Singleton<PlayerInput>
{
    public InputActions inputActions;
    public bool Move => inputActions.GamePlay.Move.ReadValue<Vector2>() != Vector2.zero;
    public Vector2 MoveDirection => inputActions.GamePlay.Move.ReadValue<Vector2>();
    public Vector2 MouseDirection => inputActions.GamePlay.Look.ReadValue<Vector2>();
    public Vector2 MouseScroll => inputActions.GamePlay.MouseScroll.ReadValue<Vector2>();
    public float AxisX => MoveDirection.x;
    override public void Awake()
    {
        base.Awake();
        inputActions = new InputActions();
        EnableGamePlay();
    }
    public void EnableGamePlay()
    {
        inputActions.GamePlay.Enable();
    }
    public void DisableGamePlay()
    {
        inputActions.GamePlay.Disable();
    }
}
