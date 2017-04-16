using UnityEngine;
using System.Collections;

/*
 * Creates a "sound" such that unaware Paragon AI agents will investigate it.
 * */

namespace ParagonAI
{
    public class SoundMaker : MonoBehaviour
    {
        public float radius = 40;
        public int[] teamsThatShouldHear;
        public int delayTime = 1;

        void Start()
        {
            //All this script does is call the method on the Controller.  Doesn't do anything else.
            ParagonAI.ControllerScript.currentController.CreateSound(transform.position, radius, teamsThatShouldHear);
        }
    }
}

