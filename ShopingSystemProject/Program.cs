namespace ShopingSystemProject
{
    /*
     * TryGetValue(item, out price) => 
     * is a bool func that used to obtain the value of specific key form the dictionary
     * and return the key in the passed parameter by out keyword, the item represent the
     * key, this func use out because, it will return the value of the specific key in 
     * that prameter (price), and we can't pass the parameter without initialization, without
     * existing out keyword.
     * 
     * private static IEnumerable<Tuple<string, double>> GetCartPrices() = private static List<Tuple<string, double>> GetCartPrices() =>
     * كان ممكن ارجع ليست عادى لاكن الليست دى عبارة عن كلاس فى الاساس و انت عارف ان الكلاس ريفرنس تيب يعنى اى حد هيكول الميثود دى و هى نوعها ليست هترجعله ليست 
     * و وقتها اى تعديل هيعمله عليها هياثر على الليست الاصليه اللى فى الميثود دى, اما لو رجعت اي أنيمربل انت كدا عملت نوع من الابستركشن فى الديزاين بتاعك بحيث
     * اللى يستقبل الليست اللى هترجع من الميثود دى يقدر يحولها لاى نوع من انواع الكوليكشن وغير ان لو رجعت اى انيمربل اللى هيكول الميثود دى اما تتنفذ و ترجعله الليست
     * هتكون ايميوتبل ليست يعنى ليست غير قابله للتعديل لان الميثود اللى بترنرنها من نوع اى انيمربل ودا طبعا ادانى سيكيورتى للبايانات الموجودة جوة الليست اللى الميثود
     * دى هترجعها لان حميت الليست اللى فى الميثود دى من اى تعجيل ممكن حد من اللى هيعمل كول للميثود دى يعمله .
     * 
     */
    internal class Program
    {
        static public List<string> _cartItems = new(); // User shopping cart.
        static public Dictionary<string, double> _itemPrices = new() // Stock.
        {
            {"Camera", 2399.00 },
            {"Labtop", 35000.00 },
            {"TV", 20000 } 
        };
        static public Stack<string> _actions = new();
        static void Main(string[] args)
        { 
            while (true)
            {
                Console.WriteLine("---------------------------------"); 
                Console.WriteLine("Welcome to the shopping system...");
                Console.WriteLine("---------------------------------");
                Console.WriteLine("1. Add item to cart");
                Console.WriteLine("2. View cart items");
                Console.WriteLine("3. Remove item from cart");
                Console.WriteLine("4. Checkout");
                Console.WriteLine("5. Undo last action");
                Console.WriteLine("6. Exit");
                Console.WriteLine("Please enter choice number:");

                int choice = int.Parse(Console.ReadLine());
                switch (choice)
                {
                    case 1: 
                        AddItem();
                        break;
                    case 2:
                        ViewCart();
                        break;
                    case 3:
                        RemoveItem();
                        break;
                    case 4:
                        CheckOut();
                        break;
                    case 5:
                        UndoLastAction();
                        break;
                    case 6:
                         Environment.Exit(0); // To break the program.
                        break;
                    default:
                        Console.WriteLine("Invalid Choice Entered, Please Try Again!!");
                        break;
                }
            } 
        }

        private static void AddItem()
        {
            Console.WriteLine("Avilable items: "); 
            foreach (var item in _itemPrices) 
                Console.WriteLine($"Item: {item.Key}\t Price: {item.Value}");

            Console.WriteLine("Please enter your product name: ");
            string cartItem = Console.ReadLine();

            if (_itemPrices.ContainsKey(cartItem))
            {
                _cartItems.Add(cartItem);
                _actions.Push($"Item {cartItem} added to cart");
                Console.WriteLine($"{cartItem} is added to cart successfully"); 
            }
            else
            {
                Console.WriteLine("Item is out of stock or not avaliable !");
            }


        }
        private static void ViewCart()
        {
            var itemPriceCollection = GetCartPrices(); 

            if (_cartItems.Any()) // Any() is an extension method to determine wheter a sequence contain any elements.
            {
                Console.WriteLine("Your cart items: ");

                foreach(var item in itemPriceCollection)
                    Console.WriteLine($"Item: {item.Item1}\t Price: {item.Item2}"); 
            }
            else
            {
                Console.WriteLine("Your cart is empty"); 
            }
             
        }
        //private static IEnumerable<Dictionary<string, double>> GetCartPrice()
        //{
        //    Dictionary<string, double> carPrices = new();
        //    foreach (var item in _cartItems)
        //    {
        //        double price;
        //        bool foundItem = _itemPrices.TryGetValue(item, out price);
        //        if (true)
        //        {
        //            carPrices.Add(item, price);  
        //        }
        //    }
        //    return carPrices;
        //}
        private static IEnumerable<Tuple<string, double>> GetCartPrices()
        {
            List<Tuple<string, double>> cartPrices = new(); // is the same dictionary.
            foreach(var item in _cartItems)
            {
                double price;
                bool foundItem = _itemPrices.TryGetValue(item, out price);

                if (foundItem)
                    cartPrices.Add(new Tuple<string, double>(item, price)); 
            }
            return cartPrices;  
        }
        private static void RemoveItem()
        { 
            if (_cartItems.Any())
            {
                Console.WriteLine("The current items in your cart: ");
                ViewCart();

                Console.WriteLine("Please enter the item that you want to remove it");
                var itemToRemove = Console.ReadLine();

                if (_cartItems.Contains(itemToRemove))
                {
                    _cartItems.Remove(itemToRemove);
                    _actions.Push($"Item {itemToRemove} removed from cart");
                    Console.WriteLine($"{itemToRemove} has been removed successfully");
                }
                else
                    Console.WriteLine("Invalid cart item"); 
            }
            else
            {
                Console.WriteLine("Item doesn't exist in shopping cart");
            }
        }
        private static void CheckOut()
        {
            double totalPrice = 0; 
            if (_cartItems.Any())
            { 
                IEnumerable<Tuple<string, double>> itemsinCart = GetCartPrices();
                foreach (var item in itemsinCart)
                    totalPrice += item.Item2;

                Console.WriteLine($"Total price to pay = {totalPrice}");
                Console.WriteLine("Please proceed to payment section, Thank you for shopping with us");

                _cartItems.Clear(); // after the user payed, we must clear the cart.
                _actions.Push("Checkout");
            }
            else
            {
                Console.WriteLine("Your cart is empty");
            }
        }
        private static void UndoLastAction()
        {
            if (_actions.Count > 0)
            {
                string lastAction = _actions.Pop();               // If lastAction contain added to cart that's mean the item is already added to cart so, when I make undo I need to remove this item from cart
                var _actionsArray = lastAction.Split(" ");

                if (lastAction.Contains("added"))
                    _cartItems.Remove(_actionsArray[1]);
                else if (lastAction.Contains("removed"))
                    _cartItems.Add(_actionsArray[1]);
                else 
                    Console.WriteLine("Checkout can't be undo, please ask for refund"); 
            }
        } 
    }
}
