using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogArea : MonoBehaviour,IInteractable
{
    public DialogManager dialogManager;
    public int dialogIndexBeforeFinishTask;      //完成任务前的对话
    public int dialogIndexAfterFinishTask;       //完成任务后的对话
    public bool isFinishTask;                    //是否完成任务

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
