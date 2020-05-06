using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoData : MonoBehaviour
{

    public storyboard[] storyboardData;

    [ContextMenu("OnValidate")]
    private void OnValidate() {
        for (int i = 0; i < storyboardData.Length; i++) {
            for (int j = 0; j < storyboardData[i].allCharacters.Length; j++)
            {
                System.Array.Sort(storyboardData[i].allCharacters[j].allFrames, (a, b) => a.name.CompareTo(b.name));
            }
        }
    }
}
[Serializable]
public class storyboard
{
    public string name;
    public Character[] allCharacters;
}

[Serializable]
public class Character
{
    public Texture2D[] allFrames;
}
