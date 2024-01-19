using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommandInput : MonoBehaviour
{
    public Sprite empty;
    public List<Sprite> SpriteList;
    public List<Image> ImageList;
    public TMP_InputField inputField;
    public TMP_Text commandText; 
    private string promptSymbol = "> ";
    private bool valid;
    private string check;
    public static Sprite[] final = new Sprite[3];

    private void Start()
    {
        inputField.onEndEdit.AddListener(OnEndEdit);
        inputField.onValueChanged.AddListener(OnValueChanged);
        UpdatePrompt();
        inputField.ActivateInputField();
    }

    private void OnValueChanged(string inputText)
    {
        if (inputText.Length < promptSymbol.Length || !inputText.StartsWith(promptSymbol))
            inputField.text = promptSymbol;
    }

    private void OnEndEdit(string inputText)
    {
        if (inputText.Length <= promptSymbol.Length)
            UpdatePrompt();
        else
        {
            string command = inputText.Substring(promptSymbol.Length);
            ProcessCommand(command);
            UpdatePrompt();
        }

        inputField.ActivateInputField();
    }

    private void ProcessCommand(string command)
    {
        valid = false;

        switch (command)
        {
            case "grid1":
            case "grid2":
            case "grid3":
            case "alt11":
            case "alt12":
            case "alt21":
            case "alt22":
            case "alt31":
            case "alt32":
                valid = true;
                AddCommandToText($"Command: {command} is valid.", command);
                break;

            case "help":
                AddCommandToText("List of commands: grid1, grid2, grid3, alt11, alt12, alt21, alt22, alt31, alt32, exit, help", command);
                break;

            case "exit":
                final[0] = ImageList[0].sprite;
                final[1] = ImageList[1].sprite;
                final[2] = ImageList[2].sprite;
                for (int i = 0; i < final.Length; i++)
                {
                    if (ImageList[i].sprite == empty)
                    {
                        if (i == 2)
                        {
                            final[i] = SpriteList[6];
                        }
                        else
                        {
                            final[i] = SpriteList[i];
                        }
                    }
                }
                SceneManager.LoadScene("Menu", LoadSceneMode.Single);
                break;


            default:
                AddCommandToText("That command doesn't exist, help for a list of commands", command);
                break;
        }
    }

    private void UpdatePrompt()
    {
        inputField.text = promptSymbol;
        inputField.MoveTextEnd(false);
        inputField.caretPosition = promptSymbol.Length;
        inputField.selectionAnchorPosition = promptSymbol.Length;
        inputField.selectionFocusPosition = promptSymbol.Length;
    }

    private void AddCommandToText(string message, string command)
    {
        if (valid)
        {
            check = $"{command[0]}{command[^2]}{command[^1]}";
            switch (check)
            {
                case "gd1":
                    ImageList[2].sprite = SpriteList[6];
                    break;
                case "gd2":
                    ImageList[2].sprite = SpriteList[7];
                    break;
                case "gd3":
                    ImageList[2].sprite = SpriteList[8];
                    break;
                case "a11":
                    ImageList[0].sprite = SpriteList[0];
                    break;
                case "a12":
                    ImageList[1].sprite = SpriteList[1];
                    break;
                case "a21":
                    ImageList[0].sprite = SpriteList[2];
                    break;
                case "a22":
                    ImageList[1].sprite = SpriteList[3];
                    break;
                case "a31":
                    ImageList[0].sprite = SpriteList[4];
                    break;
                case "a32":
                    ImageList[1].sprite = SpriteList[5];
                    break;
            }
            valid = false;
        }
        commandText.text += $"{message}\n";
    }
}