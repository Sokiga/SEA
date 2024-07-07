using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region Ðý×ª
    public float rotationSpeed;
    public float rotationAngle;
    public float rotationSmoothTime;
    private float rotationSmoothSpeed;
    #endregion
    #region ÒÆ¶¯
    private Rigidbody rb;
    public float currentSpeed;
    public float basicSpeed=10;
    public float frictionCoefficient=2f;
    private Vector3 currentVelocity;
    //public float nitroBoostMultiplier = 2.0f; // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ù±ï¿½ï¿½ï¿½
    //public float nitroDuration = 2.0f; // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ê±ï¿½ï¿½
    #endregion
    #region ÒÆ¶¯
    public float spaceTimer;
    public float boostDuration=0.4f;
    public bool firstEnter;
    public float rhymeBoostAmount;
    #endregion
    #region ½Ú×à
    private float rhymeTimer;
    private bool startRhymeTimer;
    public GameObject rhymeObject;
    public float rhymeExtraSpeed = 5f;
    #endregion
    #region º£Áé
    public float soulAmount;
    public float maxSoulAmount=2f;
    public bool isInSoulTime;
    public float soulExtraSpeed = 4f;
    public float soulTimer;
    public float soulDuration = 5f;
    #endregion
    #region guide
    public bool isInGuideArea;
    public Vector3 environmentVelocity;
    #endregion
    #region oar
    public Rotater oarLeft;
    public Rotater oarRight;
    #endregion
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        HandleRotationInput();
        ApplyRotation();
        Move();
        ApplyFriction();
        HandleSpace();
        HandleRhymeTimer();
        HandleSoulTimer();
    }
    #region Ðý×ª

    private void HandleRotationInput()
    {
        Vector2 input = PlayerInput.instance.MoveDirection;
        if (input.x != 0&&PlayerInput.instance.MouseDirection.x==0)
        {
            // ï¿½ï¿½ï¿½ï¿½Dï¿½ï¿½Ê±Ë³Ê±ï¿½ï¿½ï¿½ï¿½×ªï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Aï¿½ï¿½Ê±ï¿½ï¿½Ê±ï¿½ï¿½ï¿½ï¿½×ª
            rotationAngle += input.x * rotationSpeed * Time.deltaTime;
            if(input.x < 0)
            {
                oarRight.RotateOnce();
            }
            else
            {
                oarLeft.RotateOnce();
            }

        }
    }
    private void ApplyRotation()
    {
        float currentYAngle = transform.eulerAngles.y;
        float targetYAngle = rotationAngle;
        float smoothYAngle = Mathf.SmoothDampAngle(currentYAngle, targetYAngle, ref rotationSmoothSpeed, rotationSmoothTime);

        // Ö»ÐÞ¸Ä y ÖáµÄÐý×ª½Ç¶È£¬±£Áô x ºÍ z ÖáµÄÐý×ª½Ç¶È
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, smoothYAngle, transform.eulerAngles.z);

        //transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref rotationSmoothSpeed, rotationSmoothTime);
    }

    #endregion
    #region ÒÆ¶¯
    private void Move()
    {
        currentVelocity = currentSpeed  * transform.forward+environmentVelocity;
        rb.velocity = currentVelocity;
    }
    public void ChangeEnvironmentVelocity(Vector3 velocity)
    {
        environmentVelocity = velocity;
    }
    private void ApplyFriction()
    {
        // ï¿½ï¿½ï¿½ï¿½ï¿½Ç°ï¿½Ù¶È²ï¿½Îªï¿½ã£¬ï¿½ï¿½Ó¦ï¿½ï¿½Ä¦ï¿½ï¿½ï¿½ï¿?
        if (currentSpeed > 0)
        {
            // ï¿½ï¿½ï¿½ï¿½Ä¦ï¿½ï¿½ï¿½ï¿½ï¿½Ä´ï¿½Ð¡ï¿½ï¿½ï¿½ï¿½ï¿½Ù¶È·ï¿½ï¿½ï¿½ï¿½à·´
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
                            currentSpeed += rhymeExtraSpeed;
                            rhymeObject.SetActive(true);
                            rhymeBoostAmount++;
                            rhymeTimer = 0f;
                        }
                    }
                    oarLeft.RotateOnce();
                    oarRight.RotateOnce();
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
    #region º£Áé
    public void HandleShiftInput()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(!isInSoulTime&&soulAmount>0)
            {
                soulAmount -= 1;
                isInSoulTime = true;
                currentSpeed += soulExtraSpeed;
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
    public void AddSoulAmount(int amount)
    {
        soulAmount = Mathf.Clamp(soulAmount+amount, 0f, maxSoulAmount);
    }
    #endregion
}
