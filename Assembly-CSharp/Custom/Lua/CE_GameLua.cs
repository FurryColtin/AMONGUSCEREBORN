﻿using System.Collections.Generic;
using System.IO;
using MoonSharp.Interpreter;
using UnityEngine;
using System.Linq;
using System;
using Hazel;


static class CE_GameLua
{
    public static void ActivateWin()
    {
        throw new NotImplementedException(); // TODO: implement this
    }


    public static bool UpdatePlayerInfo(DynValue dynval)
    {
        CE_PlayerInfoLua influa = (CE_PlayerInfoLua)dynval.UserData.Object;
        Debug.Log("Attempting Data Change for:" + influa.PlayerName);
        influa.refplayer.Object.RpcSetColor(influa.ColorId);
        influa.refplayer.Object.RpcSetSkin(influa.SkinId);
        influa.refplayer.Object.RpcSetHat(influa.HatId);
        influa.refplayer.Object.RpcSetName(influa.PlayerName);
        return true;
    }


    public static bool DebugLogLua(string text)
    {
        Debug.Log(text);
        return true;
    }

    public static bool SendToHostSimple(byte id)
    {
        MessageWriter messageWriter = AmongUsClient.Instance.StartRpc(PlayerControl.LocalPlayer.NetId,15,SendOption.Reliable);
        messageWriter.Write(AmongUsClient.Instance.GameId);
        messageWriter.Write(id);
        messageWriter.EndMessage();
        messageWriter.Recycle();
        return true;
    }
    public static bool ActivateCustomWin(Table plyrs, string song)
    {
        try
        {
            List<GameData.PlayerInfo> playerinfos = new List<GameData.PlayerInfo>();
            foreach (DynValue ply in plyrs.Values)
            {
                CE_PlayerInfoLua infolua = (CE_PlayerInfoLua)ply.UserData.Object;
                playerinfos.Add(infolua.refplayer);
            }
            ShipStatus.RpcCustomEndGamePublic(playerinfos.ToArray(), song);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
        return true;
    }

    public static List<CE_PlayerInfoLua> GetAllPlayers()
    {
        List<CE_PlayerInfoLua> PlayFoLua = new List<CE_PlayerInfoLua>();
        foreach (GameData.PlayerInfo plrfo in GameData.Instance.AllPlayers)
        {
            PlayFoLua.Add(new CE_PlayerInfoLua(plrfo));
        }
        return PlayFoLua;
    }


    public static bool CreateRoleSimple(string Name, Table Color, string RoleText)
    {
        Color rolcolr = new Color((float)Color.Get(1).Number / 255f, (float)Color.Get(2).Number / 255f, (float)Color.Get(3).Number / 255f);
        return CE_RoleManager.AddRole(new CE_Role(Name, rolcolr, RoleText));
    }
    public static bool CreateRoleComplex(string Name, Table Color, string RoleText, List<CE_Specials> Specials, CE_WinWith Win, CE_RoleVisibility Vis, bool ImpVis)
    {
        Color rolcolr = new Color((float)Color.Get(1).Number / 255f, (float)Color.Get(2).Number / 255f, (float)Color.Get(3).Number / 255f);
        return CE_RoleManager.AddRole(new CE_Role(Name, rolcolr, RoleText,Specials,Win,Vis,ImpVis));
    }
}
