using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

using NAudio.Midi;
using NAudio.Wave;

using Jacobi.Vst.Core;
using System.Threading;


using B.Note;
using B.Inrerface;

using System.Windows.Forms.Integration;

namespace B.GUI
{
	public partial class MainForm : Form
	{
		VSTForm vstForm = null;
		List<INSNEvent> NSNEvents;
		MidiIn midiIn;
		MidiOut midiOut;

		public static Dictionary<string, string> LastDirectoryUsed = new Dictionary<string, string>();

		public MainForm()
		{
			this.NSNEvents = new List<INSNEvent>();
			InitializeComponent();

			for (int device = 0; device < MidiIn.NumberOfDevices; device++)
			{
				comboBoxMidiInDevices.Items.Add(MidiIn.DeviceInfo(device).ProductName);
			}
			if (comboBoxMidiInDevices.Items.Count > 0)
			{
				comboBoxMidiInDevices.SelectedIndex = 0;
			}
			for (int device = 0; device < MidiOut.NumberOfDevices; device++)
			{
				comboBoxMidiOutDevices.Items.Add(MidiOut.DeviceInfo(device).ProductName);
			}
			if (comboBoxMidiOutDevices.Items.Count > 0)
			{
				comboBoxMidiOutDevices.SelectedIndex = 0;
			}

			if (comboBoxMidiInDevices.Items.Count == 0)
			{
				MessageBox.Show("No MIDI input devices available");
			}
			else
			{
				if (midiIn == null)
				{
					midiIn = new MidiIn(comboBoxMidiInDevices.SelectedIndex);
					midiIn.ErrorReceived += new EventHandler<MidiInMessageEventArgs>(midiIn_ErrorReceived);
				}
				midiIn.Start();
				comboBoxMidiInDevices.Enabled = false;
			}

			if (comboBoxMidiOutDevices.Items.Count == 0)
			{
				MessageBox.Show("No MIDI output devices available");
			}
			else
			{
				if (midiOut == null)
				{
					midiOut = new MidiOut(comboBoxMidiOutDevices.SelectedIndex);
				}
			}

			InitialiseAsioControls();
		}

		private void InitialiseAsioControls()
		{
			var asioDriverNames = AsioOut.GetDriverNames();
			foreach (string driverName in asioDriverNames)
			{
				comboBoxAudioOutDevices.Items.Add(driverName);
			}
			if (comboBoxAudioOutDevices.Items.Count > 0)
			{
				comboBoxAudioOutDevices.SelectedIndex = 0;
			}
		}

		void LoadToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (vstForm != null)
			{
				vstForm.Dispose();
				vstForm = null;

				showToolStripMenuItem.Enabled = false;
				editParametersToolStripMenuItem.Enabled = false;
				loadToolStripMenuItem.Text = "Load...";
			}
			else
			{
				var ofd = new OpenFileDialog();
				ofd.Title = "Select VST:";
				ofd.Filter = "VST Files (*.dll)|*.dll";
				if (LastDirectoryUsed.ContainsKey("VSTDir"))
				{
					ofd.InitialDirectory = LastDirectoryUsed["VSTDir"];
				}
				else
				{
					ofd.InitialDirectory = UtilityAudio.GetVSTDirectory();
				}
				DialogResult res = ofd.ShowDialog();

				if (res != DialogResult.OK || !File.Exists(ofd.FileName)) return;

				try
				{
					if (LastDirectoryUsed.ContainsKey("VSTDir"))
					{
						LastDirectoryUsed["VSTDir"] = Directory.GetParent(ofd.FileName).FullName;
					}
					else
					{
						LastDirectoryUsed.Add("VSTDir", Directory.GetParent(ofd.FileName).FullName);
					}
					vstForm = new VSTForm(ofd.FileName, comboBoxAudioOutDevices.Text);
					vstForm.Show();

					showToolStripMenuItem.Enabled = true;
					editParametersToolStripMenuItem.Enabled = true;

					loadToolStripMenuItem.Text = "Unload...";
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}

		}

		void ShowToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (vstForm != null)
			{
				if (vstForm.Visible) vstForm.BringToFront();
				else vstForm.Visible = true;
			}
		}

		void EditParametersToolStripMenuItemClick(object sender, EventArgs e)
		{
			if (vstForm != null)
				vstForm.ShowEditParameters();
		}


