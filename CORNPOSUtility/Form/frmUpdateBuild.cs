using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using System.Data.SQLite;
using System.Threading;
using System.Management;
using System.IO;

namespace CORNPOSUtility
{
    public partial class frmUpdateBuild : Form
    {
        private BackgroundWorker backgroundWorker;
        string sourceFolderPath = "";
        string destinationFolderPath = "";
        string binFolder = "bin";
        string FormsFolder = "Forms";
        string AjaxLibraryFolder = "AjaxLibrary";
        string LoginFile = "Login.aspx";
        string LoginMasterFile = "LoginMaster.master";
        string packagesFile = "packages.config";
        string PrecompiledAppFile = "PrecompiledApp.config";
        string FileCopyName = "";

        public frmUpdateBuild(Main Parent)
        {
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (cbFile.Checked)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtFrom.Text = dialog.FileName;
                    FileCopyName = dialog.SafeFileName;
                }
            }
            else
            {
                FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
                folderBrowser.SelectedPath = txtFrom.Text;
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    txtFrom.Text = folderBrowser.SelectedPath;
                }
            }
        }

        private void btnBrowseTo_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.SelectedPath = txtTo.Text;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                txtTo.Text = folderBrowser.SelectedPath;
                cblBuilds.Items.Clear();
                string[] folderNames = Directory.GetDirectories(txtTo.Text)
                                           .Select(dir => new DirectoryInfo(dir).Name)
                                           .ToArray();
                foreach (string folderName in folderNames)
                {
                    cblBuilds.Items.Add(folderName);
                }
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            var confirmResult = ZKMessgeBox.Show("Are you sure?", "Confirm Build Upload!!", ZKMessgeBox.I8Buttons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                richStatus.Text = string.Empty;
                if (txtFrom.Text.Trim().Length > 0 && txtTo.Text.Trim().Length > 0)
                {
                    backgroundWorker.RunWorkerAsync();
                }
                else
                {
                    ZKMessgeBox.Show("Enter Build from and to path!!");
                }
            }
        }

        private void txtTo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cblBuilds.Items.Clear();
                try
                {
                    string[] folderNames = Directory.GetDirectories(txtTo.Text)
                                           .Select(dir => new DirectoryInfo(dir).Name)
                                           .ToArray();
                    lblTotal.Text = "Total: " + folderNames.Length.ToString();
                    foreach (string folderName in folderNames)
                    {
                        if (folderName != "Lasani" && folderName != "NY212" && folderName != "BrimBurger" && folderName != "Bagh" && folderName != "Gauchos" && folderName != "RetroBar" && folderName != "RiceBowl")
                        {
                            cblBuilds.Items.Add(folderName);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        static void CopyFolder(string sourceFolder, string destinationFolder)
        {
            // Create the destination folder if it doesn't exist
            if (!Directory.Exists(destinationFolder))
            {
                Directory.CreateDirectory(destinationFolder);
            }

            // Copy all files
            foreach (string filePath in Directory.GetFiles(sourceFolder))
            {
                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(destinationFolder, fileName);
                File.Copy(filePath, destFilePath, true); // Overwrite if exists
            }

            // Copy all subdirectories
            foreach (string subfolderPath in Directory.GetDirectories(sourceFolder))
            {
                string folderName = Path.GetFileName(subfolderPath);
                string destSubfolderPath = Path.Combine(destinationFolder, folderName);
                CopyFolder(subfolderPath, destSubfolderPath);
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> list = new List<string>();
            for (int x = 0; x < cblBuilds.CheckedItems.Count; x++)
            {
                list.Add(cblBuilds.CheckedItems[x].ToString());
            }
            if (list.Count > 0)
            {
                sourceFolderPath = txtFrom.Text;
                destinationFolderPath = txtFrom.Text;
                foreach (string folder in list)
                {
                    if (cbFile.Checked)
                    {
                        //Copy selected file from source to destination
                        File.Copy(sourceFolderPath, Path.Combine(destinationFolderPath, FileCopyName), true); // Overwrite if exists                    
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            richStatus.Text += folder + "-" + FileCopyName + " file copied" + Environment.NewLine;
                        }));
                    }
                    else
                    {
                        destinationFolderPath = Path.Combine(txtTo.Text, folder);
                        string[] BuildFolder = Directory.GetDirectories(destinationFolderPath);
                        if (BuildFolder.Length > 0)
                        {
                            destinationFolderPath = Path.Combine(destinationFolderPath, BuildFolder[0]);
                        }
                        //Delete bin folder on destination
                        if (Directory.Exists(Path.Combine(destinationFolderPath, binFolder)))
                        {
                            Directory.Delete(Path.Combine(destinationFolderPath, binFolder), true);
                        }
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            richStatus.Text += folder + "-bin folder deleted" + Environment.NewLine;
                        }));

                        try
                        {
                            //Copy bin folder from source to destination
                            CopyFolder(Path.Combine(sourceFolderPath, binFolder), Path.Combine(destinationFolderPath, binFolder));
                            this.Invoke(new MethodInvoker(delegate ()
                            {
                                richStatus.Text += folder + "-bin folder copied" + Environment.NewLine;
                            }));
                        }
                        catch (Exception ex)
                        {
                            this.Invoke(new MethodInvoker(delegate ()
                            {
                                richStatus.Text += ex.ToString() + Environment.NewLine;
                            }));
                        }

                        //Delete Forms folder on destination
                        if (Directory.Exists(Path.Combine(destinationFolderPath, FormsFolder)))
                        {
                            Directory.Delete(Path.Combine(destinationFolderPath, FormsFolder), true);
                        }
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            richStatus.Text += folder + "-Forms folder deleted" + Environment.NewLine;
                        }));
                        //Copy Forms folder from source to destination
                        CopyFolder(Path.Combine(sourceFolderPath, FormsFolder), Path.Combine(destinationFolderPath, FormsFolder));
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            richStatus.Text += folder + "-Forms folder copied" + Environment.NewLine;
                        }));

                        //Delete AjaxLibrary folder on destination
                        if (Directory.Exists(Path.Combine(destinationFolderPath, AjaxLibraryFolder)))
                        {
                            Directory.Delete(Path.Combine(destinationFolderPath, AjaxLibraryFolder), true);
                        }
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            richStatus.Text += folder + "-AjaxLibrary folder deleted" + Environment.NewLine;
                        }));

                        //Copy AjaxLibrary folder from source to destination
                        CopyFolder(Path.Combine(sourceFolderPath, AjaxLibraryFolder), Path.Combine(destinationFolderPath, AjaxLibraryFolder));
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            richStatus.Text += folder + "-AjaxLibrary folder copied" + Environment.NewLine;
                        }));

                        //Copy Login file from source to destination
                        File.Copy(Path.Combine(sourceFolderPath, LoginFile), Path.Combine(destinationFolderPath, LoginFile), true); // Overwrite if exists
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            richStatus.Text += folder + "-Login file copied" + Environment.NewLine;
                        }));

                        //Copy LoginMaster file from source to destination
                        File.Copy(Path.Combine(sourceFolderPath, LoginMasterFile), Path.Combine(destinationFolderPath, LoginMasterFile), true); // Overwrite if exists
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            richStatus.Text += folder + "-LoginMaster file copied" + Environment.NewLine;
                        }));

                        //Copy packages file from source to destination
                        File.Copy(Path.Combine(sourceFolderPath, packagesFile), Path.Combine(destinationFolderPath, packagesFile), true); // Overwrite if exists
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            richStatus.Text += folder + "-packages file copied" + Environment.NewLine;
                        }));

                        //Copy PrecompiledApp file from source to destination
                        File.Copy(Path.Combine(sourceFolderPath, PrecompiledAppFile), Path.Combine(destinationFolderPath, PrecompiledAppFile), true); // Overwrite if exists                    
                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            richStatus.Text += folder + "-PrecompiledApp file copied" + Environment.NewLine;
                        }));

                        this.Invoke(new MethodInvoker(delegate ()
                        {
                            richStatus.Text += folder + " Build done" + Environment.NewLine;
                        }));
                    }
                }
                this.Invoke(new MethodInvoker(delegate ()
                {
                    richStatus.Text += list.Count.ToString() + " Build(s) completed";
                }));
            }
            else
            {
                ZKMessgeBox.Show("No path selected to copy build");
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ZKMessgeBox.Show("Build(s) uploading done.");
        }

        private void cblBuilds_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = 0;
            for (int x = 0; x < cblBuilds.CheckedItems.Count; x++)
            {
                count++;
            }

            lblTotal.Text = "Total: " + count.ToString();
        }

        private void cbFile_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFile.Checked)
            {
                lblFrom.Text = "File Path";
            }
            else
            {
                lblFrom.Text = "Build Path";
            }
        }
    }
}