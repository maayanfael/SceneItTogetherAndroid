using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
public class SaveFile : MonoBehaviour
{

    public RawImage img;


    IEnumerator ShowSaveDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
        yield return FileBrowser.WaitForSaveDialog(false, null, "Save File", "Save");

        // Dialog is closed
        // Print whether a file is chosen (FileBrowser.Success)
        // and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
        Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);

        if (FileBrowser.Success)
        {

            Texture2D texture = TextureToTexture2D(img.texture);
            texture = FlipTexture(texture);

            byte[] jpgData = texture.EncodeToJPG();

            string fileName = DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss");
            if (jpgData != null)
            {
                //string filePath = Application.dataPath + "/Camera/";
                string filePath = FileBrowser.Result;
                /* if (!Directory.Exists(filePath))
                 {
                     Directory.CreateDirectory(filePath);
                 }*/

                FileBrowserHelpers.WriteBytesToFile(filePath, jpgData);
                //File.WriteAllBytes(filePath, jpgData);
            }
            /*
            Image imageToSwap = GetComponent<Image>();

            // If a file was chosen, read its bytes via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            //byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);

            var fileContent = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);
            imageToSwap.sprite.texture.LoadImage(fileContent);*/


        }
    }

    public void SavePhoto() {

        StartCoroutine(ShowSaveDialogCoroutine());
        
        /*Vector3 scale = img.transform.localScale;
        scale.x = -scale.x;
        img.transform.localScale = scale;*/



        /*Texture2D texture = TextureToTexture2D(img.texture);
        texture = FlipTexture(texture);
        
        var jpgData = texture.EncodeToJPG();

        string fileName = DateTime.Now.ToString("MM-dd-yyyy-HH-mm-ss");
        if (jpgData != null)
        {
            string filePath = Application.dataPath + "/Camera/";
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            File.WriteAllBytes(filePath + fileName + ".jpg", jpgData);
        }

        /*var path = EditorUtility.SaveFilePanel(
            "Save texture as jpg",
            "",
            texture.name + ".jpg",
            "jpg");

        if (path.Length != 0)
        {
            var pngData = texture.EncodeToJPG();
            if (pngData != null)
                File.WriteAllBytes(path, pngData);
        }*/
    }

    private Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);
        return texture2D;
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
}
