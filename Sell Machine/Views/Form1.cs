using System.Linq;
using System;
using System.Windows.Forms;

namespace Sell_Machine
{
    public partial class SellMachine : Form
    {
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
                    CurrentItemPriceLabel.Text = lbl.Text;
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
    }
}