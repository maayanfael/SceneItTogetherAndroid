using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachCameraTexture : MonoBehaviour
{

    public UnityEngine.UI.Image img;
    private ReadCameraToTexture camReader;
    private void Reset() {
        if(camReader == null) camReader = GameObject.FindObjectOfType<ReadCameraToTexture>();
    }
    private void Awake() {

        Reset();
    }

    void OnEnable()
    {//TODO fix reference
        camReader.onTextureChangeEvent += OnNewTexture2dFromCamera;
        if(camReader.ActiveTexture2D != null) OnNewTexture2dFromCamera(camReader.ActiveTexture2D);
    }
    void OnDisable() {
        if(camReader)
            camReader.onTextureChangeEvent -= OnNewTexture2dFromCamera;
    }

    private void OnNewTexture2dFromCamera(Texture2D camTxtur) {

        Debug.Log("Update camera with new textures");
        //img.dis\ = true;
        img.enabled = false;
        //img.material.mainTexture = null;
        img.material.mainTexture = camTxtur;
        img.enabled = true;
    }

}
