import java.sql.*;
import java.util.concurrent.atomic.AtomicReference;
import javax.swing.*;
import javax.swing.table.DefaultTableModel;

public class Main {
    private static final String url = "jdbc:postgresql://localhost:5432/coldmilka";
    private static final String user = "coldmilka";
    private static final String password = "postgres";



    public static DefaultTableModel getFullSalary(int employee_code) {
        try {
            Connection connection = DriverManager.getConnection(url, user, password);
            String query = "SELECT * FROM get_total_salary(?)";
            CallableStatement callableStatement = connection.prepareCall(query);
            callableStatement.setInt(1, employee_code);
            callableStatement.execute();

            DefaultTableModel model = new DefaultTableModel();
            model.addColumn("Фамилия");
            model.addColumn("Имя");
            model.addColumn("Полная зарплата");

            ResultSet resultSet = callableStatement.getResultSet();
            while (resultSet.next()) {
                model.addRow(new Object[]{resultSet.getString(1), resultSet.getString(2), resultSet.getInt(3)});
            }

            return model;
        } catch (SQLException ex) {
            ex.printStackTrace();
        }
        return null;
    }

    public static DefaultTableModel getMostWorksDone(int employee_code) {
        try {
            Connection connection = DriverManager.getConnection(url, user, password);
            String query = "SELECT * FROM total_works_done(?)";
            CallableStatement callableStatement = connection.prepareCall(query);
            callableStatement.setInt(1, employee_code);
            callableStatement.execute();

            DefaultTableModel model = new DefaultTableModel();
            model.addColumn("Фамилия");
            model.addColumn("Имя");
            model.addColumn("Количество выполненных работ");

            ResultSet resultSet = callableStatement.getResultSet();
            while (resultSet.next()) {
                model.addRow(new Object[]{resultSet.getString(1), resultSet.getString(2), resultSet.getInt(3)});
            }

            return model;
        } catch (SQLException ex) {
            ex.printStackTrace();
        }

        return null;
    }

    public static void updateBonus() {
        try {
            Connection connection = DriverManager.getConnection(url, user, password);
            String query = "CALL update_bonus()";
            CallableStatement callableStatement = connection.prepareCall(query);
            callableStatement.execute();
        } catch (SQLException ex) {
            ex.printStackTrace();
        }
    }

    public static class UI {
        private DefaultTableModel createTableModel(String query) {
            try {
                Connection connection = DriverManager.getConnection(url, user, password);
                Statement statement = connection.createStatement();
                ResultSet resultSet = statement.executeQuery(query);

                DefaultTableModel model = new DefaultTableModel();

                ResultSetMetaData metaData = resultSet.getMetaData();
                int columnCount = metaData.getColumnCount();
                for (int i = 1; i <= columnCount; i++) {
                    model.addColumn(metaData.getColumnName(i));
                }
                while (resultSet.next()) {
                    Object[] row = new Object[columnCount];
                    for (int i = 1; i <= columnCount; i++) {
                        row[i - 1] = resultSet.getObject(i);
                    }
                    model.addRow(row);
                }

                return model;
            } catch (SQLException ex) {
                ex.printStackTrace();
            }
            return null;
        }

        private void addTableModelListener(JFrame frame, DefaultTableModel model) {
            model.addTableModelListener(event -> {
                int row = event.getFirstRow();
                int column = event.getColumn();
                boolean tableChanged = false;
                if (row >= 0 && column >= 0 && !tableChanged) {
                    JOptionPane.showMessageDialog(frame, "После изменения данных, необходимо сохранить изменения!");
                    tableChanged = true;
                }
            });
        }

        private void updateFrame(JFrame frame, JScrollPane scrollPane) {
            frame.getContentPane().removeAll();
            frame.add(scrollPane);
            frame.revalidate();
            frame.repaint();
        }

