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

        
        public Form1()
        {
            InitializeComponent();
            port.Open();
        }

        private void label1_Click(object sender, EventArgs e){}

        private void port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            string tagID = port.ReadLine();
            Debug.WriteLine(tagID);
            tagID = tagID.Substring(4,8);
            if (!CheckCard(tagID)) // if UID is not registered
            {
                label1.Invoke(new MethodInvoker(delegate { label1.Text = "ACCESS DENIED"; }));
            }
            else
            { 
               label1.Invoke(new MethodInvoker(delegate { label1.Text = "ACCESS GRANTED"; }));
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
                Debug.WriteLine(UID.Equals(s));
                if (UID.Equals(s))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
