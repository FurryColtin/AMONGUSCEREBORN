using InnerNet;

public class MeetingRoomManager : IDisconnectHandler
{
	public static readonly MeetingRoomManager Instance = new MeetingRoomManager();

	private PlayerControl reporter;

	private GameData.PlayerInfo target;

	public void AssignSelf(PlayerControl reporter, GameData.PlayerInfo target)
	{
		this.reporter = reporter;
		this.target = target;
		AmongUsClient.Instance.DisconnectHandlers.AddUnique(this);
	}

	public void RemoveSelf()
	{
		AmongUsClient.Instance.DisconnectHandlers.Remove(this);
	}

	public void HandleDisconnect(PlayerControl pc, DisconnectReasons reason)
	{
		if (AmongUsClient.Instance.AmHost)
		{
			reporter.CmdReportDeadBody(target);
		}
	}

	public void HandleDisconnect()
	{
		HandleDisconnect(null, DisconnectReasons.ExitGame);
	}
}
