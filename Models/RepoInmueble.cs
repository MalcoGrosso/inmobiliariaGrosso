using System;
using System.Collections.Generic;
using InmobiliariaGrosso.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace InmobiliariaGrosso.Data
{
    public class RepoInmueble
    {
        string connectionString = "Server=localhost;User=root;Password=;Database=InmobiliariaGrosso;SslMode=none";
       // Server=localhost;User=root;Password=;Database=InmobiliariaGrosso;SslMode=none

        public RepoInmueble()
        {

        }
        public int Edit(Inmueble i)
        {
            int res = -1;

            string sql = @"UPDATE inmuebles
                            SET Direccion = @direccion , Ambientes = @ambientes , Superficie = @superficie , 
                            Latitud = @latitud , Longitud = @longitud, IdPropietario = @IdPropietario  
                            WHERE Id = @id ;";


            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@direccion", i.Direccion);
                    comm.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    comm.Parameters.AddWithValue("@superficie", i.Superficie);
                    comm.Parameters.AddWithValue("@latitud", i.Latitud);
                    comm.Parameters.AddWithValue("@longitud", i.Longitud);
                    comm.Parameters.AddWithValue("@idpropietario",i.IdPropietario);
                    comm.Parameters.AddWithValue("@id", i.Id);
                    

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

                string sql = @"DELETE FROM Inmuebles WHERE Id = @id ;";

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

        public Inmueble Details(int id)
        {
			Inmueble i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT i.Id, i.Direccion, i.Ambientes, i.Superficie, i.Latitud, i.Longitud,  
                                i.IdPropietario, p.Nombre, p.apellido, p.Dni, p.Email 
                                FROM Inmuebles i INNER JOIN Propietarios p
                                ON i.IdPropietario = p.Id
                                WHERE i.Id = @id;";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
               //     command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Superficie = reader.GetInt32(3),
							Latitud = reader.GetString(4),
						    Longitud = reader.GetString(5),
                            IdPropietario = reader.GetInt32(6),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8),
							}
						
						};
					}
					connection.Close();
				}
			}
			return i;
        }


        public int Put(Inmueble i)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO inmuebles (Direccion, Ambientes, Superficie, Latitud, Longitud, IdPropietario) 
                            VALUES(@direccion, @ambientes, @superficie, @latitud, @longitud,  @idpropietario);
                            SELECT last_insert_id();";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@direccion", i.Direccion);
                    comm.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    comm.Parameters.AddWithValue("@superficie", i.Superficie);
                    comm.Parameters.AddWithValue("@latitud", i.Latitud);
                    comm.Parameters.AddWithValue("@longitud", i.Longitud);
                    comm.Parameters.AddWithValue("@idpropietario", i.IdPropietario);

                    conn.Open();

                    res = Convert.ToInt32(comm.ExecuteScalar());
                    i.Id = res;

                    conn.Close();
                }
            }
            return res;
        }

        public IList<Inmueble> All()
        {
            IList<Inmueble> list = new List<Inmueble>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id, i.Direccion, i.Ambientes, i.Superficie, i.Latitud, i.Longitud,  
                                i.IdPropietario, p.Nombre, p.apellido, p.Dni, p.Email 
                                FROM Inmuebles i INNER JOIN Propietarios p
                                ON i.IdPropietario = p.Id;";

                using (MySqlCommand command = new MySqlCommand(sql, conn))
				{
				//	command.CommandType = CommandType.Text;
					conn.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						var i = new Inmueble
						{
							Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Ambientes = reader.GetInt32(2),
                            Superficie = reader.GetInt32(3),
                            Latitud = reader.GetString(4),
                            Longitud = reader.GetString(5),
                            IdPropietario = reader.GetInt32(6),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8),
							}
						};
						list.Add(i);
					}
					conn.Close();
				}
            }   
            return list;
        }


        virtual public Inmueble ObtenerPorId(int id)
		{
			Inmueble i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT i.Id, i.Direccion, i.Ambientes, i.Superficie, i.Latitud, i.Longitud,  
                                i.IdPropietario, p.Nombre, p.apellido, p.Dni, p.Email 
                                FROM Inmuebles i INNER JOIN Propietarios p
                                ON i.IdPropietario = p.Id
                                WHERE i.Id = @id;";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
               //     command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inmueble
						{
							Id = reader.GetInt32(0),
							Direccion = reader.GetString(1),
							Ambientes = reader.GetInt32(2),
							Superficie = reader.GetInt32(3),
							Latitud = reader.GetString(4),
						    Longitud = reader.GetString(5),
                            IdPropietario = reader.GetInt32(6),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8),
							}
						
						};
					}
					connection.Close();
				}
			}
			return i;
        }
        
    }
}