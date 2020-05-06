using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class SaveFile : MonoBehaviour
{
    public System.Action<string> onDebug;

    private ReadCameraToTexture camReader;

    private void Start()
    {
        if (camReader == null) camReader = GameObject.FindObjectOfType<ReadCameraToTexture>();
    }
    public void SavePhoto()
    {
        Texture2D texture = camReader.ActiveTexture2D;
        texture = FlipTexture(texture);
        texture = rotateTexture(texture, true);
        
        var jpgData = texture.EncodeToJPG();

        string fileName = "img-" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        if (jpgData != null)
        {
            string filePath = Application.persistentDataPath + "/Camera/";

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string msg = "Saved File To: \n" + filePath + "\n File Name: " + fileName + ".jpg";
            onDebug.Invoke(msg);
            Debug.Log(msg); 
            File.WriteAllBytes(filePath + fileName + ".jpg", jpgData);

            
        }
    }


   Texture2D FlipTexture(Texture2D original)
   {
       Texture2D flipped = new Texture2D(original.width, original.height);

       int xN = original.width;
       int yN = original.height;


       for (int i = 0; i < xN; i++)
       {
           for (int j = 0; j < yN; j++)
           {
               flipped.SetPixel(xN - i - 1, j, original.GetPixel(i, j));
           }
       }
       flipped.Apply();

       return flipped;
   }

    Texture2D rotateTexture(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }
}
