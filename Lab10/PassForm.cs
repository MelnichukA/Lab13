using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lab8Lib;

namespace Lab10
{
    public partial class PassForm : Form
    {
        private Pass ThePass;
        internal PassForm(Pass t)
        {
            ThePass = t;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            ThePass.Name = tbName.Text.Trim();
            ThePass.Country = tbCountry.Text.Trim();
            ThePass.Region = tbRegion.Text.Trim();
            ThePass.Place = tbPlace.Text.Trim();
            ThePass.Prop = tbProp.Text.Trim();
            ThePass.PassNum = int.Parse(tbPassNum.Text.Trim());
            try
            {

                ThePass.PassSer = tbPassSer.Text.Trim();
            }
            catch (InvalidSerException)
            {
                MessageBox.Show("Неверная серия паспорта", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ThePass.HasMarried = chbHasMarried.Checked;
            DialogResult = DialogResult.OK;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel; ;
        }

        private void fPass_Load(object sender, EventArgs e)
        {
            if (ThePass != null)
            {
                tbName.Text = ThePass.Name;
                tbCountry.Text = ThePass.Country;
                tbRegion.Text = ThePass.Region;
                tbPlace.Text = ThePass.Place;
                tbProp.Text = ThePass.Prop;
                tbPassNum.Text = ThePass.PassNum.ToString();
                tbPassSer.Text = ThePass.PassSer;
                chbHasMarried.Checked = ThePass.HasMarried;

            }
        }
    }
}
