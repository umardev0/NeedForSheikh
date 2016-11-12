using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
	public float jumpForce = 200f;
//	bool rotated = false;
	bool canClick = false;

	// Use this for initialization
	void Start()
	{
	
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0) && canClick && SoundManager.Instance.canPlay)
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if(hit.collider != null)
			{
				if(hit.collider.gameObject.name == "NoJumpClick")
				{
					return;
				}
			}

			SoundManager.Instance.PlayJumpSound();
			canClick = false;
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce), ForceMode2D.Force);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.tag == "Ground")
		{
			canClick = true;
		}
		else if(collision.gameObject.tag == "Obstacle")
		{
			canClick = false;
			GameManager.instance.GameOver();
			gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		GameManager.instance.UpdateScore();
	}
}
