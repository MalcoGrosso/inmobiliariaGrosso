using System;
using System.Collections.Generic;
using InmobiliariaGrosso.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace InmobiliariaGrosso.Models
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
                            Latitud = @latitud , Longitud = @longitud, IdPropietario = @IdPropietario, Uso = @uso, Tipo = @tipo, Disponible = @disponible  
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
                    comm.Parameters.AddWithValue("@uso",i.Uso);
                    comm.Parameters.AddWithValue("@tipo",i.Tipo); 
                    comm.Parameters.AddWithValue("@disponible", i.Disponible);
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
                                i.IdPropietario, i.Uso, i.Tipo, i.Disponible, p.Nombre, p.apellido, p.Dni, p.Email 
                                FROM Inmuebles i INNER JOIN Propietarios p
                                ON i.IdPropietario = p.Id
                                WHERE i.Id = @id;";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
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
                            Uso = reader.GetString(7),
                            Tipo = reader.GetString(8),
                            Disponible = reader.GetBoolean(9),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
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
                string sql = @"INSERT INTO inmuebles (Direccion, Ambientes, Superficie, Latitud, Longitud, IdPropietario, Uso, Tipo, Disponible) 
                            VALUES(@direccion, @ambientes, @superficie, @latitud, @longitud,  @idpropietario, @uso, @tipo, @disponible);
                            SELECT last_insert_id();";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@direccion", i.Direccion);
                    comm.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    comm.Parameters.AddWithValue("@superficie", i.Superficie);
                    comm.Parameters.AddWithValue("@latitud", i.Latitud);
                    comm.Parameters.AddWithValue("@longitud", i.Longitud);
                    comm.Parameters.AddWithValue("@idpropietario", i.IdPropietario);
                    comm.Parameters.AddWithValue("@uso", i.Uso);
                    comm.Parameters.AddWithValue("@tipo", i.Tipo);
                    comm.Parameters.AddWithValue("@disponible", i.Disponible);

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
                                i.IdPropietario, i.Uso, i.Tipo, i.Disponible, p.Nombre, p.apellido, p.Dni, p.Email 
                                FROM Inmuebles i INNER JOIN Propietarios p
                                ON i.IdPropietario = p.Id;";

                using (MySqlCommand command = new MySqlCommand(sql, conn))
				{
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
                            Uso = reader.GetString(7),
                            Tipo = reader.GetString(8),
                            Disponible = reader.GetBoolean(9),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
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
                                i.IdPropietario, i.Uso, i.Tipo, i.Disponible, p.Nombre, p.apellido, p.Dni, p.Email 
                                FROM Inmuebles i INNER JOIN Propietarios p
                                ON i.IdPropietario = p.Id
                                WHERE i.Id = @id;";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
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
                            Uso = reader.GetString(7),
                            Tipo = reader.GetString(8),
                            Disponible = reader.GetBoolean(9),
                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(10),
                                Apellido = reader.GetString(11),
							}
						
						};
					}
					connection.Close();
				}
			}
			return i;
        }


        public IList<Inmueble> Validos()
        {
            IList<Inmueble> list = new List<Inmueble>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.Id, i.Direccion, i.Ambientes, i.Superficie, i.Latitud, i.Longitud,  
                                i.IdPropietario, i.Uso, i.Tipo, i.Disponible, p.Nombre, p.Dni, p.Email 
                               FROM Inmuebles i
                               INNER JOIN Propietarios p ON i.IdPropietario = p.Id
                               WHERE i.Disponible = 1
                               ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
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
                            Uso = reader.GetString(7),
                            Tipo = reader.GetString(8),
                            Disponible = reader.GetBoolean(9),
                            
                        };
                        
                        var p = new Propietario
                        {
                            Id = reader.GetInt32(6),
                            Nombre = reader.GetString(10),
                            Dni = reader.GetString(11),
                            Email = reader.GetString(12),
                        };

                        i.Duenio = p;

                        list.Add(i);
                    }
                    conn.Close();
                }
            }
            return list;
        }

        public IList<Inmueble> TodosPorInquilino(int id)
        {
            IList<Inmueble> list = new List<Inmueble>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))

            {
                string sql = @"SELECT i.Id, i.Direccion, i.Ambientes, i.Superficie, i.Latitud, i.Longitud,  
                                i.IdPropietario, i.Uso, i.Tipo, i.Disponible, p.Nombre, p.Dni, p.Email  
                                FROM Inmuebles i INNER JOIN Propietarios p
                                ON i.IdPropietario = p.Id 
                                WHERE i.IdPropietario = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();
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
                            Uso = reader.GetString(7),
                            Tipo = reader.GetString(8),
                            Disponible = reader.GetBoolean(9),
                        };

                        var p = new Propietario
                        {
                            Id = reader.GetInt32(6),
                            Nombre = reader.GetString(10),
                            Dni = reader.GetString(11),
                            Email = reader.GetString(12),
                        };

                        i.Duenio = p;

                        list.Add(i);
                    }
                    conn.Close();
                }
            }
            return list;
        }

        public IList<Inmueble> TodosDisponibles()
            {
                IList<Inmueble> list = new List<Inmueble>();

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string sql = @"SELECT i.Id, i.Direccion, i.Ambientes, i.Superficie, i.Latitud, i.Longitud,  
                                    i.IdPropietario, i.Uso, i.Tipo, i.Disponible, p.Nombre, p.Dni, p.Email  
                                    FROM Inmuebles i INNER JOIN Propietarios p
                                    ON i.IdPropietario = p.Id WHERE i.Disponible = 1 ;";

                    using (MySqlCommand comm = new MySqlCommand(sql, conn))
                    {
                        conn.Open();
                        var reader = comm.ExecuteReader();
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
                                Uso = reader.GetString(7),
                                Tipo = reader.GetString(8),
                                Disponible = reader.GetBoolean(9),
                            };

                            var p = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(10),
                                Dni = reader.GetString(11),
                                Email = reader.GetString(12),
                            };

                            i.Duenio = p;

                            list.Add(i);
                        }
                        conn.Close();
                    }
                }
                return list;
            }    

        
    }
}