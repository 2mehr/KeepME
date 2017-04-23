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



    public override void Die()
    {
      
        GetComponent<Renderer>().material.color = Color.red;
        //fow.visibleTarget.RemoveAt(0);
        MoveMode = NPCMoveMode.Partol;
        OnDeath(this);
    }

    public override void Move()
    {
        throw new NotImplementedException();
    }

    public override void Shoot(int currentWepID)
    {
        if (Weapons!=null&&Weapons.Count>0)
        {
            Weapons[currentWepID].Fire();
        }
      
    }

    // Use this for initialization
    void Start()
    {
  
        NavAgent = GetComponent<NavMeshAgent>();
        fow = GetComponent<FildView>();
    }
    

    void Update ()
    {
        switch (MoveMode)
        {
            case NPCMoveMode.Wander:Wander();
                break;
            case NPCMoveMode.Partol:Patrol();
                break;
            case NPCMoveMode.Attack:Attack();
                break;
               
        }
        MoveToTarget();

        if (fow.target !=null && CType != CharcterType.Enamey)
        {
            MoveMode = NPCMoveMode.Attack;
        }
      
    }
    Transform nearEnemy;
    public void ChooseTarget()
    {
       
        if (fow.visibleTarget.Count == 0)
        { 
            return; 
        }
       
      
        for (int i = 0; i < fow.visibleTarget.Count; i++)
        {
             nearEnemy = fow.visibleTarget[i];
            print(fow.visibleTarget[i].name);
        }
      

            foreach (Transform item in fow.visibleTarget)
            {
            if (Vector3.Distance(transform.position, item.position) < Vector3.Distance(transform.position, new Vector3(nearEnemy.position.x, nearEnemy.position.y, nearEnemy.position.z)))
                {
                    nearEnemy =item;
                }
              
            }
        
            Target.position = new Vector3(nearEnemy.position.x, nearEnemy.position.y, nearEnemy.position.z - 3);
            print(Target.name);
      
       
       
        RotateToTarget();
      
    }

    public void RotateToTarget()
    {
        if (CType == CharcterType.Player)
        {
           Vector3 relativePos = Target.position - transform.position;
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(relativePos),22*Time.deltaTime);
            transform.rotation = rotation;
        }
      
    }


    public void Attack()
    {
        ChooseTarget();
        Shoot(CurrentWeaponID);

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
