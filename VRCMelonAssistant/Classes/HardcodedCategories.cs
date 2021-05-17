using System.Collections.Generic;

namespace VRCMelonAssistant
{
    public static class HardcodedCategories
    {
        private static readonly Dictionary<string, List<string>> CategoryContents = new()
        {
            {"Safety & Security", new() {"Advanced Safety", "Finitizer", "True Shader Anticrash", "Safety-Presets"}},
            {"Core mods and libraries", new() {"UI Expansion Kit", "ActionMenuApi", "VRCModUpdater.Loader"}},
            {"All-in-one mods", new() {"emmVRCLoader"}},
            {"Camera mods", new() {
                "CameraMinus", "DesktopCamera", "BetterSteadycam", "ITR's Melon Cameras", "CameraResChanger",
                "LocalCameraMod", "Lag Free Screenshots"
            }},
            {"Performance & Fidelity", new() {
                "Core Limiter", "MirrorResolutionUnlimiter", "AvatarHider", "Runtime Graphics Settings",
                "GamePriority", "FrameFocus", "ClearVRAM"
            }},
            {"Utilities & Tweaks", new() {
                "ReloadAvatars", "KeyboardPaste", "No Outlines", "UnmuteSound", "SparkleBeGone",
                "BTKSAImmersiveHud", "OGTrustRanks", "ToggleMicIcon", "Friends+ home",
                "MicSensitivity", "CloningBeGone", "ToggleFullScreen", "View Point Tweaker"
            }},
            {"Hardware support", new() {"LeapMotionExtension", "ThumbParams", "VRCFaceTracking"}},
            {"Dynamic bones", new() {
                "ImmersiveTouch", "Dynamic Bones Safety", "MultiplayerDynamicBonesMod", "Multiplayer Dynamic Bones",
            }},
            {"World tweaks", new() {
                "PostProcessing", "NearClipPlaneAdj", "RemoveChairs", "ComponentToggle", "No Grabby Hands", "AOOverride"
            }},
            {"Fixes", new() {"Invite+ fix", "CursorLockFix", "DownloadFix",}},
            {"New features & Overhauls", new() {
                "IKTweaks", "JoinNotifier", "FBT Saver", "BTKSANameplateMod", "AdvancedInvites", "VRCVideoLibrary",
                "BTKSASelfPortrait", "OldMate", "BetterLoadingScreen", "Loading Screen Pictures", "FavCat",
                "ActionMenuUtils", "WorldPredownload", "AskToPortal", "Headlight", "ITR's Player Tracer",
                "InstanceHistory", "PortableMirrorMod", "VRCBonesController"
            }},
            {"UI mods", new() {
                "Particle and DynBone limiter settings UI", "CalibrateConfirm", "Emoji Page Buttons",
                "UserInfoExtensions", "MLConsoleViewer", "OwO Mod", "ActiveBackground", "PlayerList", "ComfyVRMenu",
                "DiscordMute", "MicToggle", "VRCPlusPet"
            }},
            {"Movement", new() {
                "TeleporterVR", "ImmobilizePlayerMod", "TrackingRotator", "OculusPlayspaceMover",
                "ITR's Gravity Controller", "QMFreeze", "Double-Tap Runner", "Player Rotater",
            }},
            {"Very Niche Mods", new() {"HWIDPatch", "No Steam. At all.", "Vertex Animation Remover", "LocalPlayerPrefs", "BTKSAGestureMod",}}
        };

        private static readonly Dictionary<string, string> CategoryDescriptions = new()
        {
            {"Safety & Security", "Crash less, block annoyances"},
            {"Core mods and libraries", "Other mods might require these"},
            {"All-in-one mods", "It does a lot of stuff"},
            {"Camera mods", "For all your screenshot or streaming needs"},
            {"Performance & Fidelity", "Improve performance or make the game look better"},
            {"Utilities & Tweaks", "Small mods that address specific issues"},
            {"Hardware support", "For all exotic hardware out there"},
            {"Dynamic bones", "Mods that affect jiggly bits"},
            {"World tweaks", "Change aspects of the world you're in"},
            {"Fixes", "It's not a bug, it's a feature"},
            {"New features & Overhauls", "Mods that introduce new features or significantly change existing ones"},
            {"UI mods", "Modify the user interface or introduce new functionality to it"},
            {"Movement", "Move in new exciting ways"},
            {"Very Niche Mods", "Only use these if you're really sure you need them"}
        };

        private static readonly Dictionary<string, string> ModNameToCategory = new();

        static HardcodedCategories()
        {
            foreach (var keyValuePair in CategoryContents)
            foreach (var s in keyValuePair.Value)
                ModNameToCategory.Add(s.ToLowerInvariant(), keyValuePair.Key);
        }

        public static string GetCategoryFor(Mod mod)
        {
            foreach (var alias in mod.aliases)
            {
                if (ModNameToCategory.TryGetValue(alias.ToLowerInvariant(), out var result)) return result;
            }

            foreach (var version in mod.versions)
            {
                if (ModNameToCategory.TryGetValue(version.name.ToLowerInvariant(), out var result)) return result;
            }

            return null;
        }

        public static string GetCategoryDescription(string category)
        {
            return CategoryDescriptions.TryGetValue(category, out var result) ? result : "";
        }
    }
}
