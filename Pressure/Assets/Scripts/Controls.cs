using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

    string p1Left = "";
    string p1Right = "";
    string p1Start = "";

    string p2Left = "";
    string p2Right = "";
    string p2Start = "";

    //accessors:

    public string P1L {
        get { return p1Left; }
        set { p1Left = value; } 
    }
    public string P1R
    {
        get { return p1Right; }
        set { p1Right = value; } 
    }

    public string P1START
    {
        get { return p1Start; }
        set { p1Start = value; }
    }

    //p2
    public string P2L
    {
        get { return p2Left; }
        set { p2Left = value; }
    }
    public string P2R
    {
        get { return p2Right; }
        set { p2Right = value; }
    }

    public string P2START
    {
        get { return p2Start; }
        set { p2Start = value; }
    }


}
