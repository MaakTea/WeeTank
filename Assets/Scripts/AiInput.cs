using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiInput : MonoBehaviour 
{

    public Tank tankScript;
    public Turret turretScript;

    public enum TargetType
    {
        Waypoint,
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
        public float cost;
        public float benefit;
        public ActionType type;
    }

    public Transform target;
    public Transform[] targets;

    public List<Target> currentTargets = new List<Target>();    //incl. "memory"
    public List<Action> possibleActions = new List<Action>();

    //public float reloadSpeed;
    //public float reloadTimer = 5;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        // -- setting the TARGET's direction --
        Vector3 targetDirection = target.position - transform.position;
        targetDirection = Quaternion.Inverse(transform.rotation)*targetDirection;
        // -- setting the angle to turn --
        float angle = Mathf.Atan2(targetDirection.x , targetDirection.z) * Mathf.Rad2Deg;
        if(angle > 180) 
        {
            angle -= 360;
        }

        // -- reaching target --
        if((target.position - transform.position).magnitude < 4) 
        {
            target = targets[Random.Range(0,targets.Length)];
        }

        // -- switching the steering from -1 to 1 --
        float x = Mathf.Clamp(angle,-1,1);
        float y = 1;

        // -- if not looking in the direction of the TARGET --
        if (Mathf.Abs(angle) > 5) 
        {
            y = 0;
            turretScript.fireInput = false;
        }
        else
        {
            turretScript.fireInput = true;
        }



        tankScript.steering = x;
        tankScript.gas = y;


        turretScript.targetValid = true;
        turretScript.targetPos = target.position;
        turretScript.reloadSpeed = 5;
    }

    public void FixedUpdate()
    {
        UpdateAI();
    }

    public void UpdateAI()
    {
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
                    if (false)
                    {
                        Action a = new Action();
                        a.type = ActionType.Attack;
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
                default:
                    break;
            }
        }
        else
        {
            //or do a default action here
        }
    }

    public void UpdateMemory()
    {
        //ToDo: for each known target, check line of sight: can we still see it?
        // if not, turn it into a memory
        //for each memory, increase timer and if too old, forget it
    }

    public void UpdateSensors()
    {
        float viewDistance = 20.0f;
        Vector3 dir = Quaternion.Euler(0, Random.Range(-180, 180), 0) * Vector3.forward;
        Debug.DrawRay(transform.position + Vector3.up, dir * viewDistance, Color.black);
        RaycastHit hit;
        bool seen = Physics.Raycast(transform.position + Vector3.up, dir, out hit, viewDistance);
        if (seen)
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
                    t.type = TargetType.Enemy;
                    t.lastKnownPos = hit.collider.transform.position;
                    t.actualTarget = tank.transform;
                    currentTargets.Add(t);
                }
            }
        }
    }

}
