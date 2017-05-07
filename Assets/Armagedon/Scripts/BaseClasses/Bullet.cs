using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bullet : Arms {
    float Distance;

    public void Fly()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position+transform.forward, Speed * Time.deltaTime);

        Distance += Speed * Time.deltaTime;
        if (Distance>Range)
        {
            Destroy(gameObject);
        }
      
    }
    private void Update()
    {
        Fly();
    }
    private void OnTriggerEnter(Collider other)
    {
       
        Ishooter bulletTarget = other.GetComponent<Ishooter>();
        if (bulletTarget != null)
        {
            Destroy(gameObject);
        }
        else
        {
            return;
        }
        if (bulletTarget.CType!=CType)
        {
           
            other.GetComponent<IShootable>().HP -= Damage;
            Destroy(gameObject);

            NPC npc = null;
            if (bulletTarget.Target == null || !bulletTarget.Target.GetComponent<NPC>())
                return;
            npc= bulletTarget.Target.GetComponent<NPC>();
          
            if (npc!=null)
            {
               
                if (bulletTarget.Target == null && npc.IsDaed != true)
                {

                    bulletTarget.Target = this.Shooter.transform;
                    ((NPC)bulletTarget).MoveMode = NPCMoveMode.Attack;
                    ((NPC)bulletTarget).GetComponent<NavMeshAgent>().stoppingDistance = 6;
                }
                else if( npc.IsDaed == true)
                {
                    bulletTarget.Target = null;
                   //bulletTarget.Target.GetComponent<NPC>().ChooseTarget();
                }
            }
           
        }
        if (other.GetComponent<IShootable>().HP<=0)
        {
            if(other.GetComponent<IShootable>().IsDaed==false)
            other.GetComponent<IShootable>().Die(this.Shooter);

         
        }
       
    }
}
