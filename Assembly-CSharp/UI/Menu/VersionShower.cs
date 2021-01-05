using UnityEngine;

public class VersionShower : MonoBehaviour
{
	public TextRenderer text;

	public void Start()
	{
		text.Text = "v0.4.5 - Lua API Beta + Crashfix";
		Screen.sleepTimeout = -1;
        CE_Extensions.OnStartup();
	}

	public void Update()
    {
		CE_Extensions.UpdateWindowTitle();
	}

}
