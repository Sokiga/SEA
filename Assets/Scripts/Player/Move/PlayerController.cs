using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region ��ת
    public float rotationSpeed;
    public float rotationAngle;
    public float rotationSmoothTime;
    private float rotationSmoothSpeed;
    #endregion
    #region �ƶ�
    private CharacterController characterController;
    private Rigidbody rb;
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
    public GameObject rhymeObject;
    public float rhymeExtraSpeed = 5f;
    #endregion
    #region ����
    public float soulAmount;
    public bool isInSoulTime;
    public float soulExtraSpeed = 4f;
    public float soulTimer;
    public float soulDuration = 5f;
    #endregion
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
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
    #region ����ƶ��͵�����
    private void Move()
    {
        currentVelocity = currentSpeed  * transform.forward;
        characterController.Move(currentVelocity*Time.deltaTime);
        //characterController.Move(currentVelocity*Time.deltaTime);
        //characterController.Move(currentVelocity*Time.deltaTime);
    }
    private void ApplyFriction()
    {
        // �����ǰ�ٶȲ�Ϊ�㣬��Ӧ��Ħ����
        if (currentSpeed > 0)
        {
            // ����Ħ�����Ĵ�С�����ٶȷ����෴
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

                    rb.velocity = currentVelocity;

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
                           
                            //GetComponent<Rigidbody>().velocity += rhymeExtraSpeed * transform.forward;

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
    #region ��ҵ�������
    public void HandleShiftInput()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(!isInSoulTime&&soulAmount>0)
            {
                soulAmount -= 1;
                isInSoulTime = true;
                currentSpeed += soulExtraSpeed;
                //����
                //��ʼ��ʱ��,��Ϊ5s�ڿ϶���Ħ����Ū���ˣ���û��Ҫ
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
