using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour {
   public  NPC Uccupier;
    public bool IsUccupired;

	// Use this for initialization
	void Start ()
    {
       

    }
	
	// Update is called once per frame
	void Update ()
    {
       
       
	}

    private void OnTriggerExit(Collider other)
    {

        if ( Uccupier!=null && other.transform == Uccupier.transform)
        {
          
            IsUccupired = false;
        }
    }
}
