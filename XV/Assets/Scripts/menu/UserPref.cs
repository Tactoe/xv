using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPref : MonoBehaviour
{
    string saves;

    void Start()
    {
        if (PlayerPrefs.GetString("saves") != null) {
            saves = PlayerPrefs.GetString("saves");
        } else {
            PlayerPrefs.SetString("saves", "");
        }
    }

    void Update()
    {
        
    }

    public void Save() {
        PlayerPrefs.Save();
    }

    public void Reset() {
        PlayerPrefs.SetString("saves", "test1###test2###test3###test4###test5###test6###test7###test8###test9");
        Save();
    }
}
