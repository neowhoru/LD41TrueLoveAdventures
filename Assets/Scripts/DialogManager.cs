using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : MonoBehaviour
{

	private Queue<string> sentences;

	private GameManager gameManager;
	private PlayerHandler playerHandler;
	// Use this for initialization
	void Start () {
		sentences = new Queue<string>();
		gameManager = FindObjectOfType<GameManager>();
		playerHandler = FindObjectOfType<PlayerHandler>();
	}

	public void StartDialog(Dialog dialog)
	{
		Debug.Log("Starting Dialog Conversation");
		
		sentences.Clear();

		foreach (string sentence in dialog.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialog();
			return;
		}

		string sentence = sentences.Dequeue();
		gameManager.ShowDialogText(sentence);
	}

	public void EndDialog()
	{
		gameManager.HideDialog();
		if (playerHandler.questState == 3 || playerHandler.currentLoveReputation==100)
		{
			// We are married
			gameManager.FinishGame();
		}

		if (playerHandler.currentLoveReputation <= 0)
		{
			gameManager.GameOver();
		}
		Debug.Log("End Dialog");
	}
	
	
}
