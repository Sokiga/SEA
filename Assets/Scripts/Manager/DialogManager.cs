using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text dialogText;      // UI Text组件，用于显示文字

    [Header("对话文本文件")]
    public TextAsset dialogDataFile;

    //当前对话的索引值
    public int dialogIndex;
    //对话文本按行分割
    public string[] dialogRows;

    [Header("字体滚动")]
    //对话框当前状态，是否正在滚动，滚动时间间隔
    public bool isScrolling;
    private float textSpeed = 0.05f;



    public void Awake()
    {
        ReadText(dialogDataFile);
        ShowDialogRow();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 检测鼠标左键单击
        {
            ShowDialogRow();
        }
    }

    public void ReadText(TextAsset _textAsset)
    {
        //用换行来分割开文本里的每一行
        dialogRows = _textAsset.text.Split('\n');
        Debug.Log("读取成功");
    }

    public void ShowDialogRow()
    {
        for (int i = 1; i < dialogRows.Length; i++)
        {
            //用逗号分割每一行的每一个单元格
            string[] cells = dialogRows[i].Split(',');

            //如果是上一个跳转过来的序号，那么就执行
            if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
            {
                dialogText.text = ParseAndColorizeString(cells[2]);

                dialogIndex = int.Parse(cells[3]);//更新下一次要跳转到的序号

                break;//找到要执行的行之后跳出循环
            }
            else if(cells[0] == "END")
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    //实现文字滚动
    private IEnumerator ScrollingText(string _text)
    {
        isScrolling = true;

        dialogText.text = "";

        foreach (char letter in _text.ToCharArray())
        {
            if (!isScrolling)
            {
                dialogText.text = _text;
                break;
            }
            dialogText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }

        isScrolling = false;
    }

    //实现转色高亮
    private string ParseAndColorizeString(string input)
    {
        string result = "";
        string[] parts = input.Split(';');

        for (int i = 0; i < parts.Length; i += 5)
        {
            if (i + 4 < parts.Length)
            {
                int r = int.Parse(parts[i]);
                int g = int.Parse(parts[i + 1]);
                int b = int.Parse(parts[i + 2]);
                int a = int.Parse(parts[i + 3]);
                string textPart = parts[i + 4];

                Color32 color = new Color32((byte)r, (byte)g, (byte)b, (byte)a);
                string colorHex = ColorUtility.ToHtmlStringRGBA(color);

                result += $"<color=#{colorHex}>{textPart}</color>";
            }
        }

        return result;
    }
}
