using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{

	public GameObject[] prefabs;
	private float movementSpeed = 0.15f;
	private Vector3[,] positions = new Vector3[4,4];
	private int[,] value = {{-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}, {-1, -1, -1, -1}};
	private GameObject[,] blocks = new GameObject[4,4];
	private float timer = 0;

	private void Start(){
		Debug.Log("block initiated");
		Application.targetFrameRate = 300;

		for (int i=0;i<4;i++){
			for (int j=0;j<4;j++){
				positions[i,j] = new Vector3(-29f + 19.3f * i, -29f + 19.3f * j, 0f);
			}
		}

		for (int i=0;i<4;i++){
			for (int j=0;j<4;j++){
				blocks[i,j] = null;
			}
		}

		StartCoroutine(popper());
		//newBlock = (GameObject)Instantiate(blocks[13], positions[0,0] + new Vector3(0, 0, 2f), transform.rotation);
		//newBlock.GetComponent<BlockControl>().init(2, positions[0,0]);
	}

	private void Update(){
		bool rightKey = Input.GetButtonDown("right");
		bool leftKey = Input.GetButtonDown("left");
		bool upKey = Input.GetButtonDown("up");
		bool downKey = Input.GetButtonDown("down");

		if ((rightKey || leftKey || upKey || downKey) && timer > movementSpeed){
			bool success = false;
			if (rightKey) {
				success = moveRight();
			}
			else if (leftKey) {
				success = moveLeft();
			}
			else if (upKey) {
				success = moveUp();
			}
			else if (downKey) {
				success = moveDown();
			}
			if (success) {
				StartCoroutine(popper());
				debugging();
				timer = 0;
			}
		}

		timer += Time.deltaTime;

	}

	private IEnumerator popper(){
		Debug.Log("popping");
		yield return new WaitForSeconds(movementSpeed);
		popNew();
	}

	private void popNew(){
		int count = 0;
		int[] posx = new int[16];
		int[] posy = new int[16];
		for (int i=0;i<4;i++){
			for (int j=0;j<4;j++){
				if (value[i,j] < 0){
					posx[count] = i;
					posy[count] = j;
					count++;
				}
			}
		}
		int posChoice = Random.Range(0, count);
		int numChoice = Random.Range(0, 10);
		if (numChoice < 9) {
			value[posx[posChoice], posy[posChoice]] = 0;
			blocks[posx[posChoice], posy[posChoice]] = (GameObject)Instantiate(prefabs[0], positions[posx[posChoice], posy[posChoice]], transform.rotation);
		}
		else {
			value[posx[posChoice], posy[posChoice]] = 1;
			blocks[posx[posChoice], posy[posChoice]] = (GameObject)Instantiate(prefabs[1], positions[posx[posChoice], posy[posChoice]], transform.rotation);
		}

	}

	private bool moveRight() {
		Debug.Log("move right");

		bool success = false;
		bool[,] pending = new bool[4,4];
		for (int i=0;i<4;i++){
			for (int j=0;j<4;j++){
				pending[i,j] = false;
			}
		}
	
		for (int entry = 0; entry < 4; entry++){
			for (int target = 3; target > 0; target--){
				if (value[target, entry] < 0) {
					for (int i = target - 1; i >= 0; i--){
						if (value[i, entry] >= 0) {
							value[target, entry] = value[i, entry];
							blocks[target, entry] = blocks[i, entry];
							blocks[target, entry].GetComponent<BlockControl>().move(positions[target, entry]);
							value[i, entry] = -1;
							blocks[i, entry] = null;
							success = true;
							break;
						}
					}
				}
				if (value[target, entry] >= 0) {
					for (int i = target - 1; i >= 0; i--){
						if (value[i, entry] == value[target, entry]) {
							blocks[i, entry].GetComponent<BlockControl>().move(positions[target, entry]);
							Destroy(blocks[i, entry], movementSpeed);
							value[i, entry] = -1;
							blocks[i, entry] = null;
							pending[target, entry] = true;
							success = true;
							break;
						}
						else if(value[i, entry] >= 0) {
							break;
						}
					}
				}
			}
		}

		
		for (int i=0;i<4;i++){
			for (int j=0;j<4;j++){
				if (pending[i, j]){
					merging(i, j);
				}
			}
		}

		return success;
	}
	private bool moveLeft() {
		Debug.Log("move left");

		bool success = false;
		bool[,] pending = new bool[4,4];
		for (int i=0;i<4;i++){
			for (int j=0;j<4;j++){
				pending[i,j] = false;
			}
		}
	
		for (int entry = 0; entry < 4; entry++){
			for (int target = 0; target < 3; target++){
				if (value[target, entry] < 0) {
					for (int i = target + 1; i < 4; i++){
						if (value[i, entry] >= 0) {
							value[target, entry] = value[i, entry];
							blocks[target, entry] = blocks[i, entry];
							blocks[target, entry].GetComponent<BlockControl>().move(positions[target, entry]);
							value[i, entry] = -1;
							blocks[i, entry] = null;
							success = true;
							break;
						}
					}
				}
				if (value[target, entry] >= 0) {
					for (int i = target + 1; i < 4; i++){
						if (value[i, entry] == value[target, entry]) {
							blocks[i, entry].GetComponent<BlockControl>().move(positions[target, entry]);
							Destroy(blocks[i, entry], movementSpeed);
							value[i, entry] = -1;
							blocks[i, entry] = null;
							pending[target, entry] = true;
							success = true;
							break;
						}
						else if(value[i, entry] >= 0) {
							break;
						}
					}
				}
			}
		}

		
		for (int i=0;i<4;i++){
			for (int j=0;j<4;j++){
				if (pending[i, j]){
					merging(i, j);
				}
			}
		}

		return success;
	}
	private bool moveUp() {
		Debug.Log("move up");

		bool success = false;
		bool[,] pending = new bool[4,4];
		for (int i=0;i<4;i++){
			for (int j=0;j<4;j++){
				pending[i,j] = false;
			}
		}
	
		for (int entry = 0; entry < 4; entry++){
			for (int target = 3; target > 0; target--){
				if (value[entry, target] < 0) {
					for (int i = target - 1; i >= 0; i--){
						if (value[entry, i] >= 0) {
							value[entry, target] = value[entry, i];
							blocks[entry, target] = blocks[entry, i];
							blocks[entry, target].GetComponent<BlockControl>().move(positions[entry, target]);
							value[entry, i] = -1;
							blocks[entry, i] = null;
							success = true;
							break;
						}
					}
				}
				if (value[entry, target] >= 0) {
					for (int i = target - 1; i >= 0; i--){
						if (value[entry, i] == value[entry, target]) {
							blocks[entry, i].GetComponent<BlockControl>().move(positions[entry, target]);
							Destroy(blocks[entry, i], movementSpeed);
							value[entry, i] = -1;
							blocks[entry, i] = null;
							pending[entry, target] = true;
							success = true;
							break;
						}
						else if(value[entry, i] >= 0) {
							break;
						}
					}
				}
			}
		}

		
		for (int i=0;i<4;i++){
			for (int j=0;j<4;j++){
				if (pending[i, j]){
					merging(i, j);
				}
			}
		}

		return success;
	}
	private bool moveDown() {
		Debug.Log("move down");

		bool success = false;
		bool[,] pending = new bool[4,4];
		for (int i=0;i<4;i++){
			for (int j=0;j<4;j++){
				pending[i,j] = false;
			}
		}
	
		for (int entry = 0; entry < 4; entry++){
			for (int target = 0; target < 3; target++){
				if (value[entry, target] < 0) {
					for (int i = target + 1; i < 4; i++){
						if (value[entry, i] >= 0) {
							value[entry, target] = value[entry, i];
							blocks[entry, target] = blocks[entry, i];
							blocks[entry, target].GetComponent<BlockControl>().move(positions[entry, target]);
							value[entry, i] = -1;
							blocks[entry, i] = null;
							success = true;
							break;
						}
					}
				}
				if (value[entry, target] >= 0) {
					for (int i = target + 1; i < 4; i++){
						if (value[entry, i] == value[entry, target]) {
							blocks[entry, i].GetComponent<BlockControl>().move(positions[entry, target]);
							Destroy(blocks[entry, i], movementSpeed);
							value[entry, i] = -1;
							blocks[entry, i] = null;
							pending[entry, target] = true;
							success = true;
							break;
						}
						else if(value[entry, i] >= 0) {
							break;
						}
					}
				}
			}
		}

		
		for (int i=0;i<4;i++){
			for (int j=0;j<4;j++){
				if (pending[i, j]){
					merging(i, j);
				}
			}
		}

		return success;
	}

	private void merging(int posx, int posy) {
		Destroy(blocks[posx, posy], movementSpeed);
		value[posx, posy]++;
		blocks[posx, posy] = (GameObject)Instantiate(prefabs[value[posx, posy]], positions[posx, posy], transform.rotation);
	}

	private void debugging(){
		for (int i=3;i>=0;i--){
			Debug.Log(value[0,i]+" "+value[1,i]+" "+value[2,i]+" "+value[3,i]);
		}
	}
}
