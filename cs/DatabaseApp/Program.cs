using System;
using Npgsql;


class Program
{

    static void showTableEmployee(NpgsqlConnection conn, String query)
    {
        using (var cmd = new NpgsqlCommand(query, conn))
        {
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine("Код сотрудника | Фамилия | Имя | Отчество | Зарплата | Доплата");
                while (reader.Read())
                {
                    Console.WriteLine("{0} | {1} | {2} | {3} | {4} | {5}", reader[0], reader[1], reader[2], reader[3], reader[4], reader[5]);
                }
            }
        }
    }

    static void showTableTypesOfWork(NpgsqlConnection conn, String query)
    {
        using (var cmd = new NpgsqlCommand(query, conn))
        {
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine("Код работы | Описание | Количество сотрудников | Срок | Оплата за день");
                while (reader.Read())
                {
                    Console.WriteLine("{0} | {1} | {2} | {3} | {4}", reader[0], reader[1], reader[2], reader[3], reader[4]);
                }
            }
        }
    }

    static void showTableWorks(NpgsqlConnection conn, String query)
    {
        using (var cmd = new NpgsqlCommand(query, conn))
        {
            using (var reader = cmd.ExecuteReader())
            {
                Console.WriteLine("Код работы | Код сотрудника | Дата начала | Дата окончания");
                while (reader.Read())
                {
                    Console.WriteLine("{0} | {1} | {2} | {3}", reader[0], reader[1], reader[2], reader[3]);
                }
            }
        }
    }

    static void addEmployee(NpgsqlConnection conn)
        {
            Console.Write("Введите код сотрудника: ");
            string idInput = Console.ReadLine();
            int id;

            try {
                id = int.Parse(idInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение кода сотрудника");
                return;
            }

            Console.Write("Введите фамилию: ");
            string surname = Console.ReadLine();

            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите отчество: ");
            string middle = Console.ReadLine();

            Console.Write("Введите зарплату: ");
            string salaryInput = Console.ReadLine();
            int salary;

            try {
                salary = int.Parse(salaryInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение зарплаты");
                return;
            }

            string query = "INSERT INTO Сотрудники VALUES (@id, @surname, @name, @middle, @salary, 0)";

            using (var command = new NpgsqlCommand(query, conn))
            {
                try {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("surname", surname);
                    command.Parameters.AddWithValue("name", name);
                    command.Parameters.AddWithValue("middle", middle);
                    command.Parameters.AddWithValue("salary", salary);

                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

    static void addTypeOfWork(NpgsqlConnection conn)
        {
            Console.Write("Введите код работы: ");
            string idInput = Console.ReadLine();
            int id;

            try {
                id = int.Parse(idInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение кода работы");
                return;
            }

            Console.Write("Введите описание: ");
            string description = Console.ReadLine();

            Console.Write("Введите количество сотрудников: ");
            string workersInput = Console.ReadLine();
            int workers;

            try {
                workers = int.Parse(workersInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение количества сотрудников");
                return;
            }

            Console.Write("Введите срок: ");
            string deadlineInput = Console.ReadLine();
            DateTime deadline;

            try {
                deadline = DateTime.Parse(deadlineInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение срока выполнения работы");
                return;
            }

            Console.Write("Введите оплату за день: ");
            string paymentInput = Console.ReadLine();
            int payment;

            try {
                payment = int.Parse(paymentInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение оплаты за день");
                return;
            }

            string query = "INSERT INTO Виды_работ VALUES (@id, @description, @workers, @deadline, @payment)";

            using (var command = new NpgsqlCommand(query, conn))
            {
                try {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("description", description);
                    command.Parameters.AddWithValue("workers", workers);
                    command.Parameters.AddWithValue("deadline", deadline);
                    command.Parameters.AddWithValue("payment", payment);

                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

    static void addWork(NpgsqlConnection conn)
        {
            Console.Write("Введите код работы: ");
            string idInput = Console.ReadLine();
            int id;

            try {
                id = int.Parse(idInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение кода работы");
                return;
            }

            Console.Write("Введите код сотрудника: ");
            string employeeInput = Console.ReadLine();
            int employee;

            try {
                employee = int.Parse(employeeInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение кода сотрудника");
                return;
            }

            Console.Write("Введите дату начала: ");
            string start = Console.ReadLine();

            Console.Write("Введите дату окончания: ");
            string end = Console.ReadLine();

            string query = "INSERT INTO Работы VALUES (@id, @employee, @start, @end)";

            using (var command = new NpgsqlCommand(query, conn))
            {
                try {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("employee", employee);
                    command.Parameters.AddWithValue("start", start);
                    command.Parameters.AddWithValue("end", end);

                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

    static void deleteEmployee(NpgsqlConnection conn)
        {
            Console.Write("Введите код сотрудника: ");
            string idInput = Console.ReadLine();
            int id;

            try {
                id = int.Parse(idInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение кода сотрудника");
                return;
            }

            string query = "DELETE FROM Сотрудники WHERE Код_сотрудника = @id";

            using (var command = new NpgsqlCommand(query, conn))
            {
                try {
                    command.Parameters.AddWithValue("id", id);

                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

    static void deleteTypeOfWork(NpgsqlConnection conn)
        {
            Console.Write("Введите код работы: ");
            string idInput = Console.ReadLine();
            int id;

            try {
                id = int.Parse(idInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение кода работы");
                return;
            }

            string query = "DELETE FROM Виды_работ WHERE Код_работы = @id";

            using (var command = new NpgsqlCommand(query, conn))
            {
                try {
                    command.Parameters.AddWithValue("id", id);

                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

    static void deleteWork(NpgsqlConnection conn)
        {
            Console.Write("Введите код работы: ");
            string idInput = Console.ReadLine();
            int id;

            try {
                id = int.Parse(idInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение кода работы");
                return;
            }

            string query = "DELETE FROM Работы WHERE Код_работы = @id";

            using (var command = new NpgsqlCommand(query, conn))
            {
                try {
                    command.Parameters.AddWithValue("id", id);

                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

    static void editEmployee(NpgsqlConnection conn)
        {
            Console.Write("Введите код сотрудника: ");
            string idInput = Console.ReadLine();
            int id;

            try {
                id = int.Parse(idInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение кода сотрудника");
                return;
            }

            Console.Write("Введите фамилию: ");
            string surname = Console.ReadLine();

            Console.Write("Введите имя: ");
            string name = Console.ReadLine();

            Console.Write("Введите отчество: ");
            string middle = Console.ReadLine();

            Console.Write("Введите зарплату: ");
            string salaryInput = Console.ReadLine();
            int salary;

            try {
                salary = int.Parse(salaryInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение зарплаты");
                return;
            }

            string query = "UPDATE Сотрудники SET Фамилия = @surname, Имя = @name, Отчество = @middle, Зарплата = @salary WHERE Код_сотрудника = @id";

            using (var command = new NpgsqlCommand(query, conn))
            {
                try {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("surname", surname);
                    command.Parameters.AddWithValue("name", name);
                    command.Parameters.AddWithValue("middle", middle);
                    command.Parameters.AddWithValue("salary", salary);

                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

    static void editTypeOfWork(NpgsqlConnection conn)
        {
            Console.Write("Введите код работы: ");
            string idInput = Console.ReadLine();
            int id;

            try {
                id = int.Parse(idInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение кода работы");
                return;
            }

            Console.Write("Введите описание: ");
            string description = Console.ReadLine();

            Console.Write("Введите количество сотрудников: ");
            string workersInput = Console.ReadLine();
            int workers;

            try {
                workers = int.Parse(workersInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение количества сотрудников");
                return;
            }

            Console.Write("Введите срок: ");
            string deadline = Console.ReadLine();

            Console.Write("Введите оплату за день: ");
            string paymentInput = Console.ReadLine();
            int payment;

            try {
                payment = int.Parse(paymentInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение оплаты за день");
                return;
            }

            string query = "UPDATE Виды_работ SET Описание = @description, Количество_сотрудников = @workers, Срок = @deadline, Оплата_за_день = @payment WHERE Код_работы = @id";

            using (var command = new NpgsqlCommand(query, conn))
            {
                try {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("description", description);
                    command.Parameters.AddWithValue("workers", workers);
                    command.Parameters.AddWithValue("deadline", deadline);
                    command.Parameters.AddWithValue("payment", payment);

                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }

    static void editWork(NpgsqlConnection conn)
        {
            Console.Write("Введите код работы: ");
            string idInput = Console.ReadLine();
            int id;

            try {
                id = int.Parse(idInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение кода работы");
                return;
            }

            Console.Write("Введите код сотрудника: ");
            string employeeInput = Console.ReadLine();
            int employee;

            try {
                employee = int.Parse(employeeInput);
            } catch (Exception ex) {
                Console.WriteLine("Ошибка: неверное значение кода сотрудника");
                return;
            }

            Console.Write("Введите дату начала: ");
            string start = Console.ReadLine();

            Console.Write("Введите дату окончания: ");
            string end = Console.ReadLine();

            string query = "UPDATE Работы SET Код_сотрудника = @employee, Дата_начала = @start, Дата_окончания = @end WHERE Код_работы = @id";

            using (var command = new NpgsqlCommand(query, conn))
            {
                try {
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("employee", employee);
                    command.Parameters.AddWithValue("start", start);
                    command.Parameters.AddWithValue("end", end);

                    command.ExecuteNonQuery();
                } catch (Exception ex) {
                    Console.WriteLine("Ошибка: " + ex.Message);
                }
            }
        }



    static void Main()
    {
        string connectionString = "Host=localhost; Username=postgres; Password=postgres; Database=postgres";
        
        try
        {
            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                Console.WriteLine("Соединение с базой данных установлено\n\n");
                
                while (true) {

                    Console.WriteLine("\n\nВыберите действие:");
                    Console.WriteLine("1. Вывести таблицу");
                    Console.WriteLine("2. Добавить запись");
                    Console.WriteLine("3. Удалить запись");
                    Console.WriteLine("4. Изменить запись");
                    Console.WriteLine("5. Выполнить запрос");
                    Console.WriteLine("0. Выход");

                    try {
                        int action = Convert.ToInt32(Console.ReadLine());
                        switch (action) {
                            case 1:
                                Console.WriteLine("\n\nВыберите таблицу:");
                                Console.WriteLine("1. Сотрудники");
                                Console.WriteLine("2. Виды работ");
                                Console.WriteLine("3. Работы");
                                Console.WriteLine("0. Назад");

                                try {
                                    int table = Convert.ToInt32(Console.ReadLine());
                                    switch (table) {
                                        case 1:
                                            showTableEmployee(conn, "SELECT * FROM Сотрудники");
                                            break;
                                        case 2:
                                            showTableTypesOfWork(conn, "SELECT * FROM Виды_работ");
                                            break;
                                        case 3:
                                            showTableWorks(conn, "SELECT * FROM Работы");
                                            break;
                                        case 0:
                                            break;
                                        default:
                                            Console.WriteLine("Неверное действие");
                                            break;
                                    }
                                } catch (Exception ex) {
                                    Console.WriteLine("Ошибка: " + ex.Message);
                                }
                                break;

                            case 2:
                                Console.WriteLine("\n\nВыберите таблицу:");
                                Console.WriteLine("1. Сотрудники");
                                Console.WriteLine("2. Виды работ");
                                Console.WriteLine("3. Работы");
                                Console.WriteLine("0. Назад");

                                try {
                                    int table = Convert.ToInt32(Console.ReadLine());
                                    switch (table) {
                                        case 1:
                                            showTableEmployee(conn, "SELECT * FROM Сотрудники");
                                            Console.WriteLine("\n");
                                            addEmployee(conn);
                                            break;
                                        case 2:
                                            showTableTypesOfWork(conn, "SELECT * FROM Виды_работ");
                                            Console.WriteLine("\n");
                                            addTypeOfWork(conn);
                                            break;
                                        case 3:
                                            showTableWorks(conn, "SELECT * FROM Работы");
                                            Console.WriteLine("\n");
                                            addWork(conn);
                                            break;
                                        case 0:
                                            break;
                                        default:
                                            Console.WriteLine("Неверное действие");
                                            break;
                                    }
                                } catch (Exception ex) {
                                    Console.WriteLine("Ошибка: " + ex.Message);
                                }
                                break;

                            case 3:
                                Console.WriteLine("\n\nВыберите таблицу:");
                                Console.WriteLine("1. Сотрудники");
                                Console.WriteLine("2. Виды работ");
                                Console.WriteLine("3. Работы");
                                Console.WriteLine("0. Назад");

                                try {
                                    int table = Convert.ToInt32(Console.ReadLine());
                                    switch (table) {
                                        case 1:
                                            showTableEmployee(conn, "SELECT * FROM Сотрудники");
                                            Console.WriteLine("\n");
                                            deleteEmployee(conn);
                                            break;
                                        case 2:
                                            showTableTypesOfWork(conn, "SELECT * FROM Виды_работ");
                                            Console.WriteLine("\n");
                                            deleteTypeOfWork(conn);
                                            break;
                                        case 3:
                                            showTableWorks(conn, "SELECT * FROM Работы");
                                            Console.WriteLine("\n");
                                            deleteWork(conn);
                                            break;
                                        case 0:
                                            break;
                                        default:
                                            Console.WriteLine("Неверное действие");
                                            break;
                                    }
                                } catch (Exception ex) {
                                    Console.WriteLine("Ошибка: " + ex.Message);
                                }
                                break;

                            case 4:
                                Console.WriteLine("\n\nВыберите таблицу:");
                                Console.WriteLine("1. Сотрудники");
                                Console.WriteLine("2. Виды работ");
                                Console.WriteLine("3. Работы");
                                Console.WriteLine("0. Назад");

                                try {
                                    int table = Convert.ToInt32(Console.ReadLine());
                                    switch (table) {
                                        case 1:
                                            showTableEmployee(conn, "SELECT * FROM Сотрудники");
                                            Console.WriteLine("\n");
                                            editEmployee(conn);
                                            break;
                                        case 2:
                                            showTableTypesOfWork(conn, "SELECT * FROM Виды_работ");
                                            Console.WriteLine("\n");
                                            editTypeOfWork(conn);
                                            break;
                                        case 3:
                                            showTableWorks(conn, "SELECT * FROM Работы");
                                            Console.WriteLine("\n");
                                            editWork(conn);
                                            break;
                                        case 0:
                                            break;
                                        default:
                                            Console.WriteLine("Неверное действие");
                                            break;
                                    }
                                } catch (Exception ex) {
                                    Console.WriteLine("Ошибка: " + ex.Message);
                                }
                                break;

                            case 5:
                                Console.WriteLine("Выберите запрос:");
                                Console.WriteLine("1. Посчитать зарплату сотруднику");
                                Console.WriteLine("2. Посчитать количество выполненных работ сотрудником");
                                Console.WriteLine("0. Назад");

                                try {
                                    int query = Convert.ToInt32(Console.ReadLine());

                                    switch (query) {
                                        case 1:
                                            Console.Write("Введите код сотрудника: ");
                                            string idInput = Console.ReadLine();
                                            int id;

                                            try {
                                                id = int.Parse(idInput);
                                            } catch (Exception ex) {
                                                Console.WriteLine("Ошибка: неверное значение кода сотрудника");
                                                return;
                                            }

                                            using (var command = new NpgsqlCommand("SELECT * FROM get_employee_payment(@id)", conn))
                                            {
                                                command.Parameters.AddWithValue("id", id);

                                                using (var reader = command.ExecuteReader())
                                                {
                                                    while (reader.Read())
                                                    {
                                                        for (int i = 0; i < reader.FieldCount; i++)
                                                        {
                                                            Console.WriteLine($"{reader.GetName(i)}: ");
                                                            Console.WriteLine(reader[i]);
                                                        }
                                                        Console.WriteLine();
                                                    }
                                                }
                                            }
                                            
                                            break;

                                        case 2:
                                            Console.Write("Введите код сотрудника: ");
                                            string idInput2 = Console.ReadLine();
                                            int id2;

                                            try {
                                                id2 = int.Parse(idInput2);
                                            } catch (Exception ex) {
                                                Console.WriteLine("Ошибка: неверное значение кода сотрудника");
                                                return;
                                            }

                                            using (var command = new NpgsqlCommand("SELECT * FROM total_works_done(@id)", conn))
                                            {
                                                command.Parameters.AddWithValue("id", id2);

                                                using (var reader = command.ExecuteReader())
                                                {
                                                    while (reader.Read())
                                                    {
                                                        for (int i = 0; i < reader.FieldCount; i++)
                                                        {
                                                            Console.WriteLine($"{reader.GetName(i)}: ");
                                                            Console.WriteLine(reader[i]);
                                                        }
                                                        Console.WriteLine();
                                                    }
                                                }
                                            }
                                            break;

                                        case 0:
                                            break;
                                        default:
                                            Console.WriteLine("Неверное действие");
                                            break;
                                    }
                                } catch (Exception ex) {
                                    Console.WriteLine("Ошибка: " + ex.Message);
                                }
                                break;

                            case 0:
                                conn.Close();
                                Console.WriteLine("Соединение с базой данных закрыто");
                                Console.WriteLine("Завершение работы");
                                return;
                            default:
                                Console.WriteLine("Неверное действие");
                                break;
                        }
                    } catch (Exception ex) {
                        Console.WriteLine("Ошибка: " + ex.Message);
                    
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при подключении к базе данных: " + ex.Message);
        }
    }
}