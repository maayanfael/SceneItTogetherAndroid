using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachCameraTexture : MonoBehaviour
{

    public UnityEngine.UI.Image img;
    private ReadCameraToTexture camReader;
    private Texture2D copyFrame;
    private void Reset() {
        if(camReader == null) camReader = GameObject.FindObjectOfType<ReadCameraToTexture>();
    }
    private void Awake() {

        Reset();
    }
    private void Start() {
    }

    void OnEnable()
    {
        camReader.onTextureChangeEvent += OnChangeTexture;
        if(camReader.ActiveTexture2D != null) OnChangeTexture(camReader.ActiveTexture2D);

        
    }
    void OnDisable() {
        if(camReader)
            camReader.onTextureChangeEvent -= OnChangeTexture;
        StopAllCoroutines();
    }

    public void PauseUpdateFromCamera(float pauseDelay) {
        if(copyFrame == null || copyFrame.width != camReader.ActiveTexture2D.width
            || copyFrame.height  != camReader.ActiveTexture2D.height)
            copyFrame = new Texture2D(camReader.ActiveTexture2D.width, camReader.ActiveTexture2D.height);

        copyFrame.SetPixels(camReader.ActiveTexture2D.GetPixels());
        copyFrame.Apply();

        OnChangeTexture(copyFrame);

        //TODO Consider camera might change texture and the update will occure before the time ended?
        this.WaitInvoke(() => OnChangeTexture(camReader.ActiveTexture2D), pauseDelay);
    }

    private void OnChangeTexture(Texture2D camTxtur) {
        Debug.Log("OnChangeTexture");
        img.enabled = false;
        img.material.mainTexture = camTxtur;
        img.enabled = true;
    }

}
