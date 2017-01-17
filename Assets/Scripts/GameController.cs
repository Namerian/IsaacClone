using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public static GameController Instance{ get; private set; }

	private CanvasGroup _gameOverPanelCanvasGroup;
	private Text _gameOverPanelText;

	private bool _gameRunning;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		}
	}

	// Use this for initialization
	void Start ()
	{
		_gameOverPanelCanvasGroup = this.transform.Find ("Canvas/GameOverPanel").GetComponent<CanvasGroup> ();
		_gameOverPanelCanvasGroup.alpha = 0;

		_gameOverPanelText = _gameOverPanelCanvasGroup.transform.Find ("Text").GetComponent<Text> ();
		_gameOverPanelText.text = "";
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit ();
			#endif
		}
	}

	public void CharacterDied (ICharacter character)
	{
		if (character.Team == Team.Player) {
			_gameRunning = false;
			Event.Instance.SendOnGameEndedEvent ();

			_gameOverPanelCanvasGroup.alpha = 1;
			_gameOverPanelText.text = "You lost!";
		}
	}
}
