using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ManageStoryBoardFiles : MonoBehaviour
{
    public System.Action<string> onDebug;

    private int scenceNo = -1;
    private int characterNo = -1;
    private int frameNo = -1;
    private storyboard[] storyBoardAssets;


    public RawImage photoToSwap;
    public RawImage outlineToSwap;


    // Start is called before the first frame update
    void Start()
    {
        if (storyBoardAssets == null) storyBoardAssets = GameObject.FindObjectOfType<PhotoData>().storyboardData;
        

        initiatVars();
        SwapSourcePhotos();

        
    }

    private void initiatVars() {
        if (storyBoardAssets.Length > 0)
        {
            scenceNo = 0;
            if (storyBoardAssets[scenceNo].allCharacters.Length > 0)
            {
                characterNo = 0;
                frameNo = (storyBoardAssets[scenceNo].allCharacters[characterNo].allFrames.Length > 0) ? 0 : -1;
            }
        }
    }

    private void SwapSourcePhotos()
    {

        if (scenceNo != -1 || frameNo != -1 || characterNo != -1)
        {

            photoToSwap.texture = storyBoardAssets[scenceNo].allCharacters[characterNo].allFrames[frameNo];
            outlineToSwap.texture = storyBoardAssets[scenceNo].allCharacters[characterNo].allFrames[frameNo + 1];

            onDebug.Invoke("Showing Scene: " + (scenceNo+1) +"/" + storyBoardAssets.Length + ", Character:" + (characterNo + 1) + "/"+ storyBoardAssets[scenceNo].allCharacters.Length + ", Frame: " +(frameNo / 2 + 1) + "/" + storyBoardAssets[scenceNo].allCharacters[characterNo].allFrames.Length/2);

        }

        else
        {
            Debug.Log("Cannot load photos - no scence: " + scenceNo + "or no frameNo: " + frameNo + "or no character: " + characterNo);
            onDebug.Invoke("Cannot load photos - no scence: " + scenceNo + "or no frameNo: " + frameNo + "or no character: " + characterNo);
        }

    }

    public void getNextScene() {

        if (scenceNo < storyBoardAssets.Length - 1)
        {
            scenceNo++;

            if (storyBoardAssets[scenceNo].allCharacters.Length > 0)
            {
                characterNo = 0;
                frameNo = (storyBoardAssets[scenceNo].allCharacters[characterNo].allFrames.Length > 0) ? 0 : -1;
                SwapSourcePhotos();
            }
        }
        else
        {
            Debug.Log("No more scences! Starting Over");
            onDebug.Invoke("No more scences! Starting Over");

            scenceNo = 0;

            if (storyBoardAssets[scenceNo].allCharacters.Length > 0)
            {
                characterNo = 0;
                frameNo = (storyBoardAssets[scenceNo].allCharacters[characterNo].allFrames.Length > 0) ? 0 : -1;
                SwapSourcePhotos();
            }
        }
    }

    public void getNextCharacter()
    {

        if (characterNo < storyBoardAssets[scenceNo].allCharacters.Length - 1)
        {
            characterNo++;

            frameNo = (storyBoardAssets[scenceNo].allCharacters[characterNo].allFrames.Length > 0) ? 0 : -1;
            SwapSourcePhotos();
        }
        else
        {
            Debug.Log("No more characters! Starting Over");
            onDebug.Invoke("No more characters! Starting Over");
            characterNo=0;

            frameNo = (storyBoardAssets[scenceNo].allCharacters[characterNo].allFrames.Length > 0) ? 0 : -1;
            SwapSourcePhotos();
        }
    }

    public void getNextFrame()
    {
        int realNumberOfFrames = storyBoardAssets[scenceNo].allCharacters[characterNo].allFrames.Length / 2;
        if (frameNo / 2 < realNumberOfFrames - 1)
        {
            frameNo += 2;
            SwapSourcePhotos();
        }
        else
        {
            Debug.Log("No more frames! Starting Over");
            onDebug.Invoke("No more frames! Starting Over");

            frameNo = 0;
            SwapSourcePhotos();
        }
    }
}
