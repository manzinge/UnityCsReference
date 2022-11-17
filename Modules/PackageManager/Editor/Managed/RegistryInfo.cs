// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Bindings;
using RequiredByNativeCodeAttribute = UnityEngine.Scripting.RequiredByNativeCodeAttribute;

namespace UnityEditor.PackageManager
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    [RequiredByNativeCode]
    [NativeAsStruct]
    public class RegistryInfo
    {
        [SerializeField]
        [NativeName("id")]
        private string m_Id;

        [SerializeField]
        [NativeName("name")]
        private string m_Name;

        [SerializeField]
        [NativeName("url")]
        private string m_Url;

        [SerializeField]
        [NativeName("scopes")]
        private string[] m_Scopes;

        [SerializeField]
        [NativeName("isDefault")]
        private bool m_IsDefault;

        [SerializeField]
        [NativeName("capabilities")]
        private SearchCapabilities m_Capabilities;

        [SerializeField]
        [NativeName("configSource")]
        private ConfigSource m_ConfigSource;

        internal RegistryInfo(string id = "", string name = "", string url = "", string[] scopes = null, bool isDefault = false, SearchCapabilities capabilities = SearchCapabilities.None, ConfigSource configSource = ConfigSource.Unknown)
        {
            m_Id = id;
            m_Name = name;
            m_Url = url;
            m_Scopes = scopes ?? new string[0];
            m_IsDefault = isDefault;
            m_Capabilities = capabilities;
            m_ConfigSource = configSource;
        }

        internal string id { get { return m_Id;  } }
        public string name { get { return m_Name;  } }
        public string url { get { return m_Url;  } }
        internal string[] scopes { get { return m_Scopes; } }
        public bool isDefault { get { return m_IsDefault; } }
        internal SearchCapabilities capabilities { get { return m_Capabilities; } }
        internal ConfigSource configSource { get { return m_ConfigSource; } }
    }
}
