using System.Linq;
using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;

namespace Sell_Machine
{
    public partial class SellMachine : Form
    {
        double EnteredAmount { get; set; }
        double ItemPrice { get; set; }
        double Residuse { get; set; }

        public SellMachine()
        {
            InitializeComponent();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton? rb = sender as RadioButton;
            if (!rb!.Checked)
                return;

            var panel = rb.Tag as Panel;

            if (panel == null)
                return;

            PaymentPanel.Enabled = true;

            foreach (var control in panel.Controls)
                if (control is Label lbl)
                {
                    ItemPrice = double.Parse(lbl.Tag.ToString()!);
                    CurrentItemPriceLabel.Text = lbl.Text;
                }
                else if (control is PictureBox pb)
                    CurrentItemPicture.Image = pb.Image;
        }

        private void SellMachine_SizeChanged(object sender, EventArgs e)
        {
            Form? form = sender as Form;

            if (form!.Width < 810)
                PaymentPanel.Visible = false;
            else PaymentPanel.Visible = true;
        }

        private void SellMachine_Load(object sender, EventArgs e)
        {
            EnteredAmountLbl.Text = $"Amount Entered: {EnteredAmount.ToString("0.00")} AZN";
            ResiduseLbl.Text = $"Residue: {Residuse.ToString("0.00")} AZN";
        }

        private void EnterMoneyTb_TextChanged(object sender, EventArgs e)
        {
            TextBox? tb = sender as TextBox;
            string patternPrice = "^[0-9]+\\.?[0-9]*$";


            if (string.IsNullOrWhiteSpace(tb!.Text))
            {
                EnteredAmount = 0;
                Residuse = 0;
                SellMachine_Load(sender, null!);
            }

            if (!Regex.IsMatch(tb!.Text, patternPrice)) tb.Text = string.Empty;
            else
            {
                EnteredAmount = double.Parse(tb.Text);
                EnteredAmountLbl.Text = $"Amount Entered: {EnteredAmount.ToString("0.00")} AZN";
                Residuse = EnteredAmount - ItemPrice;
                ResiduseLbl.Text = $"Residue: {Residuse.ToString("0.00")} AZN";
            }

            if (EnteredAmount >= ItemPrice) FinalPaymentLabel.Enabled = true;
            else FinalPaymentLabel.Enabled = false;
        }
    }
}