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
    public GameObject pipeModel;

    //Array of integers for tracking the position of every cell
    public int[] cells;
    public int count;
    //Initialization
    public void Initialize()
    {
        CalculateHalfDistance(lenX,lenY,lenZ);

        cells = new int[lenX * lenY * lenZ];
        pdirection = Vector3Int.zero;
        pipeItems = new List<GameObject>();

    }

    //PlacePipe function to be called for instantiating a model
    public void PlacePipe()
    {
        if (count >= cells.Length)
            return;
        Vector3Int newRandomPos = new Vector3Int(Random.Range(0, lenX), Random.Range(0, lenY), Random.Range(0, lenZ));
        Debug.Log("Position" + newRandomPos);

        Debug.Log("Get1DPositionValue" + " " + Get1DPositionValue(newRandomPos) + " " + cells[Get1DPositionValue(newRandomPos)]);

        int cellValue = Get1DPositionValue(newRandomPos);
        if (CheckifSpotAvailable(newRandomPos))
        {
            pipeItems.Add(MakeNewPipe(newRandomPos));
            count++;
            Set1DPositionValueOccupied(newRandomPos);
        }
        else
            return;
    }

    bool CheckifSpotAvailable(Vector3Int vpos)
    {
        if(Get1DPositionValue(vpos) == 0)
            return true;
        else
            return false;
    }

    GameObject MakeNewPipe(Vector3Int npoint)
    {
        GameObject newPipe = Instantiate(pipeModel, this.transform);
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

    public int Get1DPositionValue(Vector3Int vPos)
    {
        int xOff = vPos.x;
        int yOff = vPos.y * lenX;
        int zOff = vPos.z * lenX * lenY;
        return cells[xOff + yOff + zOff];
    }
    //Set the position for new Pipe
    public void Set1DPositionValueOccupied(Vector3Int vPos)
    {
        int xOff = vPos.x;
        int yOff = vPos.y * lenX;
        int zOff = vPos.z * lenX * lenY;

        cells[xOff + yOff + zOff] = 1;
        Debug.Log("cells" + (xOff + yOff + zOff));
    }
}

