using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class LoadSaves : MonoBehaviour
{
    public GameObject SavePrefab;

    private string m_saves;
    private string[] m_aSaves;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        PlayerPrefs.SetString("saves", "test1###test2###test3###test4###test5###test6###test7###test8###test9");
        m_saves = PlayerPrefs.GetString("saves");
        Debug.Log("m_saves = ");
        Debug.Log(m_saves);
        m_aSaves = m_saves.Split("###");
        Debug.Log("m_aSaves = ");
        Debug.Log( m_aSaves);

        // int x = -50;

        foreach(string line in m_aSaves)
        {
            Debug.Log("line = " + line);
            GameObject tmp =  Instantiate(SavePrefab, transform);
            // tmp.transform.localPosition = new Vector2(0, x);
            Debug.Log("tmp = ");
            Debug.Log( tmp);
            tmp.GetComponentInChildren<TextMeshProUGUI>().SetText(line);
            // x -= 100;
        }
    }
}
