using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SE1
{
    public partial class Form1 : Form
    {
        private List<EMLMessage> messages = new List<EMLMessage>();
        private BindingSource bindingSource1 = new BindingSource();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            calculate();
        }

        public void action(string fileName)
        {
            EMLMessage newMssage = new EMLMessage();

            using (FileStream r = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                EMLReader reader = new EMLReader(r);
                newMssage.Date = reader.Date;
                newMssage.Subject = reader.Subject;
                newMssage.From = reader.From;
                newMssage.To = reader.To;

                if (progressBar1.InvokeRequired)
                {
                    MethodInvoker oDelegate = (MethodInvoker)delegate
                    {
                        progressBar1.PerformStep();
                    };
                    MethodInvoker oDelegate1 = (MethodInvoker)delegate
                    {
                        dataGridView1.Rows.Add(newMssage.Date, newMssage.Subject, newMssage.From, newMssage.To);
                    };


                    progressBar1.BeginInvoke(oDelegate);
                    dataGridView1.BeginInvoke(oDelegate1);

                    return;
                }
                else
                {
                    progressBar1.PerformStep();
                    dataGridView1.Rows.Add(newMssage.Date, newMssage.Subject, newMssage.From, newMssage.To);

                }

            }
        }

        private void calculate()
        {
            String dirName = textBox1.Text;

            IEnumerable<string> files = Directory.EnumerateFiles(dirName, "*.eml");

            progressBar1.Maximum = files.Count();

            foreach (string fileName in files)
            {
                new Thread(
                run => action(fileName)
                ).Start();
            }
        }
    }
}
