using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingScrollScript : MonoBehaviour
{
	List<Transform> baseBuilds = new List<Transform>();

	Transform GenPoint;

	public Vector2 speed = new Vector2(5, 5);


	public Vector2 direction = new Vector2(-1, 0);


	private List<SpriteRenderer> backgroundPart;

	float duration = GameConstants.GEN_BUILD_DURATION;

	// 3 - Get all the children
	void Start()
	{
		GenPoint = transform.GetChild(0);
		for(int i = 1 ; i <= GameConstants.TOTAL_BUILDS ; i++)
		{
			GameObject blo = (GameObject) Resources.Load("Ban"+i);
			if(blo != null)
			{
				baseBuilds.Add(blo.transform);
			}
		}
	}

	void Update () 
	{
		duration -= Time.deltaTime;
		if (duration <= 0) 
		{
			generateBuilding();
		}

	}

	void generateBuilding()
	{
		if(!SoundManager.Instance.canPlay)
			return;

		duration = GameConstants.GEN_BUILD_DURATION;

		int n = blockToGen();
		Vector3 rt = Vector3.zero;
		Vector3 pos =  new Vector3(GenPoint.position.x + rt.x , GenPoint.position.y , GenPoint.position.z);

		RandomExtensions.Shuffle(baseBuilds);
		for(int i = 0; i < n;i++)
		{
			Vector3 nrt = baseBuilds[i].GetComponent<Renderer>().bounds.size;
			pos = new Vector3(pos.x + nrt.x/2 , pos.y , pos.z);
			GameObject clone = (GameObject)Instantiate (baseBuilds[i].gameObject, pos , GenPoint.rotation);
			clone.GetComponent<MoveScript>().speed = speed;
			rt = baseBuilds[i].GetComponent<Renderer>().bounds.size;
			pos =  new Vector3(clone.transform.position.x + rt.x/2 , clone.transform.position.y , clone.transform.position.z);
		}
	}

	int blockToGen()
	{
		int r = Random.Range(1,100)%2 + 2;
		//	Debug.Log(r);
		return r;
	}
		
}
