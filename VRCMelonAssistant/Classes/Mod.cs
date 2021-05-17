using System;
using System.Collections.Generic;
using VRCMelonAssistant.Pages;

namespace VRCMelonAssistant
{
    public class Mod
    {
        public int _id;
        public string uploaddate;
        public string category;
        public string[] aliases;
        public ModVersion[] versions;
        public Mods.ModListItem ListItem;
        public string installedFilePath;
        public string installedVersion;
        public bool installedInBrokenDir;

        public class ModVersion
        {
            public int _version;
            public string name;
            public string modversion;
            public string modtype;
            public string author;
            public string description;
            public string downloadlink;
            public string sourcelink;
            public string hash;
            public string updatedate;
            public string vrchatversion;
            public string loaderversion;
            public int approvalStatus;

            public bool IsBroken => approvalStatus == 2;
            public bool IsPlugin => modtype.Equals("plugin", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
