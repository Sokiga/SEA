using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
    // ������������ȷ���̰߳�ȫ
    private static readonly object lockObj = new object();
    private static T instance;

    public static T Instance
    {
        get
        {
            // ˫�ؼ������
            if (instance == null)
            {
                lock (lockObj) // ��һ���̻߳������������̻߳�ȴ�
                {
                    if (instance == null)
                    {
                        instance = new T();
                    }
                }
            }
            return instance;
        }
    }
}
