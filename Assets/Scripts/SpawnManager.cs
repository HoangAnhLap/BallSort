using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public List<TheStack> theStacks = new List<TheStack>();
    
    
    public List<HorizontalLayoutGroup> lines = new List<HorizontalLayoutGroup>();
    public TheStack stackPrefab;

    public List<int> type = new List<int>()
    {
        1, 1, 2, 2, 1, 0, 2, 0, 0, 1, 2, 0
    };
    private void Start()
    {
        
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
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                var bubble = theStacks[i].InstantiateBubble(type[4 * i + j]);
                theStacks[i].ForcePush(bubble);
            }
        }
    }
}
