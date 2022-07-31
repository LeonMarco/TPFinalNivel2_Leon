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
using negocio;

namespace presentacion
{
    public partial class AltaArticulo : Form
    {
        private Articulos articulo = null;
        public AltaArticulo()
        {
            InitializeComponent();
        }
        public AltaArticulo(Articulos modificar)
        {
            InitializeComponent();
            this.articulo = modificar;
            Text = "Modificar articulo";
            cargarCbox();
        }
        private void AltaArticulo_Load(object sender, EventArgs e)
        {

            try
            {
                //Precargar datos si hay que modificar
                if (articulo != null)
                {
                    txtCodArt.Text = articulo.CodigoArticulo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                    txtImgUrl.Text = articulo.ImagenUrl;
                    cargarImagen(articulo.ImagenUrl);
                    txtPrecio.Text = articulo.Precio.ToString();
                }               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        ErrorProvider error = new ErrorProvider();
        private bool validarVacios(TextBox txt)
        {
            if (string.IsNullOrEmpty(txt.Text))
            {
                error.SetError(txt, "Debe completar este campo");
                return true;

            }else
                error.Clear();

            return false;
        }
        private bool validarCombobox(ComboBox cBox)
        {
            if (cBox.SelectedItem == null)
            {
                error.SetError(cBox, "Debe completar este campo");
                return true;
            }
            else
                error.Clear();

            return false;
        }
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            try
            {
                if(articulo == null)
                   articulo = new Articulos();

                if (validarVacios(txtCodArt))                                  
                    return;                
                if (validarVacios(txtNombre))
                    return;
                if (validarVacios(txtDescripcion))
                    return;
                if (validarVacios(txtPrecio))
                    return;
                if (validarCombobox(cboMarca))
                    return;
                if (validarCombobox(cboCategoria))
                    return;

                articulo.CodigoArticulo = txtCodArt.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.ImagenUrl = txtImgUrl.Text;
                articulo.Precio = Decimal.Parse(txtPrecio.Text);
                

               if (articulo.Id != 0)
               {
                    negocio.modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                    
               }else
               {
                    negocio.agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
               }               

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                pxbModificar.Load(imagen);
            }
            catch (Exception)
            {
                pxbModificar.Load("https://efectocolibri.com/wp-content/uploads/2021/01/placeholder.png");
            }
        }

        private void txtImgUrl_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtImgUrl.Text);
        }       

        private void cboMarca_Click(object sender, EventArgs e)
        {
            MarcaNegocio marca = new MarcaNegocio();
            cboMarca.DataSource = marca.listarMarca();
            cboMarca.ValueMember = "Id";
            cboMarca.DisplayMember = "Descripcion";
        }

        private void cboCategoria_Click(object sender, EventArgs e)
        {
            CategoriaNegocio categoria = new CategoriaNegocio();
            cboCategoria.DataSource = categoria.listarCategoria();
            cboCategoria.ValueMember = "Id";
            cboCategoria.DisplayMember = "Descripcion";
            
        }
        private void cargarCbox()
        {
            MarcaNegocio marca = new MarcaNegocio();
            cboMarca.DataSource = marca.listarMarca();
            cboMarca.ValueMember = "Id";
            cboMarca.DisplayMember = "Descripcion";
            CategoriaNegocio categoria = new CategoriaNegocio();
            cboCategoria.DataSource = categoria.listarCategoria();
            cboCategoria.ValueMember = "Id";
            cboCategoria.DisplayMember = "Descripcion";
        }

        private void txtPrecio_KeyPress(object sender, KeyPressEventArgs e)
        {            
            if (!soloNumeros(e))
                MessageBox.Show("Solo números", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Error);           
                
        }
        private bool soloNumeros(KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = false;
                return true;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
                return true;
            }
            else if (Char.IsSeparator(e.KeyChar))
            {
                e.Handled = true;
                return false;
            }
            else
            {
                e.Handled = true;
                return false;
            }
        }
    }    

}
