using UnityEngine;
using UnityEngine.UI;
using uFileBrowser;

public class generic_example : MonoBehaviour 
{
    public FileBrowser browser;
    public Text file;

    public void ShowButtonClick()
    {
        browser.Show(new FileBrowserCallback(BrowserClosed));
    }

    public void BrowserClosed(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("No file selected.");
            return;
        }
        file.text = "You selected:\n" + path;
    }
}