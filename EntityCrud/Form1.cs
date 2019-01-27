using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EntityCrud
{
	public partial class Form1 : Form
	{
		Customer model = new Customer();

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			PopulateDataGridView();
			Clear();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Clear();
			PopulateDataGridView();
		}

		void Clear()
		{
			txtFirstName.Text = txtLastName.Text = txtCity.Text = txtAddress.Text = "";
			btnSave.Text = "Save";
			btnDelete.Enabled = false;
			model.CustomerID = 0;
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			model.FirstName = txtFirstName.Text.Trim();//Trim()inlatura spatiile libere inainte si dupa cuvant
			model.LastName = txtLastName.Text.Trim();
			model.City = txtCity.Text.Trim();
			model.Address = txtAddress.Text.Trim();
			using (EntityCrudEntities db = new EntityCrudEntities())
			{
				if (model.CustomerID == 0)//insert
					db.Customers.Add(model);
				else //update
					db.Entry(model).State = System.Data.Entity.EntityState.Modified;
				db.SaveChanges();
				Clear();
				PopulateDataGridView();
			}

				MessageBox.Show("Submitted succesfully");

		}
		void PopulateDataGridView()
		{
			dgvCustomers.AutoGenerateColumns = false;
			using (EntityCrudEntities db = new EntityCrudEntities())
			{
				dgvCustomers.DataSource = db.Customers.ToList<Customer>();
			}
		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			PopulateDataGridView();
		}

		private void dgvCustomers_DoubleClick(object sender, EventArgs e)
		{   if (dgvCustomers.CurrentRow.Index != -1)
			{
				model.CustomerID = Convert.ToInt32(dgvCustomers.CurrentRow.Cells["CustomerID"].Value);
				
				using (EntityCrudEntities db = new EntityCrudEntities())
				{
					model = db.Customers.Where(XmlReadMode => XmlReadMode.CustomerID == model.CustomerID).FirstOrDefault();
					txtFirstName.Text = model.FirstName;
					txtLastName.Text = model.LastName;
					txtCity.Text = model.City;
					txtAddress.Text = model.Address;
				}

				btnSave.Text = "Update";
				btnDelete.Enabled = true;
			}
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(" Are you sure ?", "EntityCrud", MessageBoxButtons.YesNo) == DialogResult.Yes)
				using (EntityCrudEntities db = new EntityCrudEntities())
				{
					var entry = db.Entry(model);
					if (entry.State == System.Data.Entity.EntityState.Detached)
						db.Customers.Attach(model);
					    db.Customers.Remove(model);
					    db.SaveChanges();
					    PopulateDataGridView();
					    Clear();
					MessageBox.Show("Deleted succesfully!");



				}
		}
	}
}
