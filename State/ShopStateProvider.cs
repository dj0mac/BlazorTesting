﻿using numbersBlazor.Model;
using System;
using System.Collections.Generic;
using System.Linq;


namespace numbersBlazor.State
{
    public class ShopStateProvider
    {
        public List<ShopItemData> ShoppingCart { get; private set; } = new List<ShopItemData>();
        public event Action OnChange;

        public double GetCartCheckoutSumme()
        {
            double summe = 0;

            foreach (var item in ShoppingCart)
            {
                summe += (item.Price * item.OrderAmount);
            }

            return Math.Round(summe, 2, MidpointRounding.AwayFromZero);
        }
        public void AddShopItem(ShopItemData shopItem)
        {
            ShopItemData item = null;
            if (ShoppingCart.Any())
            {
                item = ShoppingCart.FirstOrDefault(c => c.Id == shopItem.Id);
            }

            if (item == null)
            {
                ShoppingCart.Add(shopItem);
            }
            else
            {

                ShoppingCart[GetIndexOfShoppingCartItem(item)] = shopItem;
            }

            NotifyStateChanged();
        }

        public void SetOrderAmount(ShopItemData shopItem)
        {
            var item = ShoppingCart.First(c => c.Id == shopItem.Id);
            item.OrderAmount = shopItem.OrderAmount;
            ShoppingCart[GetIndexOfShoppingCartItem(item)] = item;

            NotifyStateChanged();
        }

        private void NotifyStateChanged()
        {
            try
            {
                OnChange?.Invoke();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                /*
                 * Prevent double load Invoke() if
                 * user refresh page
                 */
            }

        }

        private int GetIndexOfShoppingCartItem(ShopItemData shopItem)
        {
            return ShoppingCart.IndexOf(shopItem);
        }
    }
}