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
        else if (PlayerPrefs.HasKey(MyField.text)){
            Err2.SetActive(true);
        }
        else{
            //TODO call SAVE du fichier SaveManager
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
