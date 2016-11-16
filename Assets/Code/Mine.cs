using UnityEngine;

/// <summary>
/// Mine that explodes when the player rides over it
/// </summary>
public class Mine : MonoBehaviour
{
    /// <summary>
    /// Prefab for the explosion animation + force controller
    /// Fill in within editor
    /// </summary>
    public GameObject ExplosionPrefab;

    /// <summary>
    /// Go boom and respawn
    /// </summary>
    /// <param name="other">Object that entered the area</param>
    internal void OnTriggerEnter2D(Collider2D other)
    {
        GetComponent<AudioSource>().Play();
		GameObject exp = Instantiate (ExplosionPrefab);
		exp.transform.position = transform.position;
		exp.transform.rotation = Quaternion.identity;
        ScoreManager.IncreaseScore(other.gameObject, -20);
		transform.position = SpawnController.FindFreeLocation(2f);
    }
}
