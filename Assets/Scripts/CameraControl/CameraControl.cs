using UnityEngine;

public class CameraControl : MonoBehaviour
{
    #region 鼠标灵敏度
    public float xControlSpeed;
    public float yControlSpeed;
    #endregion

    #region 镜头离玩家距离
    public float playerOffset;
    public float currentOffset;
    private Transform lookTarget;
    #endregion

    #region 各种平滑速度
    public float mouseScrollSpeed;
    public float posSmoothSpeed;
    public float colliderSmoothSpeed;
    public float smoothTime;
    #endregion

    #region 镜头旋转
    public Vector2 cameraMaxVerticalAngle;
    private Vector2 rotation;
    public Vector3 cameraRotation;
    private Vector3 smoothDampSpeed = Vector3.zero;
    public Transform player;
    #endregion

    #region 碰撞
    public CameraCollider cameraCollider;
    #endregion

    #region Y轴阈值控制
    public float yThreshold; // 添加y轴变化阈值
    private float lastPlayerYPos; // 保存上一次玩家的y轴位置
    #endregion

    private void Awake()
    {
        lookTarget = GameObject.FindWithTag("CameraTarget").transform;
        player = GameObject.FindWithTag("Player").transform;
        cameraCollider = GetComponent<CameraCollider>();
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
    }

    private void LateUpdate()
    {
        UpdateCameraRotation();
        UpdateCameraPos();
    }

    // 检查鼠标的输入
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

    // 旋转摄像机
    private void UpdateCameraRotation()
    {

        Quaternion targetRotation = Quaternion.Euler(rotation.x, player.eulerAngles.y, 0f);

        // 使用球面插值来平滑旋转
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTime * Time.deltaTime);
    }

    // 鼠标滚轮改offset
    private void ChangeOffset(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        var offset = context.ReadValue<Vector2>().y;
        playerOffset -= offset * mouseScrollSpeed * 0.01f;
        playerOffset = Mathf.Clamp(playerOffset, 1f, 5f);
    }

    // 更新相机位置
    private void UpdateCameraPos()
    {
        float cameraColliderOffset = cameraCollider.UpdateCameraCollider();
        if (cameraColliderOffset != 0)
        {
            currentOffset = Mathf.Clamp(currentOffset + cameraColliderOffset, 0.8f, 2f);
        }
        else
        {
            // 没有检测到任何障碍物时，逐渐恢复到 playerOffset
            currentOffset = Mathf.Clamp(
                Mathf.Lerp(currentOffset, playerOffset, colliderSmoothSpeed * Time.deltaTime),
                0.8f, playerOffset);
        }

        // 计算玩家y轴位置变化
        float playerYPos = lookTarget.position.y;
        float yDifference = Mathf.Abs(playerYPos - lastPlayerYPos);

        // 如果y轴变化超过阈值，更新相机位置并更新lastPlayerYPos
        if (yDifference > yThreshold)
        {
            lastPlayerYPos = playerYPos;
        }
        else
        {
            playerYPos = lastPlayerYPos;
        }

        // 更新相机位置
        var newPos = new Vector3(lookTarget.position.x, playerYPos, lookTarget.position.z) + (-transform.forward * currentOffset);
        transform.position = Vector3.Lerp(transform.position, newPos, posSmoothSpeed * Time.deltaTime);
    }
}


