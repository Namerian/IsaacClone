using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnGameEndedDelegate ();

public class Event
{
	private static Event _instance;

	public static Event Instance {
		get {
			if (_instance == null) {
				_instance = new Event ();
			}

			return _instance;
		}
	}

	public event OnGameEndedDelegate OnGameEndedEvent;

	public void SendOnGameEndedEvent ()
	{
		OnGameEndedDelegate tmp = OnGameEndedEvent;

		if (tmp != null) {
			this.OnGameEndedEvent ();
		}
	}
}
