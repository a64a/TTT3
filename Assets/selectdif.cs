using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class selectdif : MonoBehaviour
{
    public static string but;
    public List<Button> buttonList;
    public List<Behaviour> popuplist;
    public bool boolc = true;

    void Start()
    {
        for (int i = 0; i < popuplist.Count; i++)
        {
            popuplist[i].enabled = false;
        }
        foreach (Button button in buttonList)
        {
            if (button.name == "terminal" || button.name == "tictactoe")
            {
                DoubleClickButton doubleClickButton = button.gameObject.AddComponent<DoubleClickButton>();
                doubleClickButton.doubleClickCallback = ButtonDoubleClicked;
            }
            else
            {
                button.onClick.AddListener(() => ButtonClicked(button));
            }
        }
    }

    void ButtonDoubleClicked(Button clickedButton)
    {
        if (clickedButton.name == "terminal")
        {
            SceneManager.LoadScene("Options", LoadSceneMode.Single);
        }
        if (clickedButton.name == "tictactoe")
        {
            for (int i = 0; i < popuplist.Count; i++)
            {
                popuplist[i].enabled = boolc;
            }
            boolc = !boolc;
        }
    }

    void ButtonClicked(Button clickedButton)
    {
        but = clickedButton.name;
        SceneManager.LoadScene("TTT", LoadSceneMode.Single);
    }

     void DisableEl<T>(List<T> what) where T : Behaviour
    {
        for (int i = 0; i < what.Count; i++)
        {
            what[i].enabled = boolc;
            Debug.Log("a");
        }
    }
}
