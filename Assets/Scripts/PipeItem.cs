using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipeItem : MonoBehaviour
{
    //Manager that manages the design of pipes
    private PipeManager pipeManager;

    //Initialization scripts
    void OnEnable()
    {
        Initialize();
    }

    private void Initialize()
    {
        pipeManager = GetComponent<PipeManager>();
        //GameObject newItem;
        pipeManager.Initialize();
        //newItem = Instantiate(verticalPipe, new Vector3(0f, 0f, 0f), Quaternion.identity);
        //pipeManager.SetColour();
        StartCoroutine(WaitAndSpawn());
    }

    //coroutine to run recursively
    public IEnumerator WaitAndSpawn()
    {        
        while (pipeManager.pipingPossible)
        {
            pipeManager.PlacePipe();
            yield return new WaitForSeconds(0.05f);
            //pipingPossible check for breaking the loop
            if (pipeManager.pipingPossible == false)
            {                
                break;
            }
                
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }    
}
