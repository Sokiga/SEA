using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region 旋转
    public float rotationSpeed;
    public float rotationAngle;
    public float rotationSmoothTime;
    private float rotationSmoothSpeed;
    #endregion
    #region 移动
    private CharacterController characterController;
    public float currentSpeed;
    public float basicSpeed=10;
    public float frictionCoefficient=2f;
    private Vector3 currentVelocity;
    public Rigidbody rigidbody;
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
    public GameObject rhymeObject;
    public float rhymeExtraSpeed = 5f;
    #endregion
    #region 海灵
    public float soulAmount;
    public bool isInSoulTime;
    public float soulExtraSpeed = 4f;
    public float soulTimer;
    public float soulDuration = 5f;
    #endregion
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        HandleRotationInput();
        ApplyRotation();
        //Move();
        ApplyFriction();
        HandleSpace();
        HandleRhymeTimer();
        HandleSoulTimer();
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
    #region 玩家移动和叠节奏
    private void Move()
    {
        currentVelocity = currentSpeed  * transform.forward;
        rigidbody.AddForce(currentVelocity);
        //characterController.Move(currentVelocity*Time.deltaTime);
        //characterController.Move(currentVelocity*Time.deltaTime);
    }
    private void ApplyFriction()
    {
        // 如果当前速度不为零，则应用摩擦力
        if (currentSpeed > 0)
        {
            // 计算摩擦力的大小，与速度方向相反
            currentSpeed = Mathf.Clamp(currentSpeed - frictionCoefficient * Time.deltaTime, 0, currentSpeed);
        }
    }
    private void HandleSpace()
    {
        if (PlayerInput.instance.spacePerform)
        {
            spaceTimer += Time.deltaTime;
            if (spaceTimer > boostDuration)
            {
                if (PlayerInput.instance.MoveDirection.y > 0 && firstEnter)
                {
                    currentSpeed = basicSpeed;

                    currentVelocity = currentSpeed * transform.forward;
                    rigidbody.velocity = currentVelocity;

                    firstEnter = false;
                    if (rhymeBoostAmount == 0)
                    {
                        rhymeBoostAmount++;
                        startRhymeTimer = true;
                    }
                    else
                    {
                        if (rhymeTimer >= 0.6f && rhymeTimer <= 1.4f)
                        {
                           
                            rigidbody.velocity += rhymeExtraSpeed * transform.forward;

                            rhymeObject.SetActive(true);
                            rhymeBoostAmount++;
                            rhymeTimer = 0f;
                        }
                    }
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
            if (rhymeTimer > 1.4f)
            {
                startRhymeTimer = false;
                rhymeTimer = 0f;
                rhymeBoostAmount = 0f;
                rhymeObject.SetActive(false);

            }
        }
        else
        {
            rhymeTimer = 0f;
        }
    }
    #endregion
    #region 玩家氮气加速
    public void HandleShiftInput()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(!isInSoulTime&&soulAmount>0)
            {
                soulAmount -= 1;
                isInSoulTime = true;
                currentSpeed += soulExtraSpeed;
                //加速
                //开始计时器,因为5s内肯定被摩擦力弄完了，就没必要
            }
        }
    }
    public void HandleSoulTimer()
    {
        if(isInSoulTime)
        {
            soulTimer += Time.deltaTime;
            if(soulTimer >soulDuration)
            {
                soulTimer = 0f;
                isInSoulTime = false;
            }
        }
        else
        {
            soulTimer = 0f;
        }
    }
    #endregion
}
