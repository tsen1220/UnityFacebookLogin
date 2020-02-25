using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FBScript : MonoBehaviour
{
	[Header("FBSimple")]
	public Text helloMsg;
	public Image profilePicture;
	public Button login;
	public Button logout;

	private void Awake()
	{
		if (!FB.IsInitialized)
		{
			FB.Init(InitCallback, OnHideUnity);
		}
		else
		{
			FB.ActivateApp();
		}
	}

	private void InitCallback()
	{
		if (FB.IsInitialized)
		{
			FB.ActivateApp();
		}
		else
		{
			Debug.Log("Failed to Initialize the facebook SDK");
		}
	}

	private void OnHideUnity(bool isGameShown)
	{
		if (!isGameShown)
		{
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		}
		else
		{
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	private void AuthCallback(ILoginResult result)
	{
		if (FB.IsLoggedIn)
		{
			AccessToken aToken = AccessToken.CurrentAccessToken;
			Debug.Log(aToken.UserId);

			foreach (string perm in aToken.Permissions)
			{
				Debug.Log(perm);
			}

		}
		else
		{
			Debug.Log("User cancelled login");
		}

		FBCallApi(FB.IsLoggedIn);
	}

	public void FBLoginReadPermissions()
	{
		List<string> permissions = new List<string> { "public_profile", "email" };
		FB.LogInWithReadPermissions(permissions, AuthCallback);

	}

	private void FBCallApi(bool isLoggedIn)
	{
		if (isLoggedIn)
		{
			login.gameObject.SetActive(false);
			logout.gameObject.SetActive(true);
			profilePicture.gameObject.SetActive(true);
			helloMsg.gameObject.SetActive(true);

			FB.API("/me?field=name", HttpMethod.GET, DisplayName);
			FB.API("/me/picture?type=square&width=300&height=300", HttpMethod.GET, DisplayPicture);
		}
		else
		{
			login.gameObject.SetActive(true);
			logout.gameObject.SetActive(false);
			profilePicture.gameObject.SetActive(false);
			helloMsg.gameObject.SetActive(false);
		}
	}

	private void DisplayName(IResult result)
	{
		if(result.Error != null)
		{
			return;
		}

		helloMsg.text = "Hello! " + (string)result.ResultDictionary["name"];
	}
	private void DisplayPicture(IGraphResult result)
	{
		if (result.Texture != null)
		{
			profilePicture.sprite = Sprite.Create(result.Texture, new Rect(0,0,300,300), new Vector2(0.5f, 0.5f));
		}
	}

	public void FBLogout()
	{
		FB.LogOut();
		FBCallApi(FB.IsLoggedIn);
	}
}
