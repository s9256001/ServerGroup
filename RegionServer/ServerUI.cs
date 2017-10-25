using RegionServer.Server;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace RegionServer
{
	public partial class ServerUI : Form
	{
		/// <summary>
		/// 遊戲Server
		/// </summary>
		public DerivedServer Server { get; set; }

		public ServerUI()
		{
			InitializeComponent();

			Server = new DerivedServer();
			UpdateServerState();

			StartPosition = FormStartPosition.Manual;
			Location = new Point(100 + 410 * 2, 100);
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			Server.Start();
			UpdateServerState();
		}
		private void btnStop_Click(object sender, EventArgs e)
		{
			Server.Stop();
			UpdateServerState();
		}
		private void ServerUI_FormClosed(object sender, FormClosedEventArgs e)
		{
			Server.Stop();
		}

		private void UpdateServerState()
		{
			txtServerState.Text = Server.ServerState.ToString();
		}
	}
}
