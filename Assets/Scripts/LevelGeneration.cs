using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class LevelGeneration : MonoBehaviour
{
    //Out put
    //Danh sách bóng được spawn

    private LevelData levelData;

    //Số loại bóng sẽ spawn
    private int typesNum;

    //Danh sách loại bóng sẽ xuất hiện
    public List<int> colorIds = new List<int>();

    private void Start()
    {
        for (int i = 1; i <= 20; i++)
        {
            colorIds = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
            SpawnEditor(i);
        }
    }

    private void SpawnEditor(int level)
    {
        //RanDom số lượng stack
        int numStack = UnityEngine.Random.Range(6, 9);
        //Tính toán số loại bóng sẽ spawn(số lượng bóng sẽ spawn là số numStack -2
        typesNum = numStack - 2;
        //Random ball corlor sẽ xuất hiện trong màn chơi
        int amountId = colorIds.Count - typesNum;
        for (int i = 0; i < amountId; i++)
        {
            int index = UnityEngine.Random.Range(0, colorIds.Count);
            colorIds.RemoveAt(index);
        }

        //Xếp bóng vào đúng vị trí trong danh sách
        List<int> listBall = new List<int>();
        for (int i = 0; i < colorIds.Count; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                listBall.Add(colorIds[i]);
            }
        }

        Debug.Log("Numstack " + numStack + " colorIds " + colorIds.Count + "So bong xep dung vi tri" + listBall.Count);

        List<int> bubbleTypes = new List<int>();
        //Xếp bóng ngẫu nhiên vào danh sách sẽ spawn
        for (int i = listBall.Count - bubbleTypes.Count; i > 0; i--)
        {
            int index = UnityEngine.Random.Range(0, listBall.Count);
            bubbleTypes.Add(listBall[index]);
            listBall.RemoveAt(index);
        }
        var levelData = new LevelData()
        {
            numStack = numStack,
           // bubbleTypes = bubbleTypes,
        };

        var content = JsonUtility.ToJson(levelData);
#if UNITY_EDITOR
        bool isExits = UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources/Levels");
        if (!isExits)
        {
            UnityEditor.AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        }

        string path = string.Format("Assets/Resources/Levels/level_{0}.json", level);
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(fs))
            {
                writer.Write(content);
            }
        }

        UnityEditor.AssetDatabase.Refresh();
#endif
    }
}