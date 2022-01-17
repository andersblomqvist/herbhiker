
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HerbHiker));
            this.button1 = new System.Windows.Forms.Button();
            this.GameStatusLabel = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.ToggleGeneratorCheckBox = new System.Windows.Forms.CheckBox();
            this.TestPathButton = new System.Windows.Forms.Button();
            this.SavePathButton = new System.Windows.Forms.Button();
            this.LoadPathButton = new System.Windows.Forms.Button();
            this.PathControlGroup = new System.Windows.Forms.GroupBox();
            this.ObjectDumpButton = new System.Windows.Forms.Button();
            this.BotControls = new System.Windows.Forms.GroupBox();
            this.LoadedPathLabel = new System.Windows.Forms.Label();
            this.StopButton = new System.Windows.Forms.Button();
            this.PauseButton = new System.Windows.Forms.Button();
            this.ResumeButton = new System.Windows.Forms.Button();
            this.BotStatusLabel = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.PathControlGroup.SuspendLayout();
            this.BotControls.SuspendLayout();
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
            this.ToggleGeneratorCheckBox.UseVisualStyleBackColor = true;
            this.ToggleGeneratorCheckBox.CheckedChanged += new System.EventHandler(this.ToggleGeneratorCheckBox_CheckedChanged);
            // 
            // TestPathButton
            // 
            resources.ApplyResources(this.TestPathButton, "TestPathButton");
            this.TestPathButton.Name = "TestPathButton";
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
            // LoadPathButton
            // 
            resources.ApplyResources(this.LoadPathButton, "LoadPathButton");
            this.LoadPathButton.Name = "LoadPathButton";
            this.LoadPathButton.UseVisualStyleBackColor = true;
            this.LoadPathButton.Click += new System.EventHandler(this.LoadPathButton_Click);
            // 
            // PathControlGroup
            // 
            this.PathControlGroup.Controls.Add(this.TestPathButton);
            this.PathControlGroup.Controls.Add(this.SavePathButton);
            resources.ApplyResources(this.PathControlGroup, "PathControlGroup");
            this.PathControlGroup.Name = "PathControlGroup";
            this.PathControlGroup.TabStop = false;
            // 
            // ObjectDumpButton
            // 
            resources.ApplyResources(this.ObjectDumpButton, "ObjectDumpButton");
            this.ObjectDumpButton.Name = "ObjectDumpButton";
            this.ObjectDumpButton.UseVisualStyleBackColor = true;
            this.ObjectDumpButton.Click += new System.EventHandler(this.ObjectDumpButton_Click);
            // 
            // BotControls
            // 
            this.BotControls.Controls.Add(this.StatusLabel);
            this.BotControls.Controls.Add(this.BotStatusLabel);
            this.BotControls.Controls.Add(this.ResumeButton);
            this.BotControls.Controls.Add(this.PauseButton);
            this.BotControls.Controls.Add(this.StopButton);
            this.BotControls.Controls.Add(this.LoadedPathLabel);
            this.BotControls.Controls.Add(this.StartButton);
            resources.ApplyResources(this.BotControls, "BotControls");
            this.BotControls.Name = "BotControls";
            this.BotControls.TabStop = false;
            // 
            // LoadedPathLabel
            // 
            resources.ApplyResources(this.LoadedPathLabel, "LoadedPathLabel");
            this.LoadedPathLabel.Name = "LoadedPathLabel";
            // 
            // StopButton
            // 
            resources.ApplyResources(this.StopButton, "StopButton");
            this.StopButton.Name = "StopButton";
            this.StopButton.UseVisualStyleBackColor = true;
            this.StopButton.Click += new System.EventHandler(this.StopButton_Click);
            // 
            // PauseButton
            // 
            resources.ApplyResources(this.PauseButton, "PauseButton");
            this.PauseButton.Name = "PauseButton";
            this.PauseButton.UseVisualStyleBackColor = true;
            // 
            // ResumeButton
            // 
            resources.ApplyResources(this.ResumeButton, "ResumeButton");
            this.ResumeButton.Name = "ResumeButton";
            this.ResumeButton.UseVisualStyleBackColor = true;
            // 
            // BotStatusLabel
            // 
            resources.ApplyResources(this.BotStatusLabel, "BotStatusLabel");
            this.BotStatusLabel.Name = "BotStatusLabel";
            // 
            // StatusLabel
            // 
            resources.ApplyResources(this.StatusLabel, "StatusLabel");
            this.StatusLabel.Name = "StatusLabel";
            // 
            // HerbHiker
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BotControls);
            this.Controls.Add(this.LoadPathButton);
            this.Controls.Add(this.ObjectDumpButton);
            this.Controls.Add(this.PathControlGroup);
            this.Controls.Add(this.ToggleGeneratorCheckBox);
            this.Controls.Add(this.GameStatusLabel);
            this.Controls.Add(this.button1);
            this.Name = "HerbHiker";
            this.PathControlGroup.ResumeLayout(false);
            this.BotControls.ResumeLayout(false);
            this.BotControls.PerformLayout();
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
        private System.Windows.Forms.GroupBox PathControlGroup;
        private System.Windows.Forms.Button LoadPathButton;
        private System.Windows.Forms.Button ObjectDumpButton;
        private System.Windows.Forms.GroupBox BotControls;
        private System.Windows.Forms.Label LoadedPathLabel;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.Button PauseButton;
        private System.Windows.Forms.Button ResumeButton;
        private System.Windows.Forms.Label BotStatusLabel;
        private System.Windows.Forms.Label StatusLabel;
    }
}

