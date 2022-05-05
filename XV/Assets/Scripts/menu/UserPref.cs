using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserPref : MonoBehaviour
{
    string savesNames;
    int nbSaves;

    void Start()
    {
        if (PlayerPrefs.GetString("savesNames") != null) {
            savesNames = PlayerPrefs.GetString("savesNames");
        } else {
            PlayerPrefs.SetString("savesNames", "");
        }
        if (PlayerPrefs.GetInt("nbSaves") != null) {
            nbSaves = PlayerPrefs.GetInt("nbSaves");
        } else {
            PlayerPrefs.SetInt("nbSaves", 0);
        }
    }

    void Update()
    {
        
    }

    public void Save() {
        PlayerPrefs.Save();
    }

    public void Reset() {
        PlayerPrefs.SetString("savesNames", "");
        PlayerPrefs.SetInt("nbSaves", 0);
        Save();
    }
}
