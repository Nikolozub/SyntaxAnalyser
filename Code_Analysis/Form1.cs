using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Code_Analysis
{
    public partial class Form1 : Form
    {
        string currentFileName;
        string windowsTitle;
        bool textSaved;
        string buff_text;

        ToolStripLabel infoLabel;
        ToolStripLabel positionLabel;

        public Form1()
        {
            InitializeComponent();

            infoLabel = new ToolStripLabel();
            infoLabel.Text = "Позиция курсора:";
            positionLabel = new ToolStripLabel();
            positionLabel.Text = "0";

            statusStrip1.Items.Add(infoLabel);
            statusStrip1.Items.Add(positionLabel);


            windowsTitle = "Анализатор кода";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.textSaved = true;
            newFile();
            buff_text = editRichTextBox.Text;
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editRichTextBox.Redo();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editRichTextBox.Undo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editRichTextBox.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editRichTextBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editRichTextBox.Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editRichTextBox.SelectedText = "";
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editRichTextBox.SelectAll();
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            editRichTextBox.Cut();
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            editRichTextBox.Copy();
        }

        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            editRichTextBox.Paste();
        }

        private void undoToolStripButton_Click(object sender, EventArgs e)
        {
            editRichTextBox.Undo();
        }

        private void redoToolStripButton_Click(object sender, EventArgs e)
        {
            editRichTextBox.Redo(); 
        }

        private void newFile() 
        {
            if (!textSaved)
            {
                var result = MessageBox.Show("Сохранить файл " + currentFileName + "?", "Сохранение",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    saveFile();
                }
            }
            editRichTextBox.Clear();
            currentFileName = "";
            Text = windowsTitle;
            textSaved = false;
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void openFile() 
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (!textSaved)
                {
                    var result = MessageBox.Show("Сохранить файл " + currentFileName + "?", "Сохранение",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        saveFile();
                    }
                }

                string openFileName = openFileDialog1.FileName;
                try
                {
                    editRichTextBox.LoadFile(openFileName, RichTextBoxStreamType.PlainText);
                    textSaved = true;
                    currentFileName = openFileName;
                    this.Text = openFileName + " - " + windowsTitle;
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Не получилось открыть файл");
                }
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void saveFileAs() 
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string newFileName = saveFileDialog1.FileName;
                try
                {
                    editRichTextBox.SaveFile(newFileName, RichTextBoxStreamType.PlainText);
                    textSaved = true;
                    currentFileName = newFileName;
                    this.Text = newFileName + " - " + windowsTitle;
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Не получилось сохранить файл");
                }
            }
        }

        private void saveFile() 
        {
            if (textSaved) return;

            if (currentFileName != "")
            {
                try
                {
                    editRichTextBox.SaveFile(currentFileName, RichTextBoxStreamType.PlainText);
                    textSaved = true;
                    this.Text = currentFileName + " - " + windowsTitle;
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Не получилось сохранить файл");
                }
            }
            else 
            {
                saveFileAs();             
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileAs();
        }


        // Изменение текста
        private void editRichTextBox_TextChanged(object sender, EventArgs e)
        {
            this.textSaved = false;
            this.Text = currentFileName + "* - " + windowsTitle;
        }


        private void createToolStripButton_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openFile();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!textSaved) 
            {
                var result = MessageBox.Show("Сохранить файл " + currentFileName + "?", "Сохранение",
                                       MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    saveFile();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void runHelp() 
        {
            string curDir = Directory.GetCurrentDirectory();
            HelpForm helpForm = new HelpForm("file:///" + curDir + "/Help/html/help.html");
            helpForm.Show();
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            runHelp();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа является курсовой работой\n по дисциплине: 'Формальные языки и компиляторы'\n Выполнил студент группы АВТ-813 Самсонов Н.А.");
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            runHelp();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            saveFileAs();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            editRichTextBox.SelectedText = "";
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            editRichTextBox.Clear();
        }

        string printLeks(string s, Leksem l) 
        {
            RowCol rc = ConvertPosition.IndexToRowCol(s, l.start);
            string type = "";

            if (l.type == TypeLeks.ident)
                type = "Иденитификатор";
            if (l.type == TypeLeks.number)
                type = "Целое без знака";
            if (l.type == TypeLeks.error)
                type = "Ошибочный символ";

            return l.value + ", " + type + ", строка: " + rc.row.ToString()  + ", столбец: " + rc.col.ToString();
        }

        private void run() 
        {
            resultRichTextBox.Clear();

            List<Leksem> leksems = new List<Leksem>();

            string s = editRichTextBox.Text;
            string result = "";
            Leksem l = Scaner.scan(s, 0);
            leksems.Add(l);

            while (l.type != TypeLeks.end_string)
            {
                l = Scaner.scan(s, l.start + l.value.Length);
                leksems.Add(l);
            }

            SynAnaliz synanaliz = new SynAnaliz(leksems, s);
            List<SyntaxError> syntaxErrors = synanaliz.Exec();

            int i = 0;
            while (i < syntaxErrors.Count)
            {
                string code = syntaxErrors[i].code.ToString();
                string pos = syntaxErrors[i].pos.ToString();
                string what = syntaxErrors[i].what;
                result += $"[Ошибка]: \" {what} \"  | позиция: {pos}\n";
                i++;
            }

            if (syntaxErrors.Count == 0)
            {
                result = "Ошибок не обнаружено";
            }
            else
            {
                result += $"Всего ошибок: {syntaxErrors.Count}\n";
            }

            resultRichTextBox.Text = result;
        }

        private void runToolStripButton_Click(object sender, EventArgs e)
        {
            run();
        }

        private void editRichTextBox_SelectionChanged(object sender, EventArgs e)
        {
            positionLabel.Text = editRichTextBox.SelectionStart.ToString();
        }

        private void problemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string curDir = Directory.GetCurrentDirectory();
            HelpForm helpForm = new HelpForm("file:///" + curDir + "/Help/html/postanovka_zadachi.html");
            helpForm.Show();
        }

        private void grammarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string curDir = Directory.GetCurrentDirectory();
            HelpForm helpForm = new HelpForm("file:///" + curDir + "/Help/html/grammatika.html");
            helpForm.Show();
        }

        private void classGramarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string curDir = Directory.GetCurrentDirectory();
            HelpForm helpForm = new HelpForm("file:///" + curDir + "/Help/html/klassifikatsia.html");
            helpForm.Show();
        }

        private void analysisMethodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string curDir = Directory.GetCurrentDirectory();
            HelpForm helpForm = new HelpForm("file:///" + curDir + "/Help/html/metodanaliza.html");
            helpForm.Show();
        }

        private void diagnosticToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string curDir = Directory.GetCurrentDirectory();
            HelpForm helpForm = new HelpForm("file:///" + curDir + "/Help/html/diagnostika.html");
            helpForm.Show();
        }

        private void testExampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editRichTextBox.Text = "((a == ) and (b == c)";
        }

        private void referencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string curDir = Directory.GetCurrentDirectory();
            HelpForm helpForm = new HelpForm("file:///" + curDir + "/Help/html/spisokliteraturi.html");
            helpForm.Show();
        }

        private void sourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string curDir = Directory.GetCurrentDirectory();
            HelpForm helpForm = new HelpForm("file:///" + curDir + "/Help/html/ishodnikod.html");
            helpForm.Show();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            run();
        }
    }
 
}

