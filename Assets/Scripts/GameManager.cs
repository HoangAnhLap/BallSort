using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Tube> tubes = new List<Tube>();
    public GameObject panelWin;


    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
    public void CheckWinCodition()
    {
        foreach (var tube in tubes)
        {
            if (!tube.CheckComplete())
            {
                Debug.Log("Chua thang ");
                return;
            }
        }
        StartCoroutine(WinGame());
    }
    public IEnumerator WinGame()
    {
        yield return new WaitForSeconds(3f);
        panelWin.SetActive(true);
    }
    
    public void RestartGame() 
    {
        Debug.Log(" Đang tải lại màn chơi...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
