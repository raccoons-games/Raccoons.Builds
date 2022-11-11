using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Raccoons.Builds.Hubs
{
    public static class BuildHubMenu
    {
        private const string MENU_PATH = "Raccoons/Builds";
        private const string HUB_FOLDER_KEY = MENU_PATH + "/HubFolder";

        public static string HubFolderLocation
        {
            get
            {
                return PlayerPrefs.GetString(HUB_FOLDER_KEY);
            }
            set
            {
                PlayerPrefs.SetString(HUB_FOLDER_KEY, value);
            }
        }

        [MenuItem(MENU_PATH + "/Set Hub Folder")]
        public static void SetHubFolder()
        {
            string folder = EditorUtility.OpenFolderPanel("Set hub folder", HubFolderLocation, "");
            ThrowIfInvalidHub(folder);
            HubFolderLocation = folder;
            Debug.Log($"[BuildVersioning] New hub folder - {folder}");
        }


        [MenuItem(MENU_PATH + "/Copy Future Build Name")]
        public static void CopyFutureBuildName()
        {
            int latestIntSubVersion;
            DirectoryInfo directoryInfo;
            GetLastBuildData(out latestIntSubVersion, out directoryInfo);
            latestIntSubVersion++;
            string futureBuildName = GetBuildName(latestIntSubVersion, directoryInfo.Name);
            string location = string.Join(Path.AltDirectorySeparatorChar, HubFolderLocation, futureBuildName);
            EditorUserBuildSettings.SetBuildLocation(EditorUserBuildSettings.activeBuildTarget, location);
            GUIUtility.systemCopyBuffer = futureBuildName;
            Debug.Log($"[BuildVersioning] Copied to clipboard - {futureBuildName}");
        }

        [MenuItem(MENU_PATH + "/Copy Current Version")]
        public static void CopyCurrentBuildVersion()
        {
            int latestIntSubVersion;
            DirectoryInfo directoryInfo;
            GetLastBuildData(out latestIntSubVersion, out directoryInfo);
            string futureBuildName = GetVersionName(latestIntSubVersion, directoryInfo.Name);
            GUIUtility.systemCopyBuffer = futureBuildName;
            Debug.Log($"[BuildVersioning] Copied to clipboard - {futureBuildName}");
        }

        [MenuItem(MENU_PATH + "/Copy Future Version")]
        public static void CopyFutureBuildVersion()
        {
            int latestIntSubVersion;
            DirectoryInfo directoryInfo;
            GetLastBuildData(out latestIntSubVersion, out directoryInfo);
            latestIntSubVersion++;
            string futureBuildName = GetVersionName(latestIntSubVersion, directoryInfo.Name);
            GUIUtility.systemCopyBuffer = futureBuildName;
            Debug.Log($"[BuildVersioning] Copied to clipboard - {futureBuildName}");
        }

        private static string GetBuildName(int latestIntSubVersion, string releaseVersion)
        {
            string versionName = GetVersionName(latestIntSubVersion, releaseVersion);
            return $"{Application.productName}_v{versionName}";
        }

        private static string GetVersionName(int latestIntSubVersion, string releaseVersion)
        {
            return $"{releaseVersion}-{latestIntSubVersion}";
        }

        private static void GetLastBuildData(out int latestIntVersion, out DirectoryInfo directoryInfo)
        {
            ThrowIfInvalidHub();
            latestIntVersion = 0;
            directoryInfo = new DirectoryInfo(HubFolderLocation);
            FileInfo[] files = directoryInfo.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(directoryInfo.Name))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.Name);
                    string versionString = fileName.Substring(file.Name.IndexOf('-') + 1);
                    if (int.TryParse(versionString, out int intVersion))
                    {
                        if (intVersion > latestIntVersion)
                        {
                            latestIntVersion = intVersion;
                        }
                    }
                }
            }
        }


        private static void ThrowIfInvalidHub()
        {
            ThrowIfInvalidHub(HubFolderLocation);
        }

        private static void ThrowIfInvalidHub(string location)
        {
            if (string.IsNullOrEmpty(location)) throw new ArgumentNullException("Hub Folder Location");
            if (!Directory.Exists(location)) throw new DirectoryNotFoundException("Hub Folder not found");
        }
    }
}