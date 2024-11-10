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
    public Vector3Int orgDirection;
    public Vector3Int nextpoint;

    //Check
    public bool pipingPossible;
    //Initialization
    public void Initialize()
    {
        //currentPipe = MakeNewPipe(new Vector3Int(lenX/2,lenY/2,lenZ/2));
        //currentPoint = new Vector3Int(Random.Range(0, lenX), Random.Range(0, lenY), Random.Range(0, lenZ));
        //currentPoint = new Vector3Int((int)lenX / 2, (int)lenY / 2, (int)lenZ / 2);
        CalculateHalfDistance(lenX, lenY, lenZ);
        cells = new int[lenX * lenY * lenZ];
        //pdirection = Vector3Int.zero;
        pipeItems = new List<GameObject>();
        //nextpoint = SelectPoint(currentPoint);
        orgDirection = Vector3Int.zero;
    }

    //PlacePipe function to be called for instantiating a model
    public void PlacePipe()
    {
/*        if (count >= cells.Length)
            return;*/
        nextpoint = SelectPoint(currentPoint);
        Debug.Log("Next" + (nextpoint));
        Debug.Log("NextDiff" + (currentPoint - nextpoint));
        while (nextpoint.x == -1)
        {
            MakeNewPipe(currentPoint, currentPoint);
            currentPoint = CheckNewPipeRequirements();

            //break
            if (currentPoint.x == -1)
            {
                pipingPossible = false;
                break;
            }
            Set1DPositionValueOccupied(currentPoint);
            nextpoint = SelectPoint(currentPoint);
        }

        if (pipingPossible)
        {
            MakeNewPipe(currentPoint, nextpoint);
            currentPoint = nextpoint;
            Set1DPositionValueOccupied(currentPoint);
        }
    }

    //Check if the spot is empty or full
    bool CheckifSpotAvailable(Vector3Int vpos)
    {
        if(Get1DPositionValue(vpos) == 0 && vpos.x != -1 && vpos.y != -1 && vpos.z !=-1)
            return true;
        else
            return false;
    }

    void MakeNewPipe(Vector3Int cpoint, Vector3Int npoint)
    {
        GameObject pipetype = hollowPipe;
        Vector3Int nDirection = npoint - cpoint;
        Vector3 npos = new Vector3(settings * (cpoint.x - halfX), settings * (cpoint.y - halfY), settings * (cpoint.z - halfZ));

        //first and last pipe
        if (orgDirection.magnitude < 0.1f)
        {
            orgDirection = nDirection;
            MakeBulbPipe(npos, orgDirection);
            return;
        }
        if (nDirection.magnitude < 0.1f)
        {
            MakeBulbPipe(npos, -orgDirection);
            orgDirection = Vector3Int.zero;
            return;
        }

        //No change in direction
        if (nDirection.x * orgDirection.x + nDirection.y * orgDirection.y + nDirection.z * orgDirection.z > 0.9)
        {
            pipetype = Instantiate(hollowPipe, npos, Quaternion.FromToRotation(Vector3Int.forward, orgDirection), this.transform);
            pipetype.transform.localPosition = npos;
            return;
        }

        // Direction change, bend Pipe formation
        Vector3Int pipeDir = nDirection - orgDirection;
        Quaternion bendPipeRot = Quaternion.LookRotation(pipeDir, Vector3.up);
        if (pipeDir.y == 0)
        {
            bendPipeRot *= Quaternion.AngleAxis(90, Vector3.forward);
            pipetype = Instantiate(bendPipe, npos, bendPipeRot, this.transform);
            pipetype.transform.localPosition = npos;
            //Pipes.Add(newPipe);
        }
        orgDirection = nDirection;
        //GameObject newPipe = Instantiate(pipetype, this.transform);
        //newPipe.transform.position = new Vector3(npoint.x, npoint.y, npoint.z);
        
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

    private void MakeBulbPipe(Vector3 pos, Vector3Int dir)
    {
        if (dir.magnitude < 0.1) { return; }
        GameObject newbulbPipe = Instantiate(
            bulbPipe, pos, Quaternion.FromToRotation(Vector3Int.forward, dir), this.transform
        );
        newbulbPipe.transform.localPosition = pos;
    }

    private Vector3Int CheckNewPipeRequirements()
    {
            // Check if there are spots available in positions
            bool spotsAvailable = false;
            foreach (int val in cells)
            {
                if (val == 0)
                {
                    spotsAvailable = true;
                    break;
                }
            }

            // If we can't continue building a new pipe, return
            if (!spotsAvailable)
            {
                return new Vector3Int(-1,-1,-1);
            }

            // Find a spot to begin the next pipe
            int sIndex = Random.Range(0, cells.Length);
            while (cells[sIndex] == 1)
            {
                sIndex++;
                if (sIndex >= cells.Length)
                {
                    sIndex = 0;
                }
            }

            // Calculate the new pipe position and return it
            Vector3Int ret = Vector3Int.zero;
            ret.z = sIndex / (lenX * lenY);
            sIndex -= ret.z * lenX * lenY;
            ret.y = sIndex / (lenX);
            ret.x = sIndex - (ret.y * lenX);
            return ret;
        }
}

