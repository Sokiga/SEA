using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float xControlSpeed;
    public float yControlSpeed;
    //public float mouseScrollSpeed;
    private Transform lookTarget;
    public float playerOffset;
    public float currentOffset;
    #region ����ƽ���ٶ�
    public float mouseScrollSpeed;
    public float posSmoothSpeed;
    public float colliderSmoothSpeed;
    public float smoothTime;
    #endregion
    public Vector2 cameraMaxVerticalAngle;
    private Vector2 rotation;
    public Vector3 cameraRotaion;
    private Vector3 smoothDampSpeed=Vector3.zero;
    public CameraCollider cameraCollider;
    private void Awake()
    {
        lookTarget = GameObject.FindWithTag("CameraTarget").transform;
        cameraCollider=GetComponent<CameraCollider>();

    }
    private void OnEnable()
    {
        PlayerInput.instance.inputActions.GamePlay.MouseScroll.performed += ChangeOffset;
    }


    private void OnDisable()
    {
        PlayerInput.instance.inputActions.GamePlay.MouseScroll.performed -= ChangeOffset;
    }
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        //�����������
        CameraInput();
    }
    private void LateUpdate()
    {
        UpdateCameraRotation(); 
        UpdateCameraPos();
    }
    //�����������
    private void CameraInput()
    {
        //����y����ʵ�����x����ƶ�
        rotation.y += PlayerInput.instance.MouseDirection.x * xControlSpeed;
        rotation.x -= PlayerInput.instance.MouseDirection.y * yControlSpeed;
        rotation.x = Mathf.Clamp(rotation.x,cameraMaxVerticalAngle.x,cameraMaxVerticalAngle.y);
    }
    //��ת�����
    private void UpdateCameraRotation()
    {
        cameraRotaion = Vector3.SmoothDamp(cameraRotaion, new Vector3(rotation.x, rotation.y, 0), ref smoothDampSpeed, smoothTime);
        transform.eulerAngles = cameraRotaion;
    }
    //�����ָ�offset
    private void ChangeOffset(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        var offset = context.ReadValue<Vector2>().y;
        playerOffset -= offset * mouseScrollSpeed * 0.01f;
        playerOffset = Mathf.Clamp(playerOffset, 1f, 5);
    }
    //�������λ��
    private void UpdateCameraPos()
    {
        float cameraColliderOffset = cameraCollider.UpdateCameraCollider();
        if (cameraColliderOffset != 0)
        {
            currentOffset = Mathf.Clamp(currentOffset + cameraColliderOffset, 0.8f, 2f);
        }
        else
        {
            // û�м�⵽�κ��ϰ���ʱ���𽥻ָ��� playerOffset
            //currentOffset = Mathf.Clamp(Vector3.Lerp(new Vector3(currentOffset, 0, 0), new Vector3(playerOffset, 0, 0), colliderSmoothSpeed * Time.deltaTime).x, 0.8f, playerOffset);
            currentOffset = Mathf.Clamp(
            Mathf.Lerp(currentOffset, playerOffset, colliderSmoothSpeed * Time.deltaTime),
                      0.8f, playerOffset);
        }
        //�����-transform.forward��֤�����һ�����룬����������������Һ���ת
        var newPos = lookTarget.position + (-transform.forward * currentOffset);
        transform.position = Vector3.Lerp(transform.position, newPos, posSmoothSpeed * Time.deltaTime);
    }

}
