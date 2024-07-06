using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    #region ï¿½ï¿½×ª
    public float rotationSpeed;
    public float rotationAngle;
    public float rotationSmoothTime;
    private float rotationSmoothSpeed;
    #endregion
    #region ï¿½Æ¶ï¿½
    private CharacterController characterController;
    private Rigidbody rb;
    public float currentSpeed;
    public float basicSpeed=10;
    public float frictionCoefficient=2f;
    private Vector3 currentVelocity;
    //public float nitroBoostMultiplier = 2.0f; // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ù±ï¿½ï¿½ï¿½
    //public float nitroDuration = 2.0f; // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Ê±ï¿½ï¿½
    #endregion
    #region ï¿½ï¿½ï¿½ï¿½ï¿½Õ¸ï¿½
    public float spaceTimer;
    public float boostDuration=0.4f;
    public bool firstEnter;
    public float rhymeBoostAmount;
    #endregion
    #region ï¿½ï¿½ï¿½ï¿½ï¿½Ê±ï¿½ï¿?
    private float rhymeTimer;
    private bool startRhymeTimer;
    public GameObject rhymeObject;
    public float rhymeExtraSpeed = 5f;
    #endregion
    #region ï¿½ï¿½ï¿½ï¿½
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
    #region ï¿½ï¿½ï¿½ï¿½ï¿½×?

    private void HandleRotationInput()
    {
        Vector2 input = PlayerInput.instance.MoveDirection;
        

        if (input.x != 0&&PlayerInput.instance.MouseDirection.x==0)
        {
            // ï¿½ï¿½ï¿½ï¿½Dï¿½ï¿½Ê±Ë³Ê±ï¿½ï¿½ï¿½ï¿½×ªï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Aï¿½ï¿½Ê±ï¿½ï¿½Ê±ï¿½ï¿½ï¿½ï¿½×ª
            rotationAngle += input.x * rotationSpeed * Time.deltaTime;
        }
    }
    private void ApplyRotation()
    {
        //float currentYAngle = transform.eulerAngles.y;
        //float targetYAngle = rotationAngle;
        //float smoothYAngle = Mathf.SmoothDampAngle(currentYAngle, targetYAngle, ref rotationSmoothSpeed, rotationSmoothTime);

        //// Ö»ÐÞ¸Ä y ÖáµÄÐý×ª½Ç¶È£¬±£Áô x ºÍ z ÖáµÄÐý×ª½Ç¶È
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, smoothYAngle, transform.eulerAngles.z);

        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref rotationSmoothSpeed, rotationSmoothTime);
    }

    #endregion
    #region ï¿½ï¿½ï¿½ï¿½Æ¶ï¿½ï¿½Íµï¿½ï¿½ï¿½ï¿½ï¿?
    private void Move()
    {
        currentVelocity = currentSpeed  * transform.forward;
        characterController.Move(currentVelocity*Time.deltaTime);
        //characterController.Move(currentVelocity*Time.deltaTime);
        //characterController.Move(currentVelocity*Time.deltaTime);
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
    #region ï¿½ï¿½Òµï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿?
    public void HandleShiftInput()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(!isInSoulTime&&soulAmount>0)
            {
                soulAmount -= 1;
                isInSoulTime = true;
                currentSpeed += soulExtraSpeed;
                //ï¿½ï¿½ï¿½ï¿½
                //ï¿½ï¿½Ê¼ï¿½ï¿½Ê±ï¿½ï¿½,ï¿½ï¿½Îª5sï¿½Ú¿Ï¶ï¿½ï¿½ï¿½Ä¦ï¿½ï¿½ï¿½ï¿½Åªï¿½ï¿½ï¿½Ë£ï¿½ï¿½ï¿½Ã»ï¿½ï¿½Òª
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
