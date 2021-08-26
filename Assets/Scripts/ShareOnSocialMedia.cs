using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ShareOnSocialMedia : MonoBehaviour
{
    public GameManager myGM;
	[SerializeField] GameObject Panel_share;
	[SerializeField] Text txtPanelScore;
	[SerializeField] Text txtHomeScore;
	[SerializeField] Text txtDate;



	public void ShareScore ()
	{
		//txtPanelScore.text = txtHomeScore.text; //get the same score in home sceen
		//DateTime dt = DateTime.Now; //get the current date

		//txtDate.text = string.Format ("{0}/{1}/{2}", dt.Month, dt.Day, dt.Year);

		////open the score panel
		//Panel_share.SetActive (true);//show the panel
        myGM.RetryPanel.SetActive(false);

        StartCoroutine ("TakeScreenShotAndShare");
	}

	IEnumerator TakeScreenShotAndShare ()
	{
		yield return new WaitForEndOfFrame ();

		Texture2D tx = new Texture2D (Screen.width, Screen.height, TextureFormat.RGB24, false);
		tx.ReadPixels (new Rect (0, 0, Screen.width, Screen.height), 0, 0);
		tx.Apply ();

		string path = Path.Combine (Application.temporaryCachePath, "sharedImage.png");//image name
		File.WriteAllBytes (path, tx.EncodeToPNG ());

		Destroy (tx); //to avoid memory leaks

		new NativeShare ()
			.AddFile (path)
			.SetSubject ("New Highscore!")
			.SetText ("I just scored " + myGM.Score + " in Grabby Globe! Download it on the Google play store to beat my score!")
			.Share ();


        myGM.RetryPanel.SetActive(true); //hide the panel
    }
}
