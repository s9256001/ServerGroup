namespace LoginServer
{
	partial class ServerUI
	{
		/// <summary>
		/// 設計工具所需的變數。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清除任何使用中的資源。
		/// </summary>
		/// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 設計工具產生的程式碼

		/// <summary>
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
		/// 修改這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
			this.txtServerState = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtServerState
			// 
			this.txtServerState.Location = new System.Drawing.Point(87, 21);
			this.txtServerState.Name = "txtServerState";
			this.txtServerState.ReadOnly = true;
			this.txtServerState.Size = new System.Drawing.Size(73, 22);
			this.txtServerState.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(19, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(57, 12);
			this.label1.TabIndex = 6;
			this.label1.Text = "ServerState";
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(296, 49);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(75, 23);
			this.btnStop.TabIndex = 5;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(296, 20);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 4;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// ServerUI
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(388, 327);
			this.Controls.Add(this.txtServerState);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnStart);
			this.Name = "ServerUI";
			this.Text = "LoginServer";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServerUI_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtServerState;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnStart;
	}
}

