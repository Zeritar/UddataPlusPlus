using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Input;

namespace UddataPlusPlus
{
    public enum View
    {
        Login,
        StudentView,
        TeacherView,
        AdminView
    }

    public partial class Methods
    {
        public FlowDocument BuildTeacherCourseView(User user)
        {
            FlowDocument fd = new FlowDocument();

            List<Course> courses = GetTeacherCourses(user);

            // Create the Table...
            var table1 = new Table();

            // Create and add an empty TableRowGroup to hold the table's Rows.
            table1.RowGroups.Add(new TableRowGroup());

            // Set some global formatting properties for the table.
            table1.CellSpacing = 0;
            table1.Background = Brushes.Black;

            // Create 6 columns and add them to the table's Columns collection.
            int numberOfColumns = 3;
            for (int x = 0; x < numberOfColumns; x++)
            {
                table1.Columns.Add(new TableColumn());
            }
            string[] headers = new string[] { "Holdnavn", "Fag", "Antal Elever" };
            table1 = CreateTableHeader(table1, headers);

            foreach (Course course in courses)
            {
                string[] cells = new string[] { course.ClassName, course.CourseType.ToString(), GetCourseStudents(course).Count.ToString() };
                table1 = CreateTableRow(table1, cells, true);
            }

            fd.Blocks.Add(table1);

            return fd;
        }

        public FlowDocument BuildStudentCourseView(User user)
        {
            FlowDocument fd = new FlowDocument();

            List<Course> courses = GetStudentCourses(user);

            // Create the Table...
            var table1 = new Table();

            // Create and add an empty TableRowGroup to hold the table's Rows.
            table1.RowGroups.Add(new TableRowGroup());

            // Set some global formatting properties for the table.
            table1.CellSpacing = 0;
            table1.Background = Brushes.White;

            // Create 6 columns and add them to the table's Columns collection.
            int numberOfColumns = 3;
            for (int x = 0; x < numberOfColumns; x++)
            {
                table1.Columns.Add(new TableColumn());
            }
            string[] headers = new string[] { "Holdnavn", "Fag", "Lærer" };
            table1 = CreateTableHeader(table1, headers);

            foreach (Course course in courses)
            {
                string[] cells = new string[] { course.ClassName, course.CourseType.ToString(), GetTeacherNameFromID(course.FK_TeacherID) };
                table1 = CreateTableRow(table1, cells, false);
            }

            fd.Blocks.Add(table1);

            return fd;
        }

        public Table CreateTableHeader(Table table, string[] names)
        {
            // Add the first (title) row.
            table.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRow = table.RowGroups[0].Rows[table.RowGroups[0].Rows.Count() - 1];

            // Global formatting for the header row.
            currentRow.FontSize = 18;
            currentRow.FontWeight = FontWeights.Bold;
            currentRow.Background = new SolidColorBrush(Color.FromRgb(0x77, 0x77, 0xCC));

            SolidColorBrush backColor = new SolidColorBrush(Color.FromRgb(0xAA, 0xAA, 0xDD));

            // Add cells with content to the second row.
            foreach (string name in names)
                currentRow.Cells.Add(TableCellFromString(name, backColor, false));

            return table;
        }

        public Table CreateTableRow(Table table, string[] cells, bool teacher)
        {
            // Add the first (title) row.
            table.RowGroups[0].Rows.Add(new TableRow());

            // Alias the current working row for easy reference.
            TableRow currentRow = table.RowGroups[0].Rows[table.RowGroups[0].Rows.Count() - 1];

            // Global formatting for the header row.
            currentRow.FontSize = 16;
            currentRow.FontWeight = FontWeights.Regular;
            currentRow.Background = new SolidColorBrush(Color.FromRgb(0x77, 0x77, 0xCC));

            // Add cells with content to the second row.

            for (int i = 0; i < cells.Length; i++)
            {
                bool courseLink = false;
                SolidColorBrush backColor;
                switch (i)
                {
                    case 0:
                        backColor = new SolidColorBrush(Color.FromRgb(0xD0, 0xD0, 0xFF));
                        if (teacher)
                            courseLink = true;
                        break;
                    case 1:
                        backColor = new SolidColorBrush(Color.FromRgb(0xDD, 0xDD, 0xFF));
                        break;
                    case 2:
                        backColor = new SolidColorBrush(Color.FromRgb(0xD0, 0xD0, 0xFF));
                        break;
                    default:
                        backColor = new SolidColorBrush(Color.FromRgb(0xDD, 0xDD, 0xFF));
                        break;
                }
                currentRow.Cells.Add(TableCellFromString(cells[i], backColor, courseLink));
            }

            return table;
        }

        public TableCell TableCellFromString(string str, SolidColorBrush backColor, bool hyperlink)
        {
            TextBlock tblock = new TextBlock();
            tblock.VerticalAlignment = VerticalAlignment.Center;
            tblock.HorizontalAlignment = HorizontalAlignment.Center;
            tblock.TextWrapping = TextWrapping.Wrap;
            tblock.Text = str;

            if (hyperlink)
            {
                tblock.MouseUp += CourseName_Click;
                tblock.TextDecorations = TextDecorations.Underline;
                tblock.Cursor = Cursors.Hand;
                tblock.Foreground = new SolidColorBrush(Color.FromRgb(0x00, 0x00, 0xA0));
            }
                

            Grid grid = new Grid();
            grid.Height = 60;
            grid.Background = Brushes.White;
            grid.VerticalAlignment = VerticalAlignment.Stretch;
            grid.Background = backColor;// new SolidColorBrush(Color.FromRgb(0xDD, 0xDD, 0xFF));


            RowDefinition rowDefin = new RowDefinition();
            rowDefin.Height = new GridLength(1.0, GridUnitType.Star);
            grid.RowDefinitions.Add(rowDefin);
            grid.Children.Add(tblock);
            BlockUIContainer bc = new BlockUIContainer(grid);
            bc.Background = Brushes.Pink;
            TableCell tc = new TableCell(bc);
            tc.BorderThickness = new Thickness(0, 0, 0, 1);


            return tc;
        }

        private void CourseName_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender.GetType() == typeof(TextBlock))
            {
                TextBlock tb = (TextBlock)sender;
                tb.Text = "Clicked!";
            }
            else
                MessageBox.Show(main.window, "CourseName_Click triggered by non-TextBlock object.", "Error");
        }
    }
}
