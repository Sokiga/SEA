using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : new()
{
    // 用于锁定对象，确保线程安全
    private static readonly object lockObj = new object();
    private static T instance;

    public static T Instance
    {
        get
        {
            // 双重检查锁定
            if (instance == null)
            {
                lock (lockObj) // 第一个线程会获得锁，其它线程会等待
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
