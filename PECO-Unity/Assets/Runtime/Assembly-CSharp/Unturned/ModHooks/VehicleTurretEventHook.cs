using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to Vehicle Turret_# GameObject to receive events.
	/// </summary>
	[AddComponentMenu("Unturned/Vehicle Turret Event Hook")]
	public class VehicleTurretEventHook : MonoBehaviour
	{
		/// <summary>
		/// Invoked when turret gun is fired.
		/// </summary>
		public UnityEvent OnShotFired;
		
		/// <summary>
		/// Invoked when turret gun begins reload sequence.
		/// </summary>
		public UnityEvent OnReloadingStarted;

		/// <summary>
		/// Invoked when turret gun begins hammer sequence.
		/// </summary>
		public UnityEvent OnChamberingStarted;

		/// <summary>
		/// Invoked when turret gun begins aiming.
		/// </summary>
		public UnityEvent OnAimingStarted;

		/// <summary>
		/// Invoked when turret gun ends aiming.
		/// </summary>
		public UnityEvent OnAimingStopped;
	}
}
