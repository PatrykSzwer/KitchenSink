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
            void Handle(Input.OrdNumber ordNumber)
            {
                var maxOrdValue = Db.SQL($"SELECT p FROM KitchenSink.Person p").Count();
                if (ordNumber.Value > maxOrdValue || ordNumber.Value < 1)
                {
                    return;
                }

                var personWithOldOrdValue = Db.SQL<Person>($"SELECT p FROM KitchenSink.Person p WHERE OrdNumber =?", ordNumber.Value).First;
                var personToSetNewOrdValue = Db.SQL<Person>($"SELECT p FROM KitchenSink.Person p WHERE OrdNumber =?", ordNumber.OldValue).First;
                personWithOldOrdValue.OrdNumber = (int)ordNumber.OldValue;
                personToSetNewOrdValue.OrdNumber = (int)ordNumber.Value;

                Transaction.Commit();
            }
        }
    }
}
