using System;
using System.Collections.Generic;
using System.Linq;

namespace OOProjectBasedLeaning
{
    public interface Hotel : Place
    {
        void CheckIn(Guest guest);
        void CheckIn(List<Guest> guests);
        void CheckOut(Guest guest);
        void CheckOut(List<Guest> guests);
        bool IsVacancies();
    }

    public class HotelModel : NotifierModelEntity, Hotel
    {
        private List<Room> vacantRooms;
        private List<Room> guestBook = new List<Room>();

        public HotelModel() : this(string.Empty)
        {
            InitializeComponent();
        }

        public HotelModel(string name)
        {
            Name = name;
            InitializeComponent();
        }

        private void InitializeComponent() // ゲスト専用ルーム2部屋、会員専用スイートルーム2部屋
        {
            vacantRooms = new List<Room>
            {
                // ゲスト専用ルーム（会員は利用不可ナリ）
                new GuestOnlyRoom(801, 18000, this),
                new GuestOnlyRoom(802, 18000, this),

                // 会員専用
                new SuiteRoom(1001, 360000, this),
                new SuiteRoom(1002, 300000, this)
            };
        }

        private Room AcquireRoom()
        {
            Room room = vacantRooms.First();
            vacantRooms.Remove(room);
            return room;
        }

        private void ReleaseRoom(Room room)
        {
            vacantRooms.Add(room);
        }

        public void CheckIn(Guest guest)
        {
            if (guest.StayAt() is not NullObject)
                throw new AlreadyCheckedInException(guest);

            if (IsVacancies())
            {
                Room room = AcquireRoom();
                try
                {
                    guestBook.Add(room.AddGuest(guest));
                    Notify();
                }
                catch (Exception ex)
                {
                    ReleaseRoom(room);
                    MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                throw new IsNotVacanciesException();
            }
        }

        public void CheckIn(List<Guest> guests)
        {
            if (guests.Any(g => g.StayAt() is not NullObject))
                throw new AlreadyCheckedInException(guests);

            if (IsVacancies())
            {
                Room room = AcquireRoom();
                try
                {
                    guestBook.Add(room.AddGuests(guests));
                    Notify();
                }
                catch (Exception ex)
                {
                    ReleaseRoom(room);
                    MessageBox.Show($"{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                throw new IsNotVacanciesException();
            }
        }

        public void CheckOut(Guest guest)
        {
            Room room = guest.StayAt();
            if (room.RemoveGuest(guest).IsEmpty())
            {
                guestBook.Remove(room);
                ReleaseRoom(room);
            }
            Notify();
        }

        public void CheckOut(List<Guest> guests)
        {
            guests.ForEach(guest => CheckOut(guest));
        }

        public bool IsVacancies()
        {
            return vacantRooms.Count > 0;
        }
    }

    public class NullHotel : Hotel
    {
        private static readonly Hotel instance = new NullHotel();
        private NullHotel() { }
        public static Hotel Instance => instance;

        public void CheckIn(Guest guest) { }
        public void CheckIn(List<Guest> guests) { }
        public void CheckOut(Guest guest) { }
        public void CheckOut(List<Guest> guests) { }
        public bool IsVacancies() => false;
    }

    // =================== 追加クラスと例外 ===================

    public class GuestOnlyRoom : RegularRoom
    {
        public GuestOnlyRoom(int number, int price, Hotel hotel)
            : base(number, price, hotel) { }

        public override Room AddGuest(Guest guest)
        {
            if (guest.IsMember())
                throw new OnlyGuestsCanStayInRegularRoomsException();
            return base.AddGuest(guest);
        }

        public override Room AddGuests(List<Guest> guests)
        {
            if (guests.Any(g => g.IsMember()))
                throw new OnlyGuestsCanStayInRegularRoomsException();
            return base.AddGuests(guests);
        }
    }

    public class OnlyGuestsCanStayInRegularRoomsException : Exception
    {
        public OnlyGuestsCanStayInRegularRoomsException()
            : base("この部屋はゲスト専用です。会員は宿泊できません。") { }
    }

    // Room, RegularRoom, SuiteRoom, Guest, Place, NullObject, NotifierModelEntity,
    // AlreadyCheckedInException, IsNotVacanciesException などは別途定義されている前提
}
