﻿using System;
using UnityEngine;
using System.Collections.Generic;


//why is this here?

public enum CE_Specials
{
	Kill,
	Sabotage,
	Vent
}

public enum CE_RoleVisibility
{ 
	None,
	Impostors,
	Crewmates, //why you would ever make a role crewmates only is beyond me
	RolesOfSameType,
	Lua,
    All
}

public enum CE_WinWith
{ 
	Impostors,
	Crewmates,
	Neither,
    All
}




//set RoleText to "default_text" to use the default "There are _ impostors among us" text.

public static class CE_RoleManager
{
    public static Dictionary<byte, CE_Role> Roles = new Dictionary<byte, CE_Role>();


    public static bool AddRole(CE_Role roletoadd)
    {
        UnityEngine.Debug.Log("Adding role with name:" + roletoadd.RoleName + ", id:" + Roles.Count);
        return Roles.TryAdd((byte)Roles.Count, roletoadd);
    }


    public static byte GetRoleFromName(string Name)
    {
        foreach (KeyValuePair<byte, CE_Role> kvp in Roles)
        {
            if (kvp.Value.RoleName == Name)
            {
                return kvp.Key;
            }
        }
        return 0; //return the null role, AKA assign no role.
    }

    public static byte GetRoleFromUUID(string Name)
    {
        foreach (KeyValuePair<byte, CE_Role> kvp in Roles)
        {
            if (kvp.Value.UUID == Name)
            {
                return kvp.Key;
            }
        }
        return 0; //return the null role, AKA assign no role.
    }

    public static CE_Role GetActualRoleFromUUID(string Name)
    {
        foreach (KeyValuePair<byte, CE_Role> kvp in Roles)
        {
            if (kvp.Value.UUID == Name)
            {
                return kvp.Value;
            }
        }
        return null; //return the null role, AKA assign no role.
    }

    public static CE_Role GetRoleFromID(byte id)
    {
        if (Roles.TryGetValue(id,out CE_Role returnvalue))
        {
            return returnvalue;
        }
        return Roles[0];
    }

    public static int GetRoleCount(byte id)
    {
        int count = 0;
        foreach (GameData.PlayerInfo plyfo in GameData.Instance.AllPlayers)
        {
            if (plyfo.role == id)
            {
                count++;
            }
        }
        return count;
    }

}


public class CE_Role
{
	public CE_Role() // Don't actually use this constructor for anything besides creating the "None" role please
    {
		RoleName = "Undefined";
		RoleColor = Palette.CrewmateBlue;
		AvailableSpecials = new List<CE_Specials>();
		RoleWinWith = CE_WinWith.All; //all does not mean all but more of "don't fuck with whether or not you win please"
		RoleVisibility = CE_RoleVisibility.None;
                UseImpVision = false;
                RoleText = "Undefined";
                HasTasks = true;
                UUID = "undefined_undefined";
                Layer = 0;
    }

    public bool CanDo(CE_Specials special)
    {
        return AvailableSpecials.Contains(special);
    }

    public bool DoesNotDoTasks()
    {
        return !HasTasks;
    }

    public bool CanSee(GameData.PlayerInfo plf) //is optimized bruh
    {
        if (plf.IsDead && PlayerControl.GameOptions.GhostsSeeRoles)
        {
            return true;
        }
        switch (RoleVisibility)
        {
            case CE_RoleVisibility.All:
                return true;
            case CE_RoleVisibility.None:
                return false;
            case CE_RoleVisibility.Crewmates:
                return !plf.IsImpostor;
            case CE_RoleVisibility.Impostors:
                return plf.IsImpostor;
            case CE_RoleVisibility.RolesOfSameType:
                return plf.role == CE_RoleManager.GetRoleFromUUID(UUID);
            case CE_RoleVisibility.Lua:
                return CE_LuaLoader.GetGamemodeResult("ShouldSeeRole", RoleName, new CE_PlayerInfoLua(plf)).Boolean;
        }
        return false;
    }
    public CE_Role(string Name, Color Color)
    {
        RoleName = Name;
        RoleColor = Color;
        AvailableSpecials = new List<CE_Specials>();
        RoleWinWith = CE_WinWith.Neither;
        RoleVisibility = CE_RoleVisibility.None;
        UseImpVision = false;
        RoleText = "default_text";
        HasTasks = false;
        UUID = CE_LuaLoader.CurrentGMName + "_" + Name;
    }

    public CE_Role(string Name, Color Color, string RoleTxT)
    {
        RoleName = Name;
        RoleColor = Color;
        AvailableSpecials = new List<CE_Specials>();
        RoleWinWith = CE_WinWith.Neither;
        RoleVisibility = CE_RoleVisibility.None;
        UseImpVision = false;
        RoleText = RoleTxT;
        HasTasks = false;
        UUID = CE_LuaLoader.CurrentGMName + "_" + Name;
    }

    public CE_Role(string Name, Color Color, string RoleTxt, List<CE_Specials> Specials, CE_WinWith WinWith = CE_WinWith.Neither, CE_RoleVisibility Visibility = CE_RoleVisibility.None, bool ImpVision = false, bool dotask = true, byte layer = 0, bool canseeimp = false, string fakestring = "")
    {
        RoleName = Name;
        RoleColor = Color;
        AvailableSpecials = Specials;
        RoleWinWith = WinWith;
        RoleVisibility = Visibility;
        UseImpVision = ImpVision;
        RoleText = RoleTxt;
        HasTasks = dotask;
        UUID = CE_LuaLoader.CurrentGMName + "_" + Name;
        Layer = layer;
        CanSeeImps = canseeimp;
        FakeTaskString = fakestring;
    }
	
    public CE_Role() // Sorry ben gotta do this. I NEED TO ADD SOME MORE ROLES AAAAAAAAAAAAAAAAAAAAAAAAAAAAA (CUSTOM ROLE 1)
    {
		RoleName = "Celebrity";
		RoleColor = Palette.CrewmateBlue;
		AvailableSpecials = new List<CE_Specials>();
		RoleWinWith = CE_WinWith.All; //hi
		RoleVisibility = CE_RoleVisibility.None;
                UseImpVision = false;
                RoleText = "Your a celebrity you cant get voted.";
                HasTasks = true;
                UUID = "mynewroles_celebrity";
                Layer = 0;
    }
    public string RoleName;

    public string UUID;

	public Color RoleColor;

    public bool HasTasks;

    public byte Layer;

	public List<CE_Specials> AvailableSpecials;

	public CE_RoleVisibility RoleVisibility;

	public CE_WinWith RoleWinWith;

    public string RoleText;

    public string FakeTaskString = "";

	public bool UseImpVision;

    public bool CanSeeImps;
}
