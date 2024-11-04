using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PipeObject : MonoBehaviour
{
    //Capacity of the field of view of user
    public int lenX, lenY, lenZ;

    [SerializeField] private GameObject verticalPipe;

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
        GameObject newItem;
        newItem = Instantiate(verticalPipe, new Vector3(0f, 0f, 0f), Quaternion.identity);
        StartCoroutine(WaitAndSpawn());
    }
    IEnumerator WaitAndSpawn()
    {

        yield return new WaitForSeconds(0.05f);
        Vector3Int newRandomPos = new Vector3Int(Random.Range(0, lenX), Random.Range(0, lenY), Random.Range(0, lenZ));
        pipeManager.PlacePipe(newRandomPos);
        //SetNewPipePosition ..

        StartCoroutine(WaitAndSpawn());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    //Get the position for new Pipe
    void GetNewPipePosition()
    {

    }
    //Set the position for new Pipe
    void SetNewPipePosition()
    {
    }
}
