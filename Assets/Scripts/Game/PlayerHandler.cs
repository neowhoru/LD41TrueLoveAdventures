using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour {

	public float maxVelocity = 4f;
	public bool canMove = true;
	
	private Rigidbody2D myBody;
	private Animator anim;

	private GameManager gameManager;
	
	public int currentLoveReputation = 20;

	public int questState = 0;

	private bool canDance = true;
	private bool isWomenInRange = false;

	public ShopItem currentItem = null;
	
	// Use this for initialization
	void Awake () {
		InitializeVariables();
	}
	
	void InitializeVariables()
	{
		myBody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		gameManager = FindObjectOfType<GameManager>();
	}
	
	
	void PlayerWalkControllerOrKeyboard()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float vAxisRaw = Input.GetAxisRaw("Vertical");

		float xVelocity = myBody.velocity.x;
		float yVelocity = myBody.velocity.y;

		bool updateMovement = false;
		
		if (h > 0)
		{
			
			// Moving Right
			updateMovement = true;
			xVelocity = maxVelocity;
			Vector3 scale = transform.localScale;
			scale.x = 3;
			transform.localScale = scale;
			anim.SetBool("IsWalkingX", true);
			anim.SetBool("IsWalkingFront", false);
			anim.SetBool("IsWalkingBack", false);
			anim.SetBool("IsDancing",false);
		} else if (h < 0){
			// Moving left
			updateMovement = true;
			xVelocity = -maxVelocity;
			Vector3 scale = transform.localScale;
			scale.x = -3;
			transform.localScale = scale;
			anim.SetBool("IsWalkingX", true);
			anim.SetBool("IsWalkingFront", false);
			anim.SetBool("IsWalkingBack", false);
			anim.SetBool("IsDancing",false);
		} else if(vAxisRaw > 0) {
			// Moving Right
			updateMovement = true;
			yVelocity = maxVelocity;
			Vector3 scale = transform.localScale;
			scale.x = 3;
			transform.localScale = scale;
			anim.SetBool("IsWalkingBack", true);
			anim.SetBool("IsWalkingFront", false);
			anim.SetBool("IsWalkingX", false);
			anim.SetBool("IsDancing",false);
		
		} else if (vAxisRaw < 0) {
			// Moving Right
			updateMovement = true;
			yVelocity = -maxVelocity;
			Vector3 scale = transform.localScale;
			scale.x = 3;
			transform.localScale = scale;
			anim.SetBool("IsWalkingFront", true);
			anim.SetBool("IsWalkingX", false);
			anim.SetBool("IsWalkingBack", false);
			anim.SetBool("IsDancing",false);

		} else {
			myBody.velocity = new Vector3(0f, 0f, 0f);
			anim.SetBool("IsWalkingFront", false);
			anim.SetBool("IsWalkingX", false);
			anim.SetBool("IsWalkingBack", false);
			anim.SetBool("IsDancing",false); // ToDo: i am not sure if we need to reset dancing - maybe we just put an event at the back or let him just dance "once"
		}
		
		// Set Final Velocity for the Rigbody (if any)

		if (updateMovement)
		{
			myBody.velocity = new Vector3(xVelocity, yVelocity, 0f);	
		}
		

		if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
		{
			// anim.SetBool("IsDancing",true); // ToDo: find a good way for dancing
			// ToDo (Nice2Have) : Player can shoot if there are enemies
			if (isWomenInRange)
			{
				if (!gameManager.isDialogOpen)
				{
					gameManager.UpdateQuests();
					if (gameManager.playPositiveSound)
					{
						gameManager.audioSource.clip = gameManager.goodSound;
					}
					else
					{
						gameManager.audioSource.clip = gameManager.badSound;
					}
					
					gameManager.audioSource.Play();
					
					gameManager.ShowDialog();
				}
				else
				{
					// if Dialog is open we want space to show the next sentence
					FindObjectOfType<DialogManager>().DisplayNextSentence();
				}
				
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Enter trigger " + other.tag);
		if (other.CompareTag("Women"))
		{
			isWomenInRange = true;
			canDance = false;
			gameManager.ToggleHintBox();
		}

		if (other.CompareTag("ShopItem"))
		{
			// Give the Player the Iitem
			currentItem = other.GetComponent<ShopItem>();
			gameManager.AddWearingItem(currentItem.shopItem);
			GetComponent<AudioSource>().Play();
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		Debug.Log("Leave trigger " + other.tag);
		canDance = true;
		if (other.CompareTag("Women"))
		{
			isWomenInRange = false;
			gameManager.ToggleHintBox();
			gameManager.HideDialog();
		}
		
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (canMove)
		{
			PlayerWalkControllerOrKeyboard();
		}
        
	}
}


