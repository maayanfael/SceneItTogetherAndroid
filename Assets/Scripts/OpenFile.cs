using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.UI;
using SimpleFileBrowser;

public class OpenFile : MonoBehaviour
{

	public Image photoToSwap;
	public Image outlineToSwap;

	const string originalFilePrefix = "CH{0}_FR{1}";
	const string outlineFilePrefix = "CH{0}_FR{1}_Outline";
	const string fileSuffix = "jpg";
	private int characterNo = 2;
	private int frameNo = 1;
	private string sourchPath;

	void Start()
	{
		// Set filters (optional)
		// It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
		// if all the dialogs will be using the same filters
		FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"));

		// Set default filter that is selected when the dialog is shown (optional)
		// Returns true if the default filter is set successfully
		// In this case, set Images filter as the default filter
		FileBrowser.SetDefaultFilter(".jpg");

		// Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
		// Note that when you use this function, .lnk and .tmp extensions will no longer be
		// excluded unless you explicitly add them as parameters to the function
		FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

		// Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
		// It is sufficient to add a quick link just once
		// Name: Users
		// Path: C:\Users
		// Icon: default (folder icon)
		FileBrowser.AddQuickLink("Users", "C:\\Users", null);
		FileBrowser.AddQuickLink("Game", Application.dataPath, null);
		
		// Show a save file dialog 
		// onSuccess event: not registered (which means this dialog is pretty useless)
		// onCancel event: not registered
		// Save file/folder: file, Initial path: "C:\", Title: "Save As", submit button text: "Save"
		// FileBrowser.ShowSaveDialog( null, null, false, "C:\\", "Save As", "Save" );

		// Show a select folder dialog 
		// onSuccess event: print the selected folder's path
		// onCancel event: print "Canceled"
		// Load file/folder: folder, Initial path: default (Documents), Title: "Select Folder", submit button text: "Select"
		// FileBrowser.ShowLoadDialog( (path) => { Debug.Log( "Selected: " + path ); }, 
		//                                () => { Debug.Log( "Canceled" ); }, 
		//                                true, null, "Select Folder", "Select" );

		// Coroutine example
		//StartCoroutine(ShowLoadDialogCoroutine());
	}

	IEnumerator ShowLoadDialogCoroutine()
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog(false, null, "Load File", "Load");

		// Dialog is closed
		// Print whether a file is chosen (FileBrowser.Success)
		// and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
		Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);

		if (FileBrowser.Success)
		{

			Image imageToSwap = GetComponent<Image>();
			
			// If a file was chosen, read its bytes via FileBrowserHelpers
			// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
			//byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);

			var fileContent = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result);
			imageToSwap.sprite.texture.LoadImage(fileContent);


		}
	}



	IEnumerator ShowLoadDialogFolderCoroutine()
	{
		// Show a load file dialog and wait for a response from user
		// Load file/folder: file, Initial path: default (Documents), Title: "Load File", submit button text: "Load"
		yield return FileBrowser.WaitForLoadDialog(true, null, "Select Folder", "Select");

		// Dialog is closed
		// Print whether a file is chosen (FileBrowser.Success)
		// and the path to the selected file (FileBrowser.Result) (null, if FileBrowser.Success is false)
		Debug.Log(FileBrowser.Success + " " + FileBrowser.Result);

		if (FileBrowser.Success)
		{
			sourchPath = FileBrowser.Result;

			string strCh = (characterNo < 10) ? ("0" + characterNo.ToString()) : characterNo.ToString();
			string strFr = (frameNo < 10) ? ("0" + frameNo.ToString()) : frameNo.ToString();

			string outlineFileStr = "//" + string.Format(outlineFilePrefix, strCh, strFr) + "." + fileSuffix;
			string originalFileStr = "//" + string.Format(originalFilePrefix, strCh, strFr) + "." + fileSuffix;
						
			var photofileContent = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result +originalFileStr);
			var outlinefileContent = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result + outlineFileStr);

			photoToSwap.sprite.texture.LoadImage(photofileContent);
			outlineToSwap.sprite.texture.LoadImage(outlinefileContent);
			
		}
	}

	public void swapImage()
	{

	}


	public void OpenFolderLocation()
	{
		// For Folders
		StartCoroutine(ShowLoadDialogFolderCoroutine());
	}
	public void OpenNewFile()
    {
		// For Spacific File
		StartCoroutine(ShowLoadDialogCoroutine());



		//string path= EditorUtility.OpenFilePanel("Overwrite file", "", "jpg");
		//ShowLoadDialogCoroutine();

		/*FileBrowser.ShowLoadDialog((path) => { Debug.Log("Selected: " + path); },
									   () => { Debug.Log("Canceled"); },
									   true, null, "Select Folder", "Select");
		if (path.Length != 0)
        {
            Image imageToSwap = GetComponent<Image>();
            var fileContent = File.ReadAllBytes(path);
            imageToSwap.sprite.texture.LoadImage(fileContent);
            
        }*/
	}

}
