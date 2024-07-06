using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogArea : MonoBehaviour
{
    public DialogManager dialogManager;
    public int dialogIndexAtThisPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("玩家进入剧情点");
            dialogManager.dialogIndex = dialogIndexAtThisPoint;
            dialogManager.gameObject.SetActive(true);
            //dialogManager.ShowDialogRow();
            Destroy(this.gameObject);
        }
    }
}
