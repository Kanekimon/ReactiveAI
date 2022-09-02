using UnityEngine;

public class Sapling : MonoBehaviour
{
    public GameObject TreePrefab;
    public float GrowthTime;
    public float Timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Timer > GrowthTime)
        {
            Vector3 position = this.transform.position;
            position.y = Terrain.activeTerrain.SampleHeight(position);
            Instantiate(TreePrefab).transform.position = position;
            Destroy(this.gameObject);
        }
        else
        {
            Timer += Time.deltaTime;
        }
    }
}
