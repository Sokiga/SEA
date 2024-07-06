using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text dialogText;      // UI Text�����������ʾ����

    [Header("�Ի��ı��ļ�")]
    public TextAsset dialogDataFile;

    //��ǰ�Ի�������ֵ
    public int dialogIndex;
    //�Ի��ı����зָ�
    public string[] dialogRows;

    [Header("�������")]
    //�Ի���ǰ״̬���Ƿ����ڹ���������ʱ����
    public bool isScrolling;
    private float textSpeed = 0.05f;



    public void Awake()
    {
        ReadText(dialogDataFile);
        ShowDialogRow();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) // �������������
        {
            ShowDialogRow();
        }
    }

    public void ReadText(TextAsset _textAsset)
    {
        //�û������ָ�ı����ÿһ��
        dialogRows = _textAsset.text.Split('\n');
        Debug.Log("��ȡ�ɹ�");
    }

    public void ShowDialogRow()
    {
        for (int i = 1; i < dialogRows.Length; i++)
        {
            //�ö��ŷָ�ÿһ�е�ÿһ����Ԫ��
            string[] cells = dialogRows[i].Split(',');

            //�������һ����ת��������ţ���ô��ִ��
            if (cells[0] == "#" && int.Parse(cells[1]) == dialogIndex)
            {
                dialogText.text = ParseAndColorizeString(cells[2]);

                dialogIndex = int.Parse(cells[3]);//������һ��Ҫ��ת�������

                break;//�ҵ�Ҫִ�е���֮������ѭ��
            }
            else if(cells[0] == "END")
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    //ʵ�����ֹ���
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

    //ʵ��תɫ����
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
