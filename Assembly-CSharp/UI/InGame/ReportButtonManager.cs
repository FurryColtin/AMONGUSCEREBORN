using UnityEngine;
using MoonSharp.Interpreter;

public class ReportButtonManager : MonoBehaviour
{
	public SpriteRenderer renderer;

	private bool KeybindClick;

	public void SetActive(bool isActive)
	{
		if (isActive)
		{
			renderer.color = Palette.EnabledColor;
			renderer.material.SetFloat("_Desat", 0f);
		}
		else
		{
			renderer.color = Palette.DisabledColor;
			renderer.material.SetFloat("_Desat", 1f);
		}
	}

	public void DoClick()
	{
		if ((KeybindClick || !SaveManager.EnableProHUDMode) && base.isActiveAndEnabled)
		{
            if (CE_LuaLoader.CurrentGMLua)
            {
                DynValue dyn = CE_LuaLoader.GetGamemodeResult("CanCallMeeting", (CE_PlayerInfoLua)PlayerControl.LocalPlayer, true);
                if (!dyn.Boolean)
                {
                    return;
                }
            }
			PlayerControl.LocalPlayer.ReportClosest();
		}
	}

	public void LateUpdate()
	{
		if (SaveManager.EnableProHUDMode && renderer.color == Palette.DisabledColor)
		{
			renderer.enabled = false;
		}
		else
		{
			renderer.enabled = true;
		}
	}

	public void DoKeyClick()
	{
		KeybindClick = true;
		DoClick();
		KeybindClick = false;
	}
}
