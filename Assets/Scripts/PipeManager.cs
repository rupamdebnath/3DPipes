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


    //Array of integers for tracking the position of every cell
    public int[] cells;
    public int count;
    public Vector3Int currentPoint;
    public Vector3Int orgDirection;
    public Vector3Int nextpoint;

    //Check
    public bool pipingPossible;

    //Materials
    public Material[] colorMaterials;
    public List<Material> tempColorList;
    private int colorIndex;
    //Initialization

    public void Initialize()
    {
        pipingPossible = true;
        tempColorList = new List<Material>();
        tempColorList.AddRange(colorMaterials);
        colorIndex = Random.Range(0, tempColorList.Count - 1);
        SetColour(colorMaterials[colorIndex]);
        tempColorList.RemoveAt(colorIndex);
        CalculateHalfDistance(lenX, lenY, lenZ);
        cells = new int[lenX * lenY * lenZ];
        pipeItems = new List<GameObject>();
        orgDirection = Vector3Int.zero;
        currentPoint = GetNewPositionForCurrent();
        MakeNewPipe(currentPoint, currentPoint);
        Set1DPositionValueOccupied(currentPoint);
    }

    //PlacePipe function to be called for instantiating a model
    public void PlacePipe()
    {
        nextpoint = SelectPoint(currentPoint);
        Debug.Log("Next" + (nextpoint));
        Debug.Log("NextDiff" + (currentPoint - nextpoint));

        if (pipingPossible)
        {
            MakeNewPipe(currentPoint, nextpoint);
            currentPoint = nextpoint;
            Set1DPositionValueOccupied(currentPoint);
        }
        else
        {
            // When no more pipes can be placed, create the end bulb at the last valid position
            CreateEndBulbPipe(currentPoint, -orgDirection);
        }
    }
    // New function to create the end bulb pipe
    private void CreateEndBulbPipe(Vector3Int position, Vector3Int direction)
    {
        Vector3 endPos = new Vector3(settings * (position.x - halfX), settings * (position.y - halfY), settings * (position.z - halfZ));
        MakeBulbPipe(endPos, direction);
    }

    //Check if the spot is empty or full
    public bool CheckifSpotAvailable(Vector3Int vpos)
    {
        if(Get1DPositionValue(vpos) == 0 && vpos.x != -1 && vpos.y != -1 && vpos.z !=-1)
            return true;
        else
            return false;
    }

    public void MakeNewPipe(Vector3Int cpoint, Vector3Int npoint)
    {
        GameObject pipetype = new GameObject();
        Vector3Int nDirection = npoint - cpoint;
        Vector3 npos = new Vector3(settings * (cpoint.x - halfX), settings * (cpoint.y - halfY), settings * (cpoint.z - halfZ));

        //first pipe
        if (orgDirection.magnitude < 0.1f)
        {
            orgDirection = nDirection;
            MakeBulbPipe(npos, orgDirection);
            return;
        }

        // Straight pipe if direction remains the same
        if (Vector3.Dot(((Vector3)nDirection).normalized, ((Vector3)orgDirection).normalized) > 0.9f)
        {
            pipetype = Instantiate(hollowPipe, npos, Quaternion.FromToRotation(Vector3Int.forward, orgDirection), transform);
        }
        else
        {
            // Curved pipe for a directional change
            Vector3Int curveDir = nDirection - orgDirection;
            Quaternion rotation = Quaternion.LookRotation(curveDir, Vector3.up);
            Debug.Log("CurvePOint" + curveDir.x + curveDir.y + curveDir.z);
            if (curveDir.y == 0)
            {
                rotation *= Quaternion.AngleAxis(90, Vector3.forward);
            }

            pipetype = Instantiate(bendPipe, npos, rotation, transform);
        }
        pipeItems.Add(pipetype);
        pipetype.transform.localPosition = npos;
        orgDirection = nDirection;       
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
        return cells[xOff + yOff + zOff];
    }
    //Set the position value for new Pipe to 1
    public void Set1DPositionValueOccupied(Vector3Int vPos)
    {
        int xOff = vPos.x;
        int yOff = vPos.y * lenX;
        int zOff = vPos.z * lenX * lenY;

        cells[xOff + yOff + zOff] = 1;
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
        pipeItems.Add(newbulbPipe);
    }

    public Vector3Int GetNewPositionForCurrent()
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
        Vector3Int posvalue = Vector3Int.zero;
        posvalue.z = sIndex / (lenX * lenY);
        sIndex -= posvalue.z * lenX * lenY;
        posvalue.y = sIndex / (lenX);
        posvalue.x = sIndex - (posvalue.y * lenX);
        return posvalue;
    }

    //Set Color function
    public void SetColour(Material m)
    {
        // Assign the new materials to each pipe type
        hollowPipe.GetComponent<MeshRenderer>().material = m;
        bendPipe.GetComponent<MeshRenderer>().material = m;
        bulbPipe.GetComponent<MeshRenderer>().material = m;
    }

    //Destroy all pipes
    public void DestroyAllPipes()
    {
        foreach(GameObject item in pipeItems)
        {
            Destroy(item);
        }
    }
}

