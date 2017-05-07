using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum NPCMoveMode
{
    Partol,
    Wander,
    Idle,
    Attack,
    Cover
    
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
    public NPCMoveMode DefaultMoveMode;
    public List<Transform> CoverPos = new List<Transform>();
   

    public override void Die( CharBase killer)
    {
         
          
                 
            OnDeath(this, killer);
                
       /// killer.GetComponent<FildView>().visibleTarget.Remove(this);
          
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
        this.OnDeath += InEnemy_OnDeath;

        DefaultMoveMode = this.MoveMode;
        NavAgent = GetComponent<NavMeshAgent>();
        fow = GetComponent<FildView>();
        
       
    }
    private void InEnemy_OnDeath(CharBase e, CharBase killer)
    {
        IsDaed = true;
        GetComponent<Renderer>().material.color = Color.red;
        if (e.CType == CharcterType.Enamey)
           Camera.main.GetComponent<LevelManager>().Enemeies.Remove(e);
        //
        ((NPC)killer).ChooseTarget();

        NPC[] allNpc = GameObject.FindObjectsOfType<NPC>();
        for (int i = 0; i < allNpc.Length; i++)
        { 
           if (allNpc[i].Target == this.transform)
            {
              //  print(allNpc[i].transform.name);
                allNpc[i].Target = null;
                allNpc[i].MoveMode = allNpc[i].DefaultMoveMode;
                allNpc[i].GetComponent<FildView>().visibleTarget.Remove(this);
               // print(allNpc[i].name);
            }
           
        }


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
            case NPCMoveMode.Cover:
                TakeCover();
                break;
               
        }
       if(MoveMode == NPCMoveMode.Cover && (transform.position - Target.position).magnitude < 1)
        {
            MoveMode = NPCMoveMode.Attack;
        }

        if (Target!=null)
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
    public void ChooseTarget()
    {
        if (MoveMode != NPCMoveMode.Attack)
            return;
        if (  fow.visibleTarget.Count == 0||(Target != null&&Target.GetComponent<CharBase>()))
        {
           
            return; 

        }
        Target = fow.visibleTarget[0].transform;

        if(Target.GetComponent<CharBase>().CType == CharcterType.Enamey)
         

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

        Shoot(CurrentWeaponID);
        NavAgent.stoppingDistance = fow.ViewRadius;
        TakeCover();
        print("oooo" + this.name);
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
   
    GameObject[] Covers;
   public  void TakeCover()
    {
       
        if (CType == CharcterType.Enamey)
        {
             Covers = GameObject.FindGameObjectsWithTag("Cover");
            
            for (int i = 0; i < Covers.Length; i++)
            {

                if ((Covers[i].transform.position - transform.position).magnitude <= fow.ViewRadius && Covers[i].GetComponent<Cover>().IsUccupired != true)
                {
                    if ((Covers[i].transform.position - Target.position).magnitude < fow.ViewRadius)
                    {
                        Target = Covers[i].transform;
                        Covers[i].GetComponent<Cover>().IsUccupired = true;
                        Covers[i].GetComponent<Cover>().Uccupier = this;
                        MoveMode = NPCMoveMode.Cover;
                        NavAgent.stoppingDistance = 0;
                        print("KKKK" + this.name);
                        break;
                    }
                }
                
            }
        }
       
    }
   
  
}
