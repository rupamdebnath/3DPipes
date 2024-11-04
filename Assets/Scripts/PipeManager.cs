using UnityEngine;

public class PipeManager : MonoBehaviour
{
    public float settings;

    public GameObject pipeModel;
    //PlacePipe function to be called for instantiating a model
    public void PlacePipe(Vector3Int npoint)
    {
        Vector3 pos = new Vector3(
        settings * (npoint.x - 2),
        settings * (npoint.y - 4),
        settings * (npoint.z - 6)
        );

        GameObject newPipe = Instantiate(pipeModel, this.transform);
        newPipe.transform.localPosition = pos;
    }
}

