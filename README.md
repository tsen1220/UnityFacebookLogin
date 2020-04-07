# Unity Sign in with Facebook

This feature is Sign in with Facebook for unity.

## 1.

We need to download Facebook Unity SDK.

We can download from https://developers.facebook.com/docs/unity/downloads.

## 2.

Import the SDK package.

## 3.

Create a script for facebook.

Initialize the SDK first.

```
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
```

## 4.

Facebook login and callback.

We can take the token to access server with user info.

```
	public void FBLoginReadPermissions()
	{
		List<string> permissions = new List<string> { "public_profile", "email" };
		FB.LogInWithReadPermissions(permissions, AuthCallback);
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
	}
```