using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActionCreator : MonoBehaviour, ActorActionCreator {

	private List<ActorAction> DeclaredActions;

	[SerializeField]
	private float UpdateDestinationDelay;

	[SerializeField]
	private int MaxActionsToPop = 3;
	private int ActionsLeftForThisFrame;

	private Transform PlayerPosition;
	private NavMeshAgent NavAgent;
	private NavMeshPath Path;
	private int ProgressPath;

	private float EnemyRadius;

	public bool CanPopAction { get { return DeclaredActions != null && DeclaredActions.Count > 0 && ActionsLeftForThisFrame > 0; } }

	public ActorAction PopAction
	{
		get
		{
			bool check = CanPopAction;
			ActorAction result = (check) ?
				DeclaredActions[0] : null;

			if(check)
			{
				DeclaredActions.RemoveAt(0);
				ActionsLeftForThisFrame--;
			}

			return result;
		}
	}

	// Use this for initialization
	void Start () 
	{
		DeclaredActions = new List<ActorAction>();
		ActionsLeftForThisFrame = MaxActionsToPop;

		NavAgent = GetComponent<NavMeshAgent> ();
		PlayerPosition = GameObject.Find ("Player").GetComponent<Transform> ();

		Path = new NavMeshPath ();

		EnemyRadius = GetComponent<CapsuleCollider> ().radius;

		StartCoroutine ("UpdateDestination");
	}
	
	// Update is called once per frame
	void Update () 
	{
		ActionsLeftForThisFrame = MaxActionsToPop;

		if (Path != null) 
		{
			if (ProgressPath < Path.corners.Length && Vector3.Distance (Path.corners [ProgressPath], this.transform.position) < 0.5f) 
			{
				ProgressPath++;
			}

			if (ProgressPath < Path.corners.Length) 
			{
				Move act = new Move (Path.corners [ProgressPath] - this.transform.position);

				DeclaredActions.Add (act);
			}

			if (Path.corners.Length == 0) 
			{
				Move act = new Move (PlayerPosition.position - this.transform.position);

				DeclaredActions.Add (act);
			}
		}

		if (Vector3.Distance (transform.position, PlayerPosition.position) < EnemyRadius + 1.2f) 
		{
			MeleeAttack act = new MeleeAttack (0.2f, 0, "AlliedCharacters");

			DeclaredActions.Add (act);
		}
	}

	private IEnumerator UpdateDestination()
	{
		while (true) 
		{
			ProgressPath = 0;
			//NavAgent.CalculatePath (PlayerPosition.position, Path);

			//Debug.Log ("Calculating path to: " + PlayerPosition.position + ", Path length: " + Path.corners.Length);

			yield return new WaitForSeconds (GameRandom.NextFloat (0.9f, 1.1f) * UpdateDestinationDelay);
		}
	}
}
