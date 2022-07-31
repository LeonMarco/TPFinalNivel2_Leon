using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using dominio;

namespace negocio
{
    public class ArticulosNegocio
    {
        public List<Articulos> listar()
        {
            List<Articulos> lista = new List<Articulos>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("select A.Id, Codigo, Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, ImagenUrl, Precio, M.Descripcion Marca, C.Descripcion Categoria from ARTICULOS A, MARCAS M, CATEGORIAS C where M.Id = A.IdMarca And C.Id = A.IdCategoria");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();

                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodigoArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];                
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = (Decimal)Convert.ToDouble((decimal)datos.Lector["Precio"]);                    

                    lista.Add(aux);
                }
                datos.cerrarConexion();
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void agregar(Articulos nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio)values(@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @img, @precio)");
                datos.setearParametro("@id", nuevo.Id);
                datos.setearParametro("@codigo", nuevo.CodigoArticulo);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@descripcion", nuevo.Descripcion);
                datos.setearParametro("@idMarca", nuevo.Marca.Id);
                datos.setearParametro("@idCategoria", nuevo.Categoria.Id);
                datos.setearParametro("@img", nuevo.ImagenUrl);
                datos.setearParametro("@precio", nuevo.Precio);

                datos.ejecutarAccion();
                datos.cerrarConexion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void modificar(Articulos modificar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @codigo, Nombre = @nombre, Descripcion = @descripcion, IdMarca = @idMarca, IdCategoria = @idCategoria, ImagenUrl = @img, Precio = @precio where Id = @id");
                datos.setearParametro("@codigo", modificar.CodigoArticulo);
                datos.setearParametro("@nombre", modificar.Nombre);
                datos.setearParametro("@descripcion", modificar.Descripcion);
                datos.setearParametro("@idMarca", modificar.Marca.Id);
                datos.setearParametro("@idCategoria", modificar.Categoria.Id);
                datos.setearParametro("@img", modificar.ImagenUrl);
                datos.setearParametro("@id", modificar.Id);
                datos.setearParametro("@precio", modificar.Precio);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("delete from ARTICULOS where Id = @id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
                

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<Articulos> filtroAvanzado(string categoria, string marca)
        {
            List<Articulos> listaFiltro = new List<Articulos>();
            AccesoDatos datos = new AccesoDatos();
            string consulta = "select A.Id, Codigo, Nombre, A.Descripcion, A.IdMarca, A.IdCategoria, ImagenUrl, Precio, M.Descripcion Marca, C.Descripcion Categoria from ARTICULOS A, MARCAS M, CATEGORIAS C where C.Id = A.IdCategoria And M.Id = A.IdMarca And ";
            try
            {
                if(categoria == "Celulares")
                {
                    switch (marca)
                    {
                        case "Samsung":
                            consulta += "A.IdCategoria = 1 And A.IdMarca = 1";
                            break;
                        case "Apple":
                            consulta += "A.IdCategoria = 1 And A.IdMarca = 2";
                            break;
                        case "Huawei":
                            consulta += "A.IdCategoria = 1 And A.IdMarca = 4";
                            break;
                        default:
                            consulta += "A.IdCategoria = 1 And A.IdMarca = 5";
                            break;
                    }                        
                }
                else if(categoria == "Televisores")
                {
                    switch (marca)
                    {
                        case "Sony":
                            consulta += "A.IdCategoria = 2 And A.IdMarca = 3";
                            break;
                    }

                }
                else
                {
                    switch (marca)
                    {
                        case "Sony":
                            consulta += "A.IdCategoria = 3 And A.IdMarca = 3";
                            break;
                        case "Apple":
                            consulta += "A.IdCategoria = 3 And A.IdMarca = 2";                            
                            break;
                    }
                }
                   

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulos aux = new Articulos();

                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodigoArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    listaFiltro.Add(aux);
                }
                return listaFiltro;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        

    }
}
