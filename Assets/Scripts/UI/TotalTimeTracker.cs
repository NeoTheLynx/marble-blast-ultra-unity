using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalTimeTracker : MonoBehaviour
{
    public static TotalTimeTracker instance;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private float sessionStart;
    private const string TotalTimeKey = "TotalRuntimeSeconds";

    void Start()
    {
        sessionStart = Time.realtimeSinceStartup;
    }

    void OnApplicationQuit()
    {
        SaveTotalTime();
    }

    public void SaveTotalTime()
    {
        float now = Time.realtimeSinceStartup;
        float sessionTime = now - sessionStart;

        if (sessionTime <= 0f)
            return;

        float total = PlayerPrefs.GetFloat(TotalTimeKey, 0f);
        PlayerPrefs.SetFloat(TotalTimeKey, total + sessionTime);
        PlayerPrefs.Save();

        sessionStart = now;
    }

}
