using UnityEngine;

public class spawnPoint : MonoBehaviour
{
    
    public bool canSpawn;
    private void OnDrawGizmos() {

        if(canSpawn)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawCube(transform.position, Vector3.one * 5);
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject.CompareTag(Tags.water) || other.gameObject.layer.Equals(LayerMask.NameToLayer(Tags.building)) || other.gameObject.CompareTag(Tags.unit))
        {
            canSpawn = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.CompareTag(Tags.water) || other.gameObject.layer.Equals(LayerMask.NameToLayer(Tags.building)) || other.gameObject.CompareTag(Tags.unit))
        {
            canSpawn = true;
        }
    }



}
