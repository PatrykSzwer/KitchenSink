using Starcounter;
using System.Linq;

namespace KitchenSink
{
    [Database]
    public class Person
    {
        public string Name;
        public int OrdNumber;
    }

    partial class SortableListPage : Json
    {
        protected override void OnData()
        {
            base.OnData();
            this.Persons.Data = Db.SQL("SELECT p FROM KitchenSink.Person p ORDER BY OrdNumber");
        }


        [SortableListPage_json.Persons]
        public partial class FoundPlacesItem
        {
            void Handle(Input.OrdNumber orderNumber)
            {
                var maxOrderValue = Db.SQL($"SELECT p FROM KitchenSink.Person p").Count();
                if (orderNumber.Value > maxOrderValue || orderNumber.Value < 1)
                {
                    return;
                }

                var personWithOldOrdValue = Db.SQL<Person>($"SELECT p FROM KitchenSink.Person p WHERE OrdNumber =?", orderNumber.Value).First;
                var personToSetNewValue = Db.SQL<Person>($"SELECT p FROM KitchenSink.Person p WHERE OrdNumber =?", orderNumber.OldValue).First;
                personWithOldOrdValue.OrdNumber = (int)orderNumber.OldValue;
                personToSetNewValue.OrdNumber = (int)orderNumber.Value;

                Transaction.Commit();
            }
        }
    }
}
