using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInput2 : MonoBehaviour 
{
    public Tank tankScript;
    public Turret turretScript;

	public float viewHeight = 1.5f;

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

    //public float reloadSpeed;
    //public float reloadTimer = 5;

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
		UpdateAI();
	}

	public void UpdateAI()
    {
		//NEW AI

        //ToDo:
        // refresh targets (requires support code, "sensors")
        UpdateMemory();
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
                        a.cost = 1;
                        a.benefit = 1;
                        //...
                        possibleActions.Add(a);
                    }
                    //...
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
					turretScript.targetValid = true;
					turretScript.fireInput = Vector3.Angle(turretScript.transform.InverseTransformPoint(turretScript.targetPos), Vector3.forward) < 10;
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
			turretScript.targetValid = false;
			turretScript.fireInput = false;
		}
	}

    public void UpdateMemory()
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
						currentTargets.RemoveAt(i);
						break;
					}
					{
						RaycastHit hit;
						Vector3 toTarget = (t.actualTarget.transform.position + Vector3.up * viewHeight) - (transform.position + Vector3.up * viewHeight);
						Debug.DrawRay(transform.position + Vector3.up * viewHeight, toTarget, Color.grey);
						bool hitSomething = Physics.Raycast(transform.position + Vector3.up * viewHeight, toTarget.normalized, out hit, toTarget.magnitude);
						if (!hitSomething)
						{
							currentTargets.RemoveAt(i);
						} else
						{
							if (hit.collider.transform.IsChildOf(t.actualTarget.transform))
							{//it's the same
								t.lastKnownPos = hit.collider.transform.position;
							} else
							{//hit something else; can't see it
								currentTargets.RemoveAt(i);
							}
						}
					}
					break;
			}
		}
    }

    public float viewDistance = 20.0f;

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
                    t.actualTarget = tank.transform;
                    currentTargets.Add(t);
                }
            }
        }
    }

}
