using System;
using System.Collections.Generic;
using Unity.Profiling;

namespace UnityEngine.UIElements
{
    internal static class UIElementsRuntimeUtility
    {
        private static event Action s_onRepaintOverlayPanels;
        internal static event Action onRepaintOverlayPanels
        {
            add
            {
                if (s_onRepaintOverlayPanels == null)
                {
                    RegisterPlayerloopCallback();
                }

                s_onRepaintOverlayPanels += value;
            }

            remove
            {
                s_onRepaintOverlayPanels -= value;

                if (s_onRepaintOverlayPanels == null)
                {
                    UnregisterPlayerloopCallback();
                }
            }
        }

        public static event Action<BaseRuntimePanel> onCreatePanel;

        static UIElementsRuntimeUtility()
        {
            UIElementsRuntimeUtilityNative.RepaintOverlayPanelsCallback = RepaintOverlayPanels;

        }

        public static EventBase CreateEvent(Event systemEvent)
        {
            return UIElementsUtility.CreateEvent(systemEvent, systemEvent.rawType);
        }

        public delegate BaseRuntimePanel CreateRuntimePanelDelegate(ScriptableObject ownerObject);

        public static BaseRuntimePanel FindOrCreateRuntimePanel(ScriptableObject ownerObject,
            CreateRuntimePanelDelegate createDelegate)
        {
            if (UIElementsUtility.TryGetPanel(ownerObject.GetInstanceID(), out Panel cachedPanel))
            {
                if (cachedPanel is BaseRuntimePanel runtimePanel)
                    return runtimePanel;
                RemoveCachedPanelInternal(ownerObject.GetInstanceID()); // Maybe throw exception instead?
            }

            var panel = createDelegate(ownerObject);
            panel.IMGUIEventInterests = new EventInterests {wantsMouseMove = true, wantsMouseEnterLeaveWindow = true};
            RegisterCachedPanelInternal(ownerObject.GetInstanceID(), panel);
            onCreatePanel?.Invoke(panel);
            return panel;
        }

        public static void DisposeRuntimePanel(ScriptableObject ownerObject)
        {
            Panel panel;
            if (UIElementsUtility.TryGetPanel(ownerObject.GetInstanceID(), out panel))
            {
                panel.Dispose();
                RemoveCachedPanelInternal(ownerObject.GetInstanceID());
            }
        }

        private static bool s_RegisteredPlayerloopCallback = false;

        private static void RegisterCachedPanelInternal(int instanceID, IPanel panel)
        {
            UIElementsUtility.RegisterCachedPanel(instanceID, panel as Panel);
            s_PanelOrderingDirty = true;
            if (!s_RegisteredPlayerloopCallback)
            {
                s_RegisteredPlayerloopCallback = true;
                RegisterPlayerloopCallback();
            }
        }

        private static void RemoveCachedPanelInternal(int instanceID)
        {
            UIElementsUtility.RemoveCachedPanel(instanceID);

            s_PanelOrderingDirty = true;

            // We don't call GetSortedPanels() here to avoid always sorting when we remove multiple panels in a row
            // the ordering is dirty anyways, it will eventually get recreated
            s_SortedRuntimePanels.Clear();
            UIElementsUtility.GetAllPanels(s_SortedRuntimePanels, ContextType.Player);

            // un-register the playerloop callback as the last panel gets un-registered
            if (s_SortedRuntimePanels.Count == 0)
            {
                s_RegisteredPlayerloopCallback = false;
                UnregisterPlayerloopCallback();
            }
        }

        static List<Panel> s_SortedRuntimePanels = new List<Panel>();
        private static bool s_PanelOrderingDirty = true;

        internal static readonly string s_RepaintProfilerMarkerName = "UIElementsRuntimeUtility.DoDispatch(Repaint Event)";
        private static readonly ProfilerMarker s_RepaintProfilerMarker = new ProfilerMarker(s_RepaintProfilerMarkerName);

