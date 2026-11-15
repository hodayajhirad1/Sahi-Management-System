using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SahiApp
{
    // דרישה 5+6: תכנות מונחה אירועים וייצוג גרפי של אובייקטים
    public partial class Form1 : Form
    {
        private DataManager dataManager;
        private List<string> selectedVolunteers = new List<string>();
        private Route selectedRoute = null;

        // פקדים שיווצרו דינמית לחסכון בעיצוב ידני
        private Panel panelStep1, panelStep2, panelStep3;
        private ListBox lbPool, lbSelected, lbRoutes;
        private RadioButton rbFoot, rbVehicle;
        private Label lblPackageCount;
        private DataGridView dgvFamilies;
        private Button btnFinish;

        public Form1()
        {
            InitializeComponent();
            InitializeData();
            InitializeCustomComponents();
            ShowStep1();
        }

        // טעינת נתונים ראשונית או שחזור (דרישה 7)
        private void InitializeData()
        {
            dataManager = Serializer.LoadData();

            if (dataManager == null)
            {
                dataManager = new DataManager();

                // מאגר שמות של מתנדבים בסח"י
                dataManager.AddVolunteer("אבי כהן");
                dataManager.AddVolunteer("יוסי לוי");
                dataManager.AddVolunteer("מיכל אברהם");
                dataManager.AddVolunteer("דניאל משה");
                dataManager.AddVolunteer("נועה פרץ");
                dataManager.AddVolunteer("רוני אזולאי");

                // יצירת מסלולים לדוגמה (פולימורפיזם בפעולה)
                FootRoute f1 = new FootRoute("מסלול רגלי - שכונה א'", 0.5);
                f1.Families.Add(new Family("מזרחי", "החלוצים 4", 2, 5, "קוד בשער: 1234"));
                f1.Families.Add(new Family("אלבז", "בן גוריון 12", 0, 1, "להשאיר ליד הדלת"));
                f1.Families.Add(new Family("עמר", "השלום 8", 3, 11, "לדפוק חזק, לא שומעים"));

                FootRoute f2 = new FootRoute("מסלול רגלי - מרכז העיר", 0.8);
                f2.Families.Add(new Family("חזן", "רמבם 3", 1, 2, "צד שמאל של המעלית"));

                VehicleRoute v1 = new VehicleRoute("מסלול רכב - שכונות מזרחיות", "משפחתית");
                v1.Families.Add(new Family("ביטון", "הקוממיות 45", 4, 16, "יש מעלית"));
                v1.Families.Add(new Family("ועקנין", "האורנים 2", 1, 3, "קוד 5566"));
                v1.Families.Add(new Family("שפירא", "צה\"ל 102", 0, 2, "כניסה מאחור"));
                v1.Families.Add(new Family("גולן", "שדרות ירושלים 9", 5, 20, "להתקשר לפני"));
                v1.Families.Add(new Family("דדון", "החצב 14", 2, 4, "אין קוד"));
                v1.Families.Add(new Family("קדוש", "התאנה 3", 1, 1, "ליד המקלט"));

                dataManager.AddRoute(f1);
                dataManager.AddRoute(f2);
                dataManager.AddRoute(v1);

                // שמירה ראשונית
                Serializer.SaveData(dataManager);
            }
        }

        // בניית הממשק בצורה תכנותית נקייה שמתאימה ישירות להוראות
        private void InitializeCustomComponents()
        {
            this.Text = "מערכת ניהול חלוקה - סח\"י";
            this.Size = new Size(850, 600);
            this.RightToLeft = RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = FormStartPosition.CenterScreen;

            // ---- שלב 1: בחירת צוות וסוג חלוקה ----
            panelStep1 = new Panel { Dock = DockStyle.Fill, Visible = false };

            Label lblTitle1 = new Label { Text = "שלב א': בחירת חברי הצוות וסוג חלוקה", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 14, FontStyle.Bold) };
            panelStep1.Controls.Add(lblTitle1);

            Label lblPoolTag = new Label { Text = "מאגר מתנדבים (לחץ כפול להוספה):", Location = new Point(20, 60), AutoSize = true };
            lbPool = new ListBox { Location = new Point(20, 80), Size = new Size(200, 250) };
            lbPool.DoubleClick += LbPool_DoubleClick;

            Label lblSelectedTag = new Label { Text = "הצוות שנבחר (לחץ כפול להסרה):", Location = new Point(260, 60), AutoSize = true };
            lbSelected = new ListBox { Location = new Point(260, 80), Size = new Size(200, 250) };
            lbSelected.DoubleClick += LbSelected_DoubleClick;

            GroupBox gbType = new GroupBox { Text = "סוג חלוקה", Location = new Point(490, 75), Size = new Size(200, 100) };
            rbFoot = new RadioButton { Text = "חלוקה ברגל (עד 6 חבילות)", Location = new Point(15, 30), Checked = true, AutoSize = true };
            rbVehicle = new RadioButton { Text = "חלוקה ברכב (עד 12 חבילות)", Location = new Point(15, 60), AutoSize = true };
            gbType.Controls.Add(rbFoot);
            gbType.Controls.Add(rbVehicle);

            Button btnToStep2 = new Button { Text = "המשך לבחירת מסלול", Location = new Point(260, 360), Size = new Size(200, 40), BackColor = Color.LightBlue };
            btnToStep2.Click += BtnToStep2_Click;

            panelStep1.Controls.AddRange(new Control[] { lblPoolTag, lbPool, lblSelectedTag, lbSelected, gbType, btnToStep2 });

            // ---- שלב 2: בחירת מסלול וכמות ארגזים ----
            panelStep2 = new Panel { Dock = DockStyle.Fill, Visible = false };
            Label lblTitle2 = new Label { Text = "שלב ב': בחירת מסלול חלוקה", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 14, FontStyle.Bold) };
            panelStep2.Controls.Add(lblTitle2);

            lbRoutes = new ListBox { Location = new Point(20, 70), Size = new Size(350, 250), Font = new Font("Arial", 11) };
            lbRoutes.SelectedIndexChanged += LbRoutes_SelectedIndexChanged;

            lblPackageCount = new Label { Text = "כמות ארגזים לקחת: 0", Location = new Point(400, 70), AutoSize = true, Font = new Font("Arial", 14, FontStyle.Bold), ForeColor = Color.DarkBlue };

            Button btnGo = new Button { Text = "צא לדרך! ➔", Location = new Point(20, 350), Size = new Size(150, 45), BackColor = Color.LightGreen, Font = new Font("Arial", 12, FontStyle.Bold) };
            btnGo.Click += BtnGo_Click;
            panelStep2.Controls.AddRange(new Control[] { lbRoutes, lblPackageCount, btnGo });

            // ---- שלב 3: טבלת מעקב חלוקה ----
            panelStep3 = new Panel { Dock = DockStyle.Fill, Visible = false };
            Label lblTitle3 = new Label { Text = "שלב ג': סטטוס מסירת חבילות בשטח", Location = new Point(20, 20), AutoSize = true, Font = new Font("Arial", 14, FontStyle.Bold) };
            panelStep3.Controls.Add(lblTitle3);

            dgvFamilies = new DataGridView
            {
                Location = new Point(20, 60),
                Size = new Size(780, 280),
                AllowUserToAddRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };
            dgvFamilies.CellValueChanged += dgvFamilies_CellValueChanged;
            dgvFamilies.CurrentCellDirtyStateChanged += dgvFamilies_CurrentCellDirtyStateChanged;

            btnFinish = new Button { Text = "סיימנו!", Location = new Point(20, 360), Size = new Size(150, 45), BackColor = Color.LightGray, Enabled = false, Font = new Font("Arial", 12, FontStyle.Bold) };
            btnFinish.Click += BtnFinish_Click;
            panelStep3.Controls.AddRange(new Control[] { dgvFamilies, btnFinish });

            // הוספת הפאנלים לטופס
            this.Controls.AddRange(new Control[] { panelStep1, panelStep2, panelStep3 });
        }

        private void ShowStep1()
        {
            panelStep1.Visible = true;
            panelStep2.Visible = false;
            panelStep3.Visible = false;

            lbPool.Items.Clear();
            foreach (var vol in dataManager.VolunteersPool)
                lbPool.Items.Add(vol);

            lbSelected.Items.Clear();
            selectedVolunteers.Clear();
        }

        private void LbPool_DoubleClick(object sender, EventArgs e)
        {
            if (lbPool.SelectedItem != null)
            {
                string name = lbPool.SelectedItem.ToString();
                selectedVolunteers.Add(name);
                lbSelected.Items.Add(name);
                lbPool.Items.Remove(name);
            }
        }

        private void LbSelected_DoubleClick(object sender, EventArgs e)
        {
            if (lbSelected.SelectedItem != null)
            {
                string name = lbSelected.SelectedItem.ToString();
                selectedVolunteers.Remove(name);
                lbPool.Items.Add(name);
                lbSelected.Items.Remove(name);
            }
        }

        private void BtnToStep2_Click(object sender, EventArgs e)
        {
            if (selectedVolunteers.Count == 0)
            {
                MessageBox.Show("בבקשה תבחרו לפחות חבר צוות אחד לפני שממשיכים.");
                return;
            }
            ShowStep2();
        }

        private void ShowStep2()
        {
            panelStep1.Visible = false;
            panelStep2.Visible = true;
            panelStep3.Visible = false;

            lbRoutes.Items.Clear();
            lblPackageCount.Text = "כמות ארגזים לקחת: 0";

            foreach (Route route in dataManager.AllRoutes)
            {
                if (rbFoot.Checked && route is FootRoute && route.TotalPackages <= 6)
                {
                    lbRoutes.Items.Add(route);
                }
                else if (rbVehicle.Checked && route is VehicleRoute && route.TotalPackages <= 12)
                {
                    lbRoutes.Items.Add(route);
                }
            }
        }

        private void LbRoutes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbRoutes.SelectedItem is Route route)
            {
                selectedRoute = route;
                lblPackageCount.Text = $"כמות ארגזים לקחת: {route.TotalPackages}\nסוג מסלול מוגדר: {route.RouteType}";
            }
        }

        private void BtnGo_Click(object sender, EventArgs e)
        {
            if (selectedRoute == null)
            {
                MessageBox.Show("יש לבחור מסלול מתוך הרשימה.");
                return;
            }
            ShowStep3();
        }

        private void ShowStep3()
        {
            panelStep2.Visible = false;
            panelStep3.Visible = true;

            dgvFamilies.Columns.Clear();
            dgvFamilies.Columns.Add("LastName", "שם משפחה");
            dgvFamilies.Columns.Add("Address", "כתובת");
            dgvFamilies.Columns.Add("Floor", "קומה");
            dgvFamilies.Columns.Add("Apartment", "דירה");
            dgvFamilies.Columns.Add("Notes", "הנחיות נוספות (קוד/דלת)");

            DataGridViewCheckBoxColumn checkCol = new DataGridViewCheckBoxColumn
            {
                Name = "IsDelivered",
                HeaderText = "בוצע העברה?",
                Width = 100
            };
            dgvFamilies.Columns.Add(checkCol);

            foreach (var fam in selectedRoute.Families)
            {
                fam.IsDelivered = false;
                dgvFamilies.Rows.Add(fam.LastName, fam.Address, fam.Floor, fam.Apartment, fam.Notes, fam.IsDelivered);
            }

            btnFinish.Enabled = false;
            btnFinish.BackColor = Color.LightGray;
        }

        private void dgvFamilies_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvFamilies.IsCurrentCellDirty)
            {
                dgvFamilies.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgvFamilies_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvFamilies.Columns[e.ColumnIndex].Name == "IsDelivered")
            {
                bool isChecked = (bool)dgvFamilies.Rows[e.RowIndex].Cells["IsDelivered"].Value;
                selectedRoute.Families[e.RowIndex].IsDelivered = isChecked;

                if (isChecked)
                    dgvFamilies.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                else
                    dgvFamilies.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;

                CheckIfAllDelivered();
            }
        }

        private void CheckIfAllDelivered()
        {
            bool allDone = true;
            foreach (DataGridViewRow row in dgvFamilies.Rows)
            {
                var val = row.Cells["IsDelivered"].Value;
                if (val == null || !(bool)val)
                {
                    allDone = false;
                    break;
                }
            }

            if (allDone)
            {
                btnFinish.Enabled = true;
                btnFinish.BackColor = Color.Green;
                btnFinish.ForeColor = Color.White;
            }
            else
            {
                btnFinish.Enabled = false;
                btnFinish.BackColor = Color.LightGray;
                btnFinish.ForeColor = Color.Black;
            }
        }

        private void BtnFinish_Click(object sender, EventArgs e)
        {
            Serializer.SaveData(dataManager);
            string msg = $"תודה על העזרה! שימחת היום {selectedRoute.TotalPackages} משפחות.\n\nהאם ברצונך לבצע חלוקה נוספת?";
            DialogResult result = MessageBox.Show(msg, "חלוקה הושלמה בהצלחה!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                selectedRoute = null;
                ShowStep1();
            }
            else
            {
                this.Close();
            }
        }
    }
}