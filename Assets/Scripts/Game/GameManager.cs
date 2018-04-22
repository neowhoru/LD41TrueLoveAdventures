using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	public Text hintBox;
	public Canvas DialogCanvas;

	public bool isDialogOpen;
	public Text DialogText;
	public Image WomenEmotionDialog;

	public Sprite womenNormal;
	public Sprite womenAnger;
	public Sprite womenHappy;

	public Text LovebarProgressText;
	public Image LovebarProgressImage;
	private PlayerHandler playerHandler;
	private DialogTrigger womenTrigger;

	public Image currentItemWearing;
	
	public AudioClip badSound;
	public AudioClip goodSound;

	public AudioSource audioSource;

	public bool playPositiveSound = true;

	private void Awake()
	{
		playerHandler = FindObjectOfType<PlayerHandler>();
		womenTrigger = GameObject.FindGameObjectWithTag("Women").GetComponent<DialogTrigger>();
		audioSource = GetComponent<AudioSource>();
	}

	public void ToggleHintBox()
	{
		
		if (hintBox.isActiveAndEnabled)
		{
			Debug.Log("Disable Hint Box");
			hintBox.gameObject.SetActive(false);
		}
		else
		{
			Debug.Log("Enable Hint Box");
			hintBox.gameObject.SetActive(true);	
		}
	}

	public void ShowDialog()
	{
		isDialogOpen = true;
		DialogCanvas.gameObject.SetActive(true);
		GameObject.FindGameObjectWithTag("Women").GetComponent<DialogTrigger>().TriggerDialog();
	}

	public void HideDialog()
	{
		isDialogOpen = false;
		DialogCanvas.gameObject.SetActive(false);
	}

	public void ShowDialogText(string sentence)
	{
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));		
	}

	public void SwitchEmotion(int emotion)
	{
		switch (emotion)
		{
			case 1:
				WomenEmotionDialog.sprite = womenHappy;
				break;
				
			case 2:
				WomenEmotionDialog.sprite = womenAnger;
				break;
				
			default:
				WomenEmotionDialog.sprite = womenNormal;
				break;
				
		}
	}

	IEnumerator TypeSentence(string sentence)
	{
		DialogText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			DialogText.text += letter;
			yield return null;
		}
	}

	public void UpdateLoveProgress(int amount, bool decrease)
	{
		if (decrease)
		{
			// Substrakt Progress	
			playerHandler.currentLoveReputation -= amount;
			LovebarProgressText.text = playerHandler.currentLoveReputation + "/ 100";
			LovebarProgressImage.fillAmount = (float) playerHandler.currentLoveReputation / 100; 
		}
		else
		{
			playerHandler.currentLoveReputation += amount;
			LovebarProgressText.text = playerHandler.currentLoveReputation + "/ 100";
			LovebarProgressImage.fillAmount = (float) playerHandler.currentLoveReputation / 100;			
			
		}
	}

	public void UpdateQuests()
	{
		// Just update the state if he has an item
		if (currentItemWearing.sprite != null)
		{
			// ToDo: fing a way to handle after "bad" dialog to reset dialog - but not necessary at all
			switch (playerHandler.questState)
			{
				case 0:
					if (playerHandler.currentItem.name.Equals("GreenBucks"))
					{
						SwitchEmotion(1);
						// Solve the first quest
						HideWearingItem();
						playerHandler.currentItem = null;
						UpdateLoveProgress(33, false);
						womenTrigger.dialog.sentences = new string[3];
						womenTrigger.dialog.sentences[0] = "Thanks that was the right one.";
						womenTrigger.dialog.sentences[1] = "The coffee was good but there is something missing too my coffee. Can you imagine what ?";
						womenTrigger.dialog.sentences[2] = "It would be really sweet if you bring me this.";
						playerHandler.questState = 1;
						playPositiveSound = true;

					}else {
						// Women anger - substract
						SwitchEmotion(2);
						UpdateLoveProgress(10, true);
						womenTrigger.dialog.sentences = new string[1];
						womenTrigger.dialog.sentences[0] = "What the hell? This is not what i want! I am disapointed.";
						HideWearingItem();
						playPositiveSound = false;
					}
					break;
					
				case 1:
					if (playerHandler.currentItem.name.Equals("CandyShop"))
					{
						// Solve the first quest
						SwitchEmotion(1);
						HideWearingItem();
						UpdateLoveProgress(33, false);
						womenTrigger.dialog.sentences = new string[3];
						womenTrigger.dialog.sentences[0] = "My favorite! I love you.";
						womenTrigger.dialog.sentences[1] = "As we both really like much there could be something that you can bring me.To show me how much do you love me, and that we will be together forever ? ";
						womenTrigger.dialog.sentences[2] = "I hope you know what is mean - shiny gold smile :)";
						playerHandler.questState = 2;
						playPositiveSound = true;
					}else {
						SwitchEmotion(2);
						UpdateLoveProgress(10, true);
						womenTrigger.dialog.sentences = new string[1];
						womenTrigger.dialog.sentences[0] = "Uhh this doesnt taste! I thought you know me better.";
						HideWearingItem();
						playPositiveSound = false;
					}
					break;
					
				case 2:
					if (playerHandler.currentItem.name.Equals("GoldRing"))
					{
						SwitchEmotion(1);
						// Solve the first quest
						HideWearingItem();
						UpdateLoveProgress(33, false);
						womenTrigger.dialog.sentences = new string[1];
						womenTrigger.dialog.sentences[0] = " Thanks that was the right one! I love you.";
						playerHandler.questState = 3;
						playPositiveSound = true;

					}else {
						SwitchEmotion(2);
						UpdateLoveProgress(10, true);
						womenTrigger.dialog.sentences = new string[1];
						womenTrigger.dialog.sentences[0] = "That was the wrong! I thought you know me better ?!";
						HideWearingItem();
						playPositiveSound = false;
					}
					break;
			}
			currentItemWearing.sprite = null;
		}
		
	}

	public void AddWearingItem(Sprite image)
	{
		if (!currentItemWearing.isActiveAndEnabled)
		{
			currentItemWearing.gameObject.SetActive(true);
		}
		currentItemWearing.sprite = image;
	}

	public void HideWearingItem()
	{
		currentItemWearing.gameObject.SetActive(false);
		currentItemWearing.sprite = null;
	}

	public void GameOver()
	{
		SceneManager.LoadScene("GameOverScene");
	}

	public void FinishGame()
	{
		SceneManager.LoadScene("FinishGameScene");
	}
}
