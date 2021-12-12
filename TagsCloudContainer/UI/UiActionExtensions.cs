﻿using System.Collections.Generic;
using TagsCloudContainer.UI.Menu;

namespace TagsCloudContainer.UI
{
    public static class UiActionExtensions
    {
        public static MainMenu GetMenu(this IUiAction[] actions)
        {
            var categories = GetCategories(actions);
            var menuCategories = new Dictionary<int, Category>();
            for (var i = 0; i < categories.Length; i++) 
                menuCategories[i + 1] = categories[i];
            return new MainMenu(menuCategories);
        }

        private static Category[] GetCategories(IUiAction[] actions)
        {
            var result = new List<Category>();
            var categories = new Dictionary<string, List<MenuItem>>();
            
            foreach (var uiAction in actions)
            {
                var category = uiAction.Category;
                var menuItem = CreateMenuItem(uiAction);
                if (!categories.ContainsKey(category))
                    categories[category] = new List<MenuItem>();
                categories[category].Add(menuItem);
            }
            
            foreach (var name in categories.Keys)
            {
                var categoryItems = new Dictionary<int, MenuItem>();
                var itmes = categories[name];
                for (var i = 0; i < itmes.Count; i++) 
                    categoryItems[i + 1] = itmes[i];
                result.Add(new Category(categoryItems, name));
            }

            return result.ToArray();
        }

        private static MenuItem CreateMenuItem(IUiAction action) 
            => new MenuItem(action.Name, action.Perform);
    }
}