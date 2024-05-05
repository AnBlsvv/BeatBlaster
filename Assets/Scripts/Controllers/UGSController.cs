using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Analytics;

public class UGSController : MonoBehaviour
{
	async void Awake()
	{
		try
		{
			await UnityServices.InitializeAsync();
		}
		catch (ConsentCheckException e)
		{
            Debug.Log(e.ToString());
		}
		DontDestroyOnLoad(this);
	}	
}
