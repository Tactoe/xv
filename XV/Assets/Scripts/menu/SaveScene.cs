using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SaveScene : MonoBehaviour
{
    public TMP_InputField MyField;
    public GameObject Err1;
    public GameObject Err2;

    // Start is called before the first frame update
    void Start()
    {
        MyField.onSubmit.AddListener(sayChatMessage);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckBeforeSave(){
        // Debug.Log("j'envoie " + MyField.text);
        string m_stringToCheck = MyField.text;
        if(m_stringToCheck == ""){
            Err1.SetActive(true);
        }
        else if (PlayerPrefs.HasKey(m_stringToCheck)){
            Err2.SetActive(true);
        }
        else{
            Debug.Log("SAVE OK ");
            SaveManager.Instance.Save(m_stringToCheck);
            PlayerPrefs.SetString("savesNames", PlayerPrefs.GetString("savesNames") + ";" + m_stringToCheck);
            Debug.Log(PlayerPrefs.GetString(m_stringToCheck));
        }
    }

    public void CleanErr(){
        Err1.SetActive(false);
        Err2.SetActive(false);
    }


 
    void sayChatMessage(string msg){
        CheckBeforeSave();
    }
}
