using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gewürze_Regal_Verwaltung;

namespace Gewürze_Regal_Verwaltung
{
    public partial class MainForm : Form
    {
        private Common.Regal regal;
        private string filePath = string.Empty;

        public MainForm()
        {
            InitializeComponent();
            btnProcessData.Enabled = false;
            btnShowData.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbRegalName.Text))
            {
                MessageBox.Show("Der Regal Name fehlt.",
                    "Fehlende Informationen",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            else if (string.IsNullOrWhiteSpace(tbBodenBreite.Text))
            {
                MessageBox.Show("Die Breite des Regalbodens fehlt.",
                    "Fehlende Informationen",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            else if (string.IsNullOrWhiteSpace(tbBodenAnzahl.Text))
            {
                MessageBox.Show("Die Anzahl der Regalböden fehlt.",
                    "Fehlende Informationen",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
            else
            {
                regal = new Common.Regal(tbRegalName.Text, int.Parse(tbBodenBreite.Text), int.Parse(tbBodenAnzahl.Text));
                regal.readFromExcelFile(filePath);
                regal.fillRegal();

                // Update UI Elemente
                btnShowData.Enabled = true;
                labelAnzahlProdukte.Text = regal.getProdukte().Count.ToString();
                labelProduktOverFlowCount.Text = regal.getProduktOverFlow().Count.ToString();


                lvProduktOverFlow.Clear();
                foreach(Produkt produkt in regal.getProduktOverFlow())
                {
                    lvProduktOverFlow.Items.Add(produkt.getName() + " - " + produkt.GetVerpackung().getBezeichnung());
                }

                int currentRow = 0;
                flowLayoutPanel1.Controls.Clear();
                foreach(List<Produkt> row in regal.getProduktsPerRow())
                {
                    ListView lv = new ListView();
                    lv.View = View.List;
                    lv.Width = 200;
                    lv.Height = flowLayoutPanel1.Height - 15;
                    flowLayoutPanel1.Controls.Add(lv);

                    foreach(Produkt produkt in row)
                    {
                        lv.Items.Add(produkt.getName() + " - " + produkt.GetVerpackung().getBezeichnung());
                    }
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            regal.printRows();
            regal.printOverFlow();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "Downloads";
            openFileDialog.Filter = "CSV Datein (*.csv)|*.csv";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                btnProcessData.Enabled = true;
                labelDateiPfad.Text = filePath;
            }
        }

        private void tbBodenBreite_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void tbBodenAnzahl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
