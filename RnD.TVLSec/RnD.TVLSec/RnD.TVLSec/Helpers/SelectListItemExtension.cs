using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RnD.TVLSec.Helpers
{
    public static class SelectListItemExtension
    {
        /// <summary>
        /// Dropdown Selected List Show
        /// </summary>
        /// <typeparam name="T">Any Type Object</typeparam>
        /// <param name="objectList">Object Collection</param>
        /// <param name="valueField">Dropdown Value Field</param>
        /// <param name="textField">Dropdown Text Field</param>
        /// <param name="isEdit">Used For Edit. If yes write 'true' else write 'false'</param>
        /// <param name="selectedValue">Dropdown Selected Value</param>
        /// <param name="selectText">Dropdown Selected Value</param>
        /// <returns></returns>
        public static List<SelectListItem> PopulateDropdownList<T>(this List<T> objectList, string valueField, string textField, bool isEdit = false, string selectedValue = "0", string selectText = "- Select -")
        {
            try
            {
                if (string.IsNullOrEmpty(selectedValue))
                {
                    selectedValue = "0";

                }
                var selectedList = new SelectList(objectList, valueField, textField);
                List<SelectListItem> items;
                IEnumerable<SelectListItem> listOfItems;
                if (isEdit)
                {
                    listOfItems = from obj in selectedList select new SelectListItem { Selected = (obj.Value == selectedValue), Text = obj.Text, Value = obj.Value };
                    items = listOfItems.ToList();
                    items.Add(selectedValue == "0"
                                  ? new SelectListItem { Text = selectText, Value = "0", Selected = true }
                                  : new SelectListItem { Text = selectText, Value = "0", Selected = false });
                }
                else
                {
                    listOfItems = from obj in selectedList select new SelectListItem { Selected = false, Text = obj.Text, Value = obj.Value };
                    items = listOfItems.ToList();
                    items.Add(new SelectListItem { Text = selectText, Value = selectedValue, Selected = true });
                }

                return items.OrderByDescending(s => s.Selected).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}