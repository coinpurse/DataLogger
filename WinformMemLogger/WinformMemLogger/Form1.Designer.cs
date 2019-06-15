using System;
using System.Windows.Forms;
using DataLibrary;
using System.Diagnostics;

// A Winform application that asks for an interval amount of time in milliseconds and for a file path.
// Once they have been given by the user, start a log provided by the DataLibrary DLL file and log the memory usage of the system along with a time stamp
// When the Start Log button is pressed, the application will record the percent of committed memory usage at the given interval for 10 seconds
// When the program has finished recording, a message will appear to tell the user that the logging is complete
namespace WinformMemLogger
{
    partial class Form1
    {

        private static int logTimer = 0;        // The log timer
        private static int interval = 0;        // The interval timer
        private static System.Timers.Timer timer;      // The timer used to handle each event at the given interval
        private static DataLog<String> log;         // The log library
        private static PerformanceCounter pc = new PerformanceCounter("Memory", "% Committed Bytes In Use");    // A performance counter object to obtain memory usage

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
            this.LabelInterval = new System.Windows.Forms.Label();
            this.labelLocation = new System.Windows.Forms.Label();
            this.txtboxInterval = new System.Windows.Forms.TextBox();
            this.txtboxLocation = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LabelInterval
            // 
            this.LabelInterval.AutoSize = true;
            this.LabelInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelInterval.Location = new System.Drawing.Point(32, 27);
            this.LabelInterval.Name = "LabelInterval";
            this.LabelInterval.Size = new System.Drawing.Size(158, 25);
            this.LabelInterval.TabIndex = 0;
            this.LabelInterval.Text = "Log Interval (ms)";
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.labelLocation.Location = new System.Drawing.Point(37, 104);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(160, 25);
            this.labelLocation.TabIndex = 1;
            this.labelLocation.Text = "Log File Location";
            // 
            // txtboxInterval
            // 
            this.txtboxInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.txtboxInterval.Location = new System.Drawing.Point(218, 31);
            this.txtboxInterval.Name = "txtboxInterval";
            this.txtboxInterval.Size = new System.Drawing.Size(63, 30);
            this.txtboxInterval.TabIndex = 2;
            // 
            // txtboxLocation
            // 
            this.txtboxLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.txtboxLocation.Location = new System.Drawing.Point(218, 108);
            this.txtboxLocation.Name = "txtboxLocation";
            this.txtboxLocation.Size = new System.Drawing.Size(271, 30);
            this.txtboxLocation.TabIndex = 3;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.btnBrowse.Location = new System.Drawing.Point(555, 108);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(101, 30);
            this.btnBrowse.TabIndex = 4;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new EventHandler(btnBrowse_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.btnStart.Location = new System.Drawing.Point(583, 153);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(143, 45);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start Log";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new EventHandler(btnStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 210);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtboxLocation);
            this.Controls.Add(this.txtboxInterval);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.LabelInterval);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelInterval;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.TextBox txtboxInterval;
        private System.Windows.Forms.TextBox txtboxLocation;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnStart;

        // The Event Handler for the Start Log button
        // If the interval and location is valid, then set the logTimer to 0, create a new log, and start the timer
        private void btnStart_Click(object sender, System.EventArgs e)
        {
            string location = txtboxLocation.Text;
            string interval_string = txtboxInterval.Text;

            if(Int32.TryParse(interval_string, out interval))
            {
                if (interval <= 0 || location == "")    // Check that the interval and locations are valid
                {
                    // Invalid interval or location
                }
                else
                {
                    // Interval parsing has passed and the number is valid, create the log and start collecting data
                    logTimer = 0;
                    log = new DataLog<String>(location);
                    timer = new System.Timers.Timer(interval);
                    timer.Elapsed += OnTimerEvent;
                    timer.AutoReset = true;
                    timer.Enabled = true;

                }
            }
            else
            {
                // parse has failed
            }


        }

        // The Event Handler for the Browse button
        // Opens a Browse file window and allows the user to select the location for the log file
        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            string location = "";

            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.ShowNewFolderButton = false;
            fbd.RootFolder = System.Environment.SpecialFolder.MyComputer;
            DialogResult dr = fbd.ShowDialog();

            if(dr == DialogResult.OK)
            {
                location = fbd.SelectedPath;
            }

            if(location != "")
            {
                txtboxLocation.Text = location;
            }
        }

        // The Event Handler for the timer
        // At every interval, add the interval time to the logTimer and record data into the log
        // If the logTimer is greater than 10 seconds, then stop the timer and close the log
        private static void OnTimerEvent(object sender, System.EventArgs e)
        {
            logTimer += interval;

            log.writeToLog("Memory Usage: " + pc.NextValue() + "%       " + DateTime.Now.ToString());
            if(logTimer > 10000)
            {
                timer.Stop();
                timer.Dispose();
                MessageBox.Show("Log Complete");
                log.closeLog();
            }
        }
        
    }
}

