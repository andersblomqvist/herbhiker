
namespace HerbHikerApp
{
    partial class HerbHiker
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HerbHiker));
            this.button1 = new System.Windows.Forms.Button();
            this.GameStatusLabel = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.ToggleGeneratorCheckBox = new System.Windows.Forms.CheckBox();
            this.TestPathButton = new System.Windows.Forms.Button();
            this.SavePathButton = new System.Windows.Forms.Button();
            this.buttonToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.LoadPathButton = new System.Windows.Forms.Button();
            this.PathControlGroup = new System.Windows.Forms.GroupBox();
            this.BotWorker = new System.ComponentModel.BackgroundWorker();
            this.ObjectDumpButton = new System.Windows.Forms.Button();
            this.PathControlGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.CheckForGameButton);
            // 
            // GameStatusLabel
            // 
            resources.ApplyResources(this.GameStatusLabel, "GameStatusLabel");
            this.GameStatusLabel.Name = "GameStatusLabel";
            // 
            // StartButton
            // 
            resources.ApplyResources(this.StartButton, "StartButton");
            this.StartButton.Name = "StartButton";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ToggleGeneratorCheckBox
            // 
            resources.ApplyResources(this.ToggleGeneratorCheckBox, "ToggleGeneratorCheckBox");
            this.ToggleGeneratorCheckBox.Name = "ToggleGeneratorCheckBox";
            this.buttonToolTip.SetToolTip(this.ToggleGeneratorCheckBox, resources.GetString("ToggleGeneratorCheckBox.ToolTip"));
            this.ToggleGeneratorCheckBox.UseVisualStyleBackColor = true;
            // 
            // TestPathButton
            // 
            resources.ApplyResources(this.TestPathButton, "TestPathButton");
            this.TestPathButton.Name = "TestPathButton";
            this.buttonToolTip.SetToolTip(this.TestPathButton, resources.GetString("TestPathButton.ToolTip"));
            this.TestPathButton.UseVisualStyleBackColor = true;
            this.TestPathButton.Click += new System.EventHandler(this.TestPathButton_Click);
            // 
            // SavePathButton
            // 
            resources.ApplyResources(this.SavePathButton, "SavePathButton");
            this.SavePathButton.Name = "SavePathButton";
            this.SavePathButton.UseVisualStyleBackColor = true;
            this.SavePathButton.Click += new System.EventHandler(this.SavePathButton_Click);
            // 
            // buttonToolTip
            // 
            this.buttonToolTip.Popup += new System.Windows.Forms.PopupEventHandler(this.ButtonHoverToolTip);
            // 
            // LoadPathButton
            // 
            resources.ApplyResources(this.LoadPathButton, "LoadPathButton");
            this.LoadPathButton.Name = "LoadPathButton";
            this.buttonToolTip.SetToolTip(this.LoadPathButton, resources.GetString("LoadPathButton.ToolTip"));
            this.LoadPathButton.UseVisualStyleBackColor = true;
            this.LoadPathButton.Click += new System.EventHandler(this.LoadPathButton_Click);
            // 
            // PathControlGroup
            // 
            this.PathControlGroup.Controls.Add(this.LoadPathButton);
            this.PathControlGroup.Controls.Add(this.TestPathButton);
            this.PathControlGroup.Controls.Add(this.SavePathButton);
            resources.ApplyResources(this.PathControlGroup, "PathControlGroup");
            this.PathControlGroup.Name = "PathControlGroup";
            this.PathControlGroup.TabStop = false;
            // 
            // BotWorker
            // 
            this.BotWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BotWorker_DoWork);
            // 
            // ObjectDumpButton
            // 
            resources.ApplyResources(this.ObjectDumpButton, "ObjectDumpButton");
            this.ObjectDumpButton.Name = "ObjectDumpButton";
            this.ObjectDumpButton.UseVisualStyleBackColor = true;
            this.ObjectDumpButton.Click += new System.EventHandler(this.ObjectDumpButton_Click);
            // 
            // HerbHiker
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ObjectDumpButton);
            this.Controls.Add(this.PathControlGroup);
            this.Controls.Add(this.ToggleGeneratorCheckBox);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.GameStatusLabel);
            this.Controls.Add(this.button1);
            this.Name = "HerbHiker";
            this.PathControlGroup.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label GameStatusLabel;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.CheckBox ToggleGeneratorCheckBox;
        private System.Windows.Forms.Button TestPathButton;
        private System.Windows.Forms.Button SavePathButton;
        private System.Windows.Forms.ToolTip buttonToolTip;
        private System.Windows.Forms.GroupBox PathControlGroup;
        private System.ComponentModel.BackgroundWorker BotWorker;
        private System.Windows.Forms.Button LoadPathButton;
        private System.Windows.Forms.Button ObjectDumpButton;
    }
}

