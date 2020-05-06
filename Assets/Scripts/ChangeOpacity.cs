using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeOpacity : MonoBehaviour
{
    private RawImage img;


    // Start is called before the first frame update
    void Awake()
    {
        img = GetComponent<RawImage>();
    }

    public void AdjustOpacity(float value) {
        
            img.color = new Color(img.color.r, img.color.g, img.color.b, value);
        
    }

}
