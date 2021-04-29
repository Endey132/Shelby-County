using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to Vehicle GameObject to receive events.
	/// </summary>
	[AddComponentMenu("Unturned/Vehicle Event Hook")]
	public class VehicleEventHook : MonoBehaviour
	{
		/// <summary>
		/// Invoked when a player enters the driver seat.
		/// </summary>
		public UnityEvent OnDriverAdded;
		
		/// <summary>
		/// Invoked when a player exits the driver seat.
		/// </summary>
		public UnityEvent OnDriverRemoved;
	}
}
