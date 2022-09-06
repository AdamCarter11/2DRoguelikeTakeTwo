using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    private Vector3 distanceToCam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distanceToCam = Camera.main.transform.position - transform.position;
        if(distanceToCam.x < -20 || distanceToCam.x > 20){
            //print("DESTROY");
            Destroy(this.gameObject);
        }
        if(distanceToCam.y < -17 || distanceToCam.y > 17){
            //print("DESTROY");
            Destroy(this.gameObject);
        }
        
    }
}
