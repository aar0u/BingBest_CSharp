using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BingWallpaper
{
    public class Form1 : Form
    {
        private readonly IContainer components;
        private readonly ContextMenu contextMenu1;
        private readonly MenuItem menuItem1;
        private readonly NotifyIcon notifyIcon1;

        public Form1()
        {
            components = new Container();
            contextMenu1 = new ContextMenu();
            menuItem1 = new MenuItem();

            // Initialize contextMenu1
            contextMenu1.MenuItems.AddRange(
                new[] {menuItem1});

            // Initialize menuItem1
            menuItem1.Index = 0;
            menuItem1.Text = "E&xit";
            menuItem1.Click += menuItem1_Click;

            // Set up how the form should be displayed.
            ClientSize = new Size(292, 266);
            Text = "Notify Icon Example";

            // Create the NotifyIcon.
            notifyIcon1 = new NotifyIcon(components);

            // The Icon property sets the icon that will appear
            // in the systray for this application.
            notifyIcon1.Icon = new Icon("appicon.ico");

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenu = contextMenu1;

            // The Text property sets the text that will be displayed,
            // in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon1.Text = "Form1 (NotifyIcon example)";
            notifyIcon1.Visible = true;

            // Handle the DoubleClick event to activate the form.
            notifyIcon1.DoubleClick += notifyIcon1_DoubleClick;
        }

        protected override void Dispose(bool disposing)
        {
            // Clean up any components being used.
            if (disposing)
                if (components != null)
                    components.Dispose();

            base.Dispose(disposing);
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            // Show the form when the user double clicks on the notify icon.

            // Set the WindowState to normal if the form is minimized.
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;

            // Activate the form.
            Activate();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            // Close the form, which closes the application.
            Close();
        }
    }
}