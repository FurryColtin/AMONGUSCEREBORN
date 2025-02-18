using UnityEngine;
using MoonSharp.Interpreter;

public class DeadBody : MonoBehaviour
{
	public bool Reported;

	public short KillIdx;

	public byte ParentId;

	public Collider2D myCollider;

	public SpriteRenderer rend;

	public CE_RainbowColorLol rgb;

	private float colmul;
	private float coldec;
	public Vector2 TruePosition => base.transform.position + (Vector3)myCollider.offset;

	public void Start()
    {
        rend = transform.gameObject.GetComponent<SpriteRenderer>();
		coldec = 10000f * GameOptionsData.BodyDecayMul[PlayerControl.GameOptions.BodyDecayTime];
		rgb = transform.gameObject.GetComponent<CE_RainbowColorLol>();
		if (PlayerControl.GameOptions.BodyEffect == 2)
        {
			if (transform.gameObject.GetComponent<CE_RainbowColorLol>() != null)
			{
				transform.gameObject.GetComponent<CE_RainbowColorLol>().Cease();
			}
            transform.gameObject.GetComponent<SpriteRenderer>().material.SetColor("_BackColor", Color.black);
			transform.gameObject.GetComponent<SpriteRenderer>().material.SetColor("_BodyColor", Color.black);
		}
	}

	public void OnClick()
	{
		if (!Reported && !PhysicsHelpers.AnythingBetween(PlayerControl.LocalPlayer.GetTruePosition(), TruePosition, Constants.ShipAndObjectsMask, useTriggers: false))
		{
			Reported = true;
			GameData.PlayerInfo target = PlayerControl.LocalPlayer.Data;
            if (PlayerControl.GameOptions.BodyEffect != 1 && PlayerControl.GameOptions.BodyEffect != 2)
            {
                target = GameData.Instance.GetPlayerById(ParentId);
            }
			if (CE_LuaLoader.CurrentGMLua)
			{
				DynValue dyn = CE_LuaLoader.GetGamemodeResult("CanCallMeeting", (CE_PlayerInfoLua)PlayerControl.LocalPlayer, true);
				if (!dyn.Boolean)
				{
					return;
				}
			}
			PlayerControl.LocalPlayer.CmdReportDeadBody(target);
		}
	}

	public void FixedUpdate()
    {
		if (PlayerControl.GameOptions.BodyEffect == 1)
		{
			colmul += (Time.fixedDeltaTime / coldec);
			if (rgb != null)
            {
				rgb.Cease();
				rgb = null;
            }
			transform.gameObject.GetComponent<SpriteRenderer>().material.SetColor("_BackColor", Color.Lerp(rend.material.GetColor("_BackColor"), Palette.Brown, colmul));
			transform.gameObject.GetComponent<SpriteRenderer>().material.SetColor("_BodyColor", Color.Lerp(rend.material.GetColor("_BodyColor"), Palette.Brown, colmul));
		}
	}
}
