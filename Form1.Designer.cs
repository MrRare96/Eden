namespace AnimeXDCCWatcher
{
    partial class AnimeXDCCWatcher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.uname = new System.Windows.Forms.TextBox();
            this.login = new System.Windows.Forms.Button();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.refresh = new System.Windows.Forms.Button();
            this.printimg = new System.Windows.Forms.Button();
            this.password = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.searchbut = new System.Windows.Forms.Button();
            this.searchinput = new System.Windows.Forms.TextBox();
            this.PlayerLoc = new System.Windows.Forms.Button();
            this.PlayerLocOut = new System.Windows.Forms.TextBox();
            this.DelCovers = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(9, 26);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(420, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(9, 52);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(420, 20);
            this.textBox2.TabIndex = 1;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(9, 78);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(420, 20);
            this.textBox3.TabIndex = 2;
            this.textBox3.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1266, 647);
            this.flowLayoutPanel1.TabIndex = 3;
            this.flowLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.flowLayoutPanel1_Paint);
            // 
            // uname
            // 
            this.uname.Location = new System.Drawing.Point(6, 19);
            this.uname.Name = "uname";
            this.uname.Size = new System.Drawing.Size(100, 20);
            this.uname.TabIndex = 4;
            this.uname.Text = "Username";
            this.uname.TextChanged += new System.EventHandler(this.uname_TextChanged);
            // 
            // login
            // 
            this.login.Location = new System.Drawing.Point(6, 72);
            this.login.Name = "login";
            this.login.Size = new System.Drawing.Size(75, 23);
            this.login.TabIndex = 6;
            this.login.Text = "login";
            this.login.UseVisualStyleBackColor = true;
            this.login.Click += new System.EventHandler(this.login_Click);
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(9, 104);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(420, 20);
            this.textBox4.TabIndex = 7;
            this.textBox4.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(11, 23);
            this.refresh.Name = "refresh";
            this.refresh.Size = new System.Drawing.Size(94, 23);
            this.refresh.TabIndex = 8;
            this.refresh.Text = "Retrieve Anime";
            this.refresh.UseVisualStyleBackColor = true;
            this.refresh.Click += new System.EventHandler(this.refresh_Click);
            // 
            // printimg
            // 
            this.printimg.Location = new System.Drawing.Point(484, 104);
            this.printimg.Name = "printimg";
            this.printimg.Size = new System.Drawing.Size(94, 24);
            this.printimg.TabIndex = 9;
            this.printimg.Text = "HOME";
            this.printimg.UseVisualStyleBackColor = true;
            this.printimg.Click += new System.EventHandler(this.printimg_Click);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(6, 45);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(100, 20);
            this.password.TabIndex = 5;
            this.password.Text = "Password";
            this.password.TextChanged += new System.EventHandler(this.password_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.groupBox1.Controls.Add(this.uname);
            this.groupBox1.Controls.Add(this.password);
            this.groupBox1.Controls.Add(this.login);
            this.groupBox1.Location = new System.Drawing.Point(1094, 654);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(142, 134);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MAL Login";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.groupBox2.Controls.Add(this.searchbut);
            this.groupBox2.Controls.Add(this.searchinput);
            this.groupBox2.Controls.Add(this.PlayerLoc);
            this.groupBox2.Controls.Add(this.PlayerLocOut);
            this.groupBox2.Controls.Add(this.DelCovers);
            this.groupBox2.Controls.Add(this.refresh);
            this.groupBox2.Controls.Add(this.printimg);
            this.groupBox2.Location = new System.Drawing.Point(463, 654);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(625, 134);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controls";
            // 
            // searchbut
            // 
            this.searchbut.Location = new System.Drawing.Point(384, 104);
            this.searchbut.Name = "searchbut";
            this.searchbut.Size = new System.Drawing.Size(94, 23);
            this.searchbut.TabIndex = 14;
            this.searchbut.Text = "Search!";
            this.searchbut.UseVisualStyleBackColor = true;
            this.searchbut.Click += new System.EventHandler(this.searchbut_Click);
            // 
            // searchinput
            // 
            this.searchinput.Location = new System.Drawing.Point(11, 104);
            this.searchinput.Name = "searchinput";
            this.searchinput.Size = new System.Drawing.Size(367, 20);
            this.searchinput.TabIndex = 13;
            this.searchinput.Text = "Here you can search for your anime!";
            this.searchinput.TextChanged += new System.EventHandler(this.searchinput_TextChanged);
            // 
            // PlayerLoc
            // 
            this.PlayerLoc.Location = new System.Drawing.Point(384, 70);
            this.PlayerLoc.Name = "PlayerLoc";
            this.PlayerLoc.Size = new System.Drawing.Size(94, 23);
            this.PlayerLoc.TabIndex = 12;
            this.PlayerLoc.Text = "Player Location";
            this.PlayerLoc.UseVisualStyleBackColor = true;
            this.PlayerLoc.Click += new System.EventHandler(this.PlayerLoc_Click);
            // 
            // PlayerLocOut
            // 
            this.PlayerLocOut.Location = new System.Drawing.Point(11, 70);
            this.PlayerLocOut.Name = "PlayerLocOut";
            this.PlayerLocOut.Size = new System.Drawing.Size(367, 20);
            this.PlayerLocOut.TabIndex = 11;
            this.PlayerLocOut.TextChanged += new System.EventHandler(this.PlayerLocOut_TextChanged);
            // 
            // DelCovers
            // 
            this.DelCovers.Location = new System.Drawing.Point(111, 23);
            this.DelCovers.Name = "DelCovers";
            this.DelCovers.Size = new System.Drawing.Size(83, 23);
            this.DelCovers.TabIndex = 10;
            this.DelCovers.Text = "Delete Covers";
            this.DelCovers.UseVisualStyleBackColor = true;
            this.DelCovers.Click += new System.EventHandler(this.DelCovers_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.textBox3);
            this.groupBox3.Controls.Add(this.textBox4);
            this.groupBox3.Location = new System.Drawing.Point(16, 654);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(441, 134);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Debug/Output";
            // 
            // AnimeXDCCWatcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1267, 800);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "AnimeXDCCWatcher";
            this.Text = "AnimeXDCCWatcher";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TextBox uname;
        private System.Windows.Forms.Button login;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button refresh;
        private System.Windows.Forms.Button printimg;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button DelCovers;
        private System.Windows.Forms.Button PlayerLoc;
        private System.Windows.Forms.TextBox PlayerLocOut;
        private System.Windows.Forms.Button searchbut;
        private System.Windows.Forms.TextBox searchinput;


    }
}

