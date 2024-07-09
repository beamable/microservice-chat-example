using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.Common.Models;
using Beamable.Common.Utils;
using Beamable.Mongo;
using Beamable.Server;
using UnityEngine;

namespace Beamable.Microservices
{
    [Microservice("BackendRoomService")]
    public class BackendRoomService : Microservice
    {
        private const string GeneralRoomName = "General";

        private async Task SendNotification(List<long> playerIds, string context, object payload)
        {
            var jsonPayload = JsonUtility.ToJson(payload);
            Debug.Log($"Sending notification with payload: {jsonPayload}");
            await Services.Notifications.NotifyPlayer(playerIds, context, jsonPayload);
        }

        [ClientCallable]
        public async Promise<Response<string>> GetPlayerAvatarName(long gamerTag)
        {
            try
            {
                var playerData = await Storage.GetByFieldName<PlayerData, long>("gamerTag", gamerTag);
                if (playerData != null && !string.IsNullOrEmpty(playerData.avatarName))
                {
                    return new Response<string>(playerData.avatarName);
                }
                else
                {
                    return new Response<string>("Avatar name not found");
                }
            }
            catch (Exception e)
            {
                BeamableLogger.LogError(e);
                return new Response<string>(e.Message);
            }
        }

        [ClientCallable]
        public async Promise<Response<PlayerData>> GetPlayerDataByUsername(string avatarName)
        {
            try
            {
                var playerData = await Storage.GetByFieldName<PlayerData, string>("avatarName", avatarName);
                if (playerData != null)
                {
                    return new Response<PlayerData>(playerData);
                }
                else
                {
                    return new Response<PlayerData>(null, "User not found");
                }
            }
            catch (Exception e)
            {
                BeamableLogger.LogError(e);
                return new Response<PlayerData>(null, e.Message);
            }
        }

        [ClientCallable]
        public async Promise<Response<bool>> SetPlayerAvatarName(long gamerTag, string avatarName)
        {
            try
            {
                // Check if the avatar name already exists
                var duplicateAvatarNameData = await Storage.GetByFieldName<PlayerData, string>("avatarName", avatarName);
                if (duplicateAvatarNameData != null)
                {
                    return new Response<bool>(false, "Avatar name already exists");
                }

                // Check if the player with the given gamerTag exists
                var existingPlayerData = await Storage.GetByFieldName<PlayerData, long>("gamerTag", gamerTag);
                if (existingPlayerData == null)
                {
                    // Create a new player data record
                    await Storage.Create<BackendStorage, PlayerData>(new PlayerData
                    {
                        gamerTag = gamerTag,
                        avatarName = avatarName
                    });
                }
                else
                {
                    // Update the existing player data with the new avatar name
                    existingPlayerData.avatarName = avatarName;
                    await Storage.Update(existingPlayerData.Id, existingPlayerData);
                }

                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                BeamableLogger.LogError(e);
                return new Response<bool>(false, "Error setting avatar name");
            }
        }

        [ClientCallable]
        public async Promise<Response<bool>> JoinRoom(long gamerTag, string roomName)
        {
            try
            {
                var roomData = await Storage.GetByFieldName<RoomData, string>("roomName", roomName);
                if (roomData == null)
                {
                    roomData = new RoomData { roomName = roomName };
                    roomData.memberGamerTags.Add(gamerTag);
                    await Storage.Create<BackendStorage, RoomData>(roomData);
                }
                else
                {
                    if (roomData.bannedGamerTags.Contains(gamerTag))
                    {
                        return new Response<bool>(false, "User is banned from this room");
                    }

                    if (roomData.memberGamerTags.Contains(gamerTag)) return new Response<bool>(true);
                    roomData.memberGamerTags.Add(gamerTag);
                    await Storage.Update(roomData.Id, roomData);
                }

                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                BeamableLogger.LogError(e);
                return new Response<bool>(false, "Error joining room");
            }
        }

        [ClientCallable]
        public async Promise<Response<List<string>>> GetUserRooms(long gamerTag)
        {
            try
            {
                var allRooms = await Storage.GetAll<RoomData>();
                var userRooms = new List<string>();

                foreach (var roomData in allRooms)
                {
                    Debug.Log(roomData.roomName);
                    if (roomData.memberGamerTags.Contains(gamerTag))
                    {
                        Debug.Log("contains " + gamerTag);
                        userRooms.Add(roomData.roomName);
                    }
                }

                return new Response<List<string>>(userRooms);
            }
            catch (Exception e)
            {
                BeamableLogger.LogError(e);
                return new Response<List<string>>(null, "Error fetching user rooms");
            }
        }

