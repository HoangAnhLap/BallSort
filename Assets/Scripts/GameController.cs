using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    
    
    public List<TheStack> theStacks = new List<TheStack>();
    public List<MoveState> undoMove = new List<MoveState>();
    
    
    public List<HorizontalLayoutGroup> lines = new List<HorizontalLayoutGroup>();
    public TheStack stackPrefab;

    public List<int> type; 
    public bool isCompleted = false;


    public GameObject panelWin;
    
    
    private void Start()
    {
        type = new List<int>()
        {
            3,1,3,2,0,1,2,0,0,1,0,2,2,1,3,3
        };
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        SpawnStack();
    }
    private void SpawnStack()
    {
        for (int i = 0; i < lines.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                var stack = Instantiate(stackPrefab, lines[i].transform);
                theStacks.Add(stack);
                
            }
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                var bubble = theStacks[i].InstantiateBubble(type[4 * i + j]);
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
            TheStack.poppedBubble.thisStack.PushPopBack();
        }
        undo.from.PushBack(undo.bubble);
        undo.to.RemoveLastBubble();
    }

    

    public void RePlayGame()
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

    public void NextLevel()
    {
        Debug.Log("next level");
    }
    
    
}
