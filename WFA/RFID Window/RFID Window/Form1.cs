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
        public Form1()
        {
            InitializeComponent();
            port.Open();
            label1.Invoke(new MethodInvoker(delegate { label1.Text = "Scan Card"; }));
        }
        private void port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string tagID = port.ReadLine();
            Debug.WriteLine(tagID);
            tagID = tagID.Substring(4,8);
            watch = Stopwatch.StartNew();
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
        public Boolean CheckCard(string UID)
        {
            string path = @".\AuthorizedCards.txt";
            StreamReader sr = new StreamReader(path);
            string[] RegisteredUIDs = sr.ReadToEnd().Split(' ');
            foreach (string s in RegisteredUIDs)
            {
                Debug.WriteLine(s);
                Debug.WriteLine(UID);
                if (UID.Equals(s))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
