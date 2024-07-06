using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region ��ת
    public CameraControl cameraControl;
    public float rotationSpeed;
    public float rotationAngle;
    public float rotationSmoothTime;
    private float rotationSmoothSpeed;
    #endregion
    #region �ƶ�
    public CharacterController characterController;
    public float currentSpeed;
    public float basicSpeed=10;
    public float frictionCoefficient=2f;
    private Vector3 currentVelocity;
    //public float nitroBoostMultiplier = 2.0f; // �������ٱ���
    //public float nitroDuration = 2.0f; // ��������ʱ��
    #endregion
    #region �����ո�
    public float spaceTimer;
    public float boostDuration=0.4f;
    public bool firstEnter;
    public float rhymeBoostAmount;
    #endregion
    #region �����ʱ��
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
    #region �����ת

    private void HandleRotationInput()
    {
        Vector2 input = PlayerInput.instance.MoveDirection;
        

        if (input.x != 0&&PlayerInput.instance.MouseDirection.x==0)
        {
            // ����D��ʱ˳ʱ����ת������A��ʱ��ʱ����ת
            rotationAngle += input.x * rotationSpeed * Time.deltaTime;
        }
    }
    private void ApplyRotation()
    {
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref rotationSmoothSpeed, rotationSmoothTime);
    }

    #endregion
    #region ����ƶ�
    private void Move()
    {
        currentVelocity = currentSpeed  * transform.forward;
        characterController.Move(currentVelocity*Time.deltaTime);
    }
    private void ApplyFriction()
    {
        // �����ǰ�ٶȲ�Ϊ�㣬��Ӧ��Ħ����
        if (currentSpeed > 0)
        {
            // ����Ħ�����Ĵ�С�����ٶȷ����෴
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

            // �ڻ�����ʱ����Χ��
            if (rhymeTimer >= 0.6f && rhymeTimer <= 1.4f)
            {
                // �����ӳ��߼�
                if (rhymeBoostAmount == 0)
                {
                    rhymeBoostAmount++;
                    // ������Կ�ʼ�����߼������������ƶ��ٶȻ������ñ�־
                    currentSpeed += 2.0f; // �������2m/s
                }
            }

            // ���������ʱ������ָ��ʱ�䣬���������ӳ�
            if (rhymeTimer > 1.4f)
            {
                startRhymeTimer = false;
                rhymeTimer = 0f;
                rhymeBoostAmount = 0;
            }
        }
        else
        {
            // ���δ��ʼ������ʱ�������ü�ʱ��
            rhymeTimer = 0f;
        }
    }
    #endregion
}
