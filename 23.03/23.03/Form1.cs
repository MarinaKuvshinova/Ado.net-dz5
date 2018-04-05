using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Threading;

namespace _23._03
{
    public partial class Form1 : Form
    {
        string connectionString = @"Data Source = CR4-12\SQLEXPRESS; Initial Catalog = BookShop; Integrated Security = true;";
        SqlConnection sqlConnection = null;
        DataTable dataTable = null;
        public Form1()
        {
            InitializeComponent();
            sqlConnection = new SqlConnection(connectionString);
            btnStart.Click += btnStart_Click;
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView1.DataSource = await GetDataFirstAsync();
            dataGridView2.DataSource = await GetDataSecondAsync();
        }

        private async Task<object> GetDataSecondAsync()
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                string sql = "select * from Authors;";
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                dataTable = new DataTable();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                int line = 0;
                do
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        if (line++ == 0)
                        {
                            for (int i = 0; i < sqlDataReader.FieldCount; i++)
                            {
                                dataTable.Columns.Add(sqlDataReader.GetName(i));
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        for (int i = 0; i < sqlDataReader.FieldCount; i++)
                        {
                            dataRow[i] = await sqlDataReader.GetFieldValueAsync<Object>(i);
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                } while (await sqlDataReader.NextResultAsync());
                return dataTable;
            }
        }

        private async Task<object> GetDataFirstAsync()
        {
            using (sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                string sql = "waitfor delay '00:00:10';";
                sql += "select * from Books;";
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                dataTable = new DataTable();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                int line = 0;
                do
                {
                    while (await sqlDataReader.ReadAsync())
                    {
                        if (line++ == 0)
                        {
                            for (int i = 0; i < sqlDataReader.FieldCount; i++)
                            {
                                dataTable.Columns.Add(sqlDataReader.GetName(i));
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        for (int i = 0; i < sqlDataReader.FieldCount; i++)
                        {
                            dataRow[i] = await sqlDataReader.GetFieldValueAsync<Object>(i);
                        }
                        dataTable.Rows.Add(dataRow);
                    }
                } while (await sqlDataReader.NextResultAsync());
                return dataTable;
            }
        }
    }
}
