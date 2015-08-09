using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string error = "";
            ClubInfo club = new ClubInfo();
            if( DataLayer.GetClubInfoByTerminal(out error, textBox2.Text, out club))
                MessageBox.Show(club.Name);
            else
                MessageBox.Show(error);
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            bool permitted = false;
            string error = "";
            try 
            {
            if (!permitted)
            {
                //verifytransaction_responseclass.Error = "";
                error = "نداشتن مجوز";
                throw new Exception(error);
                //return flagcontractpayment;
            } }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {

             
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string error="";
            if (! DataLayer.InsertTerminal(out error, textBox3.Text, textBox4.Text))
            MessageBox.Show(error);
            
        }
    }
}