        [ClientCallable]
        public async Promise<Response<bool>> LeaveRoom(long gamerTag, string roomName)
        {
            try
            {
                var roomData = await Storage.GetByFieldName<RoomData, string>("roomName", roomName);
                if (roomData != null && roomData.memberGamerTags.Contains(gamerTag))
                {
                    roomData.memberGamerTags.Remove(gamerTag);
                    if (roomData.memberGamerTags.Count == 0)
                    {
                        await Storage.Delete<BackendStorage, RoomData>(roomData.Id);
                    }
                    else
                    {
                        await Storage.Update(roomData.Id, roomData);
                    }
                }

                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                BeamableLogger.LogError(e);
                return new Response<bool>(false, "Error leaving room");
            }
        }

        [ClientCallable]
        public async Promise<Response<bool>> SendMessage(long gamerTag, string roomName, string message)
        {
            try
            {
                var roomData = await Storage.GetByFieldName<RoomData, string>("roomName", roomName);
                if (roomData == null || !roomData.memberGamerTags.Contains(gamerTag))
                {
                    return new Response<bool>(false, "Room not found or user not a member");
                }

                var messageData = new MessageData
                {
                    senderGamerTag = gamerTag,
                    content = message,
                    timestamp = DateTime.UtcNow
                };

                roomData.messages.Add(messageData);
                await Storage.Update(roomData.Id, roomData);

                await SendNotification(roomData.memberGamerTags, roomName, messageData);

                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                BeamableLogger.LogError(e);
                return new Response<bool>(false, "Error sending message");
            }
        }

        [ClientCallable]
        public async Promise<Response<List<MessageData>>> GetRoomHistory(string roomName)
        {
            try
            {
                var roomData = await Storage.GetByFieldName<RoomData, string>("roomName", roomName);
                if (roomData == null)
                {
                    return new Response<List<MessageData>>(null, "Room not found");
                }

                return new Response<List<MessageData>>(roomData.messages);
            }
            catch (Exception e)
            {
                BeamableLogger.LogError(e);
                return new Response<List<MessageData>>(null, "Error retrieving room history");
            }
        }
        
        [ClientCallable]
        public async Promise<Response<List<PlayerData>>> GetRoomMembers(string roomName)
        {
            try
            {
                var roomData = await Storage.GetByFieldName<RoomData, string>("roomName", roomName);
                if (roomData == null)
                {
                    return new Response<List<PlayerData>>(null, "Room not found");
                }

                var members = new List<PlayerData>();
                foreach (var gamerTag in roomData.memberGamerTags)
                {
                    var playerData = await Storage.GetByFieldName<PlayerData, long>("gamerTag", gamerTag);
                    if (playerData != null)
                    {
                        members.Add(playerData);
                    }
                }

                return new Response<List<PlayerData>>(members);
            }
            catch (Exception e)
            {
                BeamableLogger.LogError(e);
                return new Response<List<PlayerData>>(null, "Error retrieving room members");
            }
        }

        [ClientCallable]
        public async Promise<Response<bool>> KickMember(long gamerTag, string roomName)
        {
            try
            {
                var roomData = await Storage.GetByFieldName<RoomData, string>("roomName", roomName);
                if (roomData != null && roomData.memberGamerTags.Contains(gamerTag))
                {
                    roomData.memberGamerTags.Remove(gamerTag);
                    if (roomData.memberGamerTags.Count == 0)
                    {
                        await Storage.Delete<BackendStorage, RoomData>(roomData.Id);
                    }
                    else
                    {
                        await Storage.Update(roomData.Id, roomData);
                    }

                    return new Response<bool>(true);
                }

                return new Response<bool>(false, "Member not found in the room");
            }
            catch (Exception e)
            {
                BeamableLogger.LogError(e);
                return new Response<bool>(false, "Error kicking member");
            }
        }

        [ClientCallable]
        public async Promise<Response<bool>> BanMember(long gamerTag, string roomName)
        {
            try
            {
                var roomData = await Storage.GetByFieldName<RoomData, string>("roomName", roomName);
                if (roomData == null) return new Response<bool>(false, "Room not found");
                if (!roomData.bannedGamerTags.Contains(gamerTag))
                {
                    roomData.bannedGamerTags.Add(gamerTag);
                }

                await Storage.Update(roomData.Id, roomData);
                return new Response<bool>(true);

            }
            catch (Exception e)
            {
                BeamableLogger.LogError(e);
                return new Response<bool>(false, "Error banning member");
            }
        }

        [ClientCallable]
        public async Promise<Response<bool>> UnbanMember(long gamerTag, string roomName)
        {
            try
            {
                var roomData = await Storage.GetByFieldName<RoomData, string>("roomName", roomName);
                if (roomData == null || !roomData.bannedGamerTags.Contains(gamerTag))
                    return new Response<bool>(false, "Member not found in the banned list");
                roomData.bannedGamerTags.Remove(gamerTag);
                await Storage.Update(roomData.Id, roomData);
                return new Response<bool>(true);

            }
            catch (Exception e)
            {
                BeamableLogger.LogError(e);
                return new Response<bool>(false, "Error unbanning member");
            }
        }
    }
}
