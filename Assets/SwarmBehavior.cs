using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// An implementation of the flocking algorithm: http://www.red3d.com/cwr/boids/
// Additional resources:
// http://harry.me/2011/02/17/neat-algorithms---flocking/
public class SwarmBehavior : MonoBehaviour {
	/// <summary>
	/// the number of drones we want in this swarm
	/// </summary>

	public enum SwarmState {DEFUALT, SWARM, HALT, MARCH, FORMATION};
	public int droneCount = 200;
	public float spawnRadius = 50f;
	public List<GameObject> drones;
	public GameObject droneHero;
	public Vector2 swarmBounds = new Vector2(300f, 300f);

	public GameObject prefab;
	public GameObject heroPrefab;
	public SwarmState state = SwarmState.FORMATION;

	// Use this for initialization
	protected virtual void Start () {

		if (prefab == null)
		{
			// end early
			Debug.Log("Please assign a drone prefab.");
			return;
		}

		// instantiate the drones
		GameObject droneTemp;
		drones = new List<GameObject>();
		//droneHero = (GameObject) GameObject.Instantiate(heroPrefab);
		DroneHeroBehavior dbHero = droneHero.GetComponent<DroneHeroBehavior> ();	
		dbHero.swarm = this;

		// create all drones;
		for (int i = 0; i < droneCount; i++) {
				prefab.tag = "Drone";
				droneTemp = (GameObject) GameObject.Instantiate(prefab);
				droneTemp.GetComponent<Renderer> ().material.color = Color.red;

				DroneBehavior db = droneTemp.GetComponent<DroneBehavior> ();	
				db.drones = this.drones;
				db.swarm = this;
				db.droneHero = this.droneHero;
				drones.Add(droneTemp);
		}
		this.formation ();

		/*
		for (int i = 0; i < droneCount; i++)
		{
			prefab.tag = "Drone";
			droneTemp = (GameObject) GameObject.Instantiate(prefab);
			droneTemp.GetComponent<Renderer> ().material.color = Color.red;

			DroneBehavior db = droneTemp.GetComponent<DroneBehavior> ();	
			db.drones = this.drones;
			db.swarm = this;
			db.droneHero = this.droneHero;

			// spawn inside circle
			Vector2 pos = new Vector2(transform.position.x, transform.position.z) + Random.insideUnitCircle * spawnRadius;
			droneTemp.transform.position = new Vector3(pos.x, transform.position.y, pos.y);
			droneTemp.transform.parent = transform;

			drones.Add(droneTemp);
		}
		*/
	}

	// Update is called once per frame
	protected virtual void Update () {
		if (Input.GetKeyDown (KeyCode.H)) {
			state = SwarmState.HALT;
		} else if (Input.GetKeyDown (KeyCode.Q)) {
			state = SwarmState.SWARM;
		} else if (Input.GetKeyDown (KeyCode.F)) {
			state = SwarmState.FORMATION;
			this.formation ();
		} else if (Input.GetKeyDown (KeyCode.M)) {
			state = SwarmState.DEFUALT;
		} else if (Input.anyKeyDown && state == SwarmState.FORMATION) {
			state = SwarmState.DEFUALT;
		}
	}

	protected virtual void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireCube(transform.position, new Vector3(swarmBounds.x, 0f, swarmBounds.y));
		Gizmos.DrawWireSphere(transform.position, spawnRadius);
	}

	public void formation() {
		float increment_x = 0f;
		float increment_z = 0f;
		float width = 10f;
		int lineCount = 10;
		increment_z = increment_z - width;
		increment_x = - width * ((float)lineCount - 1.0f) / 2;

		int j = 0;
		for (int i = 0; i < this.drones.Count; i++) {
			
			GameObject droneTemp = this.drones[i];

			if (droneTemp != null) {
				if (j % lineCount == 0 && j != 0) {
					increment_z = increment_z - width;
					increment_x = - width * ((float)lineCount - 1.0f) / 2;
				}

				float drone_x = droneHero.transform.position.x + increment_x;
				float drone_y = droneHero.transform.position.y;
				float drone_z = droneHero.transform.position.z + increment_z;
				Vector3 newPositon = new Vector3 (drone_x, drone_y, drone_z);
				if (state == SwarmState.DEFUALT) {
					droneTemp.transform.position = newPositon;
				} else if (state == SwarmState.FORMATION) {
					DroneBehavior db = droneTemp.GetComponent<DroneBehavior> ();	
					db.targetPosition = newPositon;
				}
				increment_x = increment_x + width;
				j++;
			}
		}
	}
}
