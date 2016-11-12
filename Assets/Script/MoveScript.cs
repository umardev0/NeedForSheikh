using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour 
{

	public Vector2 speed = new Vector2(5, 5);

	public Vector2 direction = new Vector2(-1, 0);
	
	private Vector2 movement;

	public bool isDirected = false;

	public Vector2 guidedDirection;

	Transform endpoint;
	
	void Start()
	{
		endpoint = GameObject.FindGameObjectWithTag("EndPoint").transform;
	}
	
	void Update()
	{
		// Movement
		Vector3 movement = new Vector3(
			speed.x * direction.x,
			speed.y * direction.y,
			0);

		movement *= Time.deltaTime;
		transform.Translate(movement);

	}

	void FixedUpdate()
	{
		if(transform.position.x <= endpoint.position.x )
		{
			Destroy(gameObject, 1);
		}
	}
}
