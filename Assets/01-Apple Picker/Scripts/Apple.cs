using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour

{
    [Header("Set in Inspector")]
    public static float bottomY = -20f; // a

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < bottomY)
        {
            Destroy(this.gameObject); // b


            // Get a reference to the ApplePicker component of Main Camera
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>(); // b

            // Call the public AppleDestroyed() method of apScript
            apScript.AppleDestroyed(); // c
        }
    }
}
