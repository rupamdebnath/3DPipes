using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    //Capacity of the field of view of user
    public int lenX, lenY, lenZ;
    //variables of PipeManager
    public float settings;
    private Vector3Int pdirection;
    [SerializeField]
    private List<GameObject> pipeItems;

    private float halfX, halfY, halfZ;
    public GameObject hollowPipe;
    public GameObject bendPipe;
    public GameObject bulbPipe;

    //Types of pipes
    public List<GameObject> PipeTypes;

    //Array of integers for tracking the position of every cell
    public int[] cells;
    public int count;
    public Vector3Int currentPoint;
    public Vector3Int nextPoint;

    //Check
    public bool pipingPossible = true;

    private Vector3Int previousDirection = Vector3Int.zero;
    //Initialization
    public void Initialize()
    {
        //currentPipe = MakeNewPipe(new Vector3Int(lenX/2,lenY/2,lenZ/2));
        //currentPoint = new Vector3Int(Random.Range(0, lenX), Random.Range(0, lenY), Random.Range(0, lenZ));
        currentPoint = new Vector3Int((int)lenX / 2, (int)lenY / 2, (int)lenZ / 2);
        cells = new int[lenX * lenY * lenZ];
        pdirection = Vector3Int.zero;
        pipeItems = new List<GameObject>();

    }

    //PlacePipe function to be called for instantiating a model
    public void PlacePipe()
    {
        if (count >= cells.Length)
            return;

        if (CheckifSpotAvailable(currentPoint))
        {
            pipeItems.Add(MakeNewPipe(currentPoint));
            count++;
            Set1DPositionValueOccupied(currentPoint);
            currentPoint = SelectPoint(currentPoint);
        }
        else
            return;
    }

    //Check if the spot is empty or full
    bool CheckifSpotAvailable(Vector3Int vpos)
    {
        if(Get1DPositionValue(vpos) == 0 && vpos.x != -1 && vpos.y != -1 && vpos.z !=-1)
            return true;
        else
            return false;
    }

    GameObject MakeNewPipe(Vector3Int npoint)
    {
        GameObject newPipe = Instantiate(bendPipe, this.transform);
        newPipe.transform.position = new Vector3(npoint.x, npoint.y, npoint.z);
        return newPipe;
    }

    //Middle position
    void CalculateHalfDistance(int x, int y, int z)
    {
        halfX = (float)(x-1) / 2f;
        halfY = (float)(y-1) / 2f;
        halfZ = (float)(z-1) / 2f;
        Debug.Log("Half" + halfX + halfY + halfZ);
    }

    //Get the call index in 1D from 3D
    public int Get1DPositionValue(Vector3Int vPos)
    {
        int xOff = vPos.x;
        int yOff = vPos.y * lenX;
        int zOff = vPos.z * lenX * lenY;
        Debug.Log("Offset" + xOff + yOff + zOff);
        return cells[xOff + yOff + zOff];
    }
    //Set the position value for new Pipe to 1
    public void Set1DPositionValueOccupied(Vector3Int vPos)
    {
        int xOff = vPos.x;
        int yOff = vPos.y * lenX;
        int zOff = vPos.z * lenX * lenY;

        cells[xOff + yOff + zOff] = 1;
        Debug.Log("cells" + (xOff + yOff + zOff));
    }
    //Using current point select a new point of pipe placement
    public Vector3Int SelectPoint(Vector3Int currentPos)
    {
        //Check all directions of possible movement
        List<Vector3Int> possiblePos = new List<Vector3Int>();
        Vector3Int adjPoint;


        adjPoint = currentPos + new Vector3Int(1, 0, 0);
        if (adjPoint.x < lenX && CheckifSpotAvailable(adjPoint))
        {
            possiblePos.Add(adjPoint);
        }
        adjPoint = currentPos + new Vector3Int(-1, 0, 0);
        if (adjPoint.x > -1 && CheckifSpotAvailable(adjPoint))
        {
            possiblePos.Add(adjPoint);
        }
        adjPoint = currentPos + new Vector3Int(0, 1, 0);
        if (adjPoint.y < lenY && CheckifSpotAvailable(adjPoint))
        {
            possiblePos.Add(adjPoint);
        }
        adjPoint = currentPos + new Vector3Int(0, -1, 0);
        if (adjPoint.y > -1 && CheckifSpotAvailable(adjPoint))
        {
            possiblePos.Add(adjPoint);
        }
        adjPoint = currentPos + new Vector3Int(0, 0, 1);
        if (adjPoint.z < lenZ && CheckifSpotAvailable(adjPoint))
        {
            possiblePos.Add(adjPoint);
        }
        adjPoint = currentPos + new Vector3Int(0, 0, -1);
        if (adjPoint.z > -1 && CheckifSpotAvailable(adjPoint))
        {
            possiblePos.Add(adjPoint);
        }
        //return -1 if out of range
        if (possiblePos.Count == 0)
        {
            pipingPossible = false;
            return new Vector3Int(-1, -1, -1);
        }

        return possiblePos[Random.Range(0, possiblePos.Count)];
    }
}

