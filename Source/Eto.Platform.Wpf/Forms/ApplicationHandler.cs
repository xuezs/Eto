﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eto.Forms;
using System.Diagnostics;
using sw = System.Windows;

namespace Eto.Platform.Wpf.Forms
{
	public class ApplicationHandler : WidgetHandler<System.Windows.Application, Application>, IApplication
	{
		public override sw.Application CreateControl ()
		{
			return new sw.Application ();
		}

		public override void Initialize ()
		{
			base.Initialize ();
			Control.Startup += HandleStartup;
		}

		void HandleStartup (object sender, sw.StartupEventArgs e)
		{
			IsActive = true;
			Control.Activated += (sender2, e2) => {
				IsActive = true;
			};
			Control.Deactivated += (sender2, e2) => {
				IsActive = false;
			};
		}

		public bool IsActive { get; private set; }

		public void RunIteration()
		{
		}
		private bool shutdown;

		public void Quit()
		{
			bool cancel = false;
			foreach (sw.Window window in Control.Windows) {
				window.Close ();
				cancel |= window.IsVisible;
			}
			if (!cancel)
			{
				Control.Shutdown();
				shutdown = true;
			}
		}

		public void Invoke (Action action)
		{
			Control.Dispatcher.Invoke (action);
		}

		public void AsyncInvoke (Action action)
		{
			Control.Dispatcher.BeginInvoke (action);
		}

		public void GetSystemActions (GenerateActionArgs args, bool addStandardItems)
		{
		}

		public Key CommonModifier
		{
			get { return Key.Control; }
		}

		public Key AlternateModifier
		{
			get { return Key.Alt; }
		}

		public void Open(string url)
		{
			Process.Start(url);	
		}

		public void Run (string[] args)
		{
			Widget.OnInitialized (EventArgs.Empty);
			if (shutdown) return;
			if (Widget.MainForm != null)
				Control.Run ((System.Windows.Window)Widget.MainForm.ControlObject);
			else
				Control.Run ();
		}

		public void Restart ()
		{
			System.Diagnostics.Process.Start (System.Windows.Application.ResourceAssembly.Location);
			System.Windows.Application.Current.Shutdown ();
		}

		public override void AttachEvent (string handler)
		{
			switch (handler) {
				case Application.TerminatingEvent:
					// handled by WpfWindow
					break;
				default:
					base.AttachEvent (handler);
					break;
			}
		}
	}
}
