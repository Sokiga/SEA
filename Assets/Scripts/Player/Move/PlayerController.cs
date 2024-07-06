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
        if(PlayerInput.instance.MoveDirection.y>0)
        {
            currentSpeed = basicSpeed;
        }
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
    #endregion
}
