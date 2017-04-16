using System;
using System.Collections;
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
    public override void Die()
    {
      
        GetComponent<Renderer>().material.color = Color.red;
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
    }
    

    void Update ()
    {
        switch (MoveMode)
        {
            case NPCMoveMode.Wander:Wander();
                break;
            case NPCMoveMode.Partol:Patrol();
                break;
            case NPCMoveMode.Attack:Attck();
                break;

               
        }
        MoveToTarget();
        if (Sight.Ditect==true&&CType!=CharcterType.Enamey)
        {
            print(Sight.Ditect);
            MoveMode = NPCMoveMode.Attack;
        }
        if (Sight.AttackMode==true&& CType!= CharcterType.Enamey)
        {
              MoveMode = NPCMoveMode.Attack;
        }
     
        print(MoveMode);

    }
    public void ChooseTarget()
    {

        if ( LevelManager.Manager.Enemeies.Count == 0)
        {
           
            return;
        }
           
        Transform nearEnemy =LevelManager. Manager.Enemeies[0].GetComponent<Transform>();

        foreach ( var item in LevelManager. Manager.Enemeies)
        {
            if (Vector3.Distance(transform.position,item.transform.position)<Vector3.Distance(transform.position,nearEnemy.transform.position))
            {
                nearEnemy = item.transform;
             
            }
        }
        Target = nearEnemy;
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
   

    public void TakeCover(Transform cover)
    {
       
    }
    public void Attck()
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
