/*
 * Created by SharpDevelop.
 * User: Zsolt
 * Date: 2017.07.04.
 * Time: 23:47
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Win32;
using WMPLib;

namespace mp3
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{

        WindowsMediaPlayer wplayer = new WindowsMediaPlayer();

        public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
            //check wmp version
			RegistryKey myKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\MediaPlayer\\PlayerUpgrade", false);
			string version = (string) myKey.GetValue("PlayerVersion");
			
			version = version.Substring(0, 2);
			int versionNumber = Int32.Parse(version);
			if(versionNumber < 10) {
				lejatszas.Visible = false;
			}

            //listview drag&drop
            listView1.AllowDrop = true;
            listView1.DragDrop += listViewFiles_DragDrop;
            listView1.DragEnter += listViewFiles_DragEnter;
        }
		void Button1Click(object sender, EventArgs e)
		{
            wplayer.PlayStateChange +=
                new WMPLib._WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
            wplayer.MediaError +=
                new WMPLib._WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);

            //create a playlist
            wplayer.currentPlaylist = wplayer.newPlaylist("playlist", "");            
            foreach (ListViewItem itemRow in listView1.Items)
            {
                for (int i = 0; i < itemRow.SubItems.Count; i++)
                {
                    string song_path = itemRow.SubItems[i].Text;
                    //Console.WriteLine(itemRow.SubItems[i].Text);
                    wplayer.currentPlaylist.appendItem(wplayer.newMedia(song_path));
                }
            }

            //load the file
            //wplayer.URL = "e:\\Famous Last Words - Council Of The Dead (2014) 320\\02.Council of the Dead.mp3";
            //base volume
            wplayer.settings.volume = 50;
            //let it rock
            wplayer.controls.play();
            
		}

        private void megallitas_Click(object sender, EventArgs e)
        {
            //stop
            wplayer.controls.stop();
        }

        /*
            0	Undefined	Windows Media Player is in an undefined state.
            1	Stopped	Playback of the current media item is stopped.
            2	Paused	Playback of the current media item is paused. When a media item is paused, resuming playback begins from the same location.
            3	Playing	The current media item is playing.
            4	ScanForward	The current media item is fast forwarding.
            5	ScanReverse	The current media item is fast rewinding.
            6	Buffering	The current media item is getting additional data from the server.
            7	Waiting	Connection is established, but the server is not sending data. Waiting for session to begin.
            8	MediaEnded	Media item has completed playback.
            9	Transitioning	Preparing new media item.
            10	Ready	Ready to begin playing.
            11	Reconnecting	Reconnecting to stream.
             
             */

        private void Player_PlayStateChange(int NewState)
        {
            if ((WMPPlayState)NewState == WMPPlayState.wmppsPlaying)
            {
                //if started, the button is replaced with 'stop'
                lejatszas.Visible = false;
                megallitas.Visible = true;

                //progressbar
                double dur = wplayer.currentMedia.duration;
                progressBar1.Maximum = (int)dur;

                //duration
                label2.Text = wplayer.currentMedia.durationString;
                
                //artist
                label3.Text = "Előadó: " + wplayer.currentMedia.getItemInfo("Artist");

                //song
                label4.Text = "Szám / Album: " + wplayer.currentMedia.getItemInfo("Title") + " (" + wplayer.currentMedia.getItemInfo("Album") + ")";

                //highlight
                foreach (ListViewItem itemRow in listView1.Items)
                {
                    for (int i = 0; i < itemRow.SubItems.Count; i++)
                    {
                        string song_path = itemRow.SubItems[i].Text;
                        //Console.WriteLine(itemRow.SubItems[i].Text);
                        if (song_path == wplayer.currentMedia.getItemInfo("SourceURL"))
                        {
                            itemRow.SubItems[i].BackColor = Color.Red;
                        }
                    }
                }

            } else if ((WMPPlayState)NewState == WMPPlayState.wmppsStopped)
            {
                //close
                this.Close();
            } else
            {

            }

        }

        private void Player_MediaError(object pMediaObject)
        {
            //error messages
            MessageBox.Show("Cannot play media file.");
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //current position of the song
            //update time and progressbar both
            label1.Text = wplayer.controls.currentPositionString;
            progressBar1.Value = (int)wplayer.controls.currentPosition;
        }

        private void plus_Click(object sender, EventArgs e)
        {

            //+volume
            wplayer.settings.volume++;
            if (wplayer.settings.volume == 100)
            {
                plus.Enabled = false;
            } 
            if(minus.Enabled == false)
            {
                minus.Enabled = true;
            }
        }

        private void minus_Click(object sender, EventArgs e)
        {

            //-volume
            wplayer.settings.volume--;
            if(wplayer.settings.volume == 0)
            {
                minus.Enabled = false;
            }
            if (plus.Enabled == false)
            {
                plus.Enabled = true;
            }
        }

        private void listViewFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void listViewFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {

                if (System.IO.Path.GetExtension(file).Equals(".mp3", StringComparison.InvariantCultureIgnoreCase))
                {
                    //add the file
                    listView1.Items.Add(file);
                }
                else
                {
                    MessageBox.Show("Must be an mp3!");
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //next
            wplayer.controls.next();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //previous
            wplayer.controls.previous();
        }
    }
}
