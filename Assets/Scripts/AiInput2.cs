using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInput2 : MonoBehaviour 
{
    public Tank tankScript;
    public Turret turretScript;

	public float viewDistance = 20.0f;
	public float viewHeight = 1.5f;

	public float aimAccuracyToFire = 10.0f;
	public float aimAhead = 0;
	public float bulletSpeed = 15;	//ToDo: get it from turretScript.bullet on start

	public enum TargetType
    {
        Waypoint,
		Friend,
		Enemy,
        WasEnemy,
        PotentialEnemy,
        Powerup,
        MAX
    }
    public enum ActionType
    {
        Attack,
        MoveTo,     //investigate memory or patrol to waypoint, or go to powerup
        Avoid,
        Dodge,
        Hide,       //?
        MAX
    }

    [System.Serializable]
    public class Target
    {
        public Vector3 lastKnownPos;
		public Vector3 lastKnownVel;
		public TargetType type;
        public Transform actualTarget;
        public float timer;
    }
    [System.Serializable]
    public class Action
    {
		public ActionType type;	//do this...
		public Target target;  //...about this target
		public float benefit;	//how useful that would be
		public float cost;		//how difficult, how much time, etc
    }


    public List<Target> currentTargets = new List<Target>();    //incl. "memory"
    public List<Action> possibleActions = new List<Action>();


    // Use this for initialization
    void Start ()
	{
		//turretScript.reloadSpeed = 5;
	}

	// Update is called once per frame
	void Update () 
    {
	}

    public void FixedUpdate()
    {
		UpdateAI(Time.fixedDeltaTime);
	}

	public void UpdateAI(float dT)
    {
		//NEW AI

        //ToDo:
        // refresh targets (requires support code, "sensors")
        UpdateMemory(dT);
        UpdateSensors();

        //clear previous actions
        possibleActions.Clear();
        // maybe add a default action?

        // loop through targets, and depending on its type and state, add potential actions to take about it (also calc. cost and benefit of action)
        foreach(Target t in currentTargets)
        {
            switch(t.type)
            {
                case TargetType.Enemy:
                    if (true)
                    {
                        Action a = new Action();
                        a.type = ActionType.Attack;
						a.target = t;
                        a.cost = 1.0f;
                        a.benefit = 1.0f;
                        //...
                        possibleActions.Add(a);
                    }
                    //...
                    break;

				case TargetType.WasEnemy:
					{
						Action a = new Action();
						a.type = ActionType.MoveTo;
						a.target = t;
						a.cost = 1.0f;
						a.benefit = 0.2f;
						//...
						possibleActions.Add(a);
					}
					break;
            }
        }

        // pick best action (min cost, max benefit)
        float bestValue = -1;
        Action bestAction = null;
        foreach(Action a in possibleActions)
        {
            float value = a.benefit - a.cost;
            if (value > bestValue)
            {
                bestAction = a;
                bestValue = value;
            }
        }

        // execute 
        if (bestAction != null)
        {
            switch(bestAction.type)
            {
				case ActionType.Attack:
					turretScript.targetPos = bestAction.target.lastKnownPos;
					if (aimAhead > 0)
					{
						float distance = (turretScript.targetPos - turretScript.transform.position).magnitude;
						float extrapolationTime = aimAhead * (distance / bulletSpeed);
						turretScript.targetPos += bestAction.target.lastKnownVel * extrapolationTime;
					}
					turretScript.targetValid = true;
					turretScript.fireInput = Vector3.Angle(turretScript.transform.InverseTransformPoint(turretScript.targetPos), Vector3.forward) < aimAccuracyToFire;
					break;

				case ActionType.MoveTo:
					//ToDo: we should manage selecting waypoints HERE, not in a separate script (AiInputNav)
					// the actual communication with the navmeshagent can stay there
					turretScript.targetValid = true;
					turretScript.fireInput = false;
					break;

                default:
					turretScript.targetValid = false;
					turretScript.fireInput = false;
					break;
            }
        }
        else
        {
			//or do a default action here
			turretScript.fireInput = false;
			//turretScript.targetValid = false;
			//ToDo: make the turret look forwards or scan
			if (Random.value < 0.02f)
			{
				turretScript.targetPos = transform.position + Quaternion.AngleAxis(Random.Range(-90, 90), Vector3.up) * transform.forward * viewDistance;
				turretScript.targetValid = (Random.value < 0.25f);
			}
		}
	}

    public void UpdateMemory(float dT)
    {
        //ToDo: for each known target, check line of sight: can we still see it?
        // if not, turn it into a memory (or forget it right away)
        //for each memory, increase timer and if too old, forget it
		for(int i=currentTargets.Count-1; i>=0; i--)
		{
			Target t = currentTargets[i];
			switch(t.type)
			{
				case TargetType.Enemy:
					if (t.actualTarget == null)
					{//doesn't exist. destroyed?
					 //currentTargets.RemoveAt(i);
						t.type = TargetType.WasEnemy;
						t.timer = 1;
						DebugExtension.DebugCircle(t.lastKnownPos, 1, 10);
						break;
					}
					{
						RaycastHit hit;
						Vector3 toTarget = (t.actualTarget.transform.position + Vector3.up * viewHeight) - (transform.position + Vector3.up * viewHeight);
						Debug.DrawRay(transform.position + Vector3.up * viewHeight, toTarget, Color.grey);
						bool hitSomething = Physics.Raycast(transform.position + Vector3.up * viewHeight, toTarget.normalized, out hit, toTarget.magnitude);
						if (!hitSomething)
						{//there's nothing there
							//currentTargets.RemoveAt(i);
							t.type = TargetType.WasEnemy;
							t.timer = 3;
							DebugExtension.DebugCircle(t.lastKnownPos, 1, 10);
						} else
						{
							if (hit.collider.transform.IsChildOf(t.actualTarget.transform))
							{//it's the same
								t.lastKnownVel = Vector3.Lerp(t.lastKnownVel, (hit.collider.transform.position - t.lastKnownPos) / dT, 0.25f);
								t.lastKnownPos = hit.collider.transform.position;
							} else
							{//hit something else; can't see it
								 //currentTargets.RemoveAt(i);
								t.type = TargetType.WasEnemy;
								t.timer = 10;	//investigate
								DebugExtension.DebugCircle(t.lastKnownPos, 1, 10);
							}
						}
					}
					break;

				case TargetType.WasEnemy:
					t.timer -= dT;
					if (t.timer < 0)
					{
						currentTargets.RemoveAt(i);
						break;
					}
					break;
			}
		}
    }


    public void UpdateSensors()
    {
        Vector3 dir = Quaternion.Euler(0, Random.Range(-180, 180), 0) * Vector3.forward;
        Debug.DrawRay(transform.position + Vector3.up, dir * viewDistance, Color.black);
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(transform.position + Vector3.up * viewHeight, dir, out hit, viewDistance);
        if (hitSomething)
        {
            Tank tank = hit.collider.GetComponentInParent<Tank>();
            if (tank != null)
            {
                //ToDo: check if we already know the same target
                if (currentTargets.Find(ct => ct.actualTarget == tank.transform) != null)
                { //we already have it
                }
                else
                { //new!
                    Target t = new Target();
                    t.type = tank.team == this.tankScript.team ? TargetType.Friend : TargetType.Enemy;
                    t.lastKnownPos = hit.collider.transform.position;
					t.lastKnownVel = Vector3.zero;
					t.actualTarget = tank.transform;
                    currentTargets.Add(t);
                }
            }
        }
    }

}
