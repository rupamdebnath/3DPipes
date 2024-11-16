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
        yield return new WaitForSeconds(0.05f);
        Debug.Log("Finished");
        pipeManager.currentPoint = pipeManager.GetNewPositionForCurrent();
        if (pipeManager.tempColorList.Count > 0 && pipeManager.CheckifSpotAvailable(pipeManager.currentPoint))
        {
            pipeManager.pipingPossible = true;
            pipeManager.orgDirection = Vector3Int.zero;
            int colorIndex = Random.Range(0, pipeManager.tempColorList.Count - 1);
            pipeManager.SetColour(pipeManager.tempColorList[colorIndex]);
            pipeManager.tempColorList.RemoveAt(colorIndex);

            pipeManager.MakeNewPipe(pipeManager.currentPoint, pipeManager.currentPoint);
            pipeManager.Set1DPositionValueOccupied(pipeManager.currentPoint);
            StartCoroutine(WaitAndSpawn());
        }
        else
        {
            yield return new WaitForSeconds(1f);
            pipeManager.DestroyAllPipes();
            Restart();
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }    

    void Restart()
    {
        Initialize();
    }
}
