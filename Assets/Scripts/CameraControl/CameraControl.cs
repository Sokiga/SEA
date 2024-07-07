using UnityEngine;

public class CameraControl : MonoBehaviour
{
    #region ���������
    public float xControlSpeed;
    public float yControlSpeed;
    #endregion

    #region ��ͷ����Ҿ���
    public float playerOffset;
    public float currentOffset;
    public Transform lookTarget;
    #endregion

    #region ����ƽ���ٶ�
    public float mouseScrollSpeed;
    public float posSmoothSpeed;
    public float colliderSmoothSpeed;
    public float smoothTime;
    #endregion

    #region ��ͷ��ת
    public Vector2 cameraMaxVerticalAngle;
    private Vector2 rotation;
    public Vector3 cameraRotation;
    private Vector3 smoothDampSpeed = Vector3.zero;
    public Transform boat;
    #endregion


    #region Y����ֵ����
    public float yThreshold; // ���y��仯��ֵ
    private float lastPlayerYPos; // ������һ����ҵ�y��λ��
    #endregion

    private void Awake()
    {
        lookTarget = GameObject.FindWithTag("CameraTarget").transform;
        boat = GameObject.FindWithTag("Boat").transform;
        lastPlayerYPos = lookTarget.position.y;
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
        CameraInput();
        UpdateCameraRotation();
        UpdateCameraPos();
    }

    private void FixedUpdate()
    {
        //UpdateCameraRotation();
        //UpdateCameraPos();
    }

    // �����������
    private void CameraInput()
    {
        if (!Input.GetKey(KeyCode.LeftAlt))
        {
            return;
        }
        rotation.y += PlayerInput.instance.MouseDirection.x * xControlSpeed;
        rotation.x -= PlayerInput.instance.MouseDirection.y * yControlSpeed;
        rotation.x = Mathf.Clamp(rotation.x, cameraMaxVerticalAngle.x, cameraMaxVerticalAngle.y);
    }

    public void ChangeCameraRotation(float rotationY)
    {
        rotation.y = rotationY;
    }

    // ��ת�����
    private void UpdateCameraRotation()
    {

        Quaternion targetRotation = Quaternion.Euler(rotation.x, boat.eulerAngles.y, 0f);

        // ʹ�������ֵ��ƽ����ת
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime * Time.deltaTime);
    }

    // �����ָ�offset
    private void ChangeOffset(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        var offset = context.ReadValue<Vector2>().y;
        playerOffset -= offset * mouseScrollSpeed * 0.01f;
        playerOffset = Mathf.Clamp(playerOffset, 4f, 10f);
    }

    // �������λ��
    private void UpdateCameraPos()
    {
        // �������y��λ�ñ仯
        float playerYPos = lookTarget.position.y;
        float yDifference = Mathf.Abs(playerYPos - lastPlayerYPos);

        // ���y��仯������ֵ���������λ�ò�����lastPlayerYPos
        if (yDifference > yThreshold)
        {
            lastPlayerYPos = playerYPos;
        }
        else
        {
            playerYPos = lastPlayerYPos;
        }

        // �������λ��
        var newPos = new Vector3(lookTarget.position.x, playerYPos, lookTarget.position.z) + (-transform.forward * playerOffset);
        transform.position = Vector3.Lerp(transform.position, newPos, posSmoothSpeed * Time.deltaTime);
    }
}


