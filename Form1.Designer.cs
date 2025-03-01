namespace Cryptography_Lab_3
{
	partial class Form1
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.MessageRTB = new System.Windows.Forms.RichTextBox();
			this.EncodedMessageRTB = new System.Windows.Forms.RichTextBox();
			this.EncodeB = new System.Windows.Forms.Button();
			this.DecodeB = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// MessageRTB
			// 
			this.MessageRTB.Dock = System.Windows.Forms.DockStyle.Left;
			this.MessageRTB.Location = new System.Drawing.Point(0, 0);
			this.MessageRTB.Name = "MessageRTB";
			this.MessageRTB.Size = new System.Drawing.Size(600, 673);
			this.MessageRTB.TabIndex = 0;
			this.MessageRTB.Text = "";
			// 
			// EncodedMessageRTB
			// 
			this.EncodedMessageRTB.Dock = System.Windows.Forms.DockStyle.Right;
			this.EncodedMessageRTB.Location = new System.Drawing.Point(662, 0);
			this.EncodedMessageRTB.Name = "EncodedMessageRTB";
			this.EncodedMessageRTB.Size = new System.Drawing.Size(600, 673);
			this.EncodedMessageRTB.TabIndex = 1;
			this.EncodedMessageRTB.Text = "";
			// 
			// EncodeB
			// 
			this.EncodeB.Location = new System.Drawing.Point(606, 235);
			this.EncodeB.Name = "EncodeB";
			this.EncodeB.Size = new System.Drawing.Size(50, 23);
			this.EncodeB.TabIndex = 2;
			this.EncodeB.Text = ">>";
			this.EncodeB.UseVisualStyleBackColor = true;
			this.EncodeB.Click += new System.EventHandler(this.EncodeB_Click);
			// 
			// DecodeB
			// 
			this.DecodeB.Location = new System.Drawing.Point(606, 264);
			this.DecodeB.Name = "DecodeB";
			this.DecodeB.Size = new System.Drawing.Size(50, 23);
			this.DecodeB.TabIndex = 3;
			this.DecodeB.Text = "<<";
			this.DecodeB.UseVisualStyleBackColor = true;
			this.DecodeB.Click += new System.EventHandler(this.DecodeB_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1262, 673);
			this.Controls.Add(this.DecodeB);
			this.Controls.Add(this.EncodeB);
			this.Controls.Add(this.EncodedMessageRTB);
			this.Controls.Add(this.MessageRTB);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox MessageRTB;
		private System.Windows.Forms.RichTextBox EncodedMessageRTB;
		private System.Windows.Forms.Button EncodeB;
		private System.Windows.Forms.Button DecodeB;
	}
}

