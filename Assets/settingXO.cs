using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SettingXO : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip placeXClip;
    public AudioClip placeOClip;
    public List<Image> Assets;
    public List<Button> buttonList;
    public Sprite sprite1;
    public Sprite sprite2;
    public Sprite grid;
    public Sprite empty;
    private int who = 0;
    private string[] list;
    private bool gameover;
    public string botdif;
    private bool botMoved = false;
    public bool solo;
    public Sprite[] FinalSprites = new Sprite[3];
    
    void Start()
    {
        for (int i = 0; i < CommandInput.final.Length; i++)
        {
            if (CommandInput.final[i] == null)
            {
                if (i == 2)
                {
                    CommandInput.final[i] = FinalSprites[2];
                }
                else
                {
                    CommandInput.final[i] = FinalSprites[i];
                }
            }
        }
        FinalSprites = CommandInput.final;
        if (FinalSprites[2].name != "grid2")
        {
            for (int i = 0; i <= 3; i++)
            {
                Assets[i].enabled = false;
            }
        }
        if (FinalSprites[2].name == "grid1")
        {
            Assets[4].sprite = FinalSprites[2];
        }
        if (FinalSprites[2].name == "grid3")
        {
            Assets[4].sprite = FinalSprites[2];
        }
        if (FinalSprites[2].name == "grid2")
        {
            Assets[4].enabled = false;
        }
        if (FinalSprites[0] != null)
        {
            sprite1 = FinalSprites[0];
        }
        if (FinalSprites[1] != null)
        {
            sprite2 = FinalSprites[1];
        }
        foreach (Button button in buttonList)
        {
            button.onClick.AddListener(() => ChangeSprite(button));
        }
        list = new string[10];
        if (selectdif.but == "solo")
        {
            solo = true;
        } else if (selectdif.but == "easy")
        {
            botdif = "easy";
        } else if (selectdif.but == "hard")
        {
            botdif = "hard";
        }
    }

    void ChangeSprite(Button clickedButton)
    {
        if (clickedButton.image.sprite == empty && !gameover)
        {
            string currentPlayer = who % 2 == 1 ? "o" : "x";
            clickedButton.image.sprite = currentPlayer == "o" ? sprite1 : sprite2;
            list[int.Parse(clickedButton.name)] = currentPlayer;
            who++;

            if (currentPlayer == "o")
            {
                audioSource.PlayOneShot(placeOClip);
            }
            else
            {
                audioSource.PlayOneShot(placeXClip);
            }

            if (CheckForWin())
            {
                Debug.Log("Wygrał " + (currentPlayer == "x" ? "X" : "O"));
                gameover = true;
            }
            else if (CheckForDraw())
            {
                Debug.Log("Remis!");
                gameover = true;
            }

            if (gameover)
            {
                SceneManager.LoadScene(0);
            }

            if (!solo && !botMoved && !gameover)
            {
                StartCoroutine(BotMove());
            }
        }
    }

    IEnumerator BotMove()
    {
        botMoved = true;
        yield return new WaitForSeconds(.1f);

        if (botdif == "easy")
        {
            MakeRandomMove();
        }
        else if (botdif == "hard")
        {
            MakeHardMove();
        }

        botMoved = false;
    }

    void MakeHardMove()
    {
        int bestScore = int.MinValue;
        int bestMove = -1;

        for (int i = 1; i <= 9; i++)
        {
            if (list[i] == null)
            {
                list[i] = "o";
                if (CheckForWin())
                {
                    ChangeSprite(buttonList[i - 1]);
                    return;
                }
                list[i] = null;
            }
        }

        for (int i = 1; i <= 9; i++)
        {
            if (list[i] == null)
            {
                list[i] = "x";
                if (CheckForWin())
                {
                    ChangeSprite(buttonList[i - 1]);
                    return;
                }
                list[i] = null;
                list[i] = "o";
                int score = MiniMax(list, 0, false);
                list[i] = null;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
        }

        if (bestMove != -1)
        {
            ChangeSprite(buttonList[bestMove - 1]);
        }
    }

    int MiniMax(string[] board, int depth, bool isMaximizing)
    {
        if (CheckForWin())
        {
            return isMaximizing ? -1 : 1;
        }
        else if (CheckForDraw())
        {
            return 0;
        }

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;

        for (int i = 1; i <= 9; i++)
        {
            if (board[i] == null)
            {
                board[i] = isMaximizing ? "o" : "x";
                int score = MiniMax(board, depth + 1, !isMaximizing);
                board[i] = null;

                bestScore = isMaximizing ? Mathf.Max(bestScore, score) : Mathf.Min(bestScore, score);
            }
        }

        return bestScore;
    }

    void MakeRandomMove()
    {
        List<Button> emptyCells = buttonList.FindAll(b => b.image.sprite == empty);
        if (emptyCells.Count > 0)
        {
            int randomIndex = Random.Range(0, emptyCells.Count);
            ChangeSprite(emptyCells[randomIndex]);
        }
    }

    bool CheckForWin()
    {
        return (CheckRow(1, 2, 3) || CheckRow(4, 5, 6) || CheckRow(7, 8, 9) ||
                CheckRow(1, 4, 7) || CheckRow(2, 5, 8) || CheckRow(3, 6, 9) ||
                CheckRow(1, 5, 9) || CheckRow(3, 5, 7));
    }

    bool CheckRow(int a, int b, int c)
    {
        if (list[a] != null && list[a] == list[b] && list[b] == list[c])
        {
            return true;
        }
        return false;
    }

    bool CheckForDraw()
    {
        for (int i = 1; i < list.Length; i++)
        {
            if (list[i] == null)
            {
                return false;
            }
        }
        return true;
    }
}