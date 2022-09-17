using InmobiliariaGrosso.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaGrosso.Data
{
    public class RepoPago 
    {
        string connectionString = "Server=localhost;User=root;Password=;Database=InmobiliariaGrosso;SslMode=none";
        public RepoPago()
        {

        }
        public int Edit(Pago pago)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Pagos
                               SET Fecha = @fecha , Monto = @monto , 
                               NumeroPago = @NumeroPago 
                               WHERE Id = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@fecha", pago.Fecha);
                    comm.Parameters.AddWithValue("@monto", pago.Monto);
                    comm.Parameters.AddWithValue("@NumeroPago", pago.NumeroPago);
                    comm.Parameters.AddWithValue("@id", pago.Id);

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
                string sql = @"DELETE FROM Pagos WHERE Id = @id ;";

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
        public Pago Details(int id)
        {
            Pago pago = new Pago();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, IdContrato, Fecha,  
                                Monto, NumeroPago FROM pagos 
                                WHERE Id = @id;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);

                    conn.Open();
                    var reader = comm.ExecuteReader();

                    if (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32(1)
                        };

                        pago.Id = reader.GetInt32(0);
                        pago.IdContrato = reader.GetInt32(1);
                        pago.Fecha = reader.GetDateTime(2);
                        pago.Monto = reader.GetInt32(3);
                        pago.NumeroPago = reader.GetInt32(4);
                        pago.Contrato = contrato;
                    }

                    conn.Close();
                }
            }
            return pago;
        }

        public int Put(Pago pago)
        {
            int res = -1;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Pagos (IdContrato, Monto, NumeroPago) 
                            VALUES(@id_contrato,  @monto, @NumeroPago);
                            SELECT last_insert_id();";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id_contrato", pago.IdContrato);
                    comm.Parameters.AddWithValue("@monto", pago.Monto);
                    comm.Parameters.AddWithValue("@NumeroPago", pago.NumeroPago);

                    conn.Open();

                    res = Convert.ToInt32(comm.ExecuteScalar());
                    pago.Id = res;

                    conn.Close();
                }
            }
            return res;
        }

        public IList<Pago> All()
        {
            IList<Pago> lista = new List<Pago>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, IdContrato, Fecha,  
                                Monto, NumeroPago FROM pagos;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32(1),
                        };

                        Pago pago = new Pago
                        {
                            Id = reader.GetInt32(0),
                            IdContrato = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Monto = reader.GetInt32(3),
                            NumeroPago = reader.GetInt32(4),
                            Contrato = contrato
                        };

                        lista.Add(pago);
                    }

                    conn.Close();
                }

            }

            return lista;
        }

        public IList<Pago> AllByContrato(int id)
        {
            IList<Pago> lista = new List<Pago>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT Id, IdContrato, Fecha,  
                                Monto, NumeroPago FROM pagos  
                                WHERE IdContrato = @id ;";

                using (MySqlCommand comm = new MySqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@id", id);
                    conn.Open();
                    var reader = comm.ExecuteReader();

                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32(1),
                        };

                        Pago pago = new Pago
                        {
                            Id = reader.GetInt32(0),
                            IdContrato = reader.GetInt32(1),
                            Fecha = reader.GetDateTime(2),
                            Monto = reader.GetInt32(3),
                            NumeroPago = reader.GetInt32(4),
                            Contrato = contrato
                        };

                        lista.Add(pago);
                    }

                    conn.Close();
                }

            }

            return lista;
        }
    }
}
