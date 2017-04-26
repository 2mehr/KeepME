using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum NPCMoveMode
{
    Partol,
    Wander,
    Idle,
    Attack
    
}
public class NPC : CharBase {
    public NPCMoveMode MoveMode;
    public float SightRange;
    public override event OnDeathDelegate OnDeath;
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    float speed = 6.0F;
    NavMeshAgent NavAgent;
    CharacterController controller;
    public List<Transform> PatrolPoints;
    public int CurrentPatrolPoint;
    FildView fow;
   

    public override void Die( CharBase killer)
    {
        print(killer.CType + "v"+CType);
        if (IsDaed == true)
            return;
        if (CType== CharcterType.Player)
        {

            Time.timeScale = 0;
          
        }
        GetComponent<Renderer>().material.color = Color.red;
        OnDeath(this,killer);
        IsDaed = true;
        ((NPC)killer).Target = null;
        killer.GetComponent<FildView>().visibleTarget.Remove(this);
        ((NPC)killer).MoveMode = NPCMoveMode.Partol;
    }

  

    public override void Shoot(int currentWepID)
    {
        if (Weapons != null && Weapons.Count > 0)
        {
            Weapons[currentWepID].Fire();
        }


    }
    void Start()
    {
  
        NavAgent = GetComponent<NavMeshAgent>();
        fow = GetComponent<FildView>();
       
    }


    void Update ()
    {
      
        if (IsDaed == true)
            return;
        switch (MoveMode)
        {
            case NPCMoveMode.Wander:Wander();
                break;
            case NPCMoveMode.Partol:Patrol();
                break;
            case NPCMoveMode.Attack:
                if(Target!=null)
                {
                    Attack();
                }
              
                break;
               
        }
      
        if(Target!=null)
        {
            RotateToTarget();
            MoveToTarget();
            if (Vector3.Distance(transform.position, Target.position) < NavAgent.stoppingDistance)
            {
                NavAgent.Stop();
            }
            else
            {
                NavAgent.Resume();
            }


        }



    }
 // public  Transform NearEnemy;
    public void ChooseTarget()
    {
       
        if (fow.visibleTarget.Count == 0)
        { 
            return; 
        }
        Target = fow.visibleTarget[0].transform;

            foreach (CharBase item in fow.visibleTarget)
            {
            if (Vector3.Distance(transform.position, item.transform.position) < Vector3.Distance(transform.position, new Vector3(Target.position.x, Target.position.y, Target.position.z)))
                {
                    Target =item.transform;
                }
              
            }
        
        
    }

    public void RotateToTarget()
    {
      
        {
           Vector3 relativePos = Target.position - transform.position;
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relativePos),40*Time.deltaTime);
            transform.rotation = rotation;
        }
      
    }


    public void Attack()
    {
        // ChooseTarget();
        Shoot(CurrentWeaponID);
        NavAgent.stoppingDistance = 6;
       
    }
    public void Wander()
    {
        if (Weapons == null)
        {
            return;
        }

        if (Target == null)
            return;
        

        if (Weapons.Count == 0)
            return;

       
        if (Weapons[CurrentWeaponID].Range <Vector3.Distance( transform.position,Target.position))
        {
            NavAgent.destination = Target.position;
            
        }
        else
        {
            NavAgent.destination = transform.position;
        }
    }
    void Patrol()
    {
     
        if (PatrolPoints == null)
            return;
        NavAgent.stoppingDistance = 0;
        Target = PatrolPoints[CurrentPatrolPoint];

        if (Vector3.Distance(transform.position, PatrolPoints[CurrentPatrolPoint].position) < 1f)
        {
            CurrentPatrolPoint++;
            if (CurrentPatrolPoint > PatrolPoints.Count - 1)
            {
                
                CurrentPatrolPoint = 0;
               
            }
        }
       
    }
    private void MoveToTarget()
    {
        if(Target!=null)
        NavAgent.destination = Target.position;
    }
   
  
}
