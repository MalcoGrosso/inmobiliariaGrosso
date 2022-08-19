using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace InmobiliariaGrosso.Models
{
    public class RepoInquilino
    {
        string connectionString = "Server=localhost;User=root;Password=;Database=InmobiliariaGrosso;SslMode=none";
       // Server=localhost;User=root;Password=;Database=InmobiliariaGrosso;SslMode=none

        public RepoInquilino()
        {

        }

        public int Edit(Inquilino p)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Inquilinos SET Nombre = @nombre , Dni = @dni , Apellido = @apellido , 
                             Email = @email , Telefono = @telefono WHERE id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", p.Nombre);
                    comm.Parameters.AddWithValue("@dni", p.Dni);
                    comm.Parameters.AddWithValue("@apellido", p.Apellido);
                    comm.Parameters.AddWithValue("@email", p.Email);
                    comm.Parameters.AddWithValue("@telefono", p.Telefono);
                    comm.Parameters.AddWithValue("@id", p.Id);
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
                string sql = @"DELETE FROM Inquilinos WHERE Id = @id ;";

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

        public Inquilino Details(int id)
        {
            Inquilino p = new Inquilino();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Nombre, Dni, Apellido, Email, Telefono FROM Inquilinos WHERE Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    var reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        p.Id = reader.GetInt32(0);
                        p.Nombre = reader.GetString(1);
                        p.Dni = reader.GetString(2);
                        p.Apellido = reader.GetString(3);
                        p.Email = reader.GetString(4);
                        p.Telefono = reader.GetString(5);

                    }

                    conn.Close();

                }
            }
            return p;
        }
        public int Put(Inquilino p)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inquilinos (Nombre, Dni, Apellido, Email, Telefono) 
                            VALUES(@nombre, @dni, @apellido, @email, @telefono);
                            SELECT SCOPE_IDENTITY();";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@nombre", p.Nombre);
                    comm.Parameters.AddWithValue("@dni", p.Dni);
                    comm.Parameters.AddWithValue("@Apellido", p.Apellido);
                    comm.Parameters.AddWithValue("@email", p.Email);
                    comm.Parameters.AddWithValue("@telefono", p.Telefono);


                    conn.Open();

                    res = Convert.ToInt32(comm.ExecuteScalar());
                    p.Id = res;

                    conn.Close();
                }
            }
            return res;
        }

        public IList<Inquilino> All()
        {
            IList<Inquilino> list = new List<Inquilino>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, Nombre, Dni, Apellido,  Email, Telefono FROM Inquilinos;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while (reader.Read())
                    {
                        var p = new Inquilino
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Dni = reader.GetString(2),
                            Apellido = reader.GetString(3),
                            Email = reader.GetString(4),
                            Telefono = reader.GetString(5),

                        };

                        list.Add(p);
                    }
                    conn.Close();
                }
            }
            return list;
        }

        virtual public Inquilino ObtenerPorId(int id)
		{
			Inquilino p = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = $"SELECT Id, Nombre, Apellido, Dni, Telefono, Email FROM Inquilinos" +
					$" WHERE Id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
               //     command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Inquilino
						{
							Id = reader.GetInt32(0),
							Nombre = reader.GetString(1),
							Apellido = reader.GetString(2),
							Dni = reader.GetString(3),
							Telefono = reader.GetString(4),
							Email = reader.GetString(5),
						};
					}
					connection.Close();
				}
			}
			return p;
        }



    }
}
