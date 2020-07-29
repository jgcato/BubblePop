using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenToWorld : MonoBehaviour {
    //touch
    Stack<float> x = new Stack<float>();
    Stack<float> y = new Stack<float>();

    public Camera main;

    // Use this for initialization
    void Start () {

        x.Push(396.8f);
        x.Push(447.4f);
        x.Push(485.3f);
        x.Push(589.6f);
        x.Push(586.5f);
        x.Push(599.1f);
        x.Push(640.2f);
        x.Push(662.3f);
        x.Push(700.2f);
        x.Push(599.1f);
        x.Push(611.8f);
        x.Push(656.0f);
        x.Push(706.6f);
        x.Push(475.9f);

        y.Push(632.1f);
        y.Push(464.6f);
        y.Push(309.7f);
        y.Push(161.2f);
        y.Push(328.7f);
        y.Push(347.7f);
        y.Push(227.6f);
        y.Push(113.8f);
        y.Push(300.2f);
        y.Push(281.3f);
        y.Push(82.2f);
        y.Push(230.7f);
        y.Push(442.5f);
        y.Push(426.7f);

      
    }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < x.Count && i < y.Count; i++)
        {
            Debug.Log(i.ToString() + ":" + convertToUnits(x.Pop(), y.Pop()));
        }
    }

    public Vector2 convertToUnits(float X, float Y)
    {
        Vector3 s;
        s = main.ScreenToWorldPoint(new Vector3(X, Y, 0));
        return new Vector2(s.x, s.y);
    }

    public Vector2 convertToScreen(float X, float Y)
    {
        Vector3 s;
        s = main.WorldToScreenPoint(new Vector3(X, Y, 0));
        return new Vector2(s.x, s.y);
    }
  

}