        public void createUI() {
            JFrame frame = new JFrame("Database App");
            frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
            frame.setSize(700, 500);

            // menu
            JMenuBar menuBar = new JMenuBar();

            // таблицы
            JMenu tables = new JMenu("Таблицы");
            JMenuItem workers = new JMenuItem("Сотрудники");
            JMenuItem typesOfWork = new JMenuItem("Виды работ");
            JMenuItem works = new JMenuItem("Работы");

            tables.add(workers);
            tables.add(typesOfWork);
            tables.add(works);

            // правка
            JMenu edits = new JMenu("Правка");
            JMenuItem add = new JMenuItem("Добавить");
            JMenuItem delete = new JMenuItem("Удалить");
            JMenuItem save = new JMenuItem("Сохранить изменения");

            edits.add(add);
            edits.add(delete);
            edits.add(save);

            JMenu queries = new JMenu("Запросы");
            JMenuItem salary_budget = new JMenuItem("Бюджет на зарплату");
            JMenuItem works_done = new JMenuItem("Сотрудники, которые выполнили больше 1 работы");
            JMenuItem works_employees = new JMenuItem("Информация об обязанностях сотрудников и сроках");
            JMenuItem responsible_employee = new JMenuItem("Фамилии ответственных сотрудников по работам");
            JMenuItem count_payment = new JMenuItem("Посчитать доплату сотрудникам");
            JMenuItem full_salary = new JMenuItem("Посчитать полную зарплату сотруднику");
            JMenuItem total_works_done = new JMenuItem("Количество выполненных работ сотрудником");

            // обычные запросы
            queries.add(salary_budget);
            queries.add(works_done);
            queries.add(works_employees);
            queries.add(responsible_employee);
            // функции
            queries.add(count_payment);
            queries.add(full_salary);
            queries.add(total_works_done);

            menuBar.add(tables);
            menuBar.add(edits);
            menuBar.add(queries);

            frame.setJMenuBar(menuBar);
            frame.setVisible(true);


            DefaultTableModel workersModel = createTableModel("SELECT * FROM Сотрудники ORDER BY Код_сотрудника ASC");
            DefaultTableModel typesOfWorkModel = createTableModel("SELECT * FROM Виды_работ ORDER BY Код_работы ASC");
            DefaultTableModel worksModel = createTableModel("SELECT * FROM Работы ORDER BY Код_работы ASC");

            AtomicReference<DefaultTableModel> currentModel = new AtomicReference<>();

            // обработчики событий
            // таблицы
            workers.addActionListener(e -> {
                JTable table = new JTable(workersModel);
                JScrollPane scrollPane = new JScrollPane(table);

                updateFrame(frame, scrollPane);

                currentModel.set(workersModel);
            });
            typesOfWork.addActionListener(e -> {
                JTable table = new JTable(typesOfWorkModel);
                JScrollPane scrollPane = new JScrollPane(table);

                updateFrame(frame, scrollPane);

                currentModel.set(typesOfWorkModel);
            });
            works.addActionListener(e -> {
                JTable table = new JTable(worksModel);
                JScrollPane scrollPane = new JScrollPane(table);

                updateFrame(frame, scrollPane);

                currentModel.set(worksModel);
            });


            //правки
            add.addActionListener(e -> {
                boolean isAdded = false;
                if (!isAdded) {
                    JOptionPane.showMessageDialog(frame, "После добавления данных, необходимо сохранить изменения!");
                    isAdded = true;
                }

                currentModel.get().addRow(new Object[currentModel.get().getColumnCount()]);

                updateFrame(frame, new JScrollPane(new JTable(currentModel.get())));
            });
            delete.addActionListener(e -> {
                int row = ((JTable) ((JScrollPane) frame.getContentPane().getComponent(0)).getViewport().getView()).getSelectedRow();
                if (row >= 0) {
                    try {
                        Connection connection = DriverManager.getConnection(url, user, password);
                        Statement statement = connection.createStatement();

                        String tableName = "";
                        String condition = "";
                        if (currentModel.get() == workersModel) {
                            tableName = "Сотрудники";
                            condition = "Код_сотрудника";
                        } else if (currentModel.get() == typesOfWorkModel) {
                            tableName = "Виды_работ";
                            condition = "Код_работы";
                        } else if (currentModel.get() == worksModel) {
                            tableName = "Работы";
                            condition = "Код_работы";
                        }

                        String query = "DELETE FROM " + tableName + " WHERE " + condition + " = '" + currentModel.get().getValueAt(row, 0) + "'";
                        statement.executeUpdate(query);

                        currentModel.get().removeRow(row);

                        updateFrame(frame, new JScrollPane(new JTable(currentModel.get())));

                        JOptionPane.showMessageDialog(frame, "Удалено!");
                    } catch (SQLException ex) {
                        ex.printStackTrace();
                        JOptionPane.showMessageDialog(frame, "Нельзя удалить запись, на которую ссылаются другие таблицы!");
                    }
                }
            });

            save.addActionListener(e -> {
                try {
                    Connection connection = DriverManager.getConnection(url, user, password);
                    Statement statement = connection.createStatement();

                    for (int i = 0; i < currentModel.get().getRowCount(); i++) {
                        StringBuilder query = new StringBuilder("INSERT INTO ");
                        String condition = "";
                        if (currentModel.get() == workersModel) {
                            query.append("Сотрудники (");
                            condition = "Код_сотрудника";
                        } else if (currentModel.get() == typesOfWorkModel) {
                            query.append("Виды_работ (");
                            condition = "Код_работы";
                        } else if (currentModel.get() == worksModel) {
                            query.append("Работы (");
                            condition = "Код_работы, Код_сотрудника";
                        }

                        for (int j = 0; j < currentModel.get().getColumnCount(); j++) {
                            query.append(currentModel.get().getColumnName(j));
                            if (j != currentModel.get().getColumnCount() - 1) {
                                query.append(", ");
                            }
                        }

                        query.append(") VALUES (");

                        for (int j = 0; j < currentModel.get().getColumnCount(); j++) {
                            query.append("'").append(currentModel.get().getValueAt(i, j)).append("'");
                            if (j != currentModel.get().getColumnCount() - 1) {
                                query.append(", ");
                            }
                        }

                        query.append(") ON CONFLICT (").append(condition).append(") DO UPDATE SET ");

                        for (int j = 0; j < currentModel.get().getColumnCount(); j++) {
                            query.append(currentModel.get().getColumnName(j)).append(" = '").append(currentModel.get().getValueAt(i, j)).append("'");
                            if (j != currentModel.get().getColumnCount() - 1) {
                                query.append(", ");
                            }
                        }

                        statement.executeUpdate(query.toString());
                    }

                    JOptionPane.showMessageDialog(frame, "Изменения сохранены!");
                } catch (SQLException ex) {
                    ex.printStackTrace();
                    JOptionPane.showMessageDialog(frame, "Возникла ошибка при сохранении данных! Проверьте правильность введенных данных.");
                }
            });


            //запросы
            salary_budget.addActionListener(e -> {
                DefaultTableModel model = createTableModel("SELECT SUM(Зарплата) AS Бюджет_на_зарплату FROM Сотрудники");

                JTable table = new JTable(model);
                JScrollPane scrollPane = new JScrollPane(table);

                updateFrame(frame, scrollPane);
            });

            works_done.addActionListener(e -> {
                DefaultTableModel model = createTableModel("SELECT Сотрудники.Код_сотрудника, COUNT(*) AS Выполнено_работ\n" +
                        "FROM Сотрудники, Работы\n" +
                        "WHERE Сотрудники.Код_сотрудника = Работы.Код_сотрудника\n" +
                        "GROUP BY Сотрудники.Код_сотрудника\n" +
                        "HAVING COUNT(*) > 1;");

                JTable table = new JTable(model);
                JScrollPane scrollPane = new JScrollPane(table);

                updateFrame(frame, scrollPane);
            });

            works_employees.addActionListener(e -> {
                DefaultTableModel model = createTableModel("SELECT Виды_работ.Описание, Работы.Дата_конца, Виды_работ.Срок, Работы.Код_сотрудника\n" +
                        "FROM Виды_работ\n" +
                        "INNER JOIN Работы ON Виды_работ.Код_работы = Работы.Код_работы;\n");


                JTable table = new JTable(model);
                JScrollPane scrollPane = new JScrollPane(table);

                updateFrame(frame, scrollPane);
            });

            responsible_employee.addActionListener(e -> {
                DefaultTableModel model = createTableModel("SELECT Виды_работ.Код_работы, Виды_работ.Описание, \n" +
                        "(SELECT COUNT(*) FROM Работы WHERE Работы.Код_работы = Виды_работ.Код_работы) AS Количество_сотрудников,\n" +
                        "(SELECT Фамилия FROM Сотрудники WHERE Сотрудники.Код_сотрудника = (SELECT Код_сотрудника \n" +
                        "FROM Работы WHERE Работы.Код_работы = Виды_работ.Код_работы LIMIT 1)) AS Фамилия_ответственного\n" +
                        "FROM Виды_работ;");

                JTable table = new JTable(model);
                JScrollPane scrollPane = new JScrollPane(table);

                updateFrame(frame, scrollPane);
            });

            count_payment.addActionListener(e -> {
                updateBonus();
                JOptionPane.showMessageDialog(frame, "Доплата сотрудникам посчитана!");
            });

            full_salary.addActionListener(e -> {
                JLabel label = new JLabel("Код сотрудника:");
                JTextArea textField = new JTextArea(1, 10);

                JButton button = new JButton("Поиск");

                JPanel panel = new JPanel();
                panel.add(label);
                panel.add(textField);
                panel.add(button);

                JPanel mainPanel = new JPanel();
                mainPanel.setLayout(new BoxLayout(mainPanel, BoxLayout.Y_AXIS));

                mainPanel.add(panel);

                JScrollPane[] scrollPaneRef = new JScrollPane[1];

                frame.getContentPane().removeAll();
                frame.add(mainPanel);
                frame.revalidate();
                frame.repaint();

                button.addActionListener(e1 -> {
                    int employee_code;
                    try {
                        employee_code = Integer.parseInt(textField.getText());
                    } catch (NumberFormatException ex) {
                        JOptionPane.showMessageDialog(frame, "Код сотрудника должен быть числом!");
                        return;
                    }
                    DefaultTableModel model = getFullSalary(employee_code);
                    JTable table = new JTable(model);
                    JScrollPane newScrollPane = new JScrollPane(table);

                    if (scrollPaneRef[0] != null) {
                        mainPanel.remove(scrollPaneRef[0]);
                    }

                    mainPanel.add(newScrollPane);
                    scrollPaneRef[0] = newScrollPane;

                    frame.revalidate();
                    frame.repaint();

                });
            });
            total_works_done.addActionListener(e -> {
                JLabel label = new JLabel("Код сотрудника:");
                JTextArea textField = new JTextArea(1, 10);

                JButton button = new JButton("Поиск");

                JPanel panel = new JPanel();
                panel.add(label);
                panel.add(textField);
                panel.add(button);

                JPanel mainPanel = new JPanel();
                mainPanel.setLayout(new BoxLayout(mainPanel, BoxLayout.Y_AXIS));

                mainPanel.add(panel);

                JScrollPane[] scrollPaneRef = new JScrollPane[1];

                frame.getContentPane().removeAll();
                frame.add(mainPanel);
                frame.revalidate();
                frame.repaint();

                button.addActionListener(e1 -> {
                    int employee_code;
                    try {
                        employee_code = Integer.parseInt(textField.getText());
                    } catch (NumberFormatException ex) {
                        JOptionPane.showMessageDialog(frame, "Код сотрудника должен быть числом!");
                        return;
                    }
                    DefaultTableModel model = getMostWorksDone(employee_code);

                    JTable table = new JTable(model);
                    JScrollPane newScrollPane = new JScrollPane(table);

                    if (scrollPaneRef[0] != null) {
                        mainPanel.remove(scrollPaneRef[0]);
                    }

                    mainPanel.add(newScrollPane);
                    scrollPaneRef[0] = newScrollPane;

                    frame.revalidate();
                    frame.repaint();

                });
            });
        }
    }

    public static void main(String[] args) {
        UI ui = new UI();
        ui.createUI();
    }
}
