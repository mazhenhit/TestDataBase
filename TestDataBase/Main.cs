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
            buttonSave.Enabled = false;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //CreateOracleConnection();
        }

        public void CreateOracleConnection()
        {
            string connectionString = "Data Source=127.0.0.1/XE;User ID=test;PassWord=test";
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("ServerVersion: " + connection.ServerVersion
                    + "\nDataSource: " + connection.DataSource);


                string sql = "select stationip from station"; // C#
                OracleCommand cmd = new OracleCommand(sql, connection);
                cmd.CommandType = CommandType.Text;

                OracleDataReader dr = cmd.ExecuteReader(); // C#
                dr.Read();

                connection.Close();   // C#
                connection.Dispose();
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            conn.ConnectionString = "Data Source=127.0.0.1/XE;User ID=test;PassWord=test";

            try
            {
                conn.Open();
                buttonConnect.Enabled = false;

                //slectDevice("DA");
                deleteDevice(2);

                cmd = new OracleCommand("insert into device(deviceid,devicename) values(:deviceid, :devicename)", conn);
                cmd.CommandType = CommandType.Text;

                OracleParameter deviceid = new OracleParameter();
                deviceid.OracleDbType = OracleDbType.Decimal;
                deviceid.Value = 14;
                cmd.Parameters.Add(deviceid);

                OracleParameter devicename = new OracleParameter();
                devicename.OracleDbType = OracleDbType.Char;
                devicename.Value = null;
                cmd.Parameters.Add(devicename);


                da = new OracleDataAdapter(cmd);
                cb = new OracleCommandBuilder(da);
                ds = new DataSet();
                da.Fill(ds);

                //string sql = "select * from device where deviceid < 60";
                //cmd = new OracleCommand(sql, conn);
                //cmd.CommandType = CommandType.Text;
                //da = new OracleDataAdapter(cmd);
                //cb = new OracleCommandBuilder(da);
                //ds = new DataSet();
                //da.Fill(ds);
                
                //departments.DataSource = ds.Tables[0];

                buttonSave.Enabled = true;
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
                //conn.Dispose();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            da.Update(ds.Tables[0]);
            MessageBox.Show("OK");
        }


        private void slectDevice(string devicename)
        {
            string sql = "select deviceid from device where devicename = :device_name";
            cmd = new OracleCommand(sql, conn);
            cmd.CommandType = CommandType.Text;

            OracleParameter device_name = new OracleParameter();
            device_name.OracleDbType = OracleDbType.Char;
            device_name.Value = devicename;
            cmd.Parameters.Add(device_name);

            OracleDataReader dr = cmd.ExecuteReader();
            dr.Read();

            decimal a = dr.GetDecimal(0);

            MessageBox.Show("    OK");
        }

        private void deleteDevice(int id)
        {
            updateChannelDevice(id);
            updateHistoryDevice(id);

            string sql = "delete from device where deviceid = :device_id";
            cmd = new OracleCommand(sql, conn);
            cmd.CommandType = CommandType.Text;

            OracleParameter device_id = new OracleParameter();
            device_id.OracleDbType = OracleDbType.Decimal;
            device_id.Value = id;
            cmd.Parameters.Add(device_id);

            da = new OracleDataAdapter(cmd);
            cb = new OracleCommandBuilder(da);
            ds = new DataSet();
            da.Fill(ds);
        }

        private void updateChannelDevice(int id)
        {
            string sql = "update channel set deviceid = null where deviceid = :device_id";
            cmd = new OracleCommand(sql, conn);
            cmd.CommandType = CommandType.Text;

            OracleParameter device_id = new OracleParameter();
            device_id.OracleDbType = OracleDbType.Decimal;
            device_id.Value = id;
            cmd.Parameters.Add(device_id);

            da = new OracleDataAdapter(cmd);
            cb = new OracleCommandBuilder(da);
            ds = new DataSet();
            da.Fill(ds);
        }

        private void updateHistoryDevice(int id)
        {
            string sql = "update history set deviceid = null where deviceid = :device_id";
            cmd = new OracleCommand(sql, conn);
            cmd.CommandType = CommandType.Text;

            OracleParameter device_id = new OracleParameter();
            device_id.OracleDbType = OracleDbType.Decimal;
            device_id.Value = id;
            cmd.Parameters.Add(device_id);

            da = new OracleDataAdapter(cmd);
            cb = new OracleCommandBuilder(da);
            ds = new DataSet();
            da.Fill(ds);
        }
    }
}
