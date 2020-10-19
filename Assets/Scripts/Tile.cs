using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class Tile : MonoBehaviour
{

    [SerializeField] float hitsToDestroy = 10f;

    [SerializeField] float gridSize = 10f;
    // Start is called before the first frame update

    private void Update()
    {
        var newPos = new Vector3(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y), Mathf.FloorToInt(transform.position.z));
        transform.position = newPos;
    }

    public void Hit()
    {
        hitsToDestroy--;
        Debug.Log("I am hit");
        if(hitsToDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }
}
