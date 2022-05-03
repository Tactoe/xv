using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class recoder_test : MonoBehaviour
{

    void Awake()
    {
        // Process.Start("ffmpeg", " -f avfoundation -list_devices true -i");
        string strCmdText;
        strCmdText= "ffmpeg -f avfoundation -list_devices true -i";
        System.Diagnostics.Process.Start("ffmpeg", strCmdText);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
