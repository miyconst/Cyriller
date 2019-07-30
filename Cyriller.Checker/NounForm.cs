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
    public partial class NounForm : UserControl
    {
        private static CyrNounCollection cyrCollection;

        public NounForm()
        {
            InitializeComponent();
        }

        protected void SetResult(CyrResult Result)
        {
            txtCase1.Text = Result[1];
            txtCase2.Text = Result[2];
            txtCase3.Text = Result[3];
            txtCase4.Text = Result[4];
            txtCase5.Text = Result[5];
            txtCase6.Text = Result[6];
        }

        protected void Log(string text)
        {
            string line = $"{DateTime.Now.ToString("yyyy.MM.dd HH:mm: ")} {text}{Environment.NewLine}";
            txtLog.Text = line + txtLog.Text;
        }

        private async void NounForm_Load(object sender, EventArgs e)
        {
            await this.LoadCollection();

            ddlAction.SelectedIndex = 0;
            this.ParentForm.AcceptButton = btnDecline;
        }

        private async Task LoadCollection()
        {
            if (cyrCollection != null)
            {
                return;
            }

            Stopwatch watch = new Stopwatch();
            LoadingForm formLoading = new LoadingForm()
            {
                Dock = DockStyle.Fill
            };

            this.Cursor = Cursors.WaitCursor;
            this.tlpMain.Visible = false;
            this.Controls.Add(formLoading);

            watch.Start();
            await Task.Run(() =>
            {
                cyrCollection = new CyrNounCollection();
            });
            watch.Stop();

            this.Log($"Создание {nameof(CyrNounCollection)} заняло {watch.Elapsed}.");
            this.Cursor = Cursors.Default;
            this.tlpMain.Visible = true;
            this.Controls.Remove(formLoading);
        }

        private void btnDecline_Click(object sender, EventArgs e)
        {
            if (txtWord.Text.IsNullOrEmpty())
            {
                MessageBox.Show("Необходимо ввести слово!");
                return;
            }

            CyrNoun noun;
            string foundWord;
            CasesEnum foundCase;
            NumbersEnum foundNumber;
            CyrResult result = null;

            try
            {
                Stopwatch watch = new Stopwatch();

                watch.Start();
                noun = cyrCollection.Get(txtWord.Text, out foundWord, out foundCase, out foundNumber);

                switch (ddlAction.SelectedIndex)
                {
                    case 0:
                        result = noun.Decline();
                        break;
                    case 1:
                        result = noun.DeclinePlural();
                        break;

                    default:
                        MessageBox.Show("Необходимо выбрать тип склонения!");
                        return;
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
            txtDetails.Text = $"{foundCase}, {foundNumber}, {noun.Gender}, {noun.Animate}, {noun.WordType}";

            if (txtWord.Text == foundWord)
            {
                txtCollectionName.BackColor = System.Drawing.SystemColors.Control;
            }
            else
            {
                txtCollectionName.BackColor = System.Drawing.SystemColors.Highlight;
            }
        }
    }
}
