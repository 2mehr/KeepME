﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Arms {
    float Distance;
    // Use this for initialization
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
       
        Ishooter target = other.GetComponent<Ishooter>();
        if (target != null)
        {
            Destroy(gameObject);
        }
        else
        {
            return;
        }
        if (target.CType!=CType)
        {
            other.GetComponent<IShootable>().HP -= Damage;
            Destroy(gameObject);
        }
        if (other.GetComponent<IShootable>().HP<=0)
        {
            other.GetComponent<IShootable>().Die();
        }
    }
}
