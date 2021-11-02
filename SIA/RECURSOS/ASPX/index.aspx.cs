using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;

namespace SIA.RECURSOS.ASPX
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                Calendario1.Visible = false;
                txtFechaSol.Enabled = false;
                InicialProductos();
                txtidmesa.Enabled = false;
                txtidmesa.Visible = false;


            }
          
            
        }
        OracleConnection conexion = new OracleConnection("DATA SOURCE = xe ; PASSWORD = 123 ; USER ID = nicolas");

        protected void submit_Click(object sender, EventArgs e)
        {
            try
            {
                conexion.Open();
                OracleCommand comando = new OracleCommand("SP_Registrar_cliente2", conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.Add("Ncliente", OracleDbType.Varchar2).Value = txtnombrecliente.Text;
                comando.Parameters.Add("ApeCliente", OracleDbType.Varchar2).Value = Txtapellidocliente.Text;
                comando.Parameters.Add("MailCliente", OracleDbType.Varchar2).Value = Txtcorreo.Text;
                comando.Parameters.Add("Direc", OracleDbType.Varchar2).Value = Txtdireccion.Text;
                comando.Parameters.Add("RunCliente", OracleDbType.Varchar2).Value = Txtrut.Text;
                comando.Parameters.Add("FonCliente", OracleDbType.Int64).Value = Txttelefono.Text;
                comando.Parameters.Add("identidad", OracleDbType.Int64);
                comando.Parameters["identidad"].Direction = ParameterDirection.Output;

                comando.ExecuteNonQuery();

                var identidad = (comando.Parameters["identidad"].Value);
                Response.Write("su id es " + identidad );
               



            }
            catch (OracleException ex)
            {
                Response.Write("No se registrar, intentelo nuevamente" + ex.Message);
            }
            conexion.Close();
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            if (Calendario1.Visible)
            {
                Calendario1.Visible = false;
            }
            else
            {
                Calendario1.Visible = true;
            }
        }

        protected void Calendario1_SelectionChanged(object sender, EventArgs e)
        {
            txtFechaSol.Text = Calendario1.SelectedDate.ToString("dd/MM/yyyy");
            Calendario1.Visible = false;
            

        }

        public DataSet Consultar(string strSQL)
        {
            conexion.Open();
            OracleCommand cmd = new OracleCommand(strSQL, conexion);
            OracleDataAdapter das = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            das.Fill(ds);
            conexion.Close();
            return ds;
        }

        private void InicialProductos()
        {
            cmbmesas.DataSource = Consultar("SELECT TAMANNO_MESA,NUMERO_MESA FROM MESA where DISPONIBILIDAD =1");
            cmbmesas.DataTextField = "TAMANNO_MESA";
            cmbmesas.DataValueField = "NUMERO_MESA";
            cmbmesas.DataBind();
            cmbmesas.Items.Insert(0, new ListItem("", "0"));




        }

        protected void cmbmesas_SelectedIndexChanged(object sender, EventArgs e)
        {

            txtidmesa.Text = cmbmesas.SelectedValue;
        
        }

        protected void btnreserva_Click(object sender, EventArgs e)
        {

            try
            {
                conexion.Open();
                OracleCommand comando = new OracleCommand("SP_Realizar_Reserva", conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.Add("ID_RESERVA_CLIENT", OracleDbType.Int64).Value = txtidclienteReserva.Text;
                comando.Parameters.Add("ID_RESERVA_MES", OracleDbType.Int64).Value = txtidmesa.Text;
                comando.Parameters.Add("fec_reserva", OracleDbType.Date).Value = txtFechaSol.Text;
                comando.Parameters.Add("identidad", OracleDbType.Int64);
                comando.Parameters["identidad"].Direction = ParameterDirection.Output;


                comando.ExecuteNonQuery();


                var identidad = (comando.Parameters["identidad"].Value);
                Response.Write("su id es " + identidad);


            }
            catch
            {
                Response.Write("No se ha podido realizar su reserva, El ID de cliente no existe o hubo un problema");
            }
            conexion.Close();
        }

        protected void bntcancelar_Click(object sender, EventArgs e)
        {
            
            
                conexion.Open();
                OracleCommand comando = new OracleCommand("SP_cancelar_reserva", conexion);
                comando.CommandType = System.Data.CommandType.StoredProcedure;
                comando.Parameters.Add("N_RESERVA", OracleDbType.Int64).Value = txtidreserva.Text;
                comando.ExecuteNonQuery();
                Response.Write("Se ha cancelado la reserva");
                
            
          
        }
    }
    
    
}