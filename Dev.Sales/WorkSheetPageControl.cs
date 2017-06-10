using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinForms.Documents.Model;

namespace Dev.Sales
{
    public partial class WorkSheetPageControl : UserControl
    {
        public WorkSheetPageControl()
        {
            InitializeComponent();
            //radDock1.DocumentTabsVisible = false;
            SetDefaultFontPropertiesToEditor(rteMessages);
            this.rteMessages.Document.SectionDefaultPageMargin = new Telerik.WinForms.Documents.Layout.Padding(3, 3, 3, 3);

            richTextEditorRibbonBar1.RibbonBarElement.TabStripElement.ItemContainer.Padding = new System.Windows.Forms.Padding(0);
            richTextEditorRibbonBar1.RibbonBarElement.ApplicationButtonElement.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;

            richTextEditorRibbonBar1.RibbonBarElement.QuickAccessToolBar.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            richTextEditorRibbonBar1.RibbonBarElement.RibbonCaption.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;

        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    rteMessages.Text += Environment.NewLine + txtMessage.Text.Trim();
                    txtMessage.Text = "";
                    rteMessages.Document.CaretPosition.MoveToLastPositionInDocument(); 
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
        }

        // 기본 설정
        public void SetDefaultFontPropertiesToEditor(RadRichTextEditor editor)
        {
            editor.RichTextBoxElement.ChangeFontFamily(new Telerik.WinControls.RichTextEditor.UI.FontFamily("Arial"));
            editor.RichTextBoxElement.ChangeFontSize(Unit.PointToDip(12));
            editor.RichTextBoxElement.ChangeFontStyle(Telerik.WinControls.RichTextEditor.UI.FontStyles.Normal);
            editor.RichTextBoxElement.ChangeFontWeight(Telerik.WinControls.RichTextEditor.UI.FontWeights.Normal);

            editor.DocumentInheritsDefaultStyleSettings = true;


        }
    }
    
}
