using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : Singleton<PlayerInput>
{
    public Sign sign;
    public InputActions inputActions;
    public bool Move => inputActions.GamePlay.Move.ReadValue<Vector2>() != Vector2.zero;
    public bool spacePerform => inputActions.GamePlay.Space.IsPressed();
    public Vector2 MoveDirection => inputActions.GamePlay.Move.ReadValue<Vector2>();
    public Vector2 MouseDirection => inputActions.GamePlay.Look.ReadValue<Vector2>();
    public Vector2 MouseScroll => inputActions.GamePlay.MouseScroll.ReadValue<Vector2>();
    public float AxisX => MoveDirection.x;
    public float AxisY => MoveDirection.y;
    override public void Awake()
    {
        base.Awake();
        inputActions = new InputActions();
        EnableGamePlay();
    }
    private void Update()
    {
        //if(sign.isInDialog)
        //{
        //    DisableGamePlay();
        //}
        //else
        //{
        //    EnableGamePlay();
        //}
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
