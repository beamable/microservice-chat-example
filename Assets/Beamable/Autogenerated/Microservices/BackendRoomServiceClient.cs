//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Beamable.Server.Clients
{
    using System;
    using Beamable.Platform.SDK;
    using Beamable.Server;
    
    
    /// <summary> A generated client for <see cref="Beamable.Microservices.BackendRoomService"/> </summary
    public sealed class BackendRoomServiceClient : MicroserviceClient, Beamable.Common.IHaveServiceName
    {
        
        public BackendRoomServiceClient(BeamContext context = null) : 
                base(context)
        {
        }
        
        public string ServiceName
        {
            get
            {
                return "BackendRoomService";
            }
        }
        
        /// <summary>
        /// Call the GetPlayerAvatarName method on the BackendRoomService microservice
        /// <see cref="Beamable.Microservices.BackendRoomService.GetPlayerAvatarName"/>
        /// </summary>
        public Beamable.Common.Promise<Beamable.Common.Utils.Response<string>> GetPlayerAvatarName(long gamerTag)
        {
            object raw_gamerTag = gamerTag;
            System.Collections.Generic.Dictionary<string, object> serializedFields = new System.Collections.Generic.Dictionary<string, object>();
            serializedFields.Add("gamerTag", raw_gamerTag);
            return this.Request<Beamable.Common.Utils.Response<string>>("BackendRoomService", "GetPlayerAvatarName", serializedFields);
        }
        
        /// <summary>
        /// Call the GetPlayerDataByUsername method on the BackendRoomService microservice
        /// <see cref="Beamable.Microservices.BackendRoomService.GetPlayerDataByUsername"/>
        /// </summary>
        public Beamable.Common.Promise<Beamable.Common.Utils.Response<Beamable.Common.Models.PlayerData>> GetPlayerDataByUsername(string avatarName)
        {
            object raw_avatarName = avatarName;
            System.Collections.Generic.Dictionary<string, object> serializedFields = new System.Collections.Generic.Dictionary<string, object>();
            serializedFields.Add("avatarName", raw_avatarName);
            return this.Request<Beamable.Common.Utils.Response<Beamable.Common.Models.PlayerData>>("BackendRoomService", "GetPlayerDataByUsername", serializedFields);
        }
        
        /// <summary>
        /// Call the SetPlayerAvatarName method on the BackendRoomService microservice
        /// <see cref="Beamable.Microservices.BackendRoomService.SetPlayerAvatarName"/>
        /// </summary>
        public Beamable.Common.Promise<Beamable.Common.Utils.Response<bool>> SetPlayerAvatarName(long gamerTag, string avatarName)
        {
            object raw_gamerTag = gamerTag;
            object raw_avatarName = avatarName;
            System.Collections.Generic.Dictionary<string, object> serializedFields = new System.Collections.Generic.Dictionary<string, object>();
            serializedFields.Add("gamerTag", raw_gamerTag);
            serializedFields.Add("avatarName", raw_avatarName);
            return this.Request<Beamable.Common.Utils.Response<bool>>("BackendRoomService", "SetPlayerAvatarName", serializedFields);
        }
        
        /// <summary>
        /// Call the JoinRoom method on the BackendRoomService microservice
        /// <see cref="Beamable.Microservices.BackendRoomService.JoinRoom"/>
        /// </summary>
        public Beamable.Common.Promise<Beamable.Common.Utils.Response<bool>> JoinRoom(long gamerTag, string roomName)
        {
            object raw_gamerTag = gamerTag;
            object raw_roomName = roomName;
            System.Collections.Generic.Dictionary<string, object> serializedFields = new System.Collections.Generic.Dictionary<string, object>();
            serializedFields.Add("gamerTag", raw_gamerTag);
            serializedFields.Add("roomName", raw_roomName);
            return this.Request<Beamable.Common.Utils.Response<bool>>("BackendRoomService", "JoinRoom", serializedFields);
        }
        
        /// <summary>
        /// Call the GetUserRooms method on the BackendRoomService microservice
        /// <see cref="Beamable.Microservices.BackendRoomService.GetUserRooms"/>
        /// </summary>
        public Beamable.Common.Promise<Beamable.Common.Utils.Response<System.Collections.Generic.List<string>>> GetUserRooms(long gamerTag)
        {
            object raw_gamerTag = gamerTag;
            System.Collections.Generic.Dictionary<string, object> serializedFields = new System.Collections.Generic.Dictionary<string, object>();
            serializedFields.Add("gamerTag", raw_gamerTag);
            return this.Request<Beamable.Common.Utils.Response<System.Collections.Generic.List<string>>>("BackendRoomService", "GetUserRooms", serializedFields);
        }
        
        /// <summary>
        /// Call the LeaveRoom method on the BackendRoomService microservice
        /// <see cref="Beamable.Microservices.BackendRoomService.LeaveRoom"/>
        /// </summary>
        public Beamable.Common.Promise<Beamable.Common.Utils.Response<bool>> LeaveRoom(long gamerTag, string roomName)
        {
            object raw_gamerTag = gamerTag;
            object raw_roomName = roomName;
            System.Collections.Generic.Dictionary<string, object> serializedFields = new System.Collections.Generic.Dictionary<string, object>();
            serializedFields.Add("gamerTag", raw_gamerTag);
            serializedFields.Add("roomName", raw_roomName);
            return this.Request<Beamable.Common.Utils.Response<bool>>("BackendRoomService", "LeaveRoom", serializedFields);
        }
        
        /// <summary>
        /// Call the SendMessage method on the BackendRoomService microservice
        /// <see cref="Beamable.Microservices.BackendRoomService.SendMessage"/>
        /// </summary>
        public Beamable.Common.Promise<Beamable.Common.Utils.Response<bool>> SendMessage(long gamerTag, string roomName, string message)
        {
            object raw_gamerTag = gamerTag;
            object raw_roomName = roomName;
            object raw_message = message;
            System.Collections.Generic.Dictionary<string, object> serializedFields = new System.Collections.Generic.Dictionary<string, object>();
            serializedFields.Add("gamerTag", raw_gamerTag);
            serializedFields.Add("roomName", raw_roomName);
            serializedFields.Add("message", raw_message);
            return this.Request<Beamable.Common.Utils.Response<bool>>("BackendRoomService", "SendMessage", serializedFields);
        }
        
        /// <summary>
        /// Call the GetRoomHistory method on the BackendRoomService microservice
        /// <see cref="Beamable.Microservices.BackendRoomService.GetRoomHistory"/>
        /// </summary>
        public Beamable.Common.Promise<Beamable.Common.Utils.Response<System.Collections.Generic.List<Beamable.Common.Models.MessageData>>> GetRoomHistory(string roomName)
        {
            object raw_roomName = roomName;
            System.Collections.Generic.Dictionary<string, object> serializedFields = new System.Collections.Generic.Dictionary<string, object>();
            serializedFields.Add("roomName", raw_roomName);
            return this.Request<Beamable.Common.Utils.Response<System.Collections.Generic.List<Beamable.Common.Models.MessageData>>>("BackendRoomService", "GetRoomHistory", serializedFields);
        }
        
        /// <summary>
        /// Call the GetRoomMembers method on the BackendRoomService microservice
        /// <see cref="Beamable.Microservices.BackendRoomService.GetRoomMembers"/>
        /// </summary>
        public Beamable.Common.Promise<Beamable.Common.Utils.Response<System.Collections.Generic.List<Beamable.Common.Models.PlayerData>>> GetRoomMembers(string roomName)
        {
            object raw_roomName = roomName;
            System.Collections.Generic.Dictionary<string, object> serializedFields = new System.Collections.Generic.Dictionary<string, object>();
            serializedFields.Add("roomName", raw_roomName);
            return this.Request<Beamable.Common.Utils.Response<System.Collections.Generic.List<Beamable.Common.Models.PlayerData>>>("BackendRoomService", "GetRoomMembers", serializedFields);
        }
        
        /// <summary>
        /// Call the KickMember method on the BackendRoomService microservice
        /// <see cref="Beamable.Microservices.BackendRoomService.KickMember"/>
        /// </summary>
        public Beamable.Common.Promise<Beamable.Common.Utils.Response<bool>> KickMember(long gamerTag, string roomName)
        {
            object raw_gamerTag = gamerTag;
            object raw_roomName = roomName;
            System.Collections.Generic.Dictionary<string, object> serializedFields = new System.Collections.Generic.Dictionary<string, object>();
            serializedFields.Add("gamerTag", raw_gamerTag);
            serializedFields.Add("roomName", raw_roomName);
            return this.Request<Beamable.Common.Utils.Response<bool>>("BackendRoomService", "KickMember", serializedFields);
        }
    }
    
    internal sealed class MicroserviceParametersBackendRoomServiceClient
    {
        
        [System.SerializableAttribute()]
        internal sealed class ParameterSystem_Int64 : MicroserviceClientDataWrapper<long>
        {
        }
        
        [System.SerializableAttribute()]
        internal sealed class ParameterSystem_String : MicroserviceClientDataWrapper<string>
        {
        }
    }
    
    [BeamContextSystemAttribute()]
    public static class ExtensionsForBackendRoomServiceClient
    {
        
        [Beamable.Common.Dependencies.RegisterBeamableDependenciesAttribute()]
        public static void RegisterService(Beamable.Common.Dependencies.IDependencyBuilder builder)
        {
            builder.AddScoped<BackendRoomServiceClient>();
        }
        
        public static BackendRoomServiceClient BackendRoomService(this Beamable.Server.MicroserviceClients clients)
        {
            return clients.GetClient<BackendRoomServiceClient>();
        }
    }
}
