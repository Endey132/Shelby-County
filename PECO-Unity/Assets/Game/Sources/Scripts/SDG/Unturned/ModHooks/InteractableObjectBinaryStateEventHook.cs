using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	/// <summary>
	/// Can be added to any GameObject with an interactable binary state in its parents.
	///
	/// If players should not be allowed to interact with the object in the ordinary manner,
	/// add the Interactability_Remote flag to its asset to indicate only mod hooks should control it.
	/// </summary>
	[AddComponentMenu("Unturned/IOBS Event Hook")]
	public class InteractableObjectBinaryStateEventHook : MonoBehaviour
	{
		/// <summary>
		/// Invoked when interactable object enters the Used / On / Enabled state.
		/// </summary>
		public UnityEvent OnStateEnabled;

		/// <summary>
		/// Invoked when interactable object enters the Unused / Off / Disabled state.
		/// </summary>
		public UnityEvent OnStateDisabled;

		/// <summary>
		/// Set state to Enabled if currently Disabled.
		/// 
		/// On dedicated server this directly changes the state,
		/// but as client this will apply the usual conditions and rewards.
		/// </summary>
		public void GotoEnabledState()
		{
#if GAME
			if(interactable != null)
			{
				interactable.setUsedFromClientOrServer(true);
			}
#endif // GAME
		}

		/// <summary>
		/// Set state to Disabled if currently Enabled.
		///
		/// On dedicated server this directly changes the state,
		/// but as client this will apply the usual conditions and rewards.
		/// </summary>
		public void GotoDisabledState()
		{
#if GAME
			if(interactable != null)
			{
				interactable.setUsedFromClientOrServer(false);
			}
#endif // GAME
		}

		/// <summary>
		/// Toggle between the Enabled and Disabled states.
		///
		/// On dedicated server this directly changes the state,
		/// but as client this will apply the usual conditions and rewards. 
		/// </summary>
		public void ToggleState()
		{
#if GAME
			if(interactable != null)
			{
				interactable.setUsedFromClientOrServer(!interactable.isUsed);
			}
#endif // GAME
		}

#if GAME
		protected void OnEnable()
		{
			interactable = gameObject.GetComponentInParent<InteractableObjectBinaryState>();
			if(interactable == null)
			{
				UnturnedLog.warn("{0} unable to find interactable", name);
			}
			else
			{
				interactable.onUsedChanged += onUsedChanged;
				++interactable.modHookCounter;
				onUsedChanged(interactable.isUsed);
			}
		}

		protected void OnDisable()
		{
			if(interactable != null)
			{
				interactable.onUsedChanged -= onUsedChanged;
				--interactable.modHookCounter;
				interactable = null;
			}
		}

		protected void onUsedChanged(bool isUsed)
		{
			if(isUsed)
			{
				OnStateEnabled.Invoke();
			}
			else
			{
				OnStateDisabled.Invoke();
			}
		}

		private InteractableObjectBinaryState interactable = null;
#endif // GAME
	}
}
