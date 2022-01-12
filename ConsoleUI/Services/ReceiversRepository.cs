using ConsoleUI.Models;

namespace ConsoleUI.Services
{
    public class ReceiversRepository
    {
        private ReceiversContext db = new ReceiversContext();
        public List<Receiver>? GetAll() => db.Receivers == null ? null : db.Receivers.ToList();

        public Receiver? Find(long id) => db.Receivers?.Find(id);
     
        public bool Add(Receiver receiver)
        {
            db.Add(receiver);
            db.SaveChanges();
            return true;
        }
    }
}
