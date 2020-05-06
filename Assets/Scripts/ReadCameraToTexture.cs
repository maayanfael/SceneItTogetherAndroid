using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Android;

public static class Extensopnm {
    public static float SumValuePixel(this Color c) {
        return c.r + c.g + c.b;
    }

    public static Coroutine WaitInvoke(this MonoBehaviour that, System.Action a, float delay) {
        return that.StartCoroutine(CoroutineDelay(a, delay));
    }
    private static IEnumerator CoroutineDelay(System.Action a, float delay) {
        yield return new WaitForSeconds(delay);
        a();
    }
}

public class ReadCameraToTexture : MonoBehaviour {

    public System.Action<Texture2D> onTexturePixelsChangedEvent;
    public System.Action<Texture2D> onTextureChangeEvent;
    public System.Action<string> onDebug;

    private WebCamTexture webcamTexture;
    private Texture2D texture2D;
    public Texture2D ActiveTexture2D { get { return texture2D; } }
    private int cameraDeviceIndex = -1;

    //#if UNITY_ANDROID
    //    bool switchView = true;
    //#endif
    [ContextMenu("Next camera")]
    public void NextCamera() {

        var devices = WebCamTexture.devices;
        if (devices.Length == 0) {
            Debug.Log("No camera detectet");
            return;
        }

        if (webcamTexture != null)
            webcamTexture.Stop();

        cameraDeviceIndex = (cameraDeviceIndex + 1) % devices.Length;


        webcamTexture = new WebCamTexture(devices[cameraDeviceIndex].name);
        webcamTexture.Play();

        //#if UNITY_ANDROID
        //        UnityEngine.UI.Image img = GameObject.FindGameObjectWithTag("cameraDisplayTexture").GetComponent<UnityEngine.UI.Image>();
        //        RectTransform rt = img.GetComponent<RectTransform>();
        //        if (switchView)
        //        {
        //            rt.localScale.Set(-1, 1, 1);
        //            rt.rotation.Set(0, 0, -90, 0);
        //        }
        //        else
        //        {
        //            rt.localScale.Set(1, 1, 1);
        //            rt.rotation.Set(0, 0, -90, 0);
        //        }

        //        img.rectTransform.set = rt;


        //#endif

        TryFixTexture2dRelativeToCamera();

        Debug.Log("selected: CameraNo " + cameraDeviceIndex + ", name: " + devices[cameraDeviceIndex].name);

    }

    void Awake() {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif
    }

    private void OnEnable() {
        Debug.Log(string.Join(",", WebCamTexture.devices.Select(a => a.name)));
        cameraDeviceIndex = WebCamTexture.devices.Length - 2;
        NextCamera();
    }

    private void TryFixTexture2dRelativeToCamera() {

        int width = webcamTexture.width;
        int height = webcamTexture.height;
        if (webcamTexture.width > webcamTexture.height) {
            width = ((int)(((float)webcamTexture.height) / 16 * 9));
            height = webcamTexture.height;
        }

        if (texture2D == null || texture2D.width != width || texture2D.height != height) {
            if (onDebug != null) onDebug.Invoke("texture2D width: " + width + " height: " + height);
            texture2D = new Texture2D(width, height, TextureFormat.RGB24, true);
            if (onTextureChangeEvent != null) onTextureChangeEvent.Invoke(texture2D);
        }


    }

    private void OnDisable() {
        if (webcamTexture != null) {
            webcamTexture.Stop();
            webcamTexture = null;
        }
    }

    private void Update() {
        if (UpdatePicFromCamera() && onTexturePixelsChangedEvent != null)
            onTexturePixelsChangedEvent.Invoke(texture2D);
    }


    bool UpdatePicFromCamera() {
        if (webcamTexture.isPlaying == false || webcamTexture.didUpdateThisFrame == false) return false;

        TryFixTexture2dRelativeToCamera();
        int width = webcamTexture.width;
        int height = webcamTexture.height;
        if (webcamTexture.width > webcamTexture.height) {
            width = ((int)(((float)webcamTexture.height) / 16 * 9));
            height = webcamTexture.height;
        }

        Color[] c = webcamTexture.GetPixels(0, 0, width, height);

        texture2D.SetPixels(c);

        texture2D.Apply();



        return true;
    }
}
