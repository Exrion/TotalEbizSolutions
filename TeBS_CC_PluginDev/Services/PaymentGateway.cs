using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeBS_CC_PluginDev.Services
{
    public class PaymentGateway
    {
        private readonly string _apiKey = "";
        private readonly string _mainCurrency = "sgd";

        public Stripe.PaymentLink GeneratePaymentLink(string priceId, decimal quantity)
        {
            #region Set Price
            /*var priceOptions = new PriceCreateOptions
            {
                Currency = _mainCurrency,
                UnitAmount = long.Parse(price.ToString()),
                Product = productId
            };

            var priceService = new PriceService();
            priceService.Create(priceOptions);*/
            #endregion

            #region Generate Payment Link
            var linkOptions = new PaymentLinkCreateOptions
            {
                LineItems = new List<PaymentLinkLineItemOptions>
                {
                    new PaymentLinkLineItemOptions { 
                        Price = priceId, 
                        Quantity = long.Parse(quantity.ToString())
                    },
                },
            };
            var linkService = new PaymentLinkService();
            #endregion

            //Return PaymentLink Object
            return linkService.Create(linkOptions);
        }

        public Stripe.PaymentLink GeneratePaymentLink(
            string productName, 
            decimal productAmt, 
            string amtCurr)
        {
            var productCreateOptions = new ProductCreateOptions
            {
                Name = productName,
            };
            
            var productService = new ProductService();
            var product = productService.Create(productCreateOptions);

            var priceCreateOptions = new PriceCreateOptions
            {
                Product = product.Id,
                UnitAmount = long.Parse(productAmt.ToString()),
                Currency = amtCurr
            };

            var priceService = new PriceService();
            var price = priceService.Create(priceCreateOptions);

            var linkOptions = new PaymentLinkCreateOptions
            {
                LineItems = new List<PaymentLinkLineItemOptions>
                {
                    new PaymentLinkLineItemOptions {
                        Price = price.Id,
                        Quantity = 1,
                    },
                },
            };
            var linkService = new PaymentLinkService();

            //Return PaymentLink Object
            return linkService.Create(linkOptions);
        }
    }
}
