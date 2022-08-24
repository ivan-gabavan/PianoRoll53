using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio;
using NAudio.Midi;
using Microsoft.Win32;


namespace Sequencer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class PianoRoll : UserControl
    {
        MidiEventCollection midiEvents;
        double xScale = 1.0 / 10;
        double yScale = 15;

        public MidiEventCollection MidiEvents
        {
            get
            {
                return midiEvents;
            }
            set
            {
                // a quarter note is 20 units wide
                xScale = (20.0 / value.DeltaTicksPerQuarterNote);
                midiEvents = value;
                NoteCanvas.Children.Clear();

                long lastPosition = 0;
                for (int track = 0; track < midiEvents.Tracks; track++)
                {
                    foreach (MidiEvent midiEvent in value[track])
                    {
                        if (midiEvent.CommandCode == MidiCommandCode.NoteOn)
                        {
                            NoteOnEvent noteOn = (NoteOnEvent)midiEvent;
                            if (noteOn.OffEvent != null)
                            {
                                Rectangle rectangle = MakeNoteRectangle(noteOn.NoteNumber, noteOn.AbsoluteTime, noteOn.NoteLength, noteOn.Channel);
                                NoteCanvas.Children.Add(rectangle);
                                lastPosition = Math.Max(lastPosition, noteOn.AbsoluteTime + noteOn.NoteLength);
                            }
                        }
                    }
                }
                this.Width = lastPosition * xScale;
                this.Height = 128 * yScale;
            }
        }
        private Rectangle MakeNoteRectangle(int noteNumber, long startTime, int duration, int channel)
        {
            Rectangle rect = new Rectangle();
            if (channel == 10)
            {
                rect.Stroke = new SolidColorBrush(Colors.DarkGreen);
                rect.Fill = new SolidColorBrush(Colors.LightGreen);
                duration = midiEvents.DeltaTicksPerQuarterNote / 4;
            }
            else
            {
                rect.Stroke = new SolidColorBrush(Colors.DarkBlue);
                rect.Fill = new SolidColorBrush(Colors.LightBlue);
            }
            rect.Width = (double)duration * xScale;
            rect.Height = yScale;
            rect.SetValue(Canvas.TopProperty, (double)(127 - noteNumber) * yScale);
            rect.SetValue(Canvas.LeftProperty, (double)startTime * xScale);
            return rect;
        }
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "MIDI Files (*.mid)|*.mid|All Files (*.*)|*.*";
            /*openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog().Value)
            {
                MidiFile midiFile = new MidiFile(openFileDialog.FileName);
                this.PianoRollControl.MidiEvents = midiFile.Events;
            }*/
        }
    }
}