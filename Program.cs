using ShowArrow.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

public class ArrowForm : Form
{
    private Point arrowPosition;
    private int arrowDirection; // 0 = Up, 1 = Right, 2 = Down, 3 = Left
    private bool isDragging = false;
    private Point dragStartPoint;
    private Color ArrowColor;
    private int sizer = 5; // Initiale Größe des Pfeils
    private Button arrowButton;
    private Button colorButton;
    private Button rotateButton;

    public ArrowForm()
    {
        this.FormBorderStyle = FormBorderStyle.None;
        this.AllowTransparency = true;
        this.BackColor = Color.LimeGreen;
        this.TransparencyKey = Color.LimeGreen;
        this.Size = new Size(300 * sizer, 300 * sizer);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.arrowPosition = new Point(150 * sizer, 150 * sizer);
        this.arrowDirection = 0;
        this.TopMost = true;
        this.ArrowColor = Color.Orange;

        this.Icon = Icon.ExtractAssociatedIcon(Resources.Icon);

        this.Controls.Add(GenerateArrowButton());
        this.Controls.Add(GenerateColorButton());
        this.Controls.Add(GenerateRotateButton());

        this.MouseDown += ArrowForm_MouseDown;
        this.MouseMove += ArrowForm_MouseMove;
        this.MouseUp += ArrowForm_MouseUp;
        this.MouseClick += ArrowForm_MouseClick;
        this.MouseWheel += ArrowForm_MouseWheel;
        this.Paint += ArrowForm_Paint;
        this.Resize += ArrowForm_Resize;
    }

    private Button GenerateArrowButton()
    {
        arrowButton = new Button();
        arrowButton.Text = "X";
        arrowButton.Size = new Size(30, 30);
        arrowButton.BackColor = Color.Gray;
        arrowButton.ForeColor = Color.Black;
        arrowButton.FlatAppearance.BorderSize = 0;
        arrowButton.FlatStyle = FlatStyle.Flat;
        arrowButton.Click += ArrowButton_Click;

        ToolTip arrowToolTip = new ToolTip();
        arrowToolTip.SetToolTip(arrowButton, "Close Arrow Application");

        return arrowButton;
    }

    private Button GenerateColorButton()
    {
        colorButton = new Button();
        colorButton.Text = "C";
        colorButton.Size = new Size(30, 30);
        colorButton.BackColor = Color.Gray;
        colorButton.ForeColor = Color.Black;
        colorButton.FlatAppearance.BorderSize = 0;
        colorButton.FlatStyle = FlatStyle.Flat;
        colorButton.Click += ColorButton_Click;

        ToolTip colorToolTip = new ToolTip();
        colorToolTip.SetToolTip(colorButton, "Change Color");

        return colorButton;
    }

    private Button GenerateRotateButton()
    {
        rotateButton = new Button();
        rotateButton.Text = "R";
        rotateButton.Size = new Size(30, 30);
        rotateButton.BackColor = Color.Gray;
        rotateButton.ForeColor = Color.Black;
        rotateButton.FlatAppearance.BorderSize = 0;
        rotateButton.FlatStyle = FlatStyle.Flat;
        rotateButton.Click += RotateButton_Click;

        ToolTip rotateToolTip = new ToolTip();
        rotateToolTip.SetToolTip(rotateButton, "Rotate Arrow");

        return rotateButton;
    }

    private void ArrowForm_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        Brush brush = new SolidBrush(this.ArrowColor);
        Point[] arrowPoints = GetArrowPoints();

