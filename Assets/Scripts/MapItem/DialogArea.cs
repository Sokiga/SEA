using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogArea : MonoBehaviour,IInteractable
{
    public DialogManager dialogManager;
    public int dialogIndexBeforeFinishTask;      //�������ǰ�ĶԻ�
    public int dialogIndexAfterFinishTask;       //��������ĶԻ�
    public bool isFinishTask;                    //�Ƿ��������

    public void TriggerAction()
    {

            if (isFinishTask)
            {
                dialogManager.dialogIndex = dialogIndexAfterFinishTask;
                dialogManager.gameObject.SetActive(true);
                Destroy(this.gameObject);
            }
            else
            {
                dialogManager.dialogIndex = dialogIndexBeforeFinishTask;
                dialogManager.gameObject.SetActive(true);
            }
              
    }
}
