using UnityEngine;
using System.Collections;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance ;

	public static T GetInstance ()
	{
		return instance;
	}

	public virtual void Awake ()
	{
		if (instance == null)
			instance = (T)(Object)this;
		else {
			Debug.LogError ("Multiple Instance Creating:" + gameObject.name);
			Destroy (gameObject);

		}
	}

}
