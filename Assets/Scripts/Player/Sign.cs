using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    //��ȡһ��signSprite�����Ŀ
    public GameObject signSprite;
    //�������ɽ�����������ֱ���ýӿ�������
    private IInteractable targetItem;
    //����״̬�ж��Ƿ���Խ���
    public bool canPress;

    public DialogManager dialogManager;
    public bool isInDialog;

    private void OnDisable()
    {
        //�����ﱻ�رյ�ʱ����Ҳ�ͱ��رհգ���
        canPress = false;
    }
    private void Update()
    {
        isInDialog = dialogManager.gameObject.activeSelf;

        //����ǿɻ��������壬�ͼ��������Ŀ
        signSprite.GetComponent<Image>().enabled = canPress;

        if (canPress && Input.GetKeyDown(KeyCode.F))
            targetItem.TriggerAction();
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("����������");
        //ͬ��
        if (other.CompareTag("Interactable") && !isInDialog)
        {
            //Debug.Log("�ɽ���");
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
