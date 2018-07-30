using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace BingWallpaper
{
    public class SystemTray
    {
        public NotifyIcon TrayIcon { get; set; }
        private readonly string _systemDisplayName;
        private ContextMenu _systemTrayContextMenu;

        public SystemTray(string systemDisplayName)
        {
            TrayIcon = new NotifyIcon();
            _systemDisplayName = systemDisplayName;

            InitializeSystemTray();
            AddMenu("E&xit", (s, e) => Application.Exit());
        }

        private void InitializeSystemTray()
        {
            TrayIcon.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            TrayIcon.Visible = true;
            _systemTrayContextMenu = new ContextMenu();
            TrayIcon.ContextMenu = _systemTrayContextMenu;
        }

        public void AddMenu(string text, EventHandler eventHandler)
        {
            var menuItem = new MenuItem(text, eventHandler);
            //MenuItem[] menuList = {menuItem};
            _systemTrayContextMenu.MenuItems.Add(menuItem);
        }

        public void ShowBalloon(string msg)
        {
            TrayIcon.BalloonTipTitle = _systemDisplayName;
            TrayIcon.BalloonTipText = msg;
            TrayIcon.ShowBalloonTip(1000);
        }
    }
}