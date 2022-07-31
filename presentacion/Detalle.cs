using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dominio;

namespace presentacion
{
    public partial class Detalle : Form
    {
        Articulos seleccionado;
        public Detalle(Articulos seleccionado)
        {
            InitializeComponent();
            this.seleccionado = seleccionado;
            Text = "Detalle";
        }

        private void Detalle_Load(object sender, EventArgs e)
        {
            try
            {
                cargarImagen(seleccionado.ImagenUrl);
                lblDetalleCod.Text = seleccionado.CodigoArticulo;
                lblDetalleNom.Text = seleccionado.Nombre;
                lblDetalleDes.Text = seleccionado.Descripcion;
                lblDetalleMar.Text = seleccionado.Marca.ToString();
                lblDetalleCat.Text = seleccionado.Categoria.ToString();
                lblDetallePre.Text = seleccionado.Precio.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pbxArticulo.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