        g.FillPolygon(brush, arrowPoints);
    }

    private Point[] GetArrowPoints()
    {
        Point[] points = new Point[7];
        int arrowSize = 30 * this.sizer;
        int halfSize = arrowSize / 2;
        int quarterSize = arrowSize / 4;

        switch (arrowDirection)
        {
            case 0: // Left
                points[0] = new Point(arrowPosition.X - arrowSize, arrowPosition.Y);
                points[1] = new Point(arrowPosition.X - quarterSize, arrowPosition.Y - halfSize);
                points[2] = new Point(arrowPosition.X - quarterSize, arrowPosition.Y - quarterSize);
                points[3] = new Point(arrowPosition.X + arrowSize, arrowPosition.Y - quarterSize);
                points[4] = new Point(arrowPosition.X + arrowSize, arrowPosition.Y + quarterSize);
                points[5] = new Point(arrowPosition.X - quarterSize, arrowPosition.Y + quarterSize);
                points[6] = new Point(arrowPosition.X - quarterSize, arrowPosition.Y + halfSize);
                break;
            case 1: // Up
                points[0] = new Point(arrowPosition.X, arrowPosition.Y - arrowSize);
                points[1] = new Point(arrowPosition.X - halfSize, arrowPosition.Y - quarterSize);
                points[2] = new Point(arrowPosition.X - quarterSize, arrowPosition.Y - quarterSize);
                points[3] = new Point(arrowPosition.X - quarterSize, arrowPosition.Y + arrowSize);
                points[4] = new Point(arrowPosition.X + quarterSize, arrowPosition.Y + arrowSize);
                points[5] = new Point(arrowPosition.X + quarterSize, arrowPosition.Y - quarterSize);
                points[6] = new Point(arrowPosition.X + halfSize, arrowPosition.Y - quarterSize);
                break;
            case 2: // Right
                points[0] = new Point(arrowPosition.X + arrowSize, arrowPosition.Y);
                points[1] = new Point(arrowPosition.X + quarterSize, arrowPosition.Y - halfSize);
                points[2] = new Point(arrowPosition.X + quarterSize, arrowPosition.Y - quarterSize);
                points[3] = new Point(arrowPosition.X - arrowSize, arrowPosition.Y - quarterSize);
                points[4] = new Point(arrowPosition.X - arrowSize, arrowPosition.Y + quarterSize);
                points[5] = new Point(arrowPosition.X + quarterSize, arrowPosition.Y + quarterSize);
                points[6] = new Point(arrowPosition.X + quarterSize, arrowPosition.Y + halfSize);
                break;
            case 3: // Down
                points[0] = new Point(arrowPosition.X, arrowPosition.Y + arrowSize);
                points[1] = new Point(arrowPosition.X - halfSize, arrowPosition.Y + quarterSize);
                points[2] = new Point(arrowPosition.X - quarterSize, arrowPosition.Y + quarterSize);
                points[3] = new Point(arrowPosition.X - quarterSize, arrowPosition.Y - arrowSize);
                points[4] = new Point(arrowPosition.X + quarterSize, arrowPosition.Y - arrowSize);
                points[5] = new Point(arrowPosition.X + quarterSize, arrowPosition.Y + quarterSize);
                points[6] = new Point(arrowPosition.X + halfSize, arrowPosition.Y + quarterSize);
                break;
        }
        return points;
    }

    private void ArrowForm_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            isDragging = true;
            dragStartPoint = e.Location;
        }
    }

    private void ArrowForm_MouseMove(object sender, MouseEventArgs e)
    {
        if (isDragging)
        {
            this.Left += e.X - dragStartPoint.X;
            this.Top += e.Y - dragStartPoint.Y;
        }
    }

    private void ArrowForm_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            isDragging = false;
        }
    }

    private void ArrowForm_MouseClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            Rotate();
        }
        if (e.Button == MouseButtons.Middle)
        {
            ChangeColor();
        }
    }

    private void ArrowForm_MouseWheel(object sender, MouseEventArgs e)
    {
        if (e.Delta > 0 && sizer < 15)
        {
            sizer += 1; // Vergrößern
        }
        else if (e.Delta < 0 && sizer > 3)
        {
            sizer -= 1; // Verkleinern
        }
        this.Invalidate();
    }

    private void ArrowForm_Resize(object sender, EventArgs e)
    {
        UpdateButtonPosition();
    }

    private void UpdateButtonPosition()
    {
        int spacerX = 0;
        int spacerY = 0;

        if (arrowDirection == 0 || arrowDirection == 2)
        {
            spacerX = 35;
            spacerY = 0;
        }
        else if (arrowDirection == 1 || arrowDirection == 3)
        {
            spacerX = 0;
            spacerY = 35;
        }
        
        arrowButton.Location = new Point(arrowPosition.X - arrowButton.Width / 2, arrowPosition.Y - arrowButton.Height / 2);
        colorButton.Location = new Point(arrowPosition.X - colorButton.Width / 2 + spacerX, arrowPosition.Y - colorButton.Height / 2 + spacerY);
        rotateButton.Location = new Point(arrowPosition.X - rotateButton.Width / 2 + spacerX * 2, arrowPosition.Y - rotateButton.Height / 2 + spacerY * 2);
    }

    private void ArrowButton_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void ColorButton_Click(object sender, EventArgs e)
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        ArrowColor = ShuffleColor();
        System.Diagnostics.Debug.WriteLine(ArrowColor.Name);
        this.Invalidate();
    }

    private void RotateButton_Click(object sender, EventArgs e)
    {
        Rotate();
    }

    private void Rotate()
    {
        arrowDirection = (arrowDirection + 1) % 4;
        UpdateButtonPosition();
        this.Invalidate();
    }

    private Color ShuffleColor()
    {
        Color[] colors = { Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Pink, Color.Coral, Color.Turquoise };
        var colorList = colors.ToList().Except(new[] { ArrowColor }).ToList();
        int colorKey = new Random().Next(0, colorList.Count);
        return colorList[colorKey];
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new ArrowForm());
    }
}