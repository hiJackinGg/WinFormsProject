using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
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
            InitializeBackgroundWorker();
          //  dataGridView1.DataSource = bindingSource1;
           // bindingSource1.DataSource = messages;
           // dataGridView1.AutoGenerateColumns = true;
   

           /* EMLMessage newMssage = new EMLMessage();
            newMssage.Date = new DateTime();
            newMssage.Subject = "asd";
            newMssage.From = "asd1";
            newMssage.To = "asd2";

            dataGridView1.Rows.Add(newMssage.Date, newMssage.Subject, newMssage.From, newMssage.To);*/
        }
        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.DoWork +=
                new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.ProgressChanged +=
                new ProgressChangedEventHandler(
            backgroundWorker1_ProgressChanged);
        }

        private void Form1_Load(object sender, System.EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            EMLMessage newMssage = (EMLMessage) e.UserState;
   
            dataGridView1.Rows.Add(newMssage.Date, newMssage.Subject, newMssage.From, newMssage.To);

        }

        private void backgroundWorker1_RunWorkerCompleted(
           object sender, RunWorkerCompletedEventArgs e)
        {
   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();

        }
        private void backgroundWorker1_DoWork(object sender,
    DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            calculate(worker, e);
        }


        private void calculate(BackgroundWorker worker, DoWorkEventArgs e)
        {

            String dirName = textBox1.Text;
            EMLMessage newMssage = new EMLMessage();

            IEnumerable<string> files = Directory.EnumerateFiles(dirName, "*.eml");
            int n = 1;

            foreach (string fileName in files)
            {
                using (FileStream r = File.Open(fileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    EMLReader reader = new EMLReader(r);
                    newMssage.Date = reader.Date;
                    newMssage.Subject = reader.Subject;
                    newMssage.From = reader.From;
                    newMssage.To = reader.To;
                }

                int percentComplete =
  (int)((float)n * 100 / (float)files.Count());

                backgroundWorker1.ReportProgress(percentComplete, newMssage);
                n++;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
