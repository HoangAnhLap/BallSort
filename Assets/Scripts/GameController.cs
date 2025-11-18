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


    [SerializeField] private GameObject panelWin;
    [SerializeField]private Button continueBtn;

    [SerializeField] private GameObject loading;


    private LevelData levelData;
    public GameMode Mode { get; set; }

    //Thêm chức năng tính số lượt cho di chuyển bóng

    private void Awake()
    {
        SetStatusGame(GameStatus.Loading);
    }

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

        continueBtn.onClick.AddListener(Continue);


    }


    public void SpawnLevel()
    {
        SetStatusGame(GameStatus.Playing);
        Mode = DataPlayerToSave.Level % 3 == 0 ? GameMode.Challenging : GameMode.Classic;
        string path = $"Levels/level_{DataPlayerToSave.Level}";
        var textAsset = Resources.Load(path) as TextAsset;
        levelData = JsonUtility.FromJson<LevelData>(textAsset.text);
        int numStack = levelData.numStack;
        int count = 0;
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
        for (int i = 0; i < numStack - 2; i++) //Số lượng stack
        {
            for (int j = 0; j < 4; j++)
            {
                if (4 * i + j > levelData.bubbleTypes.Count - 1)
                {
                    break;
                }

                int type = levelData.bubbleTypes[4 * i + j];
                Bubble bubble;
                if (Mode == GameMode.Challenging)
                {
                    if (j == 3)
                    {
                        bubble = theStacks[i].InstantiateBubble(type, false);
                    }
                    else
                    {
                        bubble = theStacks[i].InstantiateBubble(type, true);
                    }
                }
                else
                {
                    bubble = theStacks[i].InstantiateBubble(type, false);
                }

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
            SetStatusGame(GameStatus.Completed);
        }
    }

    private void OnWin()
    {
        panelWin.SetActive(true);
        
    }

    private void Reload()
    {
        panelWin.SetActive(false);
        loading.SetActive(true);
        for (int i = 0; i < theStacks.Count; i++)
        {
            /*theStacks[i].gameObject.SetActive(false);
            theStacks.Remove(theStacks[i]);*/
            Destroy(theStacks[i].gameObject);
        }
        theStacks.Clear();
        SpawnLevel();
    }

    private void Continue()
    {
        DataPlayerToSave.NextLevel();
        SetStatusGame(GameStatus.Loading);
    }

    private void ReturnBubble()
    {
    }

    /*public void UndoStack()
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
    }*/


    /*public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }*/

    /*public void QuitGame()
    {
        Debug.Log("Thoat game");
        Application.Quit();
    }*/


    private void SetStatusGame(GameStatus s)
    {
        switch (s)
        {
            case GameStatus.Loading:
            {
                Debug.Log("Loading level");
                Reload();
                break;
            }
            case GameStatus.Playing:
            {
                Debug.Log("Playing");
                break;
            }
            case GameStatus.Completed:
            {
                Debug.Log("Completed");
                OnWin();
                break;
            }
            case GameStatus.Started:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(s), s, null);
        }
    }
}

public enum GameStatus
{
    Loading,
    Started,
    Playing,
    Completed,
}

public enum GameMode
{
    Classic,
    Challenging
}