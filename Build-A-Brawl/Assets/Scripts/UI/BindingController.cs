using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class BindingController : MonoBehaviour
{
	public void RebindAction(InputAction action)
	{
		action.PerformInteractiveRebinding()
            .WithControlsExcluding("<Mouse>/position")
            .WithControlsExcluding("<Mouse>/delta")
            .WithControlsExcluding("<Gamepad>/Start")
            .WithControlsExcluding("<Keyboard>/escape")
            .OnComplete(op => OnRebindComplete(op))
            .Start();
	}

	private void OnRebindComplete(InputActionRebindingExtensions.RebindingOperation op)
	{
		op.Dispose();
	}
}

[CreateAssetMenu(fileName = "DeviceDisplayConfiguration", menuName = "Device/Device Display Configuration")]
public class DeviceDisplayConfiguration : ScriptableObject
{
	[System.Serializable]
	public class DeviceSet
    {
		public string[] rawDeviceNames;
		public DeviceDisplaySettings displaySettings;

		public bool MatchDevice(string deviceName)
        {
			foreach (string device in rawDeviceNames)
            {
				if (deviceName == device)
					return true;
            }

			return false;
        }
	}

	public DeviceSet deviceSets;
}

[CreateAssetMenu(fileName = "DeviceDisplaySetting", menuName = "Device/Device Display Settings")]
public class DeviceDisplaySettings : ScriptableObject
{
	[Header("Display Name")]
	public string deviceDisplayName;

	[Header("Icon - Action Buttons")]
	public Image buttonNorthIcon;
	public Image buttonSouthIcon;
	public Image buttonWestIcon;
	public Image buttonEastIcon;

	public Image buttonLeftShoulderIcon;
	public Image buttonRightShoulderIcon;

	public Image buttonLeftStickIcon;
	public Image buttonRightStickIcon;

	public Image buttonStartIcon;
	public Image buttonSelectIcon;

	[Header("Icon - Action Triggers")]
	public Image triggerLeftTrigger;
	public Image triggerRightTrigger;

	public Image triggerLeftStick;
	public Image triggerRightStick;

	[Header("Icons - Custom Contexts")]
	public Context[] contexts;

	[System.Serializable]
	public class Context
    {
		public string name;
		public Image icon;
    }

	public bool TryGetContext(string name, out Image icon)
    {
		foreach (Context ctx in contexts)
        {
			if (ctx.name == name)
            {
				icon = ctx.icon;
				return true;
            }
        }

		icon = null;
		return false;
    }
}