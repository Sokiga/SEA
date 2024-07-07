using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    //获取一下signSprite这个项目
    public GameObject signSprite;
    //获得这个可交互的物体捏（直接用接口来创建
    private IInteractable targetItem;
    //创建状态判断是否可以交互
    public bool canPress;

    public DialogManager dialogManager;
    public bool isInDialog;

    private void OnDisable()
    {
        //“人物被关闭的时候，你也就被关闭罢！”
        canPress = false;
    }
    private void Update()
    {
        isInDialog = dialogManager.gameObject.activeSelf;

        //如果是可互动的物体，就激活这个项目
        signSprite.GetComponent<Image>().enabled = canPress;

        if (canPress && Input.GetKeyDown(KeyCode.F))
            targetItem.TriggerAction();
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("碰到东西了");
        //同上
        if (other.CompareTag("Interactable") && !isInDialog)
        {
            //Debug.Log("可交互");
            canPress = true;
            targetItem = other.GetComponent<IInteractable>();
        }
        else
        {
            canPress = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canPress = false;
    }
}
