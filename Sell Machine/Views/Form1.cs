using System.Linq;
using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data;
using Sell_Machine.Models;
using System.Text.Json;

namespace Sell_Machine
{
    public partial class SellMachine : Form
    {
        List<Product> products = null!;
        double EnteredAmount { get; set; }
        double ItemPrice { get; set; }
        double Residuse { get; set; }

        string CurrentItemName { get; set; }

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
            CurrentItemName = panel.Name;

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
            products = null!;
            using FileStream fs = new FileStream("../../../Properties/Products.json", FileMode.Open);
            products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(fs)!;

            foreach (Control control in ItemsPanel.Controls)
                if (control is RadioButton rb)
                {
                    var product = products.Find((name) => name.ProductName == (rb.Tag as Panel)!.Name);
                    rb.Text = $"{product!.Pcs} pcs.";
                    if (product.Pcs < 1)
                        rb.Enabled = false;
                }

            fs.Close();

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

        private void Penny_Click(object sender, EventArgs e)
        {
            Button? btn = sender as Button;
            if (string.IsNullOrWhiteSpace(EnterMoneyTb.Text)) EnterMoneyTb.Text = btn!.Tag.ToString();
            else
            {
                EnterMoneyTb.Text = double.Parse(new DataTable().Compute($"{EnterMoneyTb.Text} + {btn.Tag}", null).ToString()!).ToString("0.00");
            }
        }

        private void Purchase_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Thank You For Choosing Us!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (result == DialogResult.OK)
            {
                foreach (Control control in ItemsPanel.Controls)
                    if (control is RadioButton rb)
                        if (rb.Checked)
                            products.Find((name) => name.ProductName == (rb.Tag as Panel)!.Name)!.Pcs -= 1;

                WriteJsonFile(CurrentItemName);

                foreach (var control in ItemsPanel.Controls)
                    if (control is RadioButton rb)
                        if (rb.Checked)
                            rb.Checked = false;

                PaymentPanel.Enabled = false;
                EnterMoneyTb.Text = null;
                EnterMoneyTb.PlaceholderText = "00.00";

                CurrentItemPicture.Image = null;
                CurrentItemPriceLabel.Text = string.Empty;


                SellMachine_Load(null!, null!);
            }
        }

        private void WriteJsonFile(string itemname)
        {
            var jsonString = JsonSerializer.Serialize(products);
            File.WriteAllText("../../../Properties/Products.json", jsonString);

            Process process = new Process(itemname, EnteredAmount, Residuse);
            var check = JsonSerializer.Serialize(process);
            File.WriteAllText($"../../../Properties/{Guid.NewGuid().ToString()}.json", check);
        }
    }
}