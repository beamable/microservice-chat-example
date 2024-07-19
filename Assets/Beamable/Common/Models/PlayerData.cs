using System;
using System.Collections.Generic;
using Beamable.Common.Interfaces;
using Beamable.Server;

namespace Beamable.Common.Models
{
    [Serializable]
    public class PlayerData : StorageDocument, ISetStorageDocument<PlayerData>
    {
        public long gamerTag;
        public string avatarName;
        public List<long> blockedGamerTags = new List<long>();
        public List<long> blockedByGamerTags = new List<long>();

        public void Set(PlayerData document)
        {
            gamerTag = document.gamerTag;
            avatarName = document.avatarName;
            blockedGamerTags = document.blockedGamerTags;
            blockedByGamerTags = document.blockedByGamerTags;
        }
    }
}