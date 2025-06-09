using System;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;


    public List<TheStack> theStacks = new List<TheStack>();
    public List<MoveState> undoMove = new List<MoveState>();


    public List<HorizontalLayoutGroup> lines = new List<HorizontalLayoutGroup>();
    public TheStack stackPrefab;

    public bool isCompleted = false;


    public GameObject panelWin;
    private LevelData levelData;
    
    
    //Ui numbermoved
    public TextMeshProUGUI countDownText;
    private float currentTime;
    //Người chơi đã bắt đầu chơi chưa
    public bool isPlayed = false;
    
    
    //
    public TextMeshProUGUI levelText;
    
    


    //Thêm chức năng tính số lượt cho di chuyển bóng
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SpawnLevel();
    }

    private void Update()
    {
        if (isPlayed)
        {
            UpdateTimer();
        }
    }


    public void SpawnLevel()
    {
        string path = string.Format("Levels/level_{0}",  Level);
        var textAsset = Resources.Load(path) as TextAsset;
        levelData = JsonUtility.FromJson<LevelData>(textAsset.text);
        int numStack = levelData.numStack;
        currentTime = (numStack - 2) * 15;
        UpdateTimer();
        int count = 0;
        levelText.text = "Level " + Level;
        //Điều chỉnh lại việc spawn stack
        //Dựa trên số lượng sẽ spawn ở level mà quyết định một dòng có baon stack
        //Spawn stack
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                count += 1;
                if (count > numStack) break;
                var stack = Instantiate(stackPrefab, lines[i].transform);
                theStacks.Add(stack);
            }
        }


        //Spawn Bubbles
        for (int i = 0; i < numStack -2 ; i++)//Số lượng stack
        {
            for (int j = 0; j < 4; j++)
            {
                if (4 * i + j > levelData.bubbleTypes.Count - 1)
                {
                    break;
                }

                int type = levelData.bubbleTypes[4 * i + j];
                var bubble = theStacks[i].InstantiateBubble(type);
                theStacks[i].ForcePush(bubble);
            }
        }


    }
    public void CheckComplete()
    {
        isCompleted = true;
        foreach (var stack in theStacks)
        {
            if (!stack.IsSameFullColor())
            {
                isCompleted = false;
                break;
            }
        }

        if (isCompleted)
        {
            ShowWinner();
        }
    }

    public void UndoStack()
    {
        if (undoMove.Count == 0)
        {
            Debug.Log("khong co luot undo nào");
            return;
        }

        var undo = undoMove[undoMove.Count - 1];
        undoMove.RemoveAt(undoMove.Count - 1);
        if (TheStack.poppedBubble != null)
        {
            TheStack.poppedBubble.stack.PushPopBack();
        }

        undo.from.PushBack(undo.bubble);
        undo.to.RemoveLastBubble();
    }


    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Debug.Log("Thoat game");
        Application.Quit();
    }

    public void ShowWinner()
    {
        Debug.Log("Show winner");
        panelWin.SetActive(true);
    }


    public static int Level
    {
        get { return PlayerPrefs.GetInt("_level", 0); }
        set { PlayerPrefs.SetInt("_level", value); }
    }

    public void NextLevel()
    {
        Level++;
        Debug.Log("Level current" + Level);
        Reload();
    }

    private void UpdateTimer()
    {
        Debug.Log(currentTime);
        currentTime -= Time.deltaTime;
        int seconds = Mathf.FloorToInt(currentTime % 60);
        int  minutes = Mathf.FloorToInt(currentTime / 60);
        countDownText.text = $"{minutes:00}:{seconds:00}";
        if (currentTime <= 0)
        {
            countDownText.text = "00:00";
            Debug.Log("You lose");

        }
    }

    
}