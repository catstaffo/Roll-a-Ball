using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pockets : MonoBehaviour
{
    void OnCollisionEnter(Collision coll)
	{
		
		GameObject collidedWith = coll.gameObject;
        if (collidedWith.tag == "Ball") {
            Destroy(collidedWith);
        
			//ballTotal -= 1;
			//scoreGT.text = "Balls left: " + ballTotal;
        }
    }
}
