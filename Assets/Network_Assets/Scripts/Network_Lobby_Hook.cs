using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class Network_Lobby_Hook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        Setup_Local_Player local_player = gamePlayer.GetComponent<Setup_Local_Player>();

        local_player.Player_Name = lobby.playerName;

    }
}
