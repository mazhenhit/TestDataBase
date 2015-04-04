using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace TestDataBase
{
    public partial class Main : Form
    {
        private OracleConnection conn = new OracleConnection();
        private OracleCommand cmd;
        private OracleDataAdapter da;
        private OracleCommandBuilder cb;
        private DataSet ds;

        public Main()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            conn.ConnectionString = "Data Source=127.0.0.1/XE;User ID=test;PassWord=test";

            try
            {
                conn.Open();
                buttonConnect.Enabled = false;

                string sql = "select devicename,deviceid from device";
                cmd = new OracleCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                da = new OracleDataAdapter(cmd);
                cb = new OracleCommandBuilder(da);
                ds = new DataSet();
                da.Fill(ds);

                DataTableReader dr = ds.Tables[0].CreateDataReader();
                int count = ds.Tables[0].Rows.Count;
                string[] names = new string[count];
                decimal[] id = new decimal[count];
                int temp = 0;

                while (dr.Read())
                {
                    names[temp] = dr.GetString(0);
                    id[temp] = dr.GetDecimal(1);
                    temp++;
                }
            }
            catch (OracleException ex)
            {
                switch (ex.Number)
                {
                    case 1:
                        MessageBox.Show("Error attempting to insert duplicate data.");
                        break;
                    case 12560:
                        MessageBox.Show("The database is unavailable.");
                        break;
                    default:
                        MessageBox.Show("Database error: " + ex.Message.ToString());
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                conn.Dispose();
            }
        }

        private void slectHistoryvalue(int id)
        {
            string sql = "select hitoryvalue from history where historyid = (select max(historyid) from history where deviceid = :device_id) ";
            cmd = new OracleCommand(sql, conn);
            cmd.CommandType = CommandType.Text;

            OracleParameter device_id = new OracleParameter();
            device_id.OracleDbType = OracleDbType.Decimal;
            device_id.Value = id;
            cmd.Parameters.Add(device_id);

            OracleDataReader dr = cmd.ExecuteReader();
			dr.Read();
            string a = dr.GetString(0);
        }
    }
}
