using InmobiliariaGrosso.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaGrosso.Data
{
    public class RepoContrato 
    {
        string connectionString = "Server=localhost;User=root;Password=;Database=InmobiliariaGrosso;SslMode=none";
        public RepoContrato()
        {

        }
        
        public int Edit(Contrato c)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE contratos
                            SET IdInmueble = @id_inmueble , IdInquilino = @id_inquilino , Desde = @desde , 
                            Hasta = @hasta
                            WHERE Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id_inmueble", c.IdInmueble);
                    comm.Parameters.AddWithValue("@id_inquilino", c.IdInquilino);
                    comm.Parameters.AddWithValue("@desde", c.Desde);
                    comm.Parameters.AddWithValue("@hasta", c.Hasta);
                    comm.Parameters.AddWithValue("@id", c.Id);


                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteNonQuery());
                    conn.Close();
                }
            }
            return res;
        }
        public int Delete(int id)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"DELETE FROM contratos WHERE Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteNonQuery());
                    conn.Close();
                }
            }
            return res;
        }


        public Contrato Details(int id)
        {
            Contrato c = new Contrato();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.Id, c.IdInmueble, c.IdInquilino, c.Desde, c.Hasta,
                                i.IdPropietario, p.Nombre, 
                                i2.Nombre, i.Direccion 
                                FROM contratos c INNER JOIN inmuebles i ON c.IdInmueble = i.Id 
                                INNER JOIN propietarios p ON i.IdPropietario = p.Id 
                                INNER JOIN inquilinos i2 ON c.IdInquilino = i2.Id 
                                WHERE c.Id = @id;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    var reader = comm.ExecuteReader();

                    if (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            Id = reader.GetInt32(5),
                            Nombre = reader.GetString(6),
                        };

                        Inmueble i = new Inmueble
                        {
                            Id = reader.GetInt32(1),
                            Direccion = reader.GetString(8),
                            Duenio = p,
                        };

                        Inquilino i2 = new Inquilino
                        {
                            Id = reader.GetInt32(2),
                            Nombre = reader.GetString(7)
                        };


                        c.Id = reader.GetInt32(0);
                        c.IdInmueble = reader.GetInt32(1);
                        c.IdInquilino = reader.GetInt32(2);
                        c.Desde = reader.GetDateTime(3);
                        c.Hasta = reader.GetDateTime(4);
                        c.Inmueble = i;
                        c.Inquilino = i2;

                    }

                    conn.Close();
                }
            }
            return c;
        }

        public int Put(Contrato c)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO contratos (IdInmueble, IdInquilino, Desde, Hasta
                            ) 
                            VALUES(@id_inmueble, @id_inquilino, @desde, @hasta
                            );
                            SELECT last_insert_id();";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id_inmueble", c.IdInmueble);
                    comm.Parameters.AddWithValue("@id_inquilino", c.IdInquilino);
                    comm.Parameters.AddWithValue("@desde", c.Desde);
                    comm.Parameters.AddWithValue("@hasta", c.Hasta);
                    
                    

                    conn.Open();

                    res = Convert.ToInt32(comm.ExecuteScalar());
                    c.Id = res;

                    conn.Close();
                }
            }
            return res;
        }

        public IList<Contrato> All()
        {
            IList<Contrato> lista = new List<Contrato>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT c.Id, c.IdInmueble, c.IdInquilino, c.Desde, c.Hasta,  
                                i.IdPropietario, p.Nombre, 
                                i2.Nombre
                                FROM contratos c INNER JOIN inmuebles i ON c.IdInmueble = i.Id 
                                INNER JOIN propietarios p ON i.IdPropietario = p.Id 
                                INNER JOIN inquilinos i2 ON c.IdInquilino = i2.Id";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        Propietario p = new Propietario
                        {
                            Id = reader.GetInt32(5),
                            Nombre = reader.GetString(6),
                        };

                        Inmueble i = new Inmueble
                        {
                            Id = reader.GetInt32(1),
                            Duenio = p,
                        };

                        Inquilino i2 = new Inquilino
                        {
                            Id = reader.GetInt32(2),
                            Nombre = reader.GetString(7)
                        };

                        Contrato c = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            IdInmueble = reader.GetInt32(1),
                            IdInquilino = reader.GetInt32(2),
                            Desde = reader.GetDateTime(3),
                            Hasta = reader.GetDateTime(4),
                            Inmueble = i,
                            Inquilino = i2,
                        };

                        lista.Add(c);
                    }

                    conn.Close();
                }

            }

            return lista;
        }

        
    }
}