        public static void RepaintOverlayPanels()
        {
            foreach (BaseRuntimePanel panel in GetSortedPlayerPanels())
            {
                if (!panel.drawToCameras)
                {
                    RepaintOverlayPanel(panel);
                }
            }

            // Call the package override of RepaintOverlayPanels, when available
            if (s_onRepaintOverlayPanels != null)
                s_onRepaintOverlayPanels();
        }

        public static void RepaintOverlayPanel(BaseRuntimePanel panel)
        {
            using (s_RepaintProfilerMarker.Auto())
                panel.Repaint(Event.current);
            (panel.panelDebug?.debuggerOverlayPanel as Panel)?.Repaint(Event.current);
        }

        private static int currentOverlayIndex = -1;
        internal static void BeginRenderOverlays(int displayIndex)
        {
            currentOverlayIndex = 0;
        }

        internal static void RenderOverlaysBeforePriority(int displayIndex, float maxPriority)
        {
            if (currentOverlayIndex < 0)
                return;

            var runTimePanels = GetSortedPlayerPanels();

            for (; currentOverlayIndex < runTimePanels.Count; ++currentOverlayIndex)
            {
                if (runTimePanels[currentOverlayIndex] is BaseRuntimePanel p)
                {
                    if (p.sortingPriority >= maxPriority)
                        return;

                    if (p.targetDisplay == displayIndex)
                    {
                        RepaintOverlayPanel(p);
                    }
                }
            }
        }

        internal static void EndRenderOverlays(int displayIndex)
        {
            RenderOverlaysBeforePriority(displayIndex, float.MaxValue);
            currentOverlayIndex = -1;
        }

        internal static Object activeEventSystem { get; private set; }
        internal static bool useDefaultEventSystem => activeEventSystem == null;

        public static void RegisterEventSystem(Object eventSystem)
        {
            if (activeEventSystem != null && activeEventSystem != eventSystem && eventSystem.GetType().Name == "EventSystem")
                Debug.LogWarning("There can be only one active Event System.");
            activeEventSystem = eventSystem;
        }

        public static void UnregisterEventSystem(Object eventSystem)
        {
            if (activeEventSystem == eventSystem)
                activeEventSystem = null;
        }

        private static DefaultEventSystem s_DefaultEventSystem;
        internal static DefaultEventSystem defaultEventSystem =>
            s_DefaultEventSystem ?? (s_DefaultEventSystem = new DefaultEventSystem());

        public static void UpdateRuntimePanels()
        {
            foreach (BaseRuntimePanel panel in GetSortedPlayerPanels())
            {
                panel.Update();
            }

            if (Application.isPlaying && useDefaultEventSystem)
            {
                defaultEventSystem.Update();
            }
        }

        public static void RegisterPlayerloopCallback()
        {
            UIElementsRuntimeUtilityNative.RegisterPlayerloopCallback();
            UIElementsRuntimeUtilityNative.UpdateRuntimePanelsCallback = UpdateRuntimePanels;
        }

        public static void UnregisterPlayerloopCallback()
        {
            UIElementsRuntimeUtilityNative.UnregisterPlayerloopCallback();
            UIElementsRuntimeUtilityNative.UpdateRuntimePanelsCallback = null;
        }

        internal static void SetPanelOrderingDirty()
        {
            s_PanelOrderingDirty = true;
        }

        internal static List<Panel> GetSortedPlayerPanels()
        {
            if (s_PanelOrderingDirty)
                SortPanels();
            return s_SortedRuntimePanels;
        }

        static void SortPanels()
        {
            s_SortedRuntimePanels.Clear();
            UIElementsUtility.GetAllPanels(s_SortedRuntimePanels, ContextType.Player);

            s_SortedRuntimePanels.Sort((a, b) =>
            {
                var diff = a.sortingPriority - b.sortingPriority;

                if (Mathf.Approximately(0, diff))
                {
                    return 0;
                }

                return (diff < 0) ? -1 : 1;
            });

            s_PanelOrderingDirty = false;
        }
    }
}
