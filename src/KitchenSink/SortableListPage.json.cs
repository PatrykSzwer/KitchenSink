using Starcounter;

namespace KitchenSink
{
    [Database]
    public class Person
    {
        public string Name;
    }

    partial class SortableListPage : Json
    {
        protected override void OnData()
        {
            base.OnData();
            this.Persons.Data = Db.SQL("SELECT p FROM KitchenSink.Person p");
        }
    }
}
