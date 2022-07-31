using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;


namespace negocio
{
    public class MarcaNegocio
    {
        public List<Marca> listarMarca()
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                List<Marca> lista = new List<Marca>();
                datos.setearConsulta("select Id, Descripcion from MARCAS");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
