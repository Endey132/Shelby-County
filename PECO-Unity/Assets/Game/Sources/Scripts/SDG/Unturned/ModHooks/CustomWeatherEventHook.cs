using UnityEngine;
using UnityEngine.Events;

namespace SDG.Unturned
{
	/// <summary>
	/// Implement to receive callbacks for a specific custom weather asset.
	/// Preferable over delegates in this case because the asset filtering can be done by the sender.
	/// </summary>
	public interface IWeatherEventListener
	{
		/// <summary>
		/// Callback when custom weather is activated, or immediately if weather is fading in when listener is registered.
		/// </summary>
		void HandleWeatherBeginTransitionIn();

		/// <summary>
		/// Callback when custom weather finishes fading in, or immediately if weather is already
		/// fully active when listener is registered.
		/// </summary>
		void HandleWeatherEndTransitionIn();

		/// <summary>
		/// Callback when custom weather is deactivated and begins fading out.
		/// </summary>
		void HandleWeatherBeginTransitionOut();

		/// <summary>
		/// Callback when custom weather finishes fading out and is destroyed.
		/// </summary>
		void HandleWeatherEndTransitionOut();
	}

	/// <summary>
	/// Can be added to any GameObject to receive weather events for a specific custom weather asset.
	/// </summary>
	[AddComponentMenu("Unturned/Custom Weather Event Hook")]
	public class CustomWeatherEventHook : MonoBehaviour, IWeatherEventListener
	{
		/// <summary>
		/// GUID of custom weather asset to listen for.
		/// </summary>
		public string WeatherAssetGuid;

		/// <summary>
		/// Invoked when custom weather is activated, or immediately if weather is fading in when registered.
		/// </summary>
		public UnityEvent OnWeatherBeginTransitionIn;

		/// <summary>
		/// Invoked when custom weather finishes fading in, or immediately if weather is already fully active when registered.
		/// </summary>
		public UnityEvent OnWeatherEndTransitionIn;

		/// <summary>
		/// Invoked when custom weather is deactivated and begins fading out.
		/// </summary>
		public UnityEvent OnWeatherBeginTransitionOut;

		/// <summary>
		/// Invoked when custom weather finishes fading out and is destroyed.
		/// </summary>
		public UnityEvent OnWeatherEndTransitionOut;

		protected void OnEnable()
		{
#if GAME
			if(System.Guid.TryParse(WeatherAssetGuid, out parsedGuid))
			{
				WeatherEventListenerManager.addListener(parsedGuid, this);
			}
			else
			{
				parsedGuid = System.Guid.Empty;
				UnturnedLog.warn("{0} unable to parse weather asset guid", transform.GetSceneHierarchyPath());
			}
#endif
		}

		protected void OnDisable()
		{
#if GAME
			if(!parsedGuid.Equals(System.Guid.Empty))
			{
				WeatherEventListenerManager.removeListener(parsedGuid, this);
			}
#endif
		}

		public void HandleWeatherBeginTransitionIn()
		{
			OnWeatherBeginTransitionIn.Invoke();
		}

		public void HandleWeatherEndTransitionIn()
		{
			OnWeatherEndTransitionIn.Invoke();
		}

		public void HandleWeatherBeginTransitionOut()
		{
			OnWeatherBeginTransitionOut.Invoke();
		}

		public void HandleWeatherEndTransitionOut()
		{
			OnWeatherEndTransitionOut.Invoke();
		}

		/// <summary>
		/// GUID parsed from WeatherAssetGuid parameter.
		/// </summary>
		protected System.Guid parsedGuid;
	}
}
