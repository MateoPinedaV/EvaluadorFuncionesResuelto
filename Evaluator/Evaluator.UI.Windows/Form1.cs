using System;
using System.Drawing;
using System.Windows.Forms;
using Evaluator.Logic;

namespace Evaluator.UI.Windows
{
    public partial class Form1 : Form
    {
        private string input = "";

        public Form1()
        {
            InitializeComponent();
            CreateCalculatorButtons();
        }

        private void CreateCalculatorButtons()
        {
            string[] buttons = {
        "7", "8", "9", "/",
        "4", "5", "6", "*",
        "1", "2", "3", "-",
        "0", ".", "=", "+"
    };

            var display = new TextBox
            {
                Name = "txtDisplay",
                ReadOnly = true,
                Dock = DockStyle.Top,
                Height = 40,
                Font = new Font("Segoe UI", 14), // Más pequeño que antes
                TextAlign = HorizontalAlignment.Right
            };
            this.Controls.Add(display);

            var panel = new TableLayoutPanel
            {
                RowCount = 5,
                ColumnCount = 4,
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            for (int j = 0; j < 4; j++)
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            for (int j = 0; j < 4; j++)
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

            int i = 0;
            foreach (var btn in buttons)
            {
                var button = new Button
                {
                    Text = btn,
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 14)
                };
                button.Click += (s, e) => OnButtonClick(btn, display);
                panel.Controls.Add(button, i % 4, i / 4);
                i++;
            }

            // Botón "C"
            var clearButton = new Button
            {
                Text = "C",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 14)
            };
            clearButton.Click += (s, e) => OnButtonClick("C", display);
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            panel.Controls.Add(clearButton, 0, 4);
            panel.SetColumnSpan(clearButton, 4); // Ocupa todas las columnas

            this.Controls.Add(panel);
        }

        private void OnButtonClick(string btnText, TextBox display)
        {
            if (btnText == "C")
            {
                input = "";
            }
            else if (btnText == "=")
            {
                try
                {
                    // Reemplaza coma por punto para compatibilidad con CultureInfo.InvariantCulture
                    string sanitizedInput = input.Replace(',', '.');
                    double result = FunctionEvaluator.Evaluate(sanitizedInput);

                    // Muestra el resultado con coma como separador decimal y 2 cifras decimales
                    input = result.ToString("N2").Replace('.', ',');
                }
                catch (Exception)
                {
                    input = "Error";
                }
            }
            else
            {
                input += btnText;
            }

            display.Text = input;
        }

    }
}
