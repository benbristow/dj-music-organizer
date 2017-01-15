using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Data;
using System.Threading.Tasks;

namespace DJ_Music_Organizer
{
    public partial class Form1 : Form
    {
        private Song[] songs;

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_selectMusicDirectory_Click(object sender, EventArgs e)
        {
            selectFolderAndGetAllSongs();
            toolStripStatusLabel1.Text = String.Format("Selected {0} songs", this.songs.Length);
            btn_Process.Enabled = true;
        }

        private void btn_Process_Click(object sender, EventArgs e)
        {
            btn_Process.Enabled = false;
            btn_selectMusicDirectory.Enabled = false;
            toolStripStatusLabel1.Text = String.Format("Processing {0} songs", this.songs.Length);
            songProcessor.RunWorkerAsync();           
        }

        private void selectFolderAndGetAllSongs()
        {
            string directoryPath = selectFolderFromDirectoryDialog();
            if(Directory.Exists(directoryPath)) {
                DirectoryInfo musicDirectory = new DirectoryInfo(directoryPath);
                FileInfo[] musicFiles = musicDirectory.GetFiles("*.mp3", SearchOption.AllDirectories);
                this.songs = musicFiles.Select(file => new Song(file)).ToArray<Song>();
            } else
            {
                this.songs = new Song[0];
            }
        }

        private string selectFolderFromDirectoryDialog()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowNewFolderButton = false;
            folderBrowserDialog.ShowDialog();
            txt_musicDirectory.Text = folderBrowserDialog.SelectedPath;
            return folderBrowserDialog.SelectedPath;
        }

        private void songProcessor_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            for (int i = 0; i < this.songs.Length; i++)
            {
                Song song = this.songs[i];
                song.setGenre();
                songProcessor.ReportProgress((int)100.0 * (i + 1) / this.songs.Length);
            }
        }

        private void songProcessor_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            btn_Process.Enabled = true;
            btn_selectMusicDirectory.Enabled = true;      
            toolStripProgressBar1.Value = 100;
            toolStripStatusLabel1.Text = String.Format("Finished processing {0} songs", this.songs.Length);
            MessageBox.Show("Finished processing!", "Finished Processing", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void songProcessor_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }
    }
}
