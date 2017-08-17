using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour {

	[SerializeField]
	private GameObject[] EnemyPrefabs;

	[SerializeField]
	private int[] EnemyWeights;

	[SerializeField]
	private float SpawnDelay;

	[SerializeField]
	private Transform PlayerPosition;

	private bool SpawnerAreaOccupied;

	private GameObject[] WeightedEnemySource;

	private List<GameObject> ObjectsOccupying;

	// Use this for initialization
	void Start () {
		SpawnerAreaOccupied = false;
		ObjectsOccupying = new List<GameObject> ();

		int temp = 0;

		for (int i = 0; i < EnemyWeights.Length; i++) 
		{
			temp += EnemyWeights [i];
		}

		WeightedEnemySource = new GameObject[temp];
	
		int point = 0;

		for (int i = 0; i < EnemyWeights.Length; i++) 
		{
			for (int x = 0; x < EnemyWeights [i]; x++, point++) 
			{
				WeightedEnemySource [point] = EnemyPrefabs [i];
			}
		}
			
		StartCoroutine ("SpawnerWorking");
	}

	void Update()
	{
		for (int i = 0; i < ObjectsOccupying.Count;) 
		{
			if (ObjectsOccupying [i] == null) 
			{
				ObjectsOccupying.RemoveAt (i);
			}
			else 
			{
				i++;
			}
		}

		SpawnerAreaOccupied = ObjectsOccupying.Count > 0;
	}

	void OnTriggerEnter(Collider collider)
	{
		SpawnerAreaOccupied = true;
		ObjectsOccupying.Add (collider.gameObject);
	}

	void OnTriggerExit(Collider collider)
	{
		//Debug.Log ("Trigger Exit");

		ObjectsOccupying.Remove (collider.gameObject);
	}

	private IEnumerator SpawnerWorking()
	{
		while (true) 
		{
			yield return new WaitForSeconds ( GameRandom.NextFloat(0.5f, 1.5f) * SpawnDelay );

			while (SpawnerAreaOccupied) 
			{
				yield return new WaitForSeconds (0.3f);
			}

			GameObject enemy = Instantiate (ChooseMonsterToSpawn(), transform.position, new Quaternion ());
		}
	}

	private GameObject ChooseMonsterToSpawn()
	{
		return WeightedEnemySource[GameRandom.NextInt(WeightedEnemySource.Length)];
	}
}
