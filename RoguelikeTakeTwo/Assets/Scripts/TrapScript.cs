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
        if(distanceToCam.x < -15 || distanceToCam.x > 15){
            //print("DESTROY");
            Destroy(this.gameObject);
        }
        if(distanceToCam.y < -12 || distanceToCam.y > 12){
            //print("DESTROY");
            Destroy(this.gameObject);
        }
        
    }
}
