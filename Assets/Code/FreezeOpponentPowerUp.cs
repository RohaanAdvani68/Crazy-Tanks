using UnityEngine;

/// <summary>
/// A simple powerup that freezes the opponent in his current position for 5 seconds, but opponent can use a power up
/// </summary>
public class FreezeOpponentPowerUp : PowerUpController {

	public TankControl opponent;
	public float FreezeTime=5f;

	public override void Fire()
	{
		TankControl[] tanks = FindObjectsOfType<TankControl> ();
		foreach (TankControl tank in tanks) {	//to find opponent
			if (tank.transform.position != transform.parent.position) {
				opponent = tank;
				break;
			}
		}
		opponent.isFrozen = true;
		Invoke ("Unfreeze", FreezeTime);

		PowerUpDone();
	}

	public void Unfreeze()
	{
		opponent.isFrozen = false;
	}
}


