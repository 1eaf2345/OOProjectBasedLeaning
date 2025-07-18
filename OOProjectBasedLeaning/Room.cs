using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOProjectBasedLeaning
{

    public interface Room
    {

        int Number { get; }

        int Price { get; }

        Hotel Hotel { get; }

        Room AddGuest(Guest guest);

        Room AddGuests(List<Guest> guests);

        Room RemoveGuest(Guest guest);

        Room RemoveGuests(List<Guest> guests);

        bool HasMember();

        bool HasVIP();

        bool IsEmpty();

        bool CheckOutGuest(Guest guest);

    }

    public class RoomModel : ModelEntity, Room
    {

        private int number;
        private int price;
        private Hotel hotel = NullHotel.Instance;
        private List<Guest> guests = new List<Guest>();

        public RoomModel(int number, int price, Hotel hotel)
        {

            this.number = number;
            this.price = price;
            this.hotel = hotel;

        }

        public override int GetHashCode()
        {

            return Number;

        }

        public override bool Equals(object? obj)
        {

            if (obj is Room)
            {

                return Number == (obj as Room)?.Number;

            }

            return false;

        }

        public int Number { get { return number; } }

        public virtual int Price { get { return price; } }

        public Hotel Hotel => hotel;

        public virtual Room AddGuest(Guest guest)
        {

            guests.Add(guest.AddRoom(this));

            return this;

        }

        public virtual Room AddGuests(List<Guest> guests)
        {

            guests.ForEach(guest => AddGuest(guest));

            return this;

        }

        public Room RemoveGuest(Guest guest)
        {

            guests.Remove(guest.RemoveRoom());

            return this;

        }

        public Room RemoveGuests(List<Guest> guests)
        {

            guests.ForEach(guest => RemoveGuest(guest));

            return this;

        }

        public bool HasMember()
        {

            foreach (Guest guest in guests)
            {

                if (guest.IsMember())
                {

                    return true;

                }

            }

            return false;

        }

        public bool HasVIP()
        {

            foreach (Guest guest in guests)
            {

                if (guest.IsVIP())
                {

                    return true;

                }

            }

            return false;

        }

        public bool IsEmpty()
        {

            return guests.Count is 0;

        }

        public bool CheckOutGuest(Guest guest) //Guestをチェックアウトさせる
        {
            RemoveGuest(guest);
            guest.RemoveRoom(); //Guestの状態更新

            return IsEmpty(); //部屋が空になったかの確認
        }

    }

    public class RegularRoom : RoomModel
    {

        public RegularRoom(int number, int price , Hotel hotel) : base(number, price ,hotel)
        {

        }

        public override int Price
        {

            get
            {

                if (HasMember())
                {

                    return base.Price;

                }

                return base.Price + base.Price / 10;

            }

        }

    }

    public class SuiteRoom : RoomModel
    {

        public SuiteRoom(int number, int price, Hotel hotel) : base(number, price, hotel)
        {

        }

        public override int Price
        {

            get
            {

                if (HasVIP())
                {

                    return base.Price;

                }

                return base.Price + base.Price / 10;

            }

        }

        public override Room AddGuest(Guest guest)
        {

            if (guest is not Member)
            {

                throw new OnlyMembersCanStayInSuiteRoomsException();

            }

            return base.AddGuest(guest);

        }

        public override Room AddGuests(List<Guest> guests)
        {

            if (!HasMember())
            {

                throw new OnlyMembersCanStayInSuiteRoomsException();

            }

            return base.AddGuests(guests);

        }

    }

    public class NullRoom : Room, NullObject
    {

        private static Room instance = new NullRoom();

        private NullRoom()
        {

        }

        public static Room Instance { get { return instance; } }

        public int Number => int.MinValue;

        public int Price => int.MinValue;

        public Hotel Hotel => NullHotel.Instance;

        public Room AddGuest(Guest guest)
        {

            return this;

        }

        public Room AddGuests(List<Guest> guests)
        {

            return this;

        }

        public bool CheckOutGuest(Guest guest)
        {
            throw new NotImplementedException();
        }

        public bool HasMember()
        {
            throw new NotImplementedException();
        }

        public bool HasVIP()
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty()
        {
            throw new NotImplementedException();
        }

        public Room RemoveGuest(Guest guest)
        {
            throw new NotImplementedException();
        }

        public Room RemoveGuests(List<Guest> guests)
        {
            throw new NotImplementedException();
        }
    }

}
