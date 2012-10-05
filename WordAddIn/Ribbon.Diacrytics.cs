using System;
using System.Linq;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using stdole;

namespace GaudiaVedantaPublications
{
    partial class Ribbon
    {
        private struct CombiningMark
        {
            public string Id;
            public string Mark;
        }

        private static readonly CombiningMark[] combiningMarks =
        {
            new CombiningMark { Id = "Macron", Mark = "\x0304" },
            new CombiningMark { Id = "MacronBelow", Mark = "\x0331" },
            new CombiningMark { Id = "DotAbove", Mark = "\x0307" },
            new CombiningMark { Id = "DotBelow", Mark = "\x0323" },
            new CombiningMark { Id = "Tilde", Mark = "\x0303" },
            new CombiningMark { Id = "AcuteAccent", Mark = "\x0301" },
            new CombiningMark { Id = "Candrabindu", Mark = "\x0310" },
        };

        public int GetDiacryticsCount(IRibbonControl control)
        {
            return combiningMarks.Count();
        }

        public string GetDiacryticsId(IRibbonControl control, int index)
        {
            return combiningMarks[index].Id;
        }

        public IPictureDisp GetDiacryticsImage(IRibbonControl control, int index)
        {
            return LoadImage(String.Format("{0}.png", combiningMarks[index].Id));
        }

        public void InsertDiacrytics(IRibbonControl control)
        {
            InsertCombiningMark(control.Tag);
        }

        public void InsertDiacryticsItem(IRibbonControl control, string selectedId, int selectedIndex)
        {
            InsertCombiningMark(combiningMarks[selectedIndex].Mark);
        }

        private static void InsertCombiningMark(string mark)
        {
            Globals.ThisAddIn.Application.Selection.Range.InsertAfter(mark);
            Globals.ThisAddIn.Application.Selection.EndOf(WdUnits.wdCharacter);
        }
    }
}
