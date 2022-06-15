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
    [SerializeField]
    Toggle m_OnlineSave;
    [SerializeField]
    TextMeshProUGUI m_SaveIdDisplayText;
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
        Debug.Log("m_OnlineSave.isOn " + m_OnlineSave.isOn);
        string m_stringToCheck = MyField.text;
        if(m_stringToCheck == ""){
            Err1.SetActive(true);
        }
        else if (PlayerPrefs.HasKey(m_stringToCheck)){
            Err2.SetActive(true);
        }
        else{
            // Debug.Log("SAVE OK ");
            // SaveManager.Instance.Save(m_stringToCheck, m_OnlineSave.isOn);
            // PlayerPrefs.SetString("savesNames", PlayerPrefs.GetString("savesNames") + m_stringToCheck + ";" );
            // Debug.Log(PlayerPrefs.GetString(m_stringToCheck));
            // gameObject.SetActive(false);
            fonctionSave();
        }
    }

    IEnumerator WaitForSaveId()
    {
        int timer = 100;
        while (SaveManager.Instance.saveId_Aftersave == "" && timer >= 0)
        {
            Debug.Log("waiting ...." + SaveManager.Instance.saveId_Aftersave);
            m_SaveIdDisplayText.text ="waiting for saveId...";
            timer--;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        Debug.Log("OUTT ....");
        if(timer <= 0){
            m_SaveIdDisplayText.text ="Server did not respond...";
        }
        else{
            m_SaveIdDisplayText.text = "Save online done id is :" + SaveManager.Instance.saveId_Aftersave;   
        }
        yield return new WaitForSecondsRealtime(10);
        m_SaveIdDisplayText.gameObject.SetActive(false);
        SaveManager.Instance.saveId_Aftersave = "";
        gameObject.SetActive(false);
    }

    public void CleanErr(){
        Err1.SetActive(false);
        Err2.SetActive(false);
    }

    public void fonctionSave(){
        string m_stringToCheck = MyField.text;
        Debug.Log("SAVE OK ");
        SaveManager.Instance.Save(m_stringToCheck, m_OnlineSave.isOn);
        if (m_OnlineSave.isOn)
        {
            m_SaveIdDisplayText.gameObject.SetActive(true);
            StartCoroutine(WaitForSaveId());
        }
        else{
            Debug.Log(PlayerPrefs.GetString(m_stringToCheck));
            gameObject.SetActive(false);
        }
    }
    
    public void ForceSave(){
        Debug.Log("m_OnlineSave.isOn " + m_OnlineSave.isOn);
        Err2.SetActive(false);
        fonctionSave();
        // string m_stringToCheck = MyField.text;
        // Debug.Log("SAVE OK ");
        // SaveManager.Instance.Save(m_stringToCheck, m_OnlineSave.isOn);
        // Debug.Log(PlayerPrefs.GetString(m_stringToCheck));
        // gameObject.SetActive(false);
    }

 
    void sayChatMessage(string msg){
        CheckBeforeSave();
    }
}
