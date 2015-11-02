using UnityEngine;
using System.Collections;

public class GameControllerScript : MonoBehaviour {
	public GameObject cubePrefab;
	private GameObject[,] allCubes;
	public Airplane airplane;
	int numbCubes = 16;
	int numCubes = 9;
	int cargo = 10;
	int cargoCapacity = 90;
	float timeToAct = 0.0f;
	float spawnFrequency = 1.5f;
	
	// Use this for initialization
	void Start () {
		
		timeToAct += spawnFrequency;
		
		airplane = new Airplane ();
		airplane.cargo = 0;
		airplane.points = 0;
		airplane.x = 0;
		airplane.z = numCubes - 1;
		allCubes = new GameObject[numbCubes, numCubes];
		
		for (int x = 0; x < numbCubes; x++)	{
			for (int z = 0; z < numCubes; z++) {
				allCubes [x,z] = (GameObject)Instantiate (cubePrefab, new Vector3 (x * 2 -14, z * 2 - 14, 10), Quaternion.identity);
				allCubes [x,z].GetComponent<CubeBehavior>().x = x;
				allCubes [x,z].GetComponent<CubeBehavior>().z = z;
				allCubes [x,z].GetComponent<CubeBehavior>().GameController = this;
			}
		}
		allCubes[0,numCubes -1].GetComponent<Renderer>().material.color = Color.red;
		allCubes[numbCubes - 1, 0].GetComponent<Renderer> ().material.color = Color.black;
	}

	public void Activate (GameObject oneCube)	{
		
		if (airplane.x == oneCube.GetComponent<CubeBehavior>().x && airplane.z == oneCube.GetComponent<CubeBehavior>().z 
		    && airplane.isActive == false)	{
			oneCube.GetComponent<Renderer>().material.color = Color.yellow;
			airplane.isActive = true;
		}
		
		else if (airplane.x == oneCube.GetComponent<CubeBehavior>().x && airplane.z == oneCube.GetComponent<CubeBehavior>().z
		         && airplane.isActive == true)	{
			oneCube.GetComponent<Renderer>().material.color = Color.red;
			airplane.isActive = false;
		}
		
//		else if (airplane.isActive == true && (airplane.x != oneCube.GetComponent<CubeBehavior>().x || 
//		                                       airplane.z != oneCube.GetComponent<CubeBehavior>().z))	{
//			allCubes [airplane.x, airplane.z].GetComponent<Renderer>().material.color = Color.white;
//			airplane.x = oneCube.GetComponent<CubeBehavior>().x;
//			airplane.z = oneCube.GetComponent<CubeBehavior>().z;
//			allCubes [airplane.x, airplane.z].GetComponent<Renderer>().material.color = Color.yellow;
//			airplane.isActive = true;
//		}
	}

	public void Move ()	{
		airplane.isActive = true;
		allCubes [airplane.x, airplane.z].GetComponent<Renderer>().material.color = Color.white;
		airplane.Movement ();
		allCubes [airplane.x, airplane.z].GetComponent<Renderer>().material.color = Color.yellow;
	}
	
	
	// Update is called once per frame
	void Update () {

		if (Time.time >= timeToAct)	{
			if (airplane.x == 0 && airplane.z == numCubes - 1 && airplane.cargo < cargoCapacity)	{
				airplane.cargo += cargo;
				print (airplane.cargo);
			}
			Move ();
			timeToAct += spawnFrequency;
		}
		CheckInput();

		if (airplane.x == numbCubes - 1 && airplane.z == 0) {
			airplane.PointTotals ();
			print (airplane.points);
			airplane.cargo = 0;
			allCubes[numbCubes - 1, 0].GetComponent<Renderer> ().material.color = Color.black;
		}
	}

	public void CheckInput ()	{
		if (airplane.isActive == true) {
			if (Input.GetKeyDown ("up") && airplane.z < numCubes - 1) {
				airplane.SetDirection (0, 1);
			}

			if (Input.GetKeyDown ("down") && airplane.z > 0) {
				airplane.SetDirection (0, -1);
			}

			if (Input.GetKeyDown ("right") && airplane.x < numbCubes - 1) {
				airplane.SetDirection (1, 0);
			}

			if (Input.GetKeyDown ("left") && airplane.x > 0) {
				airplane.SetDirection (-1, 0);
			}
		}
	}
}