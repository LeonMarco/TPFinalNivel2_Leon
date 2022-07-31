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
    public partial class Form1: Form
    {
        private List<Articulos> ListaArticulos;
        public string categoria { get; set; }
        public Form1()
        {
            InitializeComponent();
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

        private void ocultarColumnas()
        {
            dgvLista.Columns["ImagenUrl"].Visible = false;
            dgvLista.Columns["Id"].Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cboCategoria.Items.Add("Celulares");
            cboCategoria.Items.Add("Televisores");
            cboCategoria.Items.Add("Audio");
            cboCategoria.Items.Add("Media");
            
        }
        private void cargar()
        {
            try
            {
                ArticulosNegocio negocio = new ArticulosNegocio();                

                ListaArticulos = negocio.listar();
                dgvLista.DataSource = ListaArticulos;
                //cboMarcaFiltro.DataSource = marca.listarMarca();
                ocultarColumnas();
                cargarImagen(ListaArticulos[0].ImagenUrl);           

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AltaArticulo alta = new AltaArticulo();
            alta.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow != null)
            {
                Articulos seleccionado;
                seleccionado = (Articulos)dgvLista.CurrentRow.DataBoundItem;
                AltaArticulo modificar = new AltaArticulo(seleccionado);
                modificar.ShowDialog();
                cargar();
            }
            else
                MessageBox.Show("Debe seleccionar un artículo para poder modificarlo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);            
        }

        private void dgvLista_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvLista.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.ImagenUrl);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            try
            {
                if(dgvLista.CurrentRow != null)
                {
                    DialogResult respuesta = MessageBox.Show("¿De verdad quieres eliminarlo?", "Eliminar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (respuesta == DialogResult.Yes)
                    {
                        Articulos eliminar = (Articulos)dgvLista.CurrentRow.DataBoundItem;
                        negocio.eliminar(eliminar.Id);
                        cargar();
                    }
                }else
                    MessageBox.Show("Debe seleccionar un artículo para poder eliminarlo", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtFiltroRapido_TextChanged(object sender, EventArgs e)
        {
            try
            {
                List<Articulos> listaFiltrada;
                string filtro = txtFiltroRapido.Text;

                if (filtro.Length >= 3)
                {
                    listaFiltrada = ListaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.Marca.Descripcion.ToUpper().Contains(filtro.ToUpper()));
                }
                else
                {
                    listaFiltrada = ListaArticulos;
                }

                dgvLista.DataSource = null;
                dgvLista.DataSource = listaFiltrada;
                ocultarColumnas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private bool validarFiltro()
        {
            if (cboCategoria.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione una categoría", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }
            if (cboCategoria.SelectedItem.ToString() == "Audio")
            {
                MessageBox.Show("Sin stock por el momento");
                return true;
            }
            if (cboMarcaFiltro.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione una marca", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return true;
            }

            return false;
        }


        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticulosNegocio negocio = new ArticulosNegocio();
            try
            {
                if (validarFiltro())
                    return;

                string categoria = cboCategoria.SelectedItem.ToString();
                string marca = cboMarcaFiltro.SelectedItem.ToString();
                dgvLista.DataSource = negocio.filtroAvanzado(categoria, marca);               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cboCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                string opcion = cboCategoria.SelectedItem.ToString();
                if (opcion == "Celulares")
                {
                    cboMarcaFiltro.Items.Clear();
                    cboMarcaFiltro.Items.Add("Samsung");
                    cboMarcaFiltro.Items.Add("Apple");
                    cboMarcaFiltro.Items.Add("Huawei");
                    cboMarcaFiltro.Items.Add("Motorola");
                }
                else if (opcion == "Televisores")
                {
                    cboMarcaFiltro.Items.Clear();
                    cboMarcaFiltro.Items.Add("Sony");
                }
                else if (opcion == "Audio")
                {
                    cboMarcaFiltro.Items.Clear();
                    MessageBox.Show("Sin stock por el momento");
                }
                else
                {
                    cboMarcaFiltro.Items.Clear();
                    cboMarcaFiltro.Items.Add("Sony");
                    cboMarcaFiltro.Items.Add("Apple");
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            cargar();
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow != null)
            {
                Articulos seleccionado = (Articulos)dgvLista.CurrentRow.DataBoundItem;
                Detalle detalle = new Detalle(seleccionado);
                detalle.ShowDialog();

            }
            else
                MessageBox.Show("Seleccione un artículo para ver el detalle", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }
    }
}