		void SelectMIDIINToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			if (selectMIDIINToolStripMenuItem.Checked)
				comboBoxMidiInDevices.Enabled = true;
			else
			{
				comboBoxMidiInDevices.Enabled = false;
			}

		}

		void SelectMIDIOUTToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			if (selectMIDIOUTToolStripMenuItem.Checked)
				comboBoxMidiOutDevices.Enabled = true;
			else
			{
				comboBoxMidiOutDevices.Enabled = false;
			}
		}

		void SelectAudioOutputDeviceToolStripMenuItemCheckedChanged(object sender, EventArgs e)
		{
			if (selectAudioOutputDeviceToolStripMenuItem.Checked)
				comboBoxAudioOutDevices.Enabled = true;
			else
			{
				comboBoxAudioOutDevices.Enabled = false;
			}
		}

		void TscMIDIINSelectedIndexChanged(object sender, EventArgs e)
		{
		}

		void TscMIDIOUTSelectedIndexChanged(object sender, EventArgs e)
		{
		}

		void TscASIOOutSelectedIndexChanged(object sender, EventArgs e)
		{
		}

		void midiIn_ErrorReceived(object sender, MidiInMessageEventArgs e)
		{
			//progressLog1.LogMessage(Color.Red, String.Format("Time {0} Message 0x{1:X8} Event {2}",
															 //e.Timestamp, e.RawMessage, e.MidiEvent));
		}

		void SoundNSNEvents()
		{
			int time = 0;
			foreach (var xe in NSNEvents)
			{

				while (time < xe.StartTime)
				{
					Thread.Sleep(100);
					time += 100;
				}
				time = 0;

				Thread newThread = new Thread(SoundNSNEvent);
				//newThread.IsBackground = false;
				newThread.Start(xe);
				newThread.Priority = ThreadPriority.Highest;
				newThread.Join();
			}

			
		}
		void SoundNSNEvent(object e)
			{

				if (!(e is NSNEvent)) { return; }
				Thread newThread2 = new Thread(SetPitchWeel);
				//newThread2.IsBackground = false;new
				newThread2.Priority = ThreadPriority.Highest;

				newThread2.Start(e);
				newThread2.Join();

				var xe = (NSNEvent)e;
				var curNote = xe.Note;
				int curNoteClosestAnalog = curNote.NoteLogTemper12ClosestAnalog;
				var note12Freq = xe.Note.Notes12Freq;

				VSTForm.vst.MIDI_NoteOn((byte)curNote.NoteLogTemper12ClosestAnalog, (byte)xe.Velocity);
				Thread.Sleep((int)xe.Duration);
				VSTForm.vst.MIDI_NoteOn((byte)curNote.NoteLogTemper12ClosestAnalog, 0);
			}
		void ButtonClearLogClick(object sender, EventArgs e)
		{
			using (StreamReader sr = new StreamReader("1"))
			{
				NSNEvents.Clear();
				int i = Convert.ToInt32(sr.ReadLine());
				for (; i > 0; i--)
				{
					int note = Convert.ToInt32(sr.ReadLine());
					int start = Convert.ToInt32(sr.ReadLine());
					int duration = Convert.ToInt32(sr.ReadLine());
					NSNEvents.Add(new NSNEvent(new NoteLogTemper53(note), 100, start, duration));
				}
				sr.Close();
			}
			NSNEvents.Reverse();
			Thread myThread = new Thread(SoundNSNEvents);
			//myThread.IsBackground = false;
			myThread.Start();
			
		}
		void SetPitchWeel(object e)

		{
			if (!(e is NSNEvent)) { return; }
			var xe = (NSNEvent)e;
			var curNote = xe.Note;
			int curNoteClosestAnalog = curNote.NoteLogTemper12ClosestAnalog;
			var note12Freq = xe.Note.Notes12Freq;
			float deltaFreq = (float)note12Freq[curNoteClosestAnalog] - curNote.Frequency;
			int pitch = 8192;
			float delta12NoteFreq = 0;
			if (deltaFreq > 0) // 12Note > 53Note
			{
				delta12NoteFreq = (float)note12Freq[curNoteClosestAnalog] - (float)note12Freq[curNoteClosestAnalog - 1];
				pitch = 8192 - (int)(deltaFreq / (delta12NoteFreq / 4096));
			}
			else if (deltaFreq < 0)
			{
				delta12NoteFreq = (float)note12Freq[curNoteClosestAnalog + 1] - (float)note12Freq[curNoteClosestAnalog];
				pitch = 8192 - (int)(deltaFreq / (delta12NoteFreq / 4096));
			}
			if (pitch > 16384) { pitch = 16384; }
			if (pitch < 0) { pitch = 0; }
			var midiData = new byte[] {
						0xE0, 							// Cmd
						(byte)((int)pitch & 0x7f),		// Val 1
						(byte)(((int)pitch >> 7) & 0x7f),	// Val 2
					};
			var vstMidiEvent = new VstMidiEvent(/*DeltaFrames*/ 0,
							 /*NoteLength*/ 0,
							 /*NoteOffset*/ 0,
							 midiData,
							 /*Detune*/ 0,
							 /*NoteOffVelocity*/ 0);
			var ve = new VstEvent[1];
			ve[0] = vstMidiEvent;
	
			VSTForm.vst.pluginContext.PluginCommandStub.ProcessEvents(ve);

		}


		void MainFormFormClosing(object sender, FormClosingEventArgs e)
		{
			if (midiIn != null)
			{
				midiIn.Dispose();
				midiIn = null;
			}
			if (midiOut != null)
			{
				midiOut.Dispose();
				midiOut = null;
			}
			if (vstForm != null)
			{
				vstForm.Dispose();
				vstForm = null;
			}
			UtilityAudio.Dispose();
		}
		private void button1_Click(object sender, EventArgs e)
        {
			var wpfwindow = new Piano.PianoRoll();
			ElementHost.EnableModelessKeyboardInterop(wpfwindow);
			wpfwindow.Show();
		}
    }
	}


	
