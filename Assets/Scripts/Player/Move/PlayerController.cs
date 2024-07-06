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
    #region 长按空格
    public float spaceTimer;
    public float boostDuration=0.4f;
    public bool firstEnter;
    public float rhymeBoostAmount;
    #endregion
    #region 节奏计时器
    private float rhymeTimer;
    private bool startRhymeTimer;
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
        HandleSpace();
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
    private void HandleSpace()
    {
        if(PlayerInput.instance.spacePerform)
        {
            spaceTimer += Time.deltaTime;
            if(spaceTimer>boostDuration)
            {
                if (PlayerInput.instance.MoveDirection.y > 0&&firstEnter)
                {
                    currentSpeed = basicSpeed;
                    firstEnter = false;
                    startRhymeTimer=true;
                }
            }
        }
        else
        {
            firstEnter = true;
            spaceTimer = 0f;
        }
    }
    private void HandleRhymeTimer()
    {
        if (startRhymeTimer)
        {
            rhymeTimer += Time.deltaTime;

            // 在划桨计时器范围内
            if (rhymeTimer >= 0.6f && rhymeTimer <= 1.4f)
            {
                // 划桨加成逻辑
                if (rhymeBoostAmount == 0)
                {
                    rhymeBoostAmount++;
                    // 这里可以开始加速逻辑，例如增加移动速度或者设置标志
                    currentSpeed += 2.0f; // 假设加速2m/s
                }
            }

            // 如果划桨计时器超过指定时间，结束划桨加成
            if (rhymeTimer > 1.4f)
            {
                startRhymeTimer = false;
                rhymeTimer = 0f;
                rhymeBoostAmount = 0;
            }
        }
        else
        {
            // 如果未开始划桨计时器，重置计时器
            rhymeTimer = 0f;
        }
    }
    #endregion
}
