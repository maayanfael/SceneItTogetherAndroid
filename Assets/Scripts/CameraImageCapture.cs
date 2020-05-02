using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ShenkarExtension;
using UnityEngine.UI;

namespace ShenkarExtension
{
    public static class Extensions
    {

        public static Texture2D ScaleTexture(this Texture2D source, int targetWidth, int targetHeight)
        {
            Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
            Color[] rpixels = result.GetPixels(0);
            float incX = (1.0f / (float)targetWidth);
            float incY = (1.0f / (float)targetHeight);
            for (int px = 0; px < rpixels.Length; px++)
            {
                rpixels[px] = source.GetPixelBilinear(incX * ((float)px % targetWidth), incY * ((float)Mathf.Floor(px / targetWidth)));
            }
            result.SetPixels(rpixels, 0);
            result.Apply();
            return result;
        }
    }
}

public class CameraImageCapture : MonoBehaviour
{

    public int width = 640, height = 360;
    [SerializeField] private Vector2 scale = Vector2.one;

    public string camName;
    public RawImage DisplayBoxPhoto;

    private WebCamTexture webcamTextureSelected;
    private Texture2D texture2D;
    private int CameraNo = 1;
    private WebCamDevice[] devices;

    public System.Action<Texture2D> onTexture2DEvent;

    private Renderer rndr;
    
    private void Awake()
    {
        rndr = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        
        devices = WebCamTexture.devices;
        if(devices.Length == 0)
        {
            Debug.Log("No camera detectet");
            return;
        }
        
        Debug.Log(string.Join(",", WebCamTexture.devices.Select(a => a.name)));
        webcamTextureSelected = string.IsNullOrEmpty(devices[CameraNo].name) ? new WebCamTexture() : new WebCamTexture(devices[CameraNo].name);
        webcamTextureSelected.requestedHeight = height;
        webcamTextureSelected.requestedWidth = width;
        webcamTextureSelected.Play();
        
    }

    public void SwitchCamera() {

        devices = WebCamTexture.devices;
        if (devices.Length == 0)
        {
            Debug.Log("No camera detectet");
            return;
        }
        webcamTextureSelected = string.IsNullOrEmpty(devices[CameraNo].name) ? new WebCamTexture() : new WebCamTexture(devices[CameraNo].name);
        webcamTextureSelected.Stop();
        CameraNo++;
        CameraNo = CameraNo % devices.Length;
        Debug.Log("selected: CameraNo " + CameraNo + ", name: " + devices[CameraNo].name);

        
        webcamTextureSelected = string.IsNullOrEmpty(devices[CameraNo].name) ? new WebCamTexture() : new WebCamTexture(devices[CameraNo].name);
        webcamTextureSelected.requestedHeight = height;
        webcamTextureSelected.requestedWidth = width;
        DisplayBoxPhoto.texture = webcamTextureSelected;
        webcamTextureSelected.Play();
    }


    private void OnDisable()
    {
        if (webcamTextureSelected != null)
        {
            webcamTextureSelected.Stop();
            webcamTextureSelected = null;

        }
    }


    private void Update()
    {
        Texture newOne = UpdatePic(TextureToTexture2D(webcamTextureSelected), scale);
        DisplayBoxPhoto.texture = newOne;
        //DisplayBoxPhoto.texture = webcamTextureSelected;
        
    }


    private Texture UpdateTextureSize(Texture2D webcamTextureSelected)
    {
        if (width != webcamTextureSelected.width || height != webcamTextureSelected.height)
        {
            Texture2D m2Texture;
            //m2Texture = webcamTextureSelected;

            //m2Texture = m2Texture.ScaleTexture((int)(scale.x * webcamTextureSelected.width), (int)(scale.y * webcamTextureSelected.height));
            Color[] c = webcamTextureSelected.GetPixels(0, 0, width, height);
            m2Texture = new Texture2D(width, height);
            m2Texture.SetPixels(c);
            m2Texture.Apply();

            return m2Texture;
        }

        return webcamTextureSelected;
    }



    private void UpdateTextureFromUnityCam()
    {
       
        texture2D = UpdatePic(texture2D, scale);
        if (rndr != null) rndr.sharedMaterial.mainTexture = texture2D;//TODO can be done only once
        if (onTexture2DEvent != null) onTexture2DEvent.Invoke(texture2D);
    }

    Texture2D UpdatePic(Texture2D texture, Vector2 rescale)
    {
        if (texture == null || texture.width != webcamTextureSelected.width ||
            texture.height != webcamTextureSelected.height)
        {
            texture = new Texture2D(
               webcamTextureSelected.width,
               webcamTextureSelected.height, TextureFormat.RGB24, true);
        }


        texture.SetPixels(webcamTextureSelected.GetPixels());
        texture.Apply();


        if ((rescale.x != 1 || rescale.y != 1) && (rescale.x != 0 && rescale.y != 0))
        {
            texture = texture.ScaleTexture((int)(rescale.x * texture.width), (int)(rescale.y * texture.height));
        }

        return texture;
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
}