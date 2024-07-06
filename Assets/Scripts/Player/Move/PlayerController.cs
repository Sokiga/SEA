using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region 旋转
    public CameraControl cameraControl;
    public float rotationSpeed;
    public float rotationAngle;
    public float rotationSmoothTime;
    private float rotationSmoothSpeed;
    #endregion
    #region 移动
    public CharacterController characterController;
    public float currentSpeed;
    public float basicSpeed=10;
    public float frictionCoefficient=2f;
    private Vector3 currentVelocity;
    //public float nitroBoostMultiplier = 2.0f; // 氮气加速倍数
    //public float nitroDuration = 2.0f; // 氮气持续时间
    #endregion
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        HandleRotationInput();
        ApplyRotation();
        Move();
        ApplyFriction();
    }
    #region 玩家旋转

    private void HandleRotationInput()
    {
        Vector2 input = PlayerInput.instance.MoveDirection;
        

        if (input.x != 0&&PlayerInput.instance.MouseDirection.x==0)
        {
            // 按下D键时顺时针旋转，按下A键时逆时针旋转
            rotationAngle += input.x * rotationSpeed * Time.deltaTime;
        }
    }
    private void ApplyRotation()
    {
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref rotationSmoothSpeed, rotationSmoothTime);
    }

    #endregion
    #region 玩家移动
    private void Move()
    {
        if(PlayerInput.instance.MoveDirection.y>0)
        {
            currentSpeed = basicSpeed;
        }
        currentVelocity = currentSpeed  * transform.forward;
        characterController.Move(currentVelocity*Time.deltaTime);
    }
    private void ApplyFriction()
    {
        // 如果当前速度不为零，则应用摩擦力
        if (currentSpeed > 0)
        {
            // 计算摩擦力的大小，与速度方向相反
            currentSpeed =Mathf.Clamp(currentSpeed-frictionCoefficient*Time.deltaTime, 0, currentSpeed);
        }
    }
    #endregion
}
