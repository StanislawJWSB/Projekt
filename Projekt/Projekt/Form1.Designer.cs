using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

public partial class Form1 : Form
{
    private List<string> selectedFiles = new List<string>();

    public Form1()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.btnSelectFiles = new System.Windows.Forms.Button();
        this.btnEncrypt = new System.Windows.Forms.Button();
        this.btnDecrypt = new System.Windows.Forms.Button();
        this.btnSendToServer = new System.Windows.Forms.Button();
        this.lbFiles = new System.Windows.Forms.ListBox();
        this.progressBar = new System.Windows.Forms.ProgressBar();
        this.SuspendLayout();
        // 
        // btnSelectFiles
        // 
        this.btnSelectFiles.Location = new System.Drawing.Point(13, 13);
        this.btnSelectFiles.Name = "btnSelectFiles";
        this.btnSelectFiles.Size = new System.Drawing.Size(75, 23);
        this.btnSelectFiles.TabIndex = 0;
        this.btnSelectFiles.Text = "Select Files";
        this.btnSelectFiles.UseVisualStyleBackColor = true;
        this.btnSelectFiles.Click += new System.EventHandler(this.btnSelectFiles_Click);
        // 
        // btnEncrypt
        // 
        this.btnEncrypt.Location = new System.Drawing.Point(94, 13);
        this.btnEncrypt.Name = "btnEncrypt";
        this.btnEncrypt.Size = new System.Drawing.Size(75, 23);
        this.btnEncrypt.TabIndex = 1;
        this.btnEncrypt.Text = "Encrypt";
        this.btnEncrypt.UseVisualStyleBackColor = true;
        this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
        // 
        // btnDecrypt
        // 
        this.btnDecrypt.Location = new System.Drawing.Point(175, 13);
        this.btnDecrypt.Name = "btnDecrypt";
        this.btnDecrypt.Size = new System.Drawing.Size(75, 23);
        this.btnDecrypt.TabIndex = 2;
        this.btnDecrypt.Text = "Decrypt";
        this.btnDecrypt.UseVisualStyleBackColor = true;
        this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
        // 
        // btnSendToServer
        // 
        this.btnSendToServer.Location = new System.Drawing.Point(256, 13);
        this.btnSendToServer.Name = "btnSendToServer";
        this.btnSendToServer.Size = new System.Drawing.Size(75, 23);
        this.btnSendToServer.TabIndex = 3;
        this.btnSendToServer.Text = "Send to Server";
        this.btnSendToServer.UseVisualStyleBackColor = true;
        this.btnSendToServer.Click += new System.EventHandler(this.btnSendToServer_Click);
        // 
        // lbFiles
        // 
        this.lbFiles.FormattingEnabled = true;
        this.lbFiles.Location = new System.Drawing.Point(13, 42);
        this.lbFiles.Name = "lbFiles";
        this.lbFiles.Size = new System.Drawing.Size(318, 199);
        this.lbFiles.TabIndex = 4;
        // 
        // progressBar
        // 
        this.progressBar.Location = new System.Drawing.Point(13, 247);
        this.progressBar.Name = "progressBar";
        this.progressBar.Size = new System.Drawing.Size(318, 23);
        this.progressBar.TabIndex = 5;
        // 
        // MainForm
        // 
        this.ClientSize = new System.Drawing.Size(343, 282);
        this.Controls.Add(this.progressBar);
        this.Controls.Add(this.lbFiles);
        this.Controls.Add(this.btnSendToServer);
        this.Controls.Add(this.btnDecrypt);
        this.Controls.Add(this.btnEncrypt);
        this.Controls.Add(this.btnSelectFiles);
        this.Name = "MainForm";
        this.Text = "File Encryptor";
        this.ResumeLayout(false);
    }

    private System.Windows.Forms.Button btnSelectFiles;
    private System.Windows.Forms.Button btnEncrypt;
    private System.Windows.Forms.Button btnDecrypt;
    private System.Windows.Forms.Button btnSendToServer;
    private System.Windows.Forms.ListBox lbFiles;
    private System.Windows.Forms.ProgressBar progressBar;

    private void btnSelectFiles_Click(object sender, EventArgs e)
    {
        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFiles.Clear();
                lbFiles.Items.Clear();
                foreach (string file in openFileDialog.FileNames)
                {
                    selectedFiles.Add(file);
                    lbFiles.Items.Add(file);
                }
            }
        }
    }

    private async void btnEncrypt_Click(object sender, EventArgs e)
    {
        progressBar.Value = 0;
        var encryptor = new FileEncryptor("your-encryption-key", "your-iv");
        await Task.Run(() =>
        {
            foreach (var file in selectedFiles)
            {
                string outputFile = file + ".enc";
                encryptor.EncryptFile(file, outputFile);
                this.Invoke((Action)(() => progressBar.Value += (100 / selectedFiles.Count)));
            }
        });
        MessageBox.Show("Encryption complete.");
    }

    private async void btnDecrypt_Click(object sender, EventArgs e)
    {
        progressBar.Value = 0;
        var encryptor = new FileEncryptor("your-encryption-key", "your-iv");
        await Task.Run(() =>
        {
            foreach (var file in selectedFiles)
            {
                string outputFile = file.Replace(".enc", ".dec");
                encryptor.DecryptFile(file, outputFile);
                this.Invoke((Action)(() => progressBar.Value += (100 / selectedFiles.Count)));
            }
        });
        MessageBox.Show("Decryption complete.");
    }

    private async void btnSendToServer_Click(object sender, EventArgs e)
    {
        progressBar.Value = 0;
        var communicator = new ServerCommunicator("http://your-server-url");
        await Task.Run(async () =>
        {
            foreach (var file in selectedFiles)
            {
                await communicator.SendFileAsync(file);
                this.Invoke((Action)(() => progressBar.Value += (100 / selectedFiles.Count)));
            }
        });
        MessageBox.Show("Files sent to server.");
    }
}
