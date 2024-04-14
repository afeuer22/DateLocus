using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace RFID_Window
{
    public partial class Form1 : Form
    {

        public Stopwatch watch;
        public StreamReader sr = new StreamReader(@".\AuthorizedCards.txt");
        public string[] FileStrings = null ;
        public string[] Names = null;
        public string[] UIDs = null;
        public Form1()
        {
            InitializeComponent();
            // port.Open();
            label1.Text = "Scan Card";
        }
        private void port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string tagID = port.ReadLine();
            Debug.WriteLine(tagID);
            tagID = tagID.Substring(4,8);
            watch = Stopwatch.StartNew();
            CreateArrays();
            if (!CheckCard(tagID)) // if UID is not in file
            {
                label1.Invoke(new MethodInvoker(delegate { label1.Text = "ACCESS DENIED"; }));
                while(watch.ElapsedMilliseconds <3000){}   
            }
            else
            { 
               label1.Invoke(new MethodInvoker(delegate { label1.Text = "ACCESS GRANTED"; }));
               port.Write("G");
            }
            
        }
        public void CreateArrays()
        {
            FileStrings = sr.ReadLine().Split(' ');
            int counter = 0;
            foreach (string s in FileStrings)
            { 
                if(counter%2 == 0)
                {
                    Names[counter/2] = s;
                }
                else
                {
                    UIDs[(counter-1)/2] = s;
                }
                counter++;
            }
        }

        public Boolean CheckCard(string tagID)
        {
            foreach(string s in UIDs)
            {
                if (s.Equals(tagID))
                {
                    return true;
                }
            }
            return false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.F2) {
                Form2 f2 = new Form2();
                sr.Close();
                f2.Show();
                this.Hide();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
