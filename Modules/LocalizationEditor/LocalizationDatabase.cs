// Unity C# reference source
// Copyright (c) Unity Technologies. For terms of use, see
// https://unity3d.com/legal/licenses/Unity_Reference_Only_License

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.Internal;
using UnityEngine.Assertions;

using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using System.Security.Cryptography;
using System.Reflection;

namespace UnityEditor
{
    [ExcludeFromDocs]
    public static class L10n
    {
        static object lockObject = new object();
        static Hashtable s_GroupNames = new Hashtable();

        static string GetGroupName(System.Reflection.Assembly assembly)
        {
            var name = (string)s_GroupNames[assembly];
            if (name == null)
            {
                object[] attrobjs = assembly.GetCustomAttributes(typeof(LocalizationAttribute), true /* inherit */);
                if (attrobjs != null && attrobjs.Length > 0 && attrobjs[0] != null)
                {
                    var locAttr = (LocalizationAttribute)attrobjs[0];
                    string locGroupName = locAttr.locGroupName;
                    if (locGroupName == null)
                        locGroupName = assembly.GetName().Name;
                    name = locGroupName;
                    lock (lockObject)
                    {
                        s_GroupNames[assembly] = name;
                    }
                }
            }
            return name;
        }

        public static string Tr(string str)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return str;

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_str = LocalizationDatabase.GetLocalizedStringWithGroupName(str, groupName);
                return new_str;
            }
            else
            {
                return LocalizationDatabase.GetLocalizedString(str);
            }
        }

        public static string[] Tr(string[] str_list)
        {
            var res = new string[str_list.Length];
            for (var i = 0; i < res.Length; ++i)
                res[i] = Tr(str_list[i]);
            return res;
        }

        public static string Tr(string str, string groupName)
        {
            var new_str = LocalizationDatabase.GetLocalizedStringWithGroupName(str, groupName);
            return new_str;
        }

        public static string TrPath(string path)
        {
            string[] separatingChars = { "/" };
            var result = new System.Text.StringBuilder(256);
            var items = path.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < items.Length; ++i)
            {
                result.Append(Tr(items[i]));
                if (i < items.Length - 1)
                    result.Append("/");
            }
            return result.ToString();
        }

        public static GUIContent TextContent(string text, string tooltip = null, Texture icon = null)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TrTextContent(text, tooltip, icon);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_text = LocalizationDatabase.GetLocalizedStringWithGroupName(text, groupName);
                var new_tooltip = LocalizationDatabase.GetLocalizedStringWithGroupName(tooltip, groupName);
                return EditorGUIUtility.TrTextContent(new_text, new_tooltip, icon);
            }
            else
            {
                return EditorGUIUtility.TrTextContent(text, tooltip, icon);
            }
        }

        public static GUIContent TextContent(string text, string tooltip, string iconName)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TrTextContent(text, tooltip, iconName);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_text = LocalizationDatabase.GetLocalizedStringWithGroupName(text, groupName);
                var new_tooltip = LocalizationDatabase.GetLocalizedStringWithGroupName(tooltip, groupName);
                return EditorGUIUtility.TrTextContent(new_text, new_tooltip, iconName);
            }
            else
            {
                return EditorGUIUtility.TrTextContent(text, tooltip, iconName);
            }
        }

        public static GUIContent TextContent(string text, Texture icon)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TrTextContentWithIcon(text, icon);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_text = LocalizationDatabase.GetLocalizedStringWithGroupName(text, groupName);
                return EditorGUIUtility.TrTextContentWithIcon(new_text, icon);
            }
            else
            {
                return EditorGUIUtility.TrTextContentWithIcon(text, icon);
            }
        }

        public static GUIContent TextContentWithIcon(string text, Texture icon)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TrTextContentWithIcon(text, icon);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_text = LocalizationDatabase.GetLocalizedStringWithGroupName(text, groupName);
                return EditorGUIUtility.TrTextContentWithIcon(new_text, icon);
            }
            else
            {
                return EditorGUIUtility.TrTextContentWithIcon(text, icon);
            }
        }

        public static GUIContent TextContentWithIcon(string text, string iconName)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TextContentWithIcon(text, iconName);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_text = LocalizationDatabase.GetLocalizedStringWithGroupName(text, groupName);
                return EditorGUIUtility.TextContentWithIcon(new_text, iconName);
            }
            else
            {
                return EditorGUIUtility.TextContentWithIcon(text, iconName);
            }
        }

        public static GUIContent TextContentWithIcon(string text, string tooltip, string iconName)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TrTextContentWithIcon(text, tooltip, iconName);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_text = LocalizationDatabase.GetLocalizedStringWithGroupName(text, groupName);
                var new_tooltip = LocalizationDatabase.GetLocalizedStringWithGroupName(tooltip, groupName);
                return EditorGUIUtility.TrTextContentWithIcon(new_text, new_tooltip, iconName);
            }
            else
            {
                return EditorGUIUtility.TrTextContentWithIcon(text, tooltip, iconName);
            }
        }

        public static GUIContent TextContentWithIcon(string text, string tooltip, Texture icon)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TrTextContentWithIcon(text, tooltip, icon);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_text = LocalizationDatabase.GetLocalizedStringWithGroupName(text, groupName);
                var new_tooltip = LocalizationDatabase.GetLocalizedStringWithGroupName(tooltip, groupName);
                return EditorGUIUtility.TrTextContentWithIcon(new_text, new_tooltip, icon);
            }
            else
            {
                return EditorGUIUtility.TrTextContentWithIcon(text, tooltip, icon);
            }
        }

        public static GUIContent TextContentWithIcon(string text, string tooltip, MessageType messageType)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TrTextContentWithIcon(text, tooltip, messageType);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_text = LocalizationDatabase.GetLocalizedStringWithGroupName(text, groupName);
                var new_tooltip = LocalizationDatabase.GetLocalizedStringWithGroupName(tooltip, groupName);
                return EditorGUIUtility.TrTextContentWithIcon(new_text, new_tooltip, messageType);
            }
            else
            {
                return EditorGUIUtility.TrTextContentWithIcon(text, tooltip, messageType);
            }
        }

        public static GUIContent TextContentWithIcon(string text, MessageType messageType)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TrTextContentWithIcon(text, messageType);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_text = LocalizationDatabase.GetLocalizedStringWithGroupName(text, groupName);
                return EditorGUIUtility.TrTextContentWithIcon(new_text, messageType);
            }
            else
            {
                return EditorGUIUtility.TrTextContentWithIcon(text, messageType);
            }
        }

        public static GUIContent IconContent(string iconName, string tooltip = null)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TrIconContent(iconName, tooltip);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_tooltip = LocalizationDatabase.GetLocalizedStringWithGroupName(tooltip, groupName);
                return EditorGUIUtility.TrIconContent(iconName, new_tooltip);
            }
            else
            {
                return EditorGUIUtility.TrIconContent(iconName, tooltip);
            }
        }

        public static GUIContent IconContent(Texture icon, string tooltip = null)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TrIconContent(icon, tooltip);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_tooltip = LocalizationDatabase.GetLocalizedStringWithGroupName(tooltip, groupName);
                return EditorGUIUtility.TrIconContent(icon, new_tooltip);
            }
            else
            {
                return EditorGUIUtility.TrIconContent(icon, tooltip);
            }
        }

        public static GUIContent TempContent(string t)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TempContent(t);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                var new_t = LocalizationDatabase.GetLocalizedStringWithGroupName(t, groupName);
                return EditorGUIUtility.TempContent(new_t);
            }
            else
            {
                return EditorGUIUtility.TempContent(t);
            }
        }

        public static GUIContent[] TempContent(string[] texts)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TempContent(texts);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                GUIContent[] retval = new GUIContent[texts.Length];
                for (int i = 0; i < texts.Length; i++)
                {
                    var new_t = LocalizationDatabase.GetLocalizedStringWithGroupName(texts[i], groupName);
                    retval[i] = new GUIContent(new_t);
                }
                return retval;
            }
            else
            {
                return EditorGUIUtility.TempContent(texts);
            }
        }

        public static GUIContent[] TempContent(string[] texts, string[] tooltips)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return EditorGUIUtility.TempContent(texts, tooltips);

            var groupName = GetGroupName(Assembly.GetCallingAssembly());
            if (groupName != null)
            {
                GUIContent[] retval = new GUIContent[texts.Length];
                for (int i = 0; i < texts.Length; i++)
                {
                    var new_t = LocalizationDatabase.GetLocalizedStringWithGroupName(texts[i], groupName);
                    var new_tooltip = LocalizationDatabase.GetLocalizedStringWithGroupName(tooltips[i], groupName);
                    retval[i] = new GUIContent(new_t, new_tooltip);
                }
                return retval;
            }
            else
            {
                return EditorGUIUtility.TempContent(texts);
            }
        }
    }

    internal static class LocalizationGroupStack
    {
        static Stack<string> s_GroupNameStack;
        public static void Push(string groupName)
        {
            if (s_GroupNameStack == null)
                s_GroupNameStack = new Stack<string>();
            if (s_GroupNameStack.Count >= 16)
                Assert.IsTrue(false); // check the leak.
            s_GroupNameStack.Push(groupName);
            LocalizationDatabase.SetContextGroupName(groupName);
        }

        public static void Pop()
        {
            if (s_GroupNameStack == null || s_GroupNameStack.Count <= 0)
                Assert.IsTrue(false);
            s_GroupNameStack.Pop();
            if (s_GroupNameStack.Count > 0)
            {
                string top = s_GroupNameStack.Peek();
                LocalizationDatabase.SetContextGroupName(top);
            }
            else
                LocalizationDatabase.SetContextGroupName(null);
        }
    }

    /// <summary>
    /// This provides an auto dispose Localization system.
    /// This can be called recursively.
    /// </summary>
    public class LocalizationGroup : IDisposable
    {
        string m_LocGroupName;
        bool m_Pushed = false;

        /// <summary>
        /// a current group name for the localization.
        /// </summary>
        public string locGroupName { get { return m_LocGroupName; } }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocalizationGroup()
        {
            initialize(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// constructor.
        /// <param name="behaviour">group name will become the name of Assembly the behaviour belongs to.</param>
        /// </summary>
        public LocalizationGroup(Behaviour behaviour)
        {
            if (behaviour != null)
            {
                System.Type type = behaviour.GetType();
                initialize(type.Assembly);
            }
        }

        /// <summary>
        /// constructor.
        /// <param name="type">group name will become the name of Assembly the type belongs to.</param>
        /// </summary>
        public LocalizationGroup(System.Type type)
        {
            initialize(type.Assembly);
        }

        /// <summary>
        /// constructor.
        /// <param name="obj">group name will become the name of Assembly the obj belongs to.</param>
        /// </summary>
        public LocalizationGroup(System.Object obj)
        {
            if (obj == null)
                return;
            initialize(obj.GetType().Assembly);
        }

        void initialize(System.Reflection.Assembly assembly)
        {
            string groupName = null;
            object[] attrobjs = assembly.GetCustomAttributes(typeof(LocalizationAttribute), true /* inherit */);
            if (attrobjs != null && attrobjs.Length > 0 && attrobjs[0] != null) // focus on only the first.
            {
                LocalizationAttribute locAttr = (LocalizationAttribute)attrobjs[0];
                groupName = locAttr.locGroupName;
                if (groupName == null)
                    groupName = assembly.GetName().Name;
            }
            LocalizationGroupStack.Push(groupName);
            m_Pushed = true;
            m_LocGroupName = groupName;
        }

        /// <summary>
        /// dispose current state.
        /// </summary>
        public void Dispose()
        {
            if (m_Pushed)
                LocalizationGroupStack.Pop();
        }
    }
}

namespace UnityEditor.Localization.Editor
{
    /// <summary>
    /// This provides Localization function for Packages.
    /// </summary>
    [System.Obsolete("Localization has been deprecated. Please use UnityEditor.L10n instead", true)]
    public static class Localization
    {
        /// <summary>
        /// get proper translation for the given argument.
        /// </summary>
        [System.Obsolete("Obsolete msg (UnityUpgradable) -> UnityEditor.L10n.Tr(*)", true)]
        public static string Tr(string str)
        {
            if (!LocalizationDatabase.enableEditorLocalization)
                return str;
            var assembly = Assembly.GetCallingAssembly();
            object[] attrobjs = assembly.GetCustomAttributes(typeof(LocalizationAttribute), true /* inherit */);
            if (attrobjs != null && attrobjs.Length > 0 && attrobjs[0] != null)
            {
                LocalizationAttribute locAttr = (LocalizationAttribute)attrobjs[0];
                string locGroupName = locAttr.locGroupName;
                if (locGroupName == null)
                    locGroupName = assembly.GetName().Name;
                var new_str = LocalizationDatabase.GetLocalizedStringWithGroupName(str, locGroupName);
                return new_str;
            }
            return str;
        }
    }

    /// <summary>
    /// This provides an auto dispose Localization system.
    /// This can be called recursively.
    /// </summary>
    [System.Obsolete("LocalizationGroup has been deprecated. Please use UnityEditor.LocalizationGroup instead", true)]
    public class LocalizationGroup : IDisposable
    {
        string m_LocGroupName;
        bool m_Pushed = false;

        /// <summary>
        /// a current group name for the localization.
        /// </summary>
        public string locGroupName { get { return m_LocGroupName; } }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocalizationGroup()
        {
            initialize(Assembly.GetCallingAssembly());
        }

        /// <summary>
        /// constructor.
        /// <param name="behaviour">group name will become the name of Assembly the behaviour belongs to.</param>
        /// </summary>
        public LocalizationGroup(Behaviour behaviour)
        {
            if (behaviour != null)
            {
                System.Type type = behaviour.GetType();
                initialize(type.Assembly);
            }
        }

        /// <summary>
        /// constructor.
        /// <param name="type">group name will become the name of Assembly the type belongs to.</param>
        /// </summary>
        public LocalizationGroup(System.Type type)
        {
            initialize(type.Assembly);
        }

        /// <summary>
        /// constructor.
        /// <param name="obj">group name will become the name of Assembly the obj belongs to.</param>
        /// </summary>
        public LocalizationGroup(System.Object obj)
        {
            if (obj == null)
                return;
            initialize(obj.GetType().Assembly);
        }

        void initialize(System.Reflection.Assembly assembly)
        {
            string groupName = null;
            object[] attrobjs = assembly.GetCustomAttributes(typeof(LocalizationAttribute), true /* inherit */);
            if (attrobjs != null && attrobjs.Length > 0 && attrobjs[0] != null) // focus on only the first.
            {
                LocalizationAttribute locAttr = (LocalizationAttribute)attrobjs[0];
                groupName = locAttr.locGroupName;
                if (groupName == null)
                    groupName = assembly.GetName().Name;
            }
            LocalizationGroupStack.Push(groupName);
            m_Pushed = true;
            m_LocGroupName = groupName;
        }

        /// <summary>
        /// dispose current state.
        /// </summary>
        public void Dispose()
        {
            if (m_Pushed)
                LocalizationGroupStack.Pop();
        }
    }
}
