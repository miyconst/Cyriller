using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Cyriller.Model;

namespace Cyriller.Checker
{
    public partial class AdjectiveForm : UserControl
    {
        private static CyrAdjectiveCollection cyrCollection;

        public AdjectiveForm()
        {
            InitializeComponent();
        }

        protected void SetResult(CyrResult result)
        {
            txtCase1.Text = result[1];
            txtCase2.Text = result[2];
            txtCase3.Text = result[3];
            txtCase4.Text = result[4];
            txtCase5.Text = result[5];
            txtCase6.Text = result[6];
        }

        protected void Log(string text)
        {
            string line = $"{DateTime.Now.ToString("yyyy.MM.dd HH:mm: ")} {text}{Environment.NewLine}";

            txtLog.Text = line + txtLog.Text;
        }

        private async void AdjectiveForm_Load(object sender, EventArgs e)
        {
            await this.LoadCollection();

            ddlNumber.SelectedIndex = 0;
            ddlGender.SelectedIndex = 0;
            ddlAnimate.SelectedIndex = 0;

            this.ParentForm.AcceptButton = btnDecline;
        }

        private async Task LoadCollection()
        {
            if (cyrCollection != null)
            {
                return;
            }

            Stopwatch watch = new Stopwatch(); LoadingForm formLoading = new LoadingForm()
            {
                Dock = DockStyle.Fill
            };

            this.Cursor = Cursors.WaitCursor;
            this.tlpMain.Visible = false;
            this.Controls.Add(formLoading);

            watch.Start();
            await Task.Run(() =>
            {
                cyrCollection = new CyrAdjectiveCollection();
            });
            watch.Stop();

            this.Log($"Создание {nameof(CyrAdjectiveCollection)} заняло {watch.Elapsed}.");
            this.Cursor = Cursors.Default;
            this.tlpMain.Visible = true;
            this.Controls.Remove(formLoading);
        }

        private void btnDecline_Click(object sender, EventArgs e)
        {
            if (txtWord.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Необходимо ввести слово!");
            }

            CyrAdjective adj;
            GendersEnum declineToGender = (GendersEnum)(ddlGender.SelectedIndex + 1);
            AnimatesEnum declineToAnimate = (AnimatesEnum)(ddlAnimate.SelectedIndex + 1);
            NumbersEnum declineToNumber = (NumbersEnum)(ddlNumber.SelectedIndex + 1);

            string foundWord;
            GendersEnum foundGender;
            CasesEnum foundCase;
            NumbersEnum foundNumber;
            AnimatesEnum foundAnimate;
            CyrResult result = null;

            try
            {
                Stopwatch watch = new Stopwatch();

                watch.Start();
                adj = cyrCollection.Get(txtWord.Text, out foundWord, out foundGender, out foundCase, out foundNumber, out foundAnimate);

                if (declineToNumber == NumbersEnum.Singular)
                {
                    result = adj.Decline(declineToGender, declineToAnimate);
                }
                else
                {
                    result = adj.DeclinePlural(declineToAnimate);
                }

                watch.Stop();
                this.Log($"Склонение слова {txtWord.Text} заняло {watch.Elapsed}.");
            }
            catch (CyrWordNotFoundException)
            {
                MessageBox.Show("Данное слово не найдено в коллекции!");
                return;
            }

            this.SetResult(result);
            txtCollectionName.Text = foundWord;
            txtDetails.Text = $"{foundGender}, {foundCase}, {foundNumber}, {foundAnimate}";

            if (txtWord.Text == foundWord)
            {
                txtCollectionName.BackColor = SystemColors.Control;
            }
            else
            {
                txtCollectionName.BackColor = SystemColors.Highlight;
            }
        }
    }
}
