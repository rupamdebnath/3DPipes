using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipeItem : MonoBehaviour
{

    [SerializeField] private GameObject verticalPipe;

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
        while (true)
        {
            pipeManager.PlacePipe();
            yield return new WaitForSeconds(0.02f);
        }

        //SetNewPipePosition ..
        //SetNewPipePosition(newRandomPos);
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    //Get the position for new Pipe using offset calculations from a given 3D point
    
}
