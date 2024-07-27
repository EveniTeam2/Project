using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempInput : MonoBehaviour
{
    public KeyCode exit;
    public float waitTime;
    public int limitCount;

    private int count;
    private float passed;

    private void Awake()
    {
        count = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(exit))
        {
            passed = 0;
            if (count > limitCount)
                Application.Quit();
            else
                ++count;
        }
        else
        {
            if (passed > waitTime)
                count = 0;
            else
                passed += Time.deltaTime;
        }

    }
}
