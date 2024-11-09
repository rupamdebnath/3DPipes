using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipeItem : MonoBehaviour
{
    //Manager that manages the design of pipes
    private PipeManager pipeManager;

    //Pipes position details
    public Vector3Int thisPosition3D;
    public int thisPosition1Dindex;

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
 
        StartCoroutine(WaitAndSpawn());
    }

    //coroutine to run recursively
    IEnumerator WaitAndSpawn()
    {        
        while (pipeManager.pipingPossible)
        {
            pipeManager.PlacePipe();
            yield return new WaitForSeconds(0.05f);
            //pipingPossible check for breaking the loop
            if (pipeManager.pipingPossible == false)
                break;
        }

        //SetNewPipePosition ..
        //SetNewPipePosition(newRandomPos);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }    
}

/*public enum PipeType
{
    BendPipe,
    HollowPipe,
    BulbPipe
}*/
